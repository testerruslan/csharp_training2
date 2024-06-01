using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium.Chrome;

namespace mantis_tests
{
    public class ApplicationManager
    {
        public IWebDriver driver;
        protected StringBuilder verificationErrors;
        protected string baseURL;

        public RegistrationHelper Registration { get; private set; }
        public FtpHelper Ftp { get; private set; }
        public ManagementMenuHelper Manager { get; private set; }
        public ProjectManagementHelper Project { get; private set; }
        public AdminHelper Admin { get; private set; }
        public APIHelper API { get; private set; }

        protected bool acceptNextAlert = true;

        private static ThreadLocal<ApplicationManager> app = new ThreadLocal<ApplicationManager>();

        private ApplicationManager()
        {
            driver = new ChromeDriver();
            baseURL = "http://localhost/mantisbt-2.26.2";
            Registration = new RegistrationHelper(this);
            Ftp = new FtpHelper(this);
            Manager = new ManagementMenuHelper(this, baseURL);
            Project = new ProjectManagementHelper(this);
            Admin = new AdminHelper(this,baseURL);
            API = new APIHelper(this);
        }

        public static ApplicationManager GetInstance()
        {
            if (! app.IsValueCreated)
            {
                ApplicationManager newInstance = new ApplicationManager();
                newInstance.driver.Url = newInstance.baseURL + "/login_page.php";
                newInstance.driver.FindElement(By.Name("username")).SendKeys("administrator");
                newInstance.driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
                newInstance.driver.FindElement(By.Name("password")).SendKeys("root");
                newInstance.driver.FindElement(By.CssSelector("input[type=\"submit\"]")).Click();
                app.Value = newInstance;
            }
            return app.Value;
        }

        public IWebDriver Driver 
        { 
            get { return driver; }
        }

        public void Stop()
        {
          try
            {
              driver.Close();
            }
           catch (Exception)
            {
              // Ignore errors if unable to close the browser
            }
        }
    }
}
