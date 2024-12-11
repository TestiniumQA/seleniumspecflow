using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayCoreApiTest.Configuration
{
    public class AppConficReader
    {


        public static double GetElementLoadTimeout()
        {
            return 50.0;
        }

        public static double GetPageLoadTimeout()
        {
            return 90;
        }


    }
}
