using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;

namespace WebAddressbookTests
{
    public class AddingAccountToGroupTests : AuthTestBase
    {
        [Test]
        public void TestAddingContactToGroup()
        {

            app.AccHelp.NoAccountsToAdd();
            app.Groups.NoGroupsToAddAccounts();

            GroupData group = GroupData.GetAll()[0];
            ContactData contact = ContactData.GetAll()[0];
            List<ContactData> oldList = group.GetContacts();
            List<ContactData> contacts = ContactData.GetAll();
            if (oldList.Count == contacts.Count)
            {
                app.AccHelp.AddAccount(new ContactData ("TestName111","TestLastName222"));
                contact = ContactData.GetAll().First(i => i.Id == ContactData.TopContactId());
            }
            else
            {
                contact = ContactData.GetAll().Except(oldList).First();
            }

            app.AccHelp.AddAccountToGroup(contact, group);

            List<ContactData> newList = group.GetContacts();
            oldList.Add(contact);
            newList.Sort();
            oldList.Sort();

            Assert.AreEqual(newList, oldList);

        }
    }
}
