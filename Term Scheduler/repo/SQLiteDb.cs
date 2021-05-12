using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971_Term_Sch
{
    public interface SQLiteDb
    {
        SQLiteAsyncConnection GetConnection();
    }
}
