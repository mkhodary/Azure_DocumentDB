using Microsoft.Azure.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace documentDb.Model
{
    public class Session
    {
        public Guid SessionId { get; set; }

        public string Name { get; set; }

        public int MaterialsCount { get; set; }
    }
}
