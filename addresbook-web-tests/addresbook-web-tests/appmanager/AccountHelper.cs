using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

namespace WebAddressbookTests
{
    public class AccountHelper : HelperBase
    {
        public AccountHelper(ApplicationManager manager) : base(manager)
        {
            this.manager = manager;
        }

        public AccountHelper AddAccount(ContactData account)
        {
            InitAccoutAdd();
            FillAccountInfo(account);
            SubmitAccountAdd();
            GoToAccountsPage();
            return this;
        }

        private void GoToAccountsPage()
        {
            driver.FindElement(By.LinkText("home page")).Click();
        }

        public bool NoAccountsToAction()
        {
            return !IsElementPresent(By.CssSelector("img[title = Edit]"));
        }

        public AccountHelper SubmitAccountAdd()
        {
            driver.FindElement(By.XPath("//div[@id='content']/form/input[20]")).Click();
            return this;
        }

        public AccountHelper FillAccountInfo(ContactData account)
        {
            Type(By.Name("firstname"), account.Name);
            Type(By.Name("lastname"), account.LastName);
            Type(By.Name("address"), account.Address);
            Type(By.Name("home"), account.HomePhone);
            Type(By.Name("mobile"), account.MobilePhone);
            Type(By.Name("work"), account.WorkPhone);
            Type(By.Name("email"), account.Email);
            Type(By.Name("email2"), account.Email_2);
            Type(By.Name("email3"), account.Email_3);

            return this;
        }

        public AccountHelper InitAccoutAdd()
        {
            driver.FindElement(By.LinkText("add new")).Click();
            accountCache = null;
            return this;
        }

        public AccountHelper InitAccoutModify(int index)
        {
            driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"))[7].FindElement(By.TagName("a")).Click();
            return this;
        }

        public AccountHelper InitAccoutModify(string id)
        {
            driver.FindElement(By.XPath($"//input[@name='selected[]' and @value='{id}']//..//../td[8]/a")).Click();
            return this;
        }

        public AccountHelper InitOpenProperties(int index)
        {
            driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"))[6].FindElement(By.TagName("a")).Click();
            return this;
        }

        public AccountHelper SubmitAccountModify()
        {
            driver.FindElement(By.CssSelector("input[value=Update]")).Click();
            accountCache = null;
            return this;

        }

        public AccountHelper SubmitAccountRemove()
        {
            driver.FindElement(By.CssSelector("input[value=Delete]")).Click();
            accountCache = null;
            return this;
        }

        //Удаление третьей запипи
        public AccountHelper SelectAccount()
        {
            driver.FindElement(By.Name("selected[]")).Click();
            return this;
        }

        public AccountHelper Modify(int index, ContactData newAccountData, ContactData account)
        {
            if (NoAccountsToAction())
            {
                AddAccount(account);
            }
            InitAccoutModify(index);
            FillAccountInfo(newAccountData);
            SubmitAccountModify();
            return this;
        }

        public AccountHelper Modify(ContactData account, ContactData newAccountData)
        {
            if (NoAccountsToAction())
            {
                AddAccount(account);
            }
            InitAccoutModify(account.Id);
            FillAccountInfo(newAccountData);
            SubmitAccountModify();
            return this;
        }


        public AccountHelper RemoveFromEditPage()
        {
            InitAccoutModify(0);
            SubmitAccountRemove();
            return this;
        }

        public AccountHelper RemoveFromMainPage()
        {
            SelectAccount();
            SubmitAccountRemove();
            return this;
        }

        private List<ContactData> accountCache = null;


        public List<ContactData> GetAccountList()
        {
            if (accountCache == null)
            {
                accountCache = new List<ContactData>();
                manager.Navigator.OpenHomePage();
                ICollection<IWebElement> elements = driver.FindElements(By.Name("entry"));
                foreach (IWebElement element in elements)
                {
                    accountCache.Add(new ContactData(element.FindElement(By.CssSelector("td:nth-child(3)")).Text, element.FindElement(By.CssSelector("td:nth-child(2)")).Text));
                }
            }

            return new List<ContactData>(accountCache);
        }

