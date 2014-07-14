using AutoUpdate.Demo.ServiceReference;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AutoUpdate.Demo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            _info = InitialDllInfo(Assembly.GetExecutingAssembly());
        }

        readonly AutoUpdateServiceSoapClient _client = new AutoUpdateServiceSoapClient();
        private DllInfo _info;

        private void btnCheckVersion_Click(object sender, EventArgs e)
        {
            try
            {
                _info = _client.CheckVersion(_info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("检查更新失败:" + ex.Message);
                return;
            }
            if (_info.Status == DllStatus.NeedUpdate)
            {
                MessageBox.Show("需要更新");
            }
        }

        private static DllInfo InitialDllInfo(Assembly assembly)
        {
            DllInfo info = new DllInfo
            {
                FileName = assembly.ManifestModule.Name,
                FilePath = Directory.GetCurrentDirectory(),
                TempName = "Update.exe",
                Version = assembly.GetName().Version.ToString()
            };
            return info;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                _info = _client.Update(_info);
            }
            catch (Exception ex)
            {
                MessageBox.Show("下载更新失败：" + ex.Message);
                return;
            }

            File.WriteAllBytes(_info.FilePath + _info.TempName, _info.Bytes);
            MessageBox.Show("更新完成,重启后重新安装");
            if (File.Exists(_info.FilePath + _info.TempName))
            {
                Process.Start(_info.FilePath + _info.TempName);
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
