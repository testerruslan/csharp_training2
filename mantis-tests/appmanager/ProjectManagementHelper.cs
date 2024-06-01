using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mantis_tests
{
    public class ProjectManagementHelper : HelperBase
    {
        public ProjectManagementHelper(ApplicationManager manager) : base(manager) { }

        public List<ProjectData> GetProjects()
        {
            List<ProjectData> projects = new List<ProjectData>();
            manager.Manager.OpenAddProjectPage();
            ICollection<IWebElement> elements = driver.FindElements(By.XPath("//..//tbody/tr/td/a"));
            foreach (IWebElement element in elements)
            {
                projects.Add(new ProjectData(element.Text));
            }

            return projects;

        }

        public void Create(ProjectData project)
        {
            manager.Manager.OpenAddProjectPage();
            InitProjectCreation();
            Type(By.Id("project-name"), project.Name);
            ConfirmProjectCreation();
        }

        private void ConfirmProjectCreation()
        {
            driver.FindElement(By.XPath("//input[@value='Добавить проект']")).Click();
        }

        private void InitProjectCreation()
        {
            driver.FindElement(By.XPath("//button[@type='submit']")).Click() ;
        }

        internal void CreateIfNoProjects()
        {
            List<ProjectData> projects = new List<ProjectData>();
            manager.Manager.OpenAddProjectPage();
            ICollection<IWebElement> elements = driver.FindElements(By.XPath("//..//tbody//td/a"));

            if (elements.Count == 0)
            {
                ProjectData newProject = new ProjectData("Project" + TestBase.GenerateRandomSting(10));

                Create(newProject);
            }
        }

        public void RemoveProject(ProjectData project)
        {
            manager.Manager.OpenAddProjectPage();
            driver.FindElement(By.LinkText(project.Name)).Click();
            Remove();
            SubmitRemove();
        }

        private void SubmitRemove()
        {
            driver.FindElement(By.XPath("//input[@value='Удалить проект']")).Click();
        }

        private void Remove()
        {
            driver.FindElement(By.XPath("//button[2]")).Click();
        }
    }
}
