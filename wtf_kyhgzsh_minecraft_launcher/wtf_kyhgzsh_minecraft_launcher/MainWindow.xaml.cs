using System;
using System.Collections.Generic;
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

namespace wtf_kyhgzsh_minecraft_launcher
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            run_version_setting.
        }

        private void way_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (way.SelectedIndex==0)
            {
                access_show.Content = "用户名:";
                password.IsEnabled = false;
            }
            if (way.SelectedIndex == 1)
            {
                access_show.Content = "账号:";
                password.IsEnabled = true;
            }
        }
    }
}
