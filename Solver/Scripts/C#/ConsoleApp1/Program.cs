using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Program
    {
        private static SQLiteConnection conn = new SQLiteConnection("URI=file:Database.db");

        public static void Initialize()
        {
            conn.Open();
            DeleteTable();
            CreateTable();
        }

        public static void Close()
        {
            conn.Close();
        }

        public static void CreateTable()
        {
            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE IF NOT EXISTS Database (ID INTEGER NOT NULL UNIQUE, Value INTEGER, PRIMARY KEY(ID));";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static void DeleteTable()
        {
            SQLiteCommand sqlite_cmd;
            string Createsql = "DROP TABLE Database;";
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        public static bool Add(ulong key, byte value)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = string.Format("INSERT OR IGNORE INTO Database (ID, Value) VALUES ({0},{1})", key, value);
            if (sqlite_cmd.ExecuteScalar() != null)
            {
                return true;
            }
            return false;
        }

        public static byte Get(ulong key)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = string.Format("SELECT Value" + " FROM Database" + " WHERE ID={0}", key);

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            if (sqlite_datareader.Read())
            {
                return sqlite_datareader.GetByte(0);
            }
            sqlite_datareader.Close();
            return 0;
        }

        public static bool Contains(ulong key)
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = conn.CreateCommand();
            sqlite_cmd.CommandText = string.Format("SELECT Value" + " FROM Database" + " WHERE ID={0}", key);

            sqlite_datareader = sqlite_cmd.ExecuteReader();
            bool contain = sqlite_datareader.HasRows;
            sqlite_datareader.Close();
            return contain;
        }
    }
}