using System;
using System.Collections.Generic;
using System.Text;

namespace InternalExamportal.DataAccessLayer.Entities
{
    public class CandidateNumbering
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
