using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels.Card
{
    public class ResetFreeValueViewModel
    {
        [Range(0, int.MaxValue, ErrorMessage = "欄位內容格式錯誤")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [DisplayName("免費點數（所有卡片之免費點數將重設）")]
        public int freevalue { get; set; }
    }
}