using System.Collections.Generic;
using System.Linq;

namespace NISC_MFP_MVC_Common
{
    public class PermissionHelper
    {
        private string permissionString = "";

        public PermissionHelper(string permissionString)
        {
            this.permissionString = permissionString ?? "";
        }

        public void PermissionString(string permissionString)
        {
            this.permissionString = permissionString ?? "";
        }

        /// <summary>
        /// 依Filter排序和去除Permission
        /// </summary>
        /// <param name="permissionFilter">FilterList"</param>
        /// <returns></returns>
        public List<string> Order(List<string> permissionFilter)
        {
            permissionString = permissionString.Contains(".php") ? permissionString.Replace(".php", "") : permissionString;
            List<string> source = permissionString.Split(',').ToList();

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
            permissionString = permissionString.Contains(".php") ? permissionString.Replace(".php", "") : permissionString;
            List<string> source = permissionString.Split(',').ToList();

            //取來源PermissionList和PermissionFilter之交集，舊系統也存在其他權限之字串，同時也可在此去除
            IEnumerable<string> intersectPermission = source.Intersect(permissionFilterAsList);
            return intersectPermission.OrderBy(p => permissionFilterAsList.IndexOf(p)).ToList();
        }

        /// <summary>
        /// 確認Dictionary的SubPermission加入前Dictionary的MainPermission是否已加入，若未，則加入
        /// Key : MainPermission
        /// Value : SubPermission
        /// </summary>
        /// <param name="map"></param>
        /// <returns></returns>
        public List<string> FillPermission(Dictionary<string, string> maps)
        {
            permissionString = permissionString.Contains(".php") ? permissionString.Replace(".php", "") : permissionString;
            List<string> source = permissionString.Split(',').ToList();

            foreach (KeyValuePair<string, string> map in maps)
            {
                if (!source.Contains(map.Key) && source.Contains(map.Value)) source.Add(map.Key);//沒有MainPermission但有SubPermission則新增
            }

            return source;
        }
    }
}
