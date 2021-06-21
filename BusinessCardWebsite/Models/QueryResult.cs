using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessCardWebsite.Models
{
    public class QueryResult
    {
        public bool resulCode { get; set; }

        public string resultMessage { get; set; }

        public object obj { get; set; }

        public QueryResult()
        {
            resulCode = true;
        }
    }
}