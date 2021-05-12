using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971_Term_Sch.Classes
{
    [Table("Terms")]
    public class Terms
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string TermName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
