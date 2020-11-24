using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ITS440Proj
{
    public static class Constants
    {
        public const string DatabaseFilename = "ProjectSQLite.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |  // opens the database with read / write privs
            SQLite.SQLiteOpenFlags.Create | // create the database if it does not exist
            SQLite.SQLiteOpenFlags.SharedCache; // enable multi-threaded database access

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
