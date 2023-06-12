using NISC_MFP_MVC.App_Start;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace NISC_MFP_MVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //Add filter for catch exception by NLog
            GlobalFilters.Filters.Add(new CustomErrorFilterAttribute());

            Application["Version"] = "v3.0";
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                // �����o�ӨϥΪ̪� FormsIdentity
                FormsIdentity id = (FormsIdentity)User.Identity;
                // �A���X�ϥΪ̪� FormsAuthenticationTicket
                FormsAuthenticationTicket ticket = id.Ticket;
                // �N�x�s�b FormsAuthenticationTicket ��������w�q���X�A���ন�r��}�C
                string[] roles = ticket.UserData.Split(new char[] { ',' });
                // ���������ثe�o�� HttpContext �� User ����h
                //���b�Хߪ�檺�ɭԡA�A��UserData ��ϥΪ̦W�ٴN�O���W�١A�ک񪺬O�s�եN���A�ҥH���X�ӴN�O�s�եN��
                //�M��|��o�Ӹ�Ʃ��Context.User��
                Context.User = new GenericPrincipal(Context.User.Identity, roles);
            }
        }
    }
}
