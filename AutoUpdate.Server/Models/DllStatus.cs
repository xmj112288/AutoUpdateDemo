using System;
using System.ComponentModel;

namespace AutoUpdate.Server.Models
{
    [Serializable]
    public enum DllStatus
    {
        [Description("最新版本")]
        IsNewest,

        [Description("需要更新")]
        NeedUpdate,

        [Description("服务端没有该文件")]
        NoFile,
    }
}