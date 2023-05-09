using System.Collections.Generic;

namespace NISC_MFP_MVC.ViewModels.CardReader
{
    public class MultiFunctionPrintViewModel
    {
        public MultiFunctionPrintViewModel()
        {
            cardReaderModel = new CardReaderModel();
            multiFunctionPrintModels = new List<MultiFunctionPrintModel>();
        }

        public CardReaderModel cardReaderModel { get; set; }

        public List<MultiFunctionPrintModel> multiFunctionPrintModels { get; set; }

        public MultiFunctionPrintModel multiFunctionPrintModel { get; set; }
    }
}