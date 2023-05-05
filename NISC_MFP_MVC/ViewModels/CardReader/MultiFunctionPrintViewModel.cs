using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

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
    }
}