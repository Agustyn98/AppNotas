using System;
using System.Media;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace AppNotas
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            setSettings();
            passBox.Focus();
 
        }

        private void setSettings()
        {
            if (!Properties.Settings.Default.nicoLogIn)
            {
                imageNico1.Opacity = 0; imageNico2.Opacity = 0;
                btnLogIn.Background = Brushes.WhiteSmoke;
                btnLogIn.Foreground = Brushes.Black;
                lblLog.Foreground = Brushes.DarkRed;
            }
        }
        private bool flagRestart = false;
        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            passwordEntered();

            if (flagRestart)
            {
                // System.Diagnostics.Process.Start(@"C:\Windows\System32\Notepad.exe");
                // System.Diagnostics.Process.Start("AppNotas.exe");
                this.Close();
            }
        }

        private void passwordEntered()
        {
            //string EnteredPassword = passBox.Password;
            ReUse use = new ReUse();

            if (use.calculateX(passBox.Password))
            {
                //MessageBox.Show("Welcome");
                MainWindow main = new MainWindow();
                this.Close();
                main.Show();

            }
            else
            {
                System.Media.SystemSounds.Beep.Play();
                lblLog.Content = "Incorrect password  " + ReUse.numberOfTries + "/3";
                passBox.Password = string.Empty;
                if (ReUse.numberOfTries >= 3)
                {
                    passBox.IsEnabled = false;
                    if (Properties.Settings.Default.nicoLogIn)
                        btnLogIn.Content = "BAKA";
                    else
                        btnLogIn.Content = "Close the program";
                    btnLogIn.Foreground = Brushes.Red;
                    flagRestart = true;
                }
            }
        }

        private void passBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (passBox.Password.Length >= 5)
                passwordEntered();
        }

        private void imageNico2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.nicoLogIn)
            {
                SoundPlayer s = new SoundPlayer(Properties.Resources.niconiconi_short);
                s.Load(); s.Play();
            }
        }

        private void imageNico1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Properties.Settings.Default.nicoLogIn)
            {
                SoundPlayer s = new SoundPlayer(Properties.Resources.niconiconi_short);
                s.Load(); s.Play();
            }
        }

    }
}
