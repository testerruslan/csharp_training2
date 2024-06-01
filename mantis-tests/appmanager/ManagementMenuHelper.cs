using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mantis_tests
{
    public class ManagementMenuHelper : HelperBase
    {
        private string baseUrl;
        private string addProjectUrl = "/manage_proj_page.php";

        public ManagementMenuHelper(ApplicationManager manager, string baseUrl) : base(manager) 
        {
            this.baseUrl = baseUrl;
        }

        public void OpenAddProjectPage()
        {
            manager.Driver.Url = "http://localhost/mantisbt-2.26.2/manage_proj_page.php";
        }
    }
}
