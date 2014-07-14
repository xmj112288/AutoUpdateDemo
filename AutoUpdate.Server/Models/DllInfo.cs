using System;

namespace AutoUpdate.Server.Models
{
    [Serializable]
    public class DllInfo
    {
        /// <summary>
        /// 版本号
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 文件夹路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 下载的临时文件名
        /// </summary>
        public string TempName { get; set; }

        /// <summary>
        /// 文件修改时间
        /// </summary>
        public DateTime ModifiedTime { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public DllStatus Status { get; set; }

        /// <summary>
        /// 需要更新的二进制文件
        /// </summary>
        public byte[] Bytes { get; set; }

    }
}
