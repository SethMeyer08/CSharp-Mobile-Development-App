using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace C971_Term_Sch.Classes
{
    [Table("Assessments")]
    public class Assessments
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string AssessmentName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string AssessType { get; set; }
        public int Course { get; set; }
        public int GetNotified { get; set; }
    }
}
