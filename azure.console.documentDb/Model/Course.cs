using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace azure.console.documentDb.Model
{
    public class Course
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string Name { get; set; }
        public DateTime CreationDate { get; set; }

        public List<Session> Sessions { get; set; }

        public Teacher Teacher { get; set; }
    }
}
