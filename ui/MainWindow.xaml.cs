using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using wtfmc;
using wtfmc.Config;

namespace ui
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 需要用到的配置数据（因为仍在研发，目前无用）
        /// </summary>
        private ConfigRoot config;
        private ILoginClient currentUser;
        ILoginClient CurrentUser
        {
            get => currentUser;
            set { currentUser = value; SetUpLoginData(CurrentUser); }
        }
        public MainWindow()
        {
            InitializeComponent();
        }
        private void way_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (way.SelectedIndex == 0)
            {
                access_show.Content = "用户名";
                password_show.Visibility = Visibility.Collapsed;
                password.Visibility = Visibility.Collapsed;
                login.Visibility = Visibility.Collapsed;
            }
            else if (way.SelectedIndex == 1)
            {
                access_show.Content = "邮箱/用户名";
                password_show.Visibility = Visibility.Visible;
                password.Visibility = Visibility.Visible;
                login.Visibility = Visibility.Visible;
            }

        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void play_Click(object sender, RoutedEventArgs e)
        {
        }
        /*
        private void test_java_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string procname = jvm.Text;
                string args = "-h";
                Process p = new Process();
                p.StartInfo.FileName = procname;
                p.StartInfo.Arguments = args;
                p.Start();
            }
            catch (Exception err)
            {
                (sender as Button).Content = "测试：" + err.Message;
            }
            (sender as Button).Content = "测试：OK";
        }
        */
    }
}
