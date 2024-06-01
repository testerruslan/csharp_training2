using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAddressbookTests
{
    [SetUpFixture]
    internal class TestSuiteFixture
    {
       [TearDown]
       public void TeardownApplicationManager()
       {
         ApplicationManager.GetInstance().Stop();
       }
    }
}
