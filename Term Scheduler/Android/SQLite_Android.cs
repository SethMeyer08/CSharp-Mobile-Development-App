using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using C971_Term_Sch.Droid;
using SQLite;
using System.IO;
using Xamarin.Forms;


[assembly: Dependency(typeof(SQLite_Android))]
namespace C971_Term_Sch.Droid
{

    public class SQLite_Android : SQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "MySQLite.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}