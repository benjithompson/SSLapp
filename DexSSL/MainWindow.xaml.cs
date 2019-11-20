using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DexSSL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            preloadServerConfig();
            preloadAgentConfig();
            InitializeComponent();
        }

        void preloadServerConfig()
        {
            if (File.Exists(@"C:\Program Files(x86)\TRICENTIS\Tosca Server\DEXServer\web.config"))
            {
                tbServerConfig.Text = @"C:\Program Files(x86)\TRICENTIS\Tosca Server\DEXServer\web.config";
            }
        }

        void preloadAgentConfig()
        {
            if (File.Exists(@"C:\Program Files(x86)\TRICENTIS\Tosca Testsuite\Distribution Agent\ToscaDistributionAgent.exe.config"))
            {
                tbAgentConfig.Text =
                    @"C:\Program Files(x86)\TRICENTIS\Tosca Testsuite\Distribution Agent\ToscaDistributionAgent.exe.config";
            }
        }
        private void ButtonOpenServerConfig_OnClick(object sender, RoutedEventArgs e)
        {
           var path = @"C:\Program Files (x86)\TRICENTIS\Tosca Server\DEXServer";
           OpenFileDialog dlg = new OpenFileDialog { InitialDirectory = (Directory.Exists(path)) ? path : @"C:\" };

            if (dlg.ShowDialog() != DialogResult)
            {
                var filename = dlg.FileName;
                tbServerConfig.Text = filename;
            }
        }

        private void ButtonOpenAgentConfig_OnClick(object sender, RoutedEventArgs e)
        {
            var path = @"C:\Program Files(x86)\TRICENTIS\Tosca Server\DEXServer";
            OpenFileDialog dlg = new OpenFileDialog { InitialDirectory = (Directory.Exists(path)) ? path : @"C:\Program Files (x86)\TRICENTIS" };

            if (dlg.ShowDialog() != DialogResult)
            {
                var filename = dlg.FileName;
                tbAgentConfig.Text = filename;
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
