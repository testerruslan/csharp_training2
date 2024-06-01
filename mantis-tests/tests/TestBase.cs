using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mantis_tests
{
     public class TestBase
    {
        public static bool PERFORM_UI_LONG_CHECKS = false;
        protected ApplicationManager app;


        [OneTimeSetUp]
        public void SetupApplicationManager()
        {
            app = ApplicationManager.GetInstance();
        }

        public static Random rnd = new Random();

        public static string GenerateRandomSting(int max)
        {
            int l = Convert.ToInt32(rnd.NextDouble()* max);
            StringBuilder builder = new StringBuilder();
            for(int i=0; i < l;i++)
            {
                builder.Append(Convert.ToChar(32 + Convert.ToInt32(rnd.NextDouble() * 65)));
            }
            return builder.ToString();
        }
    }
}
