using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;

namespace wpf_mvvm_post_test
{
    class DbManager
    {
        SQLiteConnection dbConnection;
        public string _dbPath;
        bool isConnected;
        bool onSync;
        bool onError;

        List<string> tempId;
        List<string> tempTitle;
        List<string> tempContent;

        public DbManager()
        {
            tempId = new List<string>();
            tempTitle = new List<string>();
            tempContent = new List<string>();
        }

        public void SetDBpath(string s)
        {
            _dbPath = s;
        }
        [ExceptionHandler]
        void Connect()
        {
            Console.WriteLine("OPEN DB");
            dbConnection = new SQLiteConnection(string.Format("Data Source={0}", _dbPath));
            dbConnection.Open();
            isConnected = true;
        }

        void CreateDatabase()
        {
            SQLiteConnection.CreateFile(_dbPath);
        }

        void CreateTable()
        {
            string sql = "CREATE TABLE ms_notes (id TEXT, title TEXT, content TEXT)";
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.ExecuteNonQuery();

            if (onError)
                onError = false;
        }

        public void SyncAll(string[] id, string[] title, string[] content)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + _dbPath;
            if (!System.IO.File.Exists(path))
            {
                CreateDatabase();
                Connect();
                CreateTable();
            } else
            {
                Connect();
            }

            onSync = true;

            tempId.Clear();
            tempTitle.Clear();
            tempContent.Clear();

            tempId = id.ToList();
            tempTitle = title.ToList();
            tempContent = content.ToList();

            int length = id.Length;

            string read = "SELECT * FROM ms_notes";
            int ctr = 0;

            using (SQLiteCommand cmd = new SQLiteCommand(read, dbConnection))
            {
                using(SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        if (string.IsNullOrEmpty((string)sdr["id"]))
                        {
                            break;
                        }
                        else
                        {
                            for(int i = 0; i<length; i++)
                            {
                                if(tempId[i].IndexOf((string)sdr["id"]) > -1)
                                {
                                    if(tempTitle[i] == (string)sdr["title"])
                                    {
                                        if(tempContent[i] == (string)sdr["content"])
                                        {
                                            return;
                                        } else
                                        {
                                            InsertWhere(tempId[i], tempTitle[i], tempContent[i], (string)sdr["id"]);
                                        }
                                    }
                                    else
                                    {
                                        InsertWhere(tempId[i], tempTitle[i], tempContent[i], (string)sdr["id"]);
                                    }
                                }
                                else
                                {
                                    InsertWhere(tempId[i], tempTitle[i], tempContent[i], (string)sdr["id"]);
                                }
                            }
                        }
                        ctr++;
                    }
                }
            }

            if (ctr == 0)
            {
                for (int i = 0; i < length; i++)
                {
                    Insert(tempId[i], tempTitle[i], tempContent[i]);
                }
            }

            dbConnection.Close();
            isConnected = false;
            onSync = false;
        }

        public void Insert(string id, string title, string content)
        {
            if(!isConnected)
                Connect();

            string sql = string.Format("INSERT INTO ms_notes (id, title, content) VALUES ('{0}','{1}','{2}')", id, title, content);
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.ExecuteNonQuery();

            if (!onSync)
            {
                dbConnection.Close();
                isConnected = false;
            }
        }

        public void InsertWhere(string id, string title, string content, string origin)
        {
            if (!isConnected)
                Connect();

            string sql = string.Format("UPDATE ms_notes SET id='{0}', title='{1}', content='{2}' WHERE id='{3}'", id, title, content, origin);
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.ExecuteNonQuery();

            if (!onSync)
            {
                dbConnection.Close();
                isConnected = false;
            }
        }

        public void Delete(string id)
        {
            if (!isConnected)
                Connect();

            string sql = string.Format("DELETE FROM ms_notes WHERE id='{0}'", id);
            SQLiteCommand cmd = new SQLiteCommand(sql, dbConnection);
            cmd.ExecuteNonQuery();

            if (!onSync)
            {
                dbConnection.Close();
                isConnected = false;
            }
        }

        public void Read(Action<string[], string[], string[]> callback)
        {
            if (!isConnected)
                Connect();

            tempId.Clear();
            tempTitle.Clear();
            tempContent.Clear();
            string read = "SELECT * FROM ms_notes";

            using (SQLiteCommand cmd = new SQLiteCommand(read, dbConnection))
            {
                using (SQLiteDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        tempId.Add((string)sdr["id"]);
                        tempTitle.Add((string)sdr["title"]);
                        tempContent.Add((string)sdr["content"]);
                    }

                    if (tempId.Count > 0)
                    {
                        callback(tempId.ToArray(), tempTitle.ToArray(), tempContent.ToArray());
                    }
                }
            }

            if (!onSync)
            {
                dbConnection.Close();
                isConnected = false;
            }
        }
    }
}
