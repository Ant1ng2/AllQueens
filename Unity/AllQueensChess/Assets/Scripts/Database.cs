using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mono.Data.Sqlite;
using System.Data;
using System;

public static class Database
{
    private static string conn = "URI=file:" + Application.dataPath + "/Plugins/Database.db"; //Path to the database
    private static IDbConnection dbconn = (IDbConnection)new SqliteConnection(conn);

    public static void CreateTable()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "CREATE TABLE Database (ID INTEGER NOT NULL UNIQUE, Value INTEGER, PRIMARY KEY(ID));";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
    }

    public static void Open()
    {
        dbconn.Open();
    }

    public static void Close()
    {
        dbconn.Close();
    }

    public static void Clear()
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "DELETE FROM Database";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();
    }

    public static bool Add(ulong key, byte value)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = string.Format("INSERT OR IGNORE INTO Database (ID, Value) VALUES ({0},{1})", key, value);
        dbcmd.CommandText = sqlQuery;
        if (dbcmd.ExecuteScalar() != null)
        {
            return true;
        }
        return false;
    }

    public static byte Get(ulong key)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = string.Format("SELECT Value" + " FROM Database" + " WHERE ID={0}", key);
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetByte(0);
        }
        reader.Close();
        return 0;
    }

    public static bool Contains(ulong key)
    {
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = string.Format("SELECT Value" + " FROM Database" + " WHERE ID={0}", key);
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();
        bool result = reader.Read();
        reader.Close();
        return result;
    }
}
