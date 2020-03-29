using System.Windows;
using SSLapp.ViewModels;
using System.Diagnostics;

namespace SSLapp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new ToscaConfigFilesViewModel();
            Trace.WriteLine("App Started.");
        }
    }
}
