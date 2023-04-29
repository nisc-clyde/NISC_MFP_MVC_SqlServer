namespace NISC_MFP_MVC_Service.DTOs.Info.Watermark
{
    public class WatermarkInfoConvert2Text : AbstractWatermarkInfo
    {

        public override string type
        {
            get => base.type;
            set
            {
                if (value == "0")
                {
                    base.type = "圖片";
                }
                else if (value == "1")
                {
                    base.type = "文字";
                }
                else
                {
                    base.type = value;
                }
            }
        }

        public override string position_mode
        {
            get => base.position_mode;
            set
            {
                switch (value)
                {
                    case "0":
                        base.position_mode = "左上";
                        break;
                    case "1":
                        base.position_mode = "左下";
                        break;
                    case "2":
                        base.position_mode = "右上";
                        break;
                    case "3":
                        base.position_mode = "右下上";
                        break;
                    case "4":
                        base.position_mode = "正中間";
                        break;
                    default:
                        base.position_mode = value;
                        break;
                }
            }
        }

        public override string fill_mode
        {
            get => base.fill_mode;
            set
            {
                switch (value)
                {
                    case "0":
                        base.fill_mode = "無";
                        break;
                    case "1":
                        base.fill_mode = "依原圖比例多餘裁切";
                        break;
                    case "2":
                        base.fill_mode = "依原圖比例不裁切";
                        break;
                    case "3":
                        base.fill_mode = "依紙張比例";
                        break;
                    case "4":
                        base.fill_mode = "重覆填滿";
                        break;
                    case "5":
                        base.fill_mode = "置中，並依原圖比例多餘裁切";
                        break;
                    default:
                        base.fill_mode = value;
                        break;
                }
            }
        }
    }
}
