using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace AppNotas
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            setSettings();
        }

        public void setSettings()
        {
            textBoxNote.FontSize = Properties.Settings.Default.noteFontSize;
            textBoxTitle.FontSize = Properties.Settings.Default.titleFontSize;
            textBoxNote.AcceptsTab = Properties.Settings.Default.acceptTab;
            textBoxNote.FontFamily = new System.Windows.Media.FontFamily(Properties.Settings.Default.defaultFontFamily);

            if (Properties.Settings.Default.saveWindowsCoords)
            {
                this.Height = Properties.Settings.Default.windowHeight;
                this.Width = Properties.Settings.Default.windowWidth;
            }
            else
            {
                this.Height = Properties.Settings.Default.windowDefaultHeight;
                this.Width = Properties.Settings.Default.windowDefaultWidth;
            }

            if (Properties.Settings.Default.scrollVisible)
            {
                textBoxNote.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
                textBoxTitle.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Visible;
            }
            else
            {
                textBoxNote.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
                textBoxTitle.VerticalScrollBarVisibility = System.Windows.Controls.ScrollBarVisibility.Hidden;
            }

        }

        private void listBoxNotes_Loaded(object sender, RoutedEventArgs e)
        {

            populateListBoxNotesFirstTime();
            listBoxNotes.Focus();
        }

        //    private ObservableCollection<Note> mynotes = new ObservableCollection<Note>();
        private bool isSelectedNotePinned = true;
        private bool unsavedChanges = false;
        private bool dontChageSelectionAgain = false;
        private void populateListBoxNotesFirstTime()
        {
            /* try
             {
                 numberOfNotes = Note.getNumberOfNotes();
             }
             catch (Exception ex)
             {
                 MessageBox.Show("Error: " + ex.Message);
             }
             numberOfNotes--;
             for (int i = numberOfNotes; i >= 0; i--)
             {
                 mynotes.Add(new Note(i));
                 //  listBoxNotes.Items.Add(new Note(i));
             }
            */

            //  mynotes = Note.getAllNotes();

            Note.getAllNotes();
            listBoxNotes.ItemsSource = Note.mynotes;
            int numberOfNotes = Note.mynotes.Count - 1;
            int numberOfPins = 0;
            for (int i = 0; i <= numberOfNotes; i++)
            {
                if (Note.mynotes[i].Pin == 1)
                {
                    Note.mynotes.Move(i, numberOfPins);
                    numberOfPins++;
                }
            }
            listBoxNotes.SelectedIndex = 0;

            /*
            listBoxNotes.UpdateLayout();
            var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes
                .ItemContainerGenerator
                .ContainerFromItem(listBoxNotes.SelectedItem);
            listBoxItem.Focus();
            listBoxItem.IsSelected = true;*/


            clicksInCheckBoxPin = 0;
            setLabelDates();

        }
        private void populateListBoxNotesNewNote(bool delete)
        {
            dontChageSelectionAgain = true;
            Note.getAllNotes();
            listBoxNotes.ItemsSource = Note.mynotes;
            int numberOfNotes = Note.mynotes.Count - 1;

            int numberOfPins = 0;
            for (int i = 0; i <= numberOfNotes; i++)
            {
                if (Note.mynotes[i].Pin == 1)
                {
                    Note.mynotes.Move(i, numberOfPins);
                    numberOfPins++;
                }
            }

            if (!delete)
                listBoxNotes.SelectedIndex = numberOfPins;
            else
                listBoxNotes.SelectedIndex = 0;

            // listBoxNotes.ItemsSource = Note.mynotes;
            /*
            if (isSelectedNotePinned)
                listBoxNotes.SelectedIndex = 0;
            else
            {
                listBoxNotes.SelectedIndex = numberOfPins;

                listBoxNotes.UpdateLayout();
                var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes
                    .ItemContainerGenerator
                    .ContainerFromItem(listBoxNotes.SelectedItem);
                listBoxItem.IsSelected = true;
            }*/
            listBoxNotes.UpdateLayout();
            var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes
                .ItemContainerGenerator
                .ContainerFromItem(listBoxNotes.SelectedItem);
            if (!(listBoxItem is null))
                listBoxItem.Focus();


            clicksInCheckBoxPin = 0;
            // setLabelDates();

        }
        private void populateListBoxNotes()
        {

            Note.getAllNotes();
            listBoxNotes.ItemsSource = Note.mynotes;
            int numberOfNotes = Note.mynotes.Count - 1;

            int numberOfPins = 0;
            for (int i = 0; i <= numberOfNotes; i++)
            {
                if (Note.mynotes[i].Pin == 1)
                {
                    Note.mynotes.Move(i, numberOfPins);
                    numberOfPins++;
                }
            }

            dontChageSelectionAgain = true;
            //listBoxNotes.ItemsSource = Note.mynotes;
            // dontChageSelectionAgain = true;
            //listBoxNotes.DisplayMemberPath = "Title";
            if (isSelectedNotePinned)
            {
                listBoxNotes.SelectedIndex = 0;
                if (listBoxNotes.IsFocused)
                {
                    listBoxNotes.UpdateLayout();
                    var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes
                        .ItemContainerGenerator
                        .ContainerFromItem(listBoxNotes.SelectedItem);
                    listBoxItem.Focus();
                }
            }
            else
            {
                listBoxNotes.SelectedIndex = numberOfPins;
                /*Esto resuelve el bug de que el focus se va al primer item de la lista, entonces al precionar keydown
                * seleccionaba el primer listboxitem en lugar del que corresponderia
                */
                if (listBoxNotes.IsFocused)
                {
                    listBoxNotes.UpdateLayout();
                    var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes
                        .ItemContainerGenerator
                        .ContainerFromItem(listBoxNotes.SelectedItem);
                    listBoxItem.Focus();
                }
            }
            selectedNote = (Note)listBoxNotes.SelectedItem;

            if (selectedNote is Note)
            {
                fillTextLabelCharacters();
                fillTextLabelCharactersText();
                clicksInCheckBoxPin = 0;
                setLabelDates();
                checkBoxE1.IsChecked = selectedNote.isDone == 1 ? true : false;
                checkBoxPin.IsChecked = selectedNote.Pin == 1 ? true : false;
                changeCheckBoxE1();
                changeCheckBoxPin();
            }

        }

        private ObservableCollection<Note> searchedNotes = new ObservableCollection<Note>();

        private void populateListboxBySearch(string search)
        {
            int numberOfNotes = listBoxNotes.Items.Count;
            searchedNotes.Clear();
            string realText = string.Empty;

            for (int i = 0; i < numberOfNotes; i++)
            {
                var note1 = (Note)listBoxNotes.Items[i];

                if (note1.Text.Length > 0)
                {
                    string rtfText = note1.Text; //string from db
                    byte[] byteArray = Encoding.UTF8.GetBytes(rtfText);
                    using (MemoryStream ms = new MemoryStream(byteArray))
                    {
                        TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                        tr.Load(ms, System.Windows.DataFormats.Xaml);
                        realText = tr.Text;
                    }
                }

                if (realText.IndexOf(search, selectCase()) >= 0 || note1.Title.IndexOf(search, selectCase()) >= 0)
                {
                    searchedNotes.Add(note1);
                }
            }
            // listBoxNotes.ItemsSource = null;
            if (searchedNotes.Count <= 0)
            {
                //clearTextBoxes();
                // listBoxNotes.ItemsSource = null;
                /*   listBoxNotes.ItemsSource = Note.mynotes;
                   dontChageSelectionAgain = true;
                   listBoxNotes.SelectedIndex = 0; */
                dontChageSelectionAgain = true;
                populateListBoxNotesNewNote(true);
                MessageBox.Show("No results", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                listBoxNotes.ItemsSource = searchedNotes;
                listBoxNotes.SelectedIndex = -1;
                listBoxNotes.SelectedIndex = 0;
                /*  string s = selectedNote.Text;
                  s = s.Substring(600);
                  MessageBox.Show(s);*/
            }

        }

        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");

        public void setLabelDates()
        {
            if (selectedNote is Note)
            {
                //  DateTime createdDate = Convert.ToDateTime(selectedNote.CreationDate, culture);
                //  DateTime lastModifiedDate = Convert.ToDateTime(selectedNote.LastModifiedDate, culture);

                textBlockCreatedDate.Text = "Created:  " + Convert.ToDateTime(selectedNote.CreationDate, culture).ToString("dd/MM/yyyy HH:mm");
                textBlockModifiedDate.Text = "Modified: " + Convert.ToDateTime(selectedNote.LastModifiedDate, culture).ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                textBlockCreatedDate.Text = string.Empty;
                textBlockModifiedDate.Text = string.Empty;
            }
        }
        //  private bool flagFirstSelectionChanged = true;
        private int selectedIndex = 0;
        private Note selectedNote;
        private void listBoxNotes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (dontChageSelectionAgain)
            {
                dontChageSelectionAgain = false;
                return;
            }


            //  MessageBox.Show("Selection changed");
            //   try
            //   {
            
            if (unsavedChanges && Properties.Settings.Default.savePrompt) //&& listBoxNotes.SelectedIndex >= 0
            {

                MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    saveNote();
                }
            }


            selectedNote = (Note)listBoxNotes.SelectedItem;
            if (selectedNote is Note)
            {
                if (selectedNote.Text.Length > 0)
                {
                    //    string rtfText = selectedNote.Text; //string from db
                    //   byte[] byteArray = Encoding.UTF8.GetBytes(rtfText);
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(selectedNote.Text)))
                    {
                        TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                        tr.Load(ms, System.Windows.DataFormats.Xaml);
                    }
                }
                else
                {
                    textBoxNote.Document.Blocks.Clear();
                }

                textBoxTitle.Document.Blocks.Clear();
                textBoxTitle.AppendText(selectedNote.Title);

                checkBoxE1.IsChecked = selectedNote.isDone == 1 ? true : false;
                checkBoxPin.IsChecked = selectedNote.Pin == 1 ? true : false;
                changeCheckBoxE1();
                changeCheckBoxPin();
                setLabelDates();
                fillTextLabelCharacters();
                fillTextLabelCharactersText();
                clearUndo();
                unsavedChanges = false;

                ListBoxItemUnsavedChangesOffWithIndex();
                selectedIndex = listBoxNotes.SelectedIndex;

            }
            /*   }
               catch (Exception ex)
               {
                   MessageBox.Show("error in selectionchanged: " + ex.Message);
               } */

        }

        private void ListBoxItemUnsavedChangesOffWithIndex()
        {
            // MessageBox.Show("selected index: " + selectedIndex + " count " + listBoxNotes.Items.Count);
            //  try
            //  {
            if (selectedIndex > listBoxNotes.Items.Count - 1)
                return;
            var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes.ItemContainerGenerator.ContainerFromItem(listBoxNotes.Items[selectedIndex]);
            if (listBoxItem is null)
            {
                //  MessageBox.Show("null :(");
                return;
            }
            listBoxItem.Foreground = Brushes.Black;

            /*   }
               catch (Exception ex)
               {
                   MessageBox.Show("Error " + ex.Message);
               }*/
        }
        private void ListBoxItemUnsavedChangesOff()
        {

            var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes.ItemContainerGenerator.ContainerFromItem(listBoxNotes.SelectedItem);
            if (listBoxItem is null)
            {
                //   MessageBox.Show("null :(");
                return;
            }
            listBoxItem.Foreground = Brushes.Black;
        }

        private void ListBoxItemUnsavedChangesOn()
        {
            if (unsavedChanges)
                return;
            var listBoxItem = (System.Windows.Controls.ListBoxItem)listBoxNotes.ItemContainerGenerator.ContainerFromItem(listBoxNotes.SelectedItem);
            if (listBoxItem is null)
                return;

            listBoxItem.Foreground = Brushes.Red;
            unsavedChanges = true;

        }

        private void clearUndo()
        {
            textBoxNote.IsUndoEnabled = false;
            textBoxNote.IsUndoEnabled = true;
            textBoxTitle.IsUndoEnabled = false;
            textBoxTitle.IsUndoEnabled = true;
        }

        private void clearTextBoxes()
        {
            textBoxNote.Document.Blocks.Clear();
            textBoxTitle.Document.Blocks.Clear();
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.savePrompt && unsavedChanges) //&& listBoxNotes.SelectedIndex >= 0
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    dontChageSelectionAgain = true;
                    // MessageBox.Show("saved");
                    saveNote();
                }
                else if (result == MessageBoxResult.Cancel)
                    return;
                else if (result == MessageBoxResult.No)
                    unsavedChanges = false;
            }
            if (listBoxNotes.Items.Count <= 0)
            {
                addNewNote();
                dontChageSelectionAgain = true;
                listBoxNotes.SelectedIndex = -1;
                listBoxNotes.SelectedIndex = 0;
                return;
            }
            addNewNote();
        }

        private void addNewNote()
        {
            //  MessageBox.Show("create new note");
            new Note("New Note", string.Empty);
            isSelectedNotePinned = false;
            populateListBoxNotesNewNote(false);
            ListBoxItemUnsavedChangesOff();
        }

        public void btnImportRTF_Click(object sender, RoutedEventArgs e)
        {

            if (Properties.Settings.Default.savePrompt && unsavedChanges) //&& listBoxNotes.SelectedIndex >= 0
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    saveNote();
                }
                else if (result == MessageBoxResult.Cancel)
                    return;
                else
                {
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                }
            }
            string path = string.Empty;
            string text = string.Empty;
            var fileContent = string.Empty;
            string filePath = string.Empty;
            string filename = string.Empty;
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.rtf)|*.rtf";
                openFileDialog.FilterIndex = 2;
                openFileDialog.Title = "Select a .rtf file to import";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    filename = Path.GetFileName(filePath);
                    filename = removeExtensionFromFilename(filename);
                    //Read the contents of the file into a stream
                    /*
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }*/


                    Note txtNote = new Note(filename, "");
                    //isSelectedNotePinned = false;
                    clearTextBoxes();
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                    populateListBoxNotesNewNote(false);
                    string rtf = File.ReadAllText(filePath);
                    using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(rtf)))
                    {
                        // textBoxNote.Selection.Load(stream, DataFormats.Rtf);
                        TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                        tr.Load(stream, System.Windows.DataFormats.Rtf);
                    }
                    saveNote();

                }
            }
        }



        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.savePrompt && unsavedChanges) //&& listBoxNotes.SelectedIndex >= 0
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    saveNote();
                }
                else if (result == MessageBoxResult.Cancel)
                    return;
                else
                {
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                }
            }
            string path = string.Empty;
            string text = string.Empty;
            var fileContent = string.Empty;
            string filePath = string.Empty;
            string filename = string.Empty;
            using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 2;
                openFileDialog.Title = "Select a .txt file to import";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    filename = Path.GetFileName(filePath);
                    filename = removeExtensionFromFilename(filename);
                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        fileContent = reader.ReadToEnd();
                    }
                    Note txtNote = new Note(filename, "");
                    //isSelectedNotePinned = false;
                    clearTextBoxes();
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                    populateListBoxNotesNewNote(false);
                    textBoxNote.AppendText(fileContent);
                    saveNote();

                }
            }
        }


        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.PrintDialog pd = new System.Windows.Controls.PrintDialog();
            //pd.DefaultPageSettings.Margins = margins;

            if (pd.ShowDialog() == true)
            {
                try
                {
                    DocumentPaginator doc = ((IDocumentPaginatorSource)textBoxNote.Document).DocumentPaginator;

                    doc.PageSize = new Size(750, 1080);
                    pd.PrintDocument(doc, "print");

                    //pd.PrintDocument(((IDocumentPaginatorSource)textBoxNote.Document).DocumentPaginator, "printing as paginator");
                    MainWindow1.Height++;
                    MainWindow1.Height--;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnBackup_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.backupDefaultLocation)
                {
                    string destination = @"C:\AppNotas";
                    string destFile = System.IO.Path.Combine(destination, "NotesDB.db");
                    System.IO.Directory.CreateDirectory(destination);
                    System.IO.File.Copy("NotesDB.db", destFile, true);
                    string mbMessage = @"Database backed up at: C:\AppNotas\NotesDB.db";
                    MessageBox.Show(mbMessage, "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    string path = string.Empty;
                    var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
                    if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        path = folderDialog.SelectedPath;
                    }
                    if (path == string.Empty)
                        return;
                    string destination = path;
                    string destFile = System.IO.Path.Combine(destination, "NotasDB.db");
                    System.IO.Directory.CreateDirectory(destination);
                    System.IO.File.Copy("NotesDB.db", destFile, true);
                    string mbMessage = @"Database backed up at: " + path;
                    MessageBox.Show(mbMessage, "Backup", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        /*    private void btnOnScreenKeyboard_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                 string path32 = @"C:\Windows\System32\osk.exe";
                  Process.Start("C:\\Windows\\System32\\");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("error: " + ex.Message);
                }
                //   System.Diagnostics.Process.Start("osk.exe");

            }*/
        public string removeExtensionFromFilename(string filename)
        {
            filename = filename.Substring(0, filename.Length - 4);
            return filename;
        }

        private void btnExportRtf_Click(object sender, RoutedEventArgs e)
        {
            int maxLenght = Properties.Settings.Default.fileNameLenght;
            string path = string.Empty;
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = folderDialog.SelectedPath;
            }
            if (path == string.Empty)
                return;
            try
            {
                //int maxLenght = Properties.Settings.Default.
                string title = new TextRange(textBoxTitle.Document.ContentStart, textBoxTitle.Document.ContentEnd).Text.TrimEnd('\r', '\n');
                if (title.Length > maxLenght)
                {
                    title = title.Substring(0, maxLenght);
                }
                title = removeForbiddenChars(title);

                string rtfText;
                TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                using (MemoryStream ms = new MemoryStream())
                {
                    tr.Save(ms, System.Windows.DataFormats.Rtf);
                    rtfText = Encoding.UTF8.GetString(ms.ToArray());

                }
                System.IO.File.WriteAllText(path + "\\" + title + ".rtf", rtfText);
                MessageBox.Show("File exported succesfully", "File Exported", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            int maxLenght = Properties.Settings.Default.fileNameLenght;
            string path = string.Empty;
            var folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = folderDialog.SelectedPath;
            }
            if (path == string.Empty)
                return;
            try
            {
                //int maxLenght = Properties.Settings.Default.
                string title = new TextRange(textBoxTitle.Document.ContentStart, textBoxTitle.Document.ContentEnd).Text.TrimEnd('\r', '\n');
                if (title.Length > maxLenght)
                {
                    title = title.Substring(0, maxLenght);
                }
                title = removeForbiddenChars(title);
                string text = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd).Text.TrimEnd('\r', '\n');
                System.IO.File.WriteAllText(path + "\\" + title + ".txt", text);
                MessageBox.Show("File exported succesfully", "File Exported", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string removeForbiddenChars(string text)
        {
            string newText = string.Empty;
            int lenght = text.Length;
            for (int i = 0; i < lenght; i++)
            {

                if (!(text[i] == '\\' || text[i] == ':' || text[i] == '/' || text[i] == '*' || text[i] == '?' || text[i] == '"' || text[i] == '>' || text[i] == '<' || text[i] == '|'))
                {
                    // MessageBox.Show("L:");
                    newText = String.Concat(newText, text[i]);
                }

            }
            return newText;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveNote();
        }
        private void saveNote()
        {
            /*
            int cantChar = 0;
            int pointerCurrentPosition = 0;
            bool isTitleFocused = false; bool isNoteFocused = false;
            if (textBoxNote.IsFocused)
            {
                pointerCurrentPosition = textBoxNote.Document.ContentStart.GetOffsetToPosition(textBoxNote.Selection.End);
                isNoteFocused = true;
            }
            else if (textBoxTitle.IsFocused)
            {
                cantChar = new TextRange(textBoxTitle.Document.ContentStart, textBoxTitle.Selection.End).Text.Length + 2;
                // pointerCurrentPosition = textBoxTitle.Document.ContentStart.GetOffsetToPosition(textBoxTitle.Selection.End);
                isTitleFocused = true;
            }
            listBoxNotes.Focus();*/

            // MessageBox.Show("saveNote caleld");

            if (flagSearchButton)
            {
                TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.White));
                flagSearchButton = false;
            }
            string title = new TextRange(textBoxTitle.Document.ContentStart, textBoxTitle.Document.ContentEnd).Text.TrimEnd('\r', '\n');
            title = trimString(title).ToString();
            if (title.Length <= 100)
            {
                if (!(selectedNote is null))
                {
                    string rtfText;
                    TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        tr.Save(ms, System.Windows.DataFormats.Xaml);
                        // rtfText = Encoding.ASCII.GetString(ms.ToArray());
                        rtfText = Encoding.UTF8.GetString(ms.ToArray());

                    }

                    //string text = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd).Text.TrimEnd('\r', '\n');
                    selectedNote.update(title, rtfText, (bool)checkBoxE1.IsChecked ? 1 : 0, (bool)checkBoxPin.IsChecked ? 1 : 0);

                    if (selectedNote.Pin == 1)
                        isSelectedNotePinned = true;
                    else
                        isSelectedNotePinned = false;
                    if (pinChanged)
                    {
                        if (isSelectedNotePinned == false)
                            isSelectedNotePinned = true;
                        else
                            isSelectedNotePinned = false;
                        // isSelectedNotePinned ^= isSelectedNotePinned;
                        pinChanged = false;
                    }


                    dontChageSelectionAgain = true;
                    populateListBoxNotes();
                    unsavedChanges = false;
                    selectedIndex = listBoxNotes.SelectedIndex;
                    ListBoxItemUnsavedChangesOff();

                }
                else
                {
                    System.Windows.MessageBox.Show("You must create a note first", "", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Title too long", "", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

            /*
            if (isNoteFocused)
                textBoxNote.Focus();
            else if (isTitleFocused)
                textBoxTitle.Focus();

            if (textBoxNote.IsFocused)
            {
                try
                {
                    //textBoxNote.Focus();
                  //  textBoxNote.CaretPosition = textBoxNote.Document.ContentStart.GetPositionAtOffset(pointerCurrentPosition, LogicalDirection.Forward);
                    // System.Diagnostics.Trace.WriteLine("Pos      : " + pointerCurrentPosition);
                }
                catch (Exception ex) when (ex.InnerException is IndexOutOfRangeException)
                {
                    MessageBox.Show("Error repositioning carret: " + ex.Message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("error: " + ex.Message);
                }
            }
            else if (textBoxTitle.IsFocused)
            {
                try
                {
                    //textBoxNote.Focus();
                    textBoxTitle.CaretPosition = textBoxTitle.Document.ContentStart.GetPositionAtOffset(cantChar, LogicalDirection.Forward);
                    // System.Diagnostics.Trace.WriteLine("Pos      : " + pointerCurrentPosition);
                }
                catch (Exception ex) when (ex.InnerException is IndexOutOfRangeException)
                {
                    MessageBox.Show("Error repositioning carret: " + ex.Message);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.WriteLine("error: " + ex.Message);
                }
            }*/

        }

        /*
        private int countCharactersTillCarret()
        {
            string text = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Selection.End).Text;
            int textLenght = text.Length;
            int counter = 3;

            for (int i = 0; i < textLenght; i++)
            {
                try
                {
                    if (text[i] == 10 && text[i + 2] != 10)
                    {
                        counter += 4;
                    }
                }
                catch (Exception ex)
                {
                    counter += 2;
                }
            }
            int total = textLenght + counter;
            return total;
        }
        */

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(selectedNote is null))
            {

                string trimmedString = selectedNote.Title;
                if (selectedNote.Title.Length >= 25)
                {
                    trimmedString = trimmedString.Substring(0, 25);
                    trimmedString = string.Concat(trimmedString, "...");
                }

                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Delete '" + trimmedString + "' ?", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    /* if(listBoxNotes.Items.Count == 1)
                     {
                         new Note("New Note","");
                     }*/
                    unsavedChanges = false;
                    selectedNote.delete();
                    populateListBoxNotesNewNote(true);
                    ListBoxItemUnsavedChangesOff();

                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            clearTextBoxes();
        }
        private bool spellCheckFlag = false;
        private void btnSpellcheck_Click(object sender, RoutedEventArgs e)
        {
            if (!spellCheckFlag)
            {
                textBoxNote.SpellCheck.IsEnabled = true;
                textBoxTitle.SpellCheck.IsEnabled = true;
                btnSpellCheck.Foreground = System.Windows.Media.Brushes.Red;
                spellCheckFlag = true;
            }
            else
            {
                textBoxNote.SpellCheck.IsEnabled = false;
                textBoxTitle.SpellCheck.IsEnabled = false;
                btnSpellCheck.Foreground = System.Windows.Media.Brushes.Black;
                spellCheckFlag = false;
            }
        }

        private void fillTextLabelCharacters()
        {
            int numberOfCharacters = new TextRange(textBoxTitle.Document.ContentStart, textBoxTitle.Document.ContentEnd).Text.Trim('\r', '\n', '\t').Length;
            if (numberOfCharacters > 100)
            {
                textBlockCountCharactersTitle.Text = numberOfCharacters.ToString();
                textBlockCountCharactersTitle.Foreground = System.Windows.Media.Brushes.Red;
                textBlockCountCharactersTitle.Background = Brushes.White;
            }
            else if (numberOfCharacters >= 90)
            {
                textBlockCountCharactersTitle.Visibility = Visibility.Visible;
                textBlockCountCharactersTitle.Foreground = System.Windows.Media.Brushes.Black;
                textBlockCountCharactersTitle.Text = numberOfCharacters.ToString();
                textBlockCountCharactersTitle.Background = Brushes.White;

            }
            else
            {
                textBlockCountCharactersTitle.Visibility = Visibility.Hidden;
                /*     textBlockCountCharactersTitle.Text = string.Empty;
                     textBlockCountCharactersTitle.Background = Brushes.Transparent;*/

            }
        }

        private void changeCheckBoxE1()
        {

            if (selectedNote.isDone == 1)
            {
                checkBoxE1.Foreground = System.Windows.Media.Brushes.DarkRed;
                checkBoxE1.FontWeight = FontWeights.Bold;
            }
            else
            {
                checkBoxE1.Foreground = System.Windows.Media.Brushes.Black;
                checkBoxE1.FontWeight = FontWeights.Normal;
            }
        }

        private void changeCheckBoxPin()
        {

            if (selectedNote.Pin == 1)//selectedNote is Note &&
            {
                checkBoxPin.Foreground = System.Windows.Media.Brushes.DarkRed;
                checkBoxPin.FontWeight = FontWeights.Bold;
            }
            else
            {
                checkBoxPin.Foreground = System.Windows.Media.Brushes.Black;
                checkBoxPin.FontWeight = FontWeights.Normal;
            }
        }

        private void textBoxTitle_KeyUp(object sender, KeyEventArgs e)
        {
            fillTextLabelCharacters();
        }
        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {

            if (Properties.Settings.Default.savePrompt && unsavedChanges == true)
            {
                MessageBoxResult result = MessageBox.Show("Save changes before leaving?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        saveNote();
                        break;
                    case MessageBoxResult.No:
                        unsavedChanges = false;
                        ListBoxItemUnsavedChangesOff();
                        break;
                    case MessageBoxResult.Cancel:
                        return;

                }
            }
            else
            {
                unsavedChanges = false; ListBoxItemUnsavedChangesOff();
            }
            SettingsWindow sw = new SettingsWindow();
            sw.setCreatingForm = this;
            sw.ShowDialog();
        }

        /*
        private void btnBlueText_Click(object sender, RoutedEventArgs e)
        {
            textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Blue);
        }
        private void btnRedText_Click(object sender, RoutedEventArgs e)
        {
            textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Red);
        }
        private void btnGreenText_Click(object sender, RoutedEventArgs e)
        {
            textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Green);
        }

        private void btnPurpleText_Click(object sender, RoutedEventArgs e)
        {
            textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.BlueViolet);
        }
        private void btnBlackText_Click(object sender, RoutedEventArgs e)
        {
            textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, Brushes.Black);
        }*/

        private void textBoxNote_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (textBoxNote.IsFocused)
            {
                ListBoxItemUnsavedChangesOn();
            }
        }

        private void textBoxTitle_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (textBoxTitle.IsFocused)
            {
                ListBoxItemUnsavedChangesOn();
            }
        }

        private TextRange FindWordFromPosition(TextPointer position, string word)
        {
            var selectedCase = selectCase();
            int wordLenght = word.Length;
            while (position != null)
            {
                if (position.GetPointerContext(LogicalDirection.Forward) == TextPointerContext.Text)
                {
                    //    string textRun = position.GetTextInRun(LogicalDirection.Forward);
                    // Find the starting index of any substring that matches "word".
                    int indexInRun = position.GetTextInRun(LogicalDirection.Forward).IndexOf(word, selectedCase );
                    if (indexInRun >= 0)
                    {
                        TextPointer start = position.GetPositionAtOffset(indexInRun);
                        //   TextPointer end = start.GetPositionAtOffset(word.Length);
                        return new TextRange(start, start.GetPositionAtOffset(wordLenght));
                    }
                }
                position = position.GetNextContextPosition(LogicalDirection.Forward);
            }

            // position will be null if "word" is not found.
            return null;
        }

        private StringComparison selectCase()
        {
            if (Properties.Settings.Default.searchCase == 0)
            {
                return StringComparison.OrdinalIgnoreCase;
            }
            return StringComparison.Ordinal;
        }

        private bool flagSearchButton = false;
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //I don't really understand how this works, but I made it work somehow.
            if (flagSearchButton)
            {
                TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
                tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.White));
                lblSearchMatches.Visibility = Visibility.Hidden;
                btnSearch.Foreground = Brushes.Black;
                flagSearchButton = false;
                return;
            }
            flagSearchButton = true;
            string word = textBoxSearch.Text;
            if (string.IsNullOrWhiteSpace(word))
            {
                return;
            }
            TextRange foundRange;
            TextPointer initialPosition = textBoxNote.Document.ContentStart;
            int matchesCounter = 0;
            System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.LowLatency;
            do
            {
                foundRange = FindWordFromPosition(initialPosition, word);
                if (foundRange is null) { break; }
                foundRange.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);//new SolidColorBrush(Colors.Yellow)
                initialPosition = foundRange.End;
                matchesCounter++;
            } while (foundRange != null);

            lblSearchMatches.Visibility = Visibility.Visible;
            btnSearch.Foreground = Brushes.Red;

            if (matchesCounter == 0)
            {
                lblSearchMatches.Content = "X";
                lblSearchMatches.Foreground = System.Windows.Media.Brushes.Red;
                lblSearchMatches.Background = System.Windows.Media.Brushes.LightGray;
            }
            else
            {
                lblSearchMatches.Content = matchesCounter;
                lblSearchMatches.Foreground = System.Windows.Media.Brushes.Green;
                lblSearchMatches.Background = System.Windows.Media.Brushes.LightGray;
            }
        }

        private void textBoxSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSearch.Text == " Search in note")
            {
                textBoxSearch.Text = string.Empty;
            }
            lblSearchMatches.Content = string.Empty;
            lblSearchMatches.Background = System.Windows.Media.Brushes.Transparent;
            lblSearchMatches.Visibility = Visibility.Visible;

        }

        private void textBoxSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearch.Text))
                textBoxSearch.Text = " Search in note";
            lblSearchMatches.Visibility = Visibility.Hidden;
            TextRange tr = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd);
            tr.ApplyPropertyValue(TextElement.BackgroundProperty, new SolidColorBrush(Colors.White));
        }


        private void fillTextLabelCharactersText()
        {
            string text = new TextRange(textBoxNote.Document.ContentStart, textBoxNote.Document.ContentEnd).Text;
            textBlockCountCharactersText.Text = "Character count: " + trimString(text).Length.ToString();
        }

        private StringBuilder trimString(string text)
        {
            int lenght = text.Length;
            StringBuilder newText = new StringBuilder();
            for (int i = 0; i < lenght; i++)
            {
                if (!(text[i] == '\n' || text[i] == '\r' || text[i] == '\t'))
                {
                    newText.Append(text[i]);
                }
            }
            return newText;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (Keyboard.IsKeyDown(Key.S))
                {
                    saveNote();
                }
                else if (Keyboard.IsKeyDown(Key.D))
                {
                    if (expanderButtom.IsExpanded)
                        expanderButtom.IsExpanded = false;
                    else
                        expanderButtom.IsExpanded = true;
                }
                else if (Keyboard.IsKeyDown(Key.F))
                {
                    if (expanderSearches.IsExpanded)
                    {
                        if (!textBoxSearch.IsFocused)
                            textBoxSearch.Focus();
                        else
                        {
                            expanderSearches.IsExpanded = false;
                        }
                    }
                    else
                    {
                        expanderSearches.IsExpanded = true;
                    }
                }
                else if (Keyboard.IsKeyDown(Key.G))
                {
                    if (expanderSearches.IsExpanded)
                    {
                        if (!textBoxSearchAll.IsFocused)
                            textBoxSearchAll.Focus();
                        else
                        {
                            expanderSearches.IsExpanded = false;
                        }
                    }
                    else
                    {
                        expanderSearches.IsExpanded = true;
                    }
                }
                else if (Keyboard.IsKeyDown(Key.P))
                {
                    expanderButtom.IsExpanded = true;
                    if ((bool)checkBoxPin.IsChecked)
                    {
                        checkBoxPin.IsChecked = false;
                    }
                    else
                    {
                        checkBoxPin.IsChecked = true;
                    }
                    pinChanged = true;
                    clicksInCheckBoxPin++;
                    if (clicksInCheckBoxPin == 2)
                    {
                        pinChanged = false;
                        clicksInCheckBoxPin = 0;
                    }

                }
                else if (Keyboard.IsKeyDown(Key.O))
                {
                    expanderButtom.IsExpanded = true;
                    if ((bool)checkBoxE1.IsChecked)
                        checkBoxE1.IsChecked = false;
                    else
                        checkBoxE1.IsChecked = true;
                }
            }
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnSearch_Click(sender, e);
            }
        }

        private bool searchAllFlag = false;
        private void btnSearchAll_Click(object sender, RoutedEventArgs e)
        {
            //  try
            //  {
            bool lastNoteWasPinned = true;
            if (Properties.Settings.Default.savePrompt && unsavedChanges)
            {
                MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    if (!(bool)checkBoxPin.IsChecked)
                        lastNoteWasPinned = false;
                    saveNote();
                    // populateListBoxNotesNewNote(false);

                }
                else if (result == MessageBoxResult.No)
                {
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                }
                else
                    return;
            }
            if (searchAllFlag)
            {
                btnSearchAll.Foreground = System.Windows.Media.Brushes.Black;
                listBoxNotes.ItemsSource = Note.mynotes;
                if (lastNoteWasPinned)
                    populateListBoxNotesNewNote(true);
                else
                    populateListBoxNotesNewNote(false);
                //listBoxNotes.SelectedIndex = 1;
                textBoxSearchAll.Focus();
                searchAllFlag = false;
            }
            else
            {
                if (Properties.Settings.Default.savePrompt && unsavedChanges)
                {
                    MessageBoxResult result = MessageBox.Show("Save changes?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        saveNote();
                    }
                    else if (result == MessageBoxResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        unsavedChanges = false;
                        ListBoxItemUnsavedChangesOff();
                    }
                }
                else
                {
                    unsavedChanges = false;
                    ListBoxItemUnsavedChangesOff();
                }

                if (string.IsNullOrEmpty(textBoxSearchAll.Text))
                    return;
                populateListboxBySearch(textBoxSearchAll.Text);
                btnSearchAll.Foreground = System.Windows.Media.Brushes.Red;
                searchAllFlag = true;
            }
            /*   }
               catch (Exception ex)
               {
                   MessageBox.Show("Error: " + ex.Message);
               }*/
        }

        private void textBoxSearchAll_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                btnSearchAll_Click(sender, e);
            }
        }

        private void textBoxTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxTitle.CaretPosition = textBoxTitle.Document.ContentEnd;
        }

        private void textBoxNote_GotFocus(object sender, RoutedEventArgs e)
        {

            textBoxNote.CaretPosition = textBoxNote.Document.ContentEnd;
        }

        private void MainWindow1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (Properties.Settings.Default.savePrompt && unsavedChanges == true)
            {
                MessageBoxResult result = MessageBox.Show("Save changes before leaving?", "Unsaved changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        saveNote();
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;

                }
            }
            if (Properties.Settings.Default.saveWindowsCoords)
            {
                if (MainWindow1.WindowState == WindowState.Maximized)
                {
                    Properties.Settings.Default.windowWidth = SystemParameters.WorkArea.Width;
                    Properties.Settings.Default.windowHeight = SystemParameters.WorkArea.Height;

                }
                else
                {
                    Properties.Settings.Default.windowY = (int)MainWindow1.Top;
                    Properties.Settings.Default.windowX = (int)MainWindow1.Left;
                    Properties.Settings.Default.windowHeight = MainWindow1.Height;
                    Properties.Settings.Default.windowWidth = MainWindow1.Width;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void MainWindow1_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.saveWindowsCoords)
            {
                if (Properties.Settings.Default.windowWidth == SystemParameters.WorkArea.Width && Properties.Settings.Default.windowHeight == SystemParameters.WorkArea.Height)
                {
                    MainWindow1.WindowState = WindowState.Maximized;
                }
                MainWindow1.Top = Properties.Settings.Default.windowY;
                MainWindow1.Left = Properties.Settings.Default.windowX;

            }
        }

        private void comboBoxFont_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                var selectionStart = textBoxNote.Selection.Start; var selectionEnd = textBoxNote.Selection.End;
                double newFontSize = comboBoxFont.SelectedIndex + 6;
                textBoxNote.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, newFontSize);
                textBoxNote.Focus();
                textBoxNote.Selection.Select(selectionStart, selectionEnd);
            }
            catch (Exception ex)
            {
                textBoxNote.Selection.ApplyPropertyValue(TextElement.FontSizeProperty, Properties.Settings.Default.noteFontSize);
            }
        }

        private void comboBoxFont_DropDownOpened(object sender, EventArgs e)
        {
            comboBoxFont.Foreground = System.Windows.Media.Brushes.Black;
            try
            {
                var currentFontSize = (double)textBoxNote.Selection.GetPropertyValue(FontSizeProperty);
                comboBoxFont.SelectedIndex = (int)(currentFontSize - 6);
            }
            catch (Exception ex)
            {
                comboBoxFont.SelectedIndex = Properties.Settings.Default.noteFontSize - 6;
            }
        }

        private void comboBoxFont_DropDownClosed(object sender, EventArgs e)
        {
            setSelectionBack();
            comboBoxFont.Foreground = System.Windows.Media.Brushes.Transparent;
        }
        private void checkBoxPin_Unchecked(object sender, RoutedEventArgs e)
        {
            changePin();
        }
        private void checkBoxPin_Checked(object sender, RoutedEventArgs e)
        {
            changePin();
        }

        private bool pinChanged = false;

        int clicksInCheckBoxPin = 0;
        private void changePin()
        {
            if (checkBoxPin.IsFocused)
            {
                pinChanged = true;
                clicksInCheckBoxPin++;
            }
            if (clicksInCheckBoxPin == 2)
            {
                pinChanged = false;
                clicksInCheckBoxPin = 0;
            }
        }

        private void comboBoxFont1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                var selectionStart = textBoxNote.Selection.Start; var selectionEnd = textBoxNote.Selection.End;
                System.Windows.Media.FontFamily newFont;
                switch (comboBoxFont1.SelectedIndex)
                {
                    case 0:
                        newFont = new System.Windows.Media.FontFamily("Arial");
                        break;
                    case 1:
                        newFont = new System.Windows.Media.FontFamily("Verdana");
                        break;
                    case 2:
                        newFont = new System.Windows.Media.FontFamily("Segoe Print");
                        break;
                    case 3:
                        newFont = new System.Windows.Media.FontFamily("Comic Sans MS");
                        break;
                    case 4:
                        newFont = new System.Windows.Media.FontFamily("Source Code Pro");
                        break;
                    case 5:
                        newFont = new System.Windows.Media.FontFamily("Castellar");
                        break;
                    case 6:
                        newFont = new System.Windows.Media.FontFamily("Times New Roman");
                        break;
                    default:
                        newFont = new System.Windows.Media.FontFamily(Properties.Settings.Default.defaultFontFamily);
                        break;
                }
                textBoxNote.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, newFont);
                textBoxNote.Focus();
                textBoxNote.Selection.Select(selectionStart, selectionEnd);
            }
            catch (Exception ex)
            {
                textBoxNote.Selection.ApplyPropertyValue(TextElement.FontFamilyProperty, new System.Windows.Media.FontFamily(Properties.Settings.Default.defaultFontFamily));
            }
        }

        private void comboBoxFont1_DropDownOpened(object sender, EventArgs e)
        {
            comboBoxFont1.Foreground = System.Windows.Media.Brushes.Black;
            try
            {
                System.Windows.Media.FontFamily currentFontFamily = (System.Windows.Media.FontFamily)textBoxNote.Selection.GetPropertyValue(FontFamilyProperty);
                switch (currentFontFamily.Source)
                {
                    case "Arial":
                        comboBoxFont1.SelectedIndex = 0;
                        break;
                    case "Verdana":
                        comboBoxFont1.SelectedIndex = 1;
                        break;
                    case "Segoe Print":
                        comboBoxFont1.SelectedIndex = 2;
                        break;
                    case "Comic Sans MS":
                        comboBoxFont1.SelectedIndex = 3;
                        break;
                    case "Source Code Pro":
                        comboBoxFont1.SelectedIndex = 4;
                        break;
                    case "Castellar":
                        comboBoxFont1.SelectedIndex = 5;
                        break;
                    case "Times New Roman":
                        comboBoxFont1.SelectedIndex = 6;
                        break;
                    default:
                        comboBoxFont1.SelectedIndex = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
                comboBoxFont1.SelectedIndex = 0;
            }
        }

        private void comboBoxFont1_DropDownClosed(object sender, EventArgs e)
        {
            setSelectionBack();
            comboBoxFont1.Foreground = System.Windows.Media.Brushes.Transparent;
        }

        private void setSelectionBack()
        {
            var selectionStart = textBoxNote.Selection.Start; var selectionEnd = textBoxNote.Selection.End;
            textBoxNote.Focus();
            textBoxNote.Selection.Select(selectionStart, selectionEnd);
        }

        private void expanderButtom_Expanded(object sender, RoutedEventArgs e)
        {
            fillTextLabelCharactersText();
            System.Windows.Controls.Panel.SetZIndex(expanderButtom, 5);
        }

        private void expanderButtom_Collapsed(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Panel.SetZIndex(expanderButtom, 0);
        }

        private void textBoxNote_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.RightCtrl))
            {
                if (Keyboard.IsKeyDown(Key.F1))
                {
                    comboBoxFont1.IsDropDownOpen = true;
                }
                else if (Keyboard.IsKeyDown(Key.F2))
                {
                    comboBoxFont.IsDropDownOpen = true;

                }
                else if (Keyboard.IsKeyDown(Key.F3))
                {
                    comboBoxColors.IsDropDownOpen = true;
                }
            }
        }


        private void comboBoxColors_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectionStart = textBoxNote.Selection.Start; var selectionEnd = textBoxNote.Selection.End;
            try
            {
                switch (comboBoxColors.SelectedIndex)
                {
                    case 1:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Blue);
                        break;
                    case 2:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Red);
                        break;
                    case 3:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Green);
                        break;
                    case 4:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.DarkViolet);
                        break;
                    case 5:
                    case 6:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Black);
                        break;
                    case 0:
                        break;
                    default:
                        textBoxNote.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, System.Windows.Media.Brushes.Black);
                        break;
                }
                textBoxNote.Focus();
                textBoxNote.Selection.Select(selectionStart, selectionEnd);
            }
            catch (Exception ex)
            {

            }
        }

        private void comboBoxColors_DropDownOpened(object sender, EventArgs e)
        {
            comboBoxColors.SelectedIndex = 0;
        }

        private void comboBoxColors_DropDownClosed(object sender, EventArgs e)
        {
            setSelectionBack();
        }

        private void textBoxSearchAll_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxSearchAll.Text == " Search all notes")
            {
                textBoxSearchAll.Text = string.Empty;
            }
        }

        private void textBoxSearchAll_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSearchAll.Text))
                textBoxSearchAll.Text = " Search all notes";
        }

        private void expanderSearches_Collapsed(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Panel.SetZIndex(expanderSearches, 1);

        }

        private void expanderSearches_Expanded(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.Panel.SetZIndex(expanderSearches, 5);

        }


    }
}
