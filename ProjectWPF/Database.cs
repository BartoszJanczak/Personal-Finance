using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProjectWPF
{
    internal class Database
    {
        public SQLiteConnection conn;

        public Database()
        {
            conn = new SQLiteConnection("Data Source=financeDB.sqlite3");
            if (!File.Exists("./financeDB.sqlite3"))
            {
                SQLiteConnection.CreateFile("financeDB.sqlite3");
                Console.WriteLine("financeDB file created");
            }
        }

        public void OpenConnection()
        {
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
        }

        public void CloseConnection()
        {
            if (conn.State != System.Data.ConnectionState.Closed)
            {
                conn.Close();
            }
        }
    }
}
