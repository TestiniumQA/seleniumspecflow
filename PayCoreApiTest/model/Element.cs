using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCoreApiTest.model
{
    public class Element
    {

        public string key { get; set; }
        public string value { get; set; }
        public string type { get; set; }

        public override string ToString()
        {
            return String.Format("key : {0}, value {1}, type {2}", key, value, type);
        }
    }
}
