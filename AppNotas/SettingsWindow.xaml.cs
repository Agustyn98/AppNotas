using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace AppNotas
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            comboBoxFontSizeTitle.SelectedIndex = Properties.Settings.Default.titleFontSize - 10;
            comboBoxFontSizeNote.SelectedIndex = Properties.Settings.Default.noteFontSize - 10;
            textBoxFileName.Text = Properties.Settings.Default.fileNameLenght.ToString();
            textBoxHeight.Text = Properties.Settings.Default.windowDefaultHeight.ToString();
            textBoxWidth.Text = Properties.Settings.Default.windowDefaultWidth.ToString();
            comboBoxSearch.SelectedIndex = Properties.Settings.Default.searchCase;
            getFontFamily();
            checkBoxTabAccept.IsChecked = Properties.Settings.Default.acceptTab;
            checkBoxSavePrompt.IsChecked = Properties.Settings.Default.savePrompt;
            checkBoxNicoLogIn.IsChecked = Properties.Settings.Default.nicoLogIn;
            checkBoxWindowsLocation.IsChecked = Properties.Settings.Default.saveWindowsCoords;
            checkBoxBackupLocation.IsChecked = Properties.Settings.Default.backupDefaultLocation;
            checkBoxScrollVisible.IsChecked = Properties.Settings.Default.scrollVisible;
        }

        private void getFontFamily()
        {
            switch (Properties.Settings.Default.defaultFontFamily)
            {
                case "Arial":
                    comboBoxFontFamily.SelectedIndex = 0;
                    break;
                case "Verdana":
                    comboBoxFontFamily.SelectedIndex = 1;
                    break;
                case "Segoe Print":
                    comboBoxFontFamily.SelectedIndex = 2;
                    break;
                case "Comic Sans MS":
                    comboBoxFontFamily.SelectedIndex = 3;
                    break;
                case "Source Code Pro":
                    comboBoxFontFamily.SelectedIndex = 4;
                    break;
                case "Castellar":
                    comboBoxFontFamily.SelectedIndex = 5;
                    break;
                case "Times New Roman":
                    comboBoxFontFamily.SelectedIndex = 6;
                    break;
            }
        }

        private void setFontFamily()
        {
            switch (comboBoxFontFamily.SelectedIndex)
            {
                case 0:
                    Properties.Settings.Default.defaultFontFamily = "Arial";
                    break;
                case 1:
                    Properties.Settings.Default.defaultFontFamily = "Verdana";
                    break;
                case 2:
                    Properties.Settings.Default.defaultFontFamily = "Segoe Print";
                    break;
                case 3:
                    Properties.Settings.Default.defaultFontFamily = "Comic Sans MS";
                    break;
                case 4:
                    Properties.Settings.Default.defaultFontFamily = "Source Code Pro";
                    break;
                case 5:
                    Properties.Settings.Default.defaultFontFamily = "Castellar";
                    break;
                case 6:
                    Properties.Settings.Default.defaultFontFamily = "Times New Roman";
                    break;
            }
        }

        private Window creatingForm;
        public Window setCreatingForm
        {
            get { return creatingForm; }
            set { creatingForm = value; }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (comboBoxFontSizeNote.SelectedIndex <= -1 || comboBoxFontSizeTitle.SelectedIndex <= -1)
            {
                MessageBox.Show("Selected value cannot be null");
                return;
            }
            Properties.Settings.Default.titleFontSize = comboBoxFontSizeTitle.SelectedIndex + 10;
            Properties.Settings.Default.noteFontSize = comboBoxFontSizeNote.SelectedIndex + 10;
            setFontFamily();

            int fileNameLenght = int.Parse(textBoxFileName.Text);
            if (fileNameLenght < 1 || fileNameLenght > 200)
            {
                MessageBox.Show("File name lenght invalid");
                return;
            }
            Properties.Settings.Default.fileNameLenght = int.Parse(textBoxFileName.Text);

            double height = int.Parse(textBoxHeight.Text);
            double width = int.Parse(textBoxWidth.Text);

            double screenHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
            double screenWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
            if (height < 50 || height > screenHeight || width < 50 || width > screenWidth)
            {
                MessageBox.Show("Invalid height/width");
                return;
            }
            Properties.Settings.Default.windowDefaultHeight = height;
            Properties.Settings.Default.windowDefaultWidth = width;

            Properties.Settings.Default.searchCase = comboBoxSearch.SelectedIndex;

            Properties.Settings.Default.acceptTab = (bool)checkBoxTabAccept.IsChecked;

            Properties.Settings.Default.savePrompt = (bool)checkBoxSavePrompt.IsChecked;

            Properties.Settings.Default.nicoLogIn = (bool)checkBoxNicoLogIn.IsChecked;

            Properties.Settings.Default.saveWindowsCoords = (bool)checkBoxWindowsLocation.IsChecked;

            Properties.Settings.Default.backupDefaultLocation = (bool)checkBoxBackupLocation.IsChecked;

            Properties.Settings.Default.scrollVisible = (bool)checkBoxScrollVisible.IsChecked;

            Properties.Settings.Default.Save();

            reloadMainWindow();
        }

        private void reloadMainWindow()
        {
            MainWindow mw = new MainWindow();
            if (creatingForm != null)
            {
                creatingForm.Close();
                this.Close();
                mw.Show();

            }
        }

        private void NumberValidationTextBox(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void WindowSettings_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    btnSave_Click(sender, e);
                }
            }
        }

        private void btnCompactDB_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long oldLength = new System.IO.FileInfo("NotesDB.db").Length;
                Note.vacuumDB();
                long newLenght = new System.IO.FileInfo("NotesDB.db").Length;
                long spaceSaved = oldLength - newLenght;
                spaceSaved = spaceSaved / 1024; //covert from Bytes to KB
                MessageBox.Show(spaceSaved + " KB recovered !" , "Database" ,  MessageBoxButton.OK , MessageBoxImage.Information );

            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message , "" , MessageBoxButton.OK , MessageBoxImage.Error);
            }
        }
    }
}
