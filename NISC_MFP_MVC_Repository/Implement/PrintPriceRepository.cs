using NISC_MFP_MVC_Repository.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class PrintPriceRepository : IDisposable
    {
        protected MFP_DB db { get; private set; }
        public PrintPriceRepository()
        {
            db = new MFP_DB();
        }

        public List<tb_print_price> GetAll()
        {
            return db.tb_print_price.AsNoTracking().ToList();
        }

        public tb_print_price Get(string column, string value, string operation)
        {
            tb_print_price result = db.tb_print_price.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return result;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
    }
}
