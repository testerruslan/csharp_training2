using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading.Tasks;

namespace WebAddressbookTests
{
    public class AccountTestBase : AuthTestBase
    {
        [TearDown]
        public void CompareAccountsUI_DB()
        {
            if (PERFORM_UI_LONG_CHECKS)
            {
                List<ContactData> fromUI = app.AccHelp.GetAccountList();
                List<ContactData> fromDB = ContactData.GetAll();
                fromUI.Sort();
                fromDB.Sort();
                Assert.AreEqual(fromUI, fromDB);
            }
        }
    }
}
