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

namespace ui
{
    /*
     * MainWindow 要用到的一些杂七杂八的东西。
     */
    public partial class MainWindow : Window
    {
        private void ShowPassword()
        {
            if (way.SelectedIndex == 0) // 离线登录
            {
                password.Visibility = Visibility.Collapsed;
            }
            else if (CurrentUser == null) // 没有登录
            {
                password.Visibility = Visibility.Visible;
            }
            else
            {
                password.Visibility = Visibility.Collapsed;
            }
        }

        private void SetUpLoginData(ILoginClient login)
        {
            if (login == null)
            {
                way.IsEnabled = true;
                access.Text = "";
                access.Focusable = true;
            }
            access.Text = login.Username;
            if (login.LoginType == LoginType.Mojang)
            {
                way.IsEnabled = false;
                access.Focusable = false;
            }
            else if (login.LoginType == LoginType.Offline)
            {
                way.IsEnabled = true;
                access.Focusable = true;
            }
            ShowPassword();
        }
    }
}