        internal int GetAccountCount()
        {
            return driver.FindElements(By.Name("entry")).Count;
        }

        public ContactData GetAccountInformationFromTable(int index)
        {
            manager.Navigator.OpenHomePage();
            IList<IWebElement> cells = driver.FindElements(By.Name("entry"))[index].FindElements(By.TagName("td"));

            string lastName = cells[1].Text;
            string firstName = cells[2].Text;
            string address = cells[3].Text;
            string allPhones = cells[5].Text;
            string allEmails = cells[4].Text;

            return new ContactData(firstName, lastName)
            {
                Address = address,
                AllPhones = allPhones,
                AllEmails = allEmails,
            };
        }

        public ContactData GetAccountInformationFromEditForm(int index)
        {
            manager.Navigator.OpenHomePage();
            InitAccoutModify(index);

            string firstName = driver.FindElement(By.Name("firstname")).GetAttribute("value");
            string lastName = driver.FindElement(By.Name("lastname")).GetAttribute("value");
            string address = driver.FindElement(By.Name("address")).GetAttribute("value");
            string homePhone = driver.FindElement(By.Name("home")).GetAttribute("value");
            string mobilePhone = driver.FindElement(By.Name("mobile")).GetAttribute("value");
            string workPhone = driver.FindElement(By.Name("work")).GetAttribute("value");
            string email = driver.FindElement(By.Name("email")).GetAttribute("value");
            string email_2 = driver.FindElement(By.Name("email2")).GetAttribute("value");
            string email_3 = driver.FindElement(By.Name("email3")).GetAttribute("value");

            return new ContactData(firstName, lastName)
            {
                Address = address,
                HomePhone = homePhone,
                MobilePhone = mobilePhone,
                WorkPhone = workPhone,
                Email = email,
                Email_2 = email_2,
                Email_3 = email_3,
            };
        }

        public int GetNumberOfSearchResults()
        {
            manager.Navigator.OpenHomePage();
            string text = driver.FindElement(By.TagName("label")).Text;
            Match m = new Regex(@"\d+").Match(text);
            return Int32.Parse(m.Value);
        }

        public ContactData GetAccountInformationFromPropertyPage(int index)
        {
            manager.Navigator.OpenHomePage();
            InitOpenProperties(index);

            string accountProperties = driver.FindElement(By.Id("content")).Text;

            return new ContactData
            {
                AccountProperties = accountProperties
            };
        }

        public void AddAccountToGroup(ContactData contact, GroupData group)
        {
            manager.Navigator.OpenHomePage();
            ClearGroupFilter();
            SelectContactById(contact.Id);
            SelectGroupToAdd(group.Name);
            SubmitAddingContactToGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(20)).Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count > 0);
        }

        public void SubmitAddingContactToGroup()
        {
            driver.FindElement(By.Name("add")).Click();
        }

        public void SelectGroupToAdd(string name)
        {
            new SelectElement(driver.FindElement(By.Name("to_group"))).SelectByText(name);
        }

        public void SelectContactById(string contactId)
        {
            driver.FindElement(By.XPath($"//input[@name='selected[]' and @value='{contactId}']")).Click();
        }

        public void ClearGroupFilter()
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText("[all]");
        }

        internal void RemoveContactFromGroup(ContactData contactsInGroups, GroupData group)
        {
            manager.Navigator.OpenHomePage();
            SelectGroupToRemove(group.Name);
            SelectContactById(contactsInGroups.Id);
            CommitRemovingContactFromGroup();
            new WebDriverWait(driver, TimeSpan.FromSeconds(10))
                .Until(d => d.FindElements(By.CssSelector("div.msgbox")).Count > 0);
        }

        private void CommitRemovingContactFromGroup()
        {
            driver.FindElement(By.Name("remove")).Click();
        }

        private void SelectGroupToRemove(string name)
        {
            new SelectElement(driver.FindElement(By.Name("group"))).SelectByText(name);
        }

        public void NoAccountsToAdd()
        {
            List<ContactData> contacts = ContactData.GetAll();

            if(contacts.Count == 0)
            {
                ContactData contact = new ContactData("TestName", "TestLastName");

                AddAccount(contact);
            }
        }
    }
}
