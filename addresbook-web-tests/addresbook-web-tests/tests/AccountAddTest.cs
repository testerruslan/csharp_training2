using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Serialization;
using System.Xml;
using NUnit.Framework;
using Newtonsoft.Json;

namespace WebAddressbookTests
{
    [TestFixture]
     public class AccountAddTests : AccountTestBase
    {
        public static IEnumerable<ContactData> RandomAccountAddDataProvider()
        {
            List<ContactData> account = new List<ContactData>();
            for (int i = 0; i < 5; i++)
            {
                account.Add(new ContactData(GenerateRandomSting(10), GenerateRandomSting(10))
                {
                    Address = GenerateRandomSting(15)
                });
            }
            return account;
        }

        public static IEnumerable<ContactData> AccountDataFromCsvFile()
        {
            List<ContactData> account = new List<ContactData>();
            string[] lines = File.ReadAllLines(@"accounts.csv");
            foreach (string l in lines)
            {
                string[] parts = l.Split(',');
                account.Add(new ContactData(parts[0], parts[1])
                {
                    Address = parts[2],
                });
            }
            return account;
        }

        public static IEnumerable<ContactData> AccountDataFromXmlFile()
        {
            return (List<ContactData>) new XmlSerializer(typeof(List<ContactData>)).Deserialize(new StreamReader(@"accounts.xml"));
        }

        public static IEnumerable<ContactData> AccountDataFromJsonFile()
        {
            return JsonConvert.DeserializeObject<List<ContactData>>(File.ReadAllText(@"accounts.json"));
        }

        [Test, TestCaseSource("AccountDataFromJsonFile")]
        public void AccountAddTest(ContactData account)
        {
            List<ContactData> oldAccounts = ContactData.GetAll();

            app.AccHelp.AddAccount(account);

            Assert.AreEqual(oldAccounts.Count + 1, app.AccHelp.GetAccountCount());

            List<ContactData> newAccounts = ContactData.GetAll();
            oldAccounts.Add(account);
            oldAccounts.Sort();
            newAccounts.Sort();
            Assert.AreEqual(oldAccounts, newAccounts);
        }
    }
}
