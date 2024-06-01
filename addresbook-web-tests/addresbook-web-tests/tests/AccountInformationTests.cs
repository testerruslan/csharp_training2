using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    [TestFixture]
    public class AccountInformationTests : AccountTestBase
    {
        [Test]
        public void TestAccountInformation()
        {
            ContactData fromTable =  app.AccHelp.GetAccountInformationFromTable(0);
            ContactData fromForm = app.AccHelp.GetAccountInformationFromEditForm(0);

            //verification
            Assert.AreEqual(fromTable, fromForm);
            Assert.AreEqual(fromTable.Address, fromForm.Address);
            Assert.AreEqual(fromTable.AllPhones, fromForm.AllPhones);
        }

        [Test]
        public void TestAccountInformationFromPropertyPage()
        {
            ContactData fromPropertyPage = app.AccHelp.GetAccountInformationFromPropertyPage(0);
            ContactData fromForm = app.AccHelp.GetAccountInformationFromEditForm(0);

            //verification
            Assert.AreEqual(fromPropertyPage.AccountProperties, fromForm.AccountProperties);
        }
    }
}
