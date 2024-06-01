using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace WebAddressbookTests
{
    public class RemovingAccountFromGroupTests : AuthTestBase
    {
        [Test]
        public void TestRemovingContactFromGroup()
        {

            app.AccHelp.NoAccountsToAdd();
            app.Groups.NoGroupsToAddAccounts();

            GroupData group = GroupData.GetAll()[0];
            List<ContactData> oldList = group.GetContacts();

            if (oldList.Count == 0) 
            { 
                ContactData contact = ContactData.GetAll().First();
                app.AccHelp.AddAccountToGroup(contact, group);
                oldList = group.GetContacts();
            }

            ContactData contactsInGroups = oldList[0];

            app.AccHelp.RemoveContactFromGroup(contactsInGroups, group);

            List<ContactData> newList = group.GetContacts();
            oldList.Add(contactsInGroups);
            newList.Sort();
            oldList.Sort();

            Assert.That(newList, Has.No.Member(contactsInGroups));

        }
    }
}
