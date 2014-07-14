using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using AutoUpdate.Server.Models;
using CSharp.Utilities;

namespace AutoUpdate.Server
{
    public class AutoUpdateManager
    {
        #region 检测版本

        /// <summary>
        /// 检查DLL版本号
        /// </summary>
        /// <param name="dllInfo"></param>
        /// <returns></returns>
        public static void CheckVersion(DllInfo dllInfo)
        {
            string serverVersion = ConfigHelper.GetAppValue(dllInfo.FileName); ;
            if (string.IsNullOrWhiteSpace(serverVersion))
            {
                dllInfo.Status = DllStatus.NoFile;
                return;
            }
            if (serverVersion == dllInfo.Version)
            {
                dllInfo.Status = DllStatus.IsNewest;
                return;
            }
            if (CompareVersion(serverVersion, dllInfo.Version))
            {
                dllInfo.Status = DllStatus.NeedUpdate;
            }
         
        }

        /// <summary>
        /// 检查DLL版本号
        /// </summary>
        /// <param name="dllList"></param>
        public static void CheckVersions(List<DllInfo> dllList)
        {
            Task[] tasks = new Task[dllList.Count];
            for (int i = 0; i < dllList.Count; i++)
            {
                int index = i;
                tasks[i] = Task.Factory.StartNew(() => CheckVersion(dllList[index]));
            }
            Task.WaitAll(tasks);
        }

        #endregion

        #region 下载更新

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="dllInfo"></param>
        /// <returns></returns>
        public static void DownLoad(DllInfo dllInfo)
        {
            string exePath = CheckExeName(dllInfo.FileName);
            if (!string.IsNullOrWhiteSpace(dllInfo.FileName))
            {
                dllInfo.Bytes = File.ReadAllBytes(exePath);
            }
        }

        /// <summary>
        /// 下载
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static void DownLoadAll(List<DllInfo> list)
        {
            Task[] tasks = new Task[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                int index = i;
                tasks[index] = Task.Factory.StartNew(() =>
                {
                    string exePath = CheckExeName(list[index].FileName);
                    if (!string.IsNullOrWhiteSpace(list[index].FileName))
                    {
                        list[index].Bytes = File.ReadAllBytes(exePath);
                    }
                });
            }
            Task.WaitAll(tasks);
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 比较版本号
        /// </summary>
        /// <param name="serverVersion"></param>
        /// <param name="clientVersion"></param>
        /// <returns></returns>
        private static bool CompareVersion(string serverVersion, string clientVersion)
        {
            if (string.IsNullOrWhiteSpace(serverVersion) || string.IsNullOrWhiteSpace(clientVersion))
            {
                return false;
            }
            string[] server = serverVersion.Split('.');
            string[] client = clientVersion.Split('.');
            if (server.Length != 4 || client.Length != 4)
            {
                return false;
            }
            for (int i = 0; i < server.Length; i++)
            {
                if (server[i].ToInt32() < client[i].ToInt32())
                {
                    return false;
                }
                if (server[i].ToInt32() == client[i].ToInt32())
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// 检查文件是否存在
        /// </summary>
        /// <param name="exeName"></param>
        /// <returns></returns>
        private static string CheckExeName(string exeName)
        {
            string exePath = GetMapPath() + "Bin\\Setups\\" + exeName;
            if (File.Exists(exePath))
            {
                return exePath;
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取服务端的绝对路径
        /// </summary>
        /// <returns></returns>
        private static string GetMapPath()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath("/");
            }
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        #endregion
    }
}