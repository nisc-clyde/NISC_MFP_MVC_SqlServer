using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels
{
    public class WatermarkViewModel
    {
        public int id { get; set; }

        [DisplayName("類別")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        public string type { get; set; }

        [DisplayName("左邊偏移")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int left_offset { get; set; }

        [DisplayName("右邊偏移")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int right_offset { get; set; }

        [DisplayName("上邊偏移")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int top_offset { get; set; }

        [DisplayName("下邊偏移")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int bottom_offset { get; set; }

        [DisplayName("浮水印位置")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        public string position_mode { get; set; }

        [DisplayName("填滿方式")]
        public string fill_mode { get; set; }

        [DisplayName("文字")]
        public string text { get; set; }

        [DisplayName("圖片位置")]
        public string image_path { get; set; }

        [DisplayName("旋轉角度")]
        [RegularExpression(@"(([0-9]{1,3})\.)(([0-9]{1,3}))", ErrorMessage = "請輸入三位數欲旋轉之角度，且最多至小數點後三位")]
        public float? rotation { get; set; }

        [DisplayName("顏色")]
        public string color { get; set; }

        [DisplayName("水平對齊")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int? horizontal_alignment { get; set; }

        [DisplayName("垂直對齊")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int? vertical_alignment { get; set; }

        [DisplayName("字型")]
        public string font_name { get; set; }

        [DisplayName("文字高度")]
        [RegularExpression("([0-9]{1,5})", ErrorMessage = "請輸入數字，至多五位數")]
        public int? font_height { get; set; }

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "type";
                case "1": return "left_offset";
                case "2": return "right_offset";
                case "3": return "top_offset";
                case "4": return "bottom_offset";
                case "5": return "position_mode";
                case "6": return "fill_mode";
                case "7": return "text";
                case "8": return "image_path";
                case "9": return "rotation";
                case "10": return "color";
                default: return "type";
            }
        }
    }
}