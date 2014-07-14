using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Services;
using System.Xml.Serialization;
using AutoUpdate.Server.Models;


namespace AutoUpdate.Server
{
    /// <summary>
    /// AutoUpdateService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class AutoUpdateService : WebService
    {
        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="dllInfo"></param>
        /// <returns></returns>
        [WebMethod]
        [XmlInclude(typeof(DllInfo))]
        public DllInfo CheckVersion(DllInfo dllInfo)
        {
            AutoUpdateManager.CheckVersion(dllInfo);
            return dllInfo;
        }

        /// <summary>
        /// 检查版本
        /// </summary>
        /// <param name="dllList"></param>
        /// <returns></returns>
        [WebMethod]
        [XmlInclude(typeof(DllInfo))]
        public List<DllInfo> CheckVersions(List<DllInfo> dllList)
        {
            AutoUpdateManager.CheckVersions(dllList);
            return dllList;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dllInfo"></param>
        /// <returns></returns>
        [WebMethod]
        [XmlInclude(typeof(DllInfo))]
        public DllInfo Update(DllInfo dllInfo)
        {
            if (CheckVersion(dllInfo).Status == DllStatus.NeedUpdate)
            {
                AutoUpdateManager.DownLoad(dllInfo);
            }
            return dllInfo;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dllList"></param>
        /// <returns></returns>
        [WebMethod]
        [XmlInclude(typeof(DllInfo))]
        public List<DllInfo> UpdateList(List<DllInfo> dllList)
        {
            AutoUpdateManager.DownLoadAll(dllList);
            return dllList;
        }

    }
}
