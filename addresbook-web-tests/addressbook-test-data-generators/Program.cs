using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WebAddressbookTests;
using System.Security.Cryptography;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Excel = Microsoft.Office.Interop.Excel;
using System.Text.RegularExpressions;
using Microsoft.Office.Interop.Excel;

namespace addressbook_test_data_generators
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = args[0];
            int count = Convert.ToInt32(args[1]);
            string filename = args[2];
            string format = args[3];

            if (data == "groups")
                {

                    List<GroupData> groups = new List<GroupData>();

                    for (int i = 0; i < count; i++)
                    {
                        groups.Add(new GroupData(TestBase.GenerateRandomSting(10))
                        {
                            Header = TestBase.GenerateRandomSting(100),
                            Footer = TestBase.GenerateRandomSting(100)
                        });
                    }

                    if (format == "excel")
                    {
                        writeGroupsToExcelFile(groups, filename);
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(args[2]);

                        if (format == "csv")
                        {
                            writeGroupsToCsvFile(groups, writer);
                        }
                        else if (format == "xml")
                        {
                            writeGroupsToXmlFile(groups, writer);
                        }
                        else if (format == "json")
                        {
                            writeGroupsToJsonFile(groups, writer);
                        }
                        else
                        {
                            Console.Out.Write("Unrecognized format " + format);
                        }
                        writer.Close();
                    }
                }
            else if (data == "accounts")
            {
                List<ContactData> accounts = new List<ContactData>();

                for (int t = 0; t < count; t++)
                {
                    accounts.Add(new ContactData()
                    {
                        Name = TestBase.GenerateRandomSting(10),
                        LastName = TestBase.GenerateRandomSting(10)
                    });
                }

                        StreamWriter writer = new StreamWriter(args[2]);

                        if (format == "csv")
                        {
                            writeAccountsToCsvFile(accounts, writer);
                        }
                        else if (format == "xml")
                        {
                            writeAccountsToXmlFile(accounts, writer);
                        }
                        else if (format == "json")
                        {
                            writeAccountsToJsonFile(accounts, writer);
                        }
                        else
                        {
                            Console.Out.Write("Unrecognized format " + format);
                        }
                        writer.Close();
            }
            else
            {
                Console.Out.Write("Unrecognized data " + data);
            }
        }

        static void writeGroupsToExcelFile(List<GroupData> groups, string filename)
        {
            Excel.Application app = new Excel.Application();
            app.Visible = true;
            Excel.Workbook wb = app.Workbooks.Add();
            Excel.Worksheet sheet = wb.ActiveSheet;

            int row = 1;
            foreach (GroupData group in groups)
            {
                sheet.Cells[row, 1] = group.Name;
                sheet.Cells[row, 2] = group.Footer;
                sheet.Cells[row, 3] = group.Header;
                row++;
            }
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filename);
            File.Delete(fullPath);
            wb.SaveAs(fullPath);

            wb.Close();
            app.Visible = false;
            app.Quit();
        }

        static void writeGroupsToCsvFile(List<GroupData> groups, StreamWriter writer)
        {
            foreach (GroupData group in groups)
            {
                writer.WriteLine(String.Format("${0},${1},${2}",
                group.Name, group.Header, group.Footer));
            }
        }

        static void writeAccountsToCsvFile(List<ContactData> accounts, StreamWriter writer)
        {
            foreach (ContactData account in accounts)
            {
                writer.WriteLine(String.Format("${0},${1}",
                account.Name, account.LastName));
            }
        }

        static void writeGroupsToXmlFile(List<GroupData> groups, StreamWriter writer)
        {
            new XmlSerializer(typeof(List<GroupData>)).Serialize(writer, groups);
        }

        static void writeAccountsToXmlFile(List<ContactData> accounts, StreamWriter writer)
        {
            new XmlSerializer(typeof(List<ContactData>)).Serialize(writer, accounts);
        }

        static void writeGroupsToJsonFile(List<GroupData> groups, StreamWriter writer)
        {
            writer.Write(JsonConvert.SerializeObject(groups, Newtonsoft.Json.Formatting.Indented));
        }

        static void writeAccountsToJsonFile(List<ContactData> accounts, StreamWriter writer)
        {
            writer.Write(JsonConvert.SerializeObject(accounts, Newtonsoft.Json.Formatting.Indented));
        }
    }
}
