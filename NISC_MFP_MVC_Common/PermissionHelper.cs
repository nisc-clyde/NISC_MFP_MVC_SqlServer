using System.Collections.Generic;
using System.Linq;

namespace NISC_MFP_MVC_Common
{
    public class PermissionHelper
    {
        private string _permissionString;

        public PermissionHelper(string permissionString)
        {
            this._permissionString = permissionString ?? "";
        }

        public void PermissionString(string permissionString)
        {
            this._permissionString = permissionString ?? "";
        }

        /// <summary>
        /// 依Filter排序和去除Permission
        /// </summary>
        /// <param name="permissionFilter">FilterList"</param>
        /// <returns></returns>
        public List<string> Order(List<string> permissionFilter)
        {
            _permissionString = _permissionString.Contains(".php") ? _permissionString.Replace(".php", "") : _permissionString;
            List<string> source = _permissionString.Split(',').ToList();

            //取來源PermissionList和PermissionFilter之交集
            IEnumerable<string> intersectPermission = source.Intersect(permissionFilter);
            return intersectPermission.OrderBy(p => permissionFilter.IndexOf(p)).ToList();
        }

        /// <summary>
        /// 依Filter排序和去除Permission
        /// </summary>
        /// <param name="permissionFilter">FilterString, Rule="permission1,permission2,..."</param>
        /// <returns></returns>
        public List<string> Order(string permissionFilter)
        {
            List<string> permissionFilterAsList = permissionFilter.Split(',').ToList();
            _permissionString = _permissionString.Contains(".php") ? _permissionString.Replace(".php", "") : _permissionString;
            List<string> source = _permissionString.Split(',').ToList();

            //取來源PermissionList和PermissionFilter之交集，舊系統也存在其他權限之字串，同時也可在此去除
            IEnumerable<string> intersectPermission = source.Intersect(permissionFilterAsList);
            return intersectPermission.OrderBy(p => permissionFilterAsList.IndexOf(p)).ToList();
        }

        /// <summary>
        /// 從Permission中檢查Dictionary規則，若Value存在但Key不存在，加入Key
        /// <para>Key : MainPermission</para>
        /// <para>Value : SubPermission</para>
        /// </summary>
        /// <param name="maps">Main and Sub rules</param>
        /// <returns></returns>
        public List<string> FillMainPermission(Dictionary<string, string> maps)
        {
            _permissionString = _permissionString.Contains(".php") ? _permissionString.Replace(".php", "") : _permissionString;
            List<string> source = _permissionString.Split(',').ToList();

            foreach (KeyValuePair<string, string> map in maps)
            {
                //沒有MainPermission但有SubPermission則新增MainPermission
                if (!source.Contains(map.Key) && source.Contains(map.Value)) source.Add(map.Key);
            }

            return source;
        }

        /// <summary>
        /// 從Permission中檢查Dictionary規則，若Key存在但Value不存在，加入Value
        /// <para>Key : MainPermission</para>
        /// <para>Value : SubPermission</para>
        /// </summary>
        /// <param name="maps">Main and Sub rules</param>
        /// <returns></returns>
        public List<string> FillSubPermission(Dictionary<string, string> maps)
        {
            _permissionString = _permissionString.Contains(".php") ? _permissionString.Replace(".php", "") : _permissionString;
            List<string> source = _permissionString.Split(',').ToList();

            foreach (KeyValuePair<string, string> map in maps)
            {
                //有MainPermission但沒有SubPermission則新增SubPermission
                if (source.Contains(map.Key) && !source.Contains(map.Value)) source.Add(map.Value);
            }

            return source;
        }

        /// <summary>
        /// 從Permission中檢查Dictionary規則，
        /// <para>若Key不存在，加入Key</para>
        /// <para>若Value不存在，加入Value</para>
        /// <para>Key : MainPermission</para>
        /// <para>Value : SubPermission</para>
        /// </summary>
        /// <param name="maps">Main and Sub rules</param>
        /// <returns></returns>
        public List<string> FillAllPermission(Dictionary<string, string> maps)
        {
            _permissionString = _permissionString.Contains(".php") ? _permissionString.Replace(".php", "") : _permissionString;
            List<string> source = _permissionString.Split(',').ToList();

            foreach (KeyValuePair<string, string> map in maps)
            {
                if (!source.Contains(map.Key)) source.Add(map.Key);//沒有MainPermission則新增MainPermission
                if (!source.Contains(map.Value)) source.Add(map.Value);//沒有SubPermission則新增SubPermission
            }

            return source;
        }
    }
}
