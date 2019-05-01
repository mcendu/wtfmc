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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using wtfmc;
using wtfmc.Config;

namespace ui
{
    /*
     * MainWindow 要用到的一些杂七杂八的东西。
     */
    public partial class MainWindow : Window
    {
        internal void InitializeConfig(string path)
        {
            try
            {
                // Attempt to read from config file.
                JObject configData = JObject.Load(new JsonTextReader(new StreamReader(File.Open(path, FileMode.Open))));
                config = (ConfigRoot)configData;
            } catch (FileNotFoundException)
            {
                // Generate a new config using latest release.
                config = new ConfigRoot();
                config.Profiles.Add("latestRelease", new Profile {
                    
                });
            }
        }

        internal void InitializeConfig() => InitializeConfig(Directory.GetCurrentDirectory());

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
                way.SelectedIndex = 1;
            }
            else if (login.LoginType == LoginType.Offline)
            {
                way.IsEnabled = true;
                access.Focusable = true;
                way.SelectedIndex = 0;
            }
            ShowPassword();
        }

        private Exception error;
        public Exception Error
        {
            get => error;
            set
            {
                error = value;
                if (value == null)
                {
                    errorborder.Visibility = Visibility.Collapsed;
                    errorbox.Text = null;
                }
                else
                {
                    errorborder.Visibility = Visibility.Visible;
                    errorbox.Text = $"{value.Message}";
                }
            }
        }

        /// <summary>
        /// Write configuration to a path.
        /// </summary>
        /// <param name="path">The path to write the configuration to.</param>
        public void WriteConfig(string path)
        {
            File.Create(path);
            File.WriteAllText(path, config.ToString());
        }

        /// <summary>
        /// Write configuration to the current directory.
        /// </summary>
        public void WriteConfig() => WriteConfig(Directory.GetCurrentDirectory());
    }
}
