using Microsoft.Ajax.Utilities;
using NISC_MFP_MVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO_Initial
{
    public class tb_department_dto
    {
        private int? _dept_value = 0;
        private int? _dept_month_sum = 0;


        public int serial { get; set; }
        public string dept_id { get; set; }
        public string dept_name { get; set; } = null;
        public Nullable<int> dept_value { get { return _dept_value; } set { _dept_value = value ?? 0; } }

        public Nullable<int> dept_month_sum { get { return _dept_month_sum; } set { _dept_month_sum = value ?? 0; } }

        public string dept_usable { get; set; } = "1";
        public string dept_email { get; set; } = null;
        public string if_deleted { get; set; } = "0";

        public tb_department Convert2DatabaseModel()
        {
            tb_department department = new tb_department();

            department.serial = serial;
            department.dept_id = dept_id;
            department.dept_name = dept_name;
            department.dept_value = dept_value;
            department.dept_month_sum = dept_month_sum;
            department.dept_usable = dept_usable;
            department.dept_email = dept_email;
            department.if_deleted = if_deleted;

            return department;
        }

        public SearchDepartmentDTO Convert2PresentationModel()
        {
            SearchDepartmentDTO department = new SearchDepartmentDTO();

            department.serial = serial;
            department.dept_id = dept_id;
            department.dept_name = dept_name;
            department.dept_value = dept_value;
            department.dept_month_sum = dept_month_sum;
            department.dept_usable = dept_usable;
            department.dept_email = dept_email;

            return department;
        }
    }
}