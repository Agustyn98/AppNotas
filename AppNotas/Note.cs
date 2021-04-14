using System;
using System.Collections.ObjectModel;
using System.Data.SQLite;

namespace AppNotas
{
    class Note
    {
        private int id;
        private string title;
        private string text;
        private string creationDate;
        private string lastModifiedDate;
        /*
        private static SQLiteConnectionStringBuilder connBuilder;
        public static string setConnString()
        {
            connBuilder = new SQLiteConnectionStringBuilder();
            connBuilder.DataSource = @"Data Source=|DataDirectory|\NotesDB.db";
            connBuilder.DataSource = "NotesDB.db";
            connBuilder.SyncMode = SynchronizationModes.Off;
            connBuilder.JournalMode = SQLiteJournalModeEnum.Off;
            connectionString = connBuilder.ToString();
            return connBuilder.ToString();
        }*/


        private static string connectionString = @"Data Source=|DataDirectory|\NotesDB.db;Synchronous=Off;journal mode=off;"; //Version=3;locking_mode = EXCLUSIVE; New=false;
        public int isDone = 0;
        private int pin = 0;
        public int Id { get => id; }

        private void getAdditional()
        {
            if (isDone == 1) // Si la bandera encrypado == 1
            {
                title = Test.changeFont(title); // titulo = Encrypt.Desencriptar(titulo)
                text = Test.changeFont(text);
            }
        }
        public string Title { get => title; }
        public string Text { get => text; }
        public string CreationDate { get => creationDate; }
        public string LastModifiedDate { get => lastModifiedDate; }
        public int Pin { get => pin; }

        /*
        public static object check()
        {
            //string sql = "PRAGMA journal_mode";
            string sql = "PRAGMA Synchronous";
            SQLiteConnection connection = new SQLiteConnection(connectionString);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(sql, connection);
            var journalMode = command.ExecuteScalar();
            return journalMode;
        }
        */
        public static ObservableCollection<Note> mynotes = new ObservableCollection<Note>();

        public static void getAllNotes()
        {
            mynotes.Clear();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // SQLiteConnection connection = new SQLiteConnection(connectionString);
                connection.Open();
                //string queryString = "SELECT * FROM note ORDER BY lastModifiedDate DESC";
                SQLiteCommand command = new SQLiteCommand("SELECT * FROM note ORDER BY lastModifiedDate DESC", connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    Note n;
                    while (reader.Read())
                    {
                        n = new Note(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader.GetString(3), reader.GetString(4),
                         int.Parse(reader[5].ToString()), int.Parse(reader[6].ToString()));
                        mynotes.Add(n);
                    }
                }
            }
        }

        public Note(int id, string title, string text, string creationDate, string lastModifiedDate, int lastChanged, int pin)
        {
            this.id = id;
            this.title = title;
            this.text = text;
            this.creationDate = creationDate;
            this.lastModifiedDate = lastModifiedDate;
            this.isDone = lastChanged;
            this.pin = pin;
            getAdditional();

        }

        //Constructor para crear nota de 0
        public Note(string title, string text)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                String query = "INSERT INTO note (title,text,creationDate,lastModifiedDate) VALUES (@title,@text, datetime('now', 'localtime'),  strftime('%Y-%m-%d %H:%M:%f', 'now' , 'localtime') )";

                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    /*  if (encrypt == 1)
                      {
                          title = Encrypt.EncryptText(title);
                          text = Encrypt.EncryptText(text);
                          isEncrypted = 1;
                      }*/
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@text", text);
                    //  command.Parameters.AddWithValue("@Encrypted", encrypt);

                    connection.Open();
                    command.ExecuteNonQuery();

                }
            }
            //Guardar los valores de las variables id, titulo, texto, fecha.
            // retrieveById();
        }

        /*
        private string retrieveById()
        {
            string returnString = "Didn't even access db";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryString = "SELECT * FROM note WHERE id =" + id + ";";
                SQLiteCommand command = new SQLiteCommand(queryString, connection);

                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = int.Parse(reader[0].ToString());
                        title = reader[1].ToString();  //cambiar 1 por "titulo"?
                        text = reader[2].ToString();
                        creationDate = reader.GetString(3);
                        lastModifiedDate = reader.GetString(4);
                        isEncrypted = reader.GetInt32(5);
                        pin = reader.GetInt32(6);
                        returnString = "Success";
                    }
                    else
                    {
                        //throw new Exception("No entro a la base de datos");
                    }
                }
            }
            decryptData();
            return returnString;
        }
        */


        /*
        private string retrieveByNumber(int number) //Retrieves the note based on its order on the DB, number = 0: last record, number = 1: second last record...
        {
            string returnString = "Didn't even access db";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryString = "SELECT * FROM note ORDER BY lastModifiedDate LIMIT 1 OFFSET " + number + " ;";
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        id = int.Parse(reader[0].ToString());
                        title = reader[1].ToString();  //cambiar 1 por "titulo"?
                        text = reader[2].ToString();
                        creationDate = reader.GetString(3);
                        lastModifiedDate = reader.GetString(4);
                        isEncrypted = int.Parse(reader[5].ToString());
                        pin = int.Parse(reader[6].ToString());
                        returnString = "success";
                    }
                    else
                    {
                        throw new Exception("No entro a la base de datos");
                    }
                }
            }
            decryptData();
            return returnString;
        }*/

        public void update(string title, string text, int get54, int pin = 0)
        {
            /*
            if (title.Length > 100)
            {
                throw new Exception("Title too long");
            }*/
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string query = "UPDATE note SET title = @title , text = @text , lastModifiedDate = strftime('%Y-%m-%d %H:%M:%f', 'now' , 'localtime'), range = @range, pin = @pin WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    if (get54 == 1)
                    {
                        title = Test.changeFont1(title);
                        text = Test.changeFont1(text);
                        isDone = 1;
                    }
                    else
                    {
                        isDone = 0;
                    }
                    command.Parameters.AddWithValue("@title", title);
                    command.Parameters.AddWithValue("@text", text);
                    command.Parameters.AddWithValue("@range", isDone);
                    command.Parameters.AddWithValue("@pin", pin);
                    // command.Parameters.AddWithValue("@lastModifiedDate", lastModifiedDate);
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    command.ExecuteNonQuery();

                    /* if (result < 0)
                     {
                         throw new Exception("No entro a la base de datos");
                     }*/
                }
            }
        }

        public void delete()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string query = "DELETE FROM note WHERE id = @id";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    command.ExecuteNonQuery();
                    /*
                    if (result < 0)
                    {
                        throw new Exception("No entro a la base de datos");
                    }*/
                }
            }
        }


        public static void vacuumDB()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                string query = "VACUUM";
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /*
        public static int getNumberOfNotes()
        {
            var count = 0;
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string queryString = "SELECT id FROM note"; //cambiar por select count
                SQLiteCommand command = new SQLiteCommand(queryString, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        */
        //Unused methods, you can't use it if you want to search encrypted text
        /*     public static ArrayList search(string search)
             {
                 ArrayList ids = new ArrayList();
                 using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                 {
                     connection.Open();
                     string queryString = "SELECT id FROM note WHERE text LIKE '%" + search + "%'";
                     SQLiteCommand command = new SQLiteCommand(queryString, connection);
                     using (SQLiteDataReader reader = command.ExecuteReader())
                     {
                         while (reader.Read())
                         {
                             ids.Add ( int.Parse(reader["id"].ToString())  );
                         }
                     }
                 }
                 if(ids.Count == 0 )
                 {
                     throw new Exception("menor a 0");
                 }
                 return ids;
             }*/
    }
}
