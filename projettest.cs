using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            IWebDriver driver = new FirefoxDriver();
            driver.Url = ("https://www.maersk.com/tracking/");

            driver.Navigate().GoToUrl("https://www.maersk.com/tracking/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);


            //Create file
            var file = @"C:\Users\hp\Desktop\myOutput2.csv";
            using var stream = File.CreateText(file);
            Thread.Sleep(3000);

            // Créer une liste 
            List<string> container = new List<string>
            {
                // Ajouter des éléments à la liste 
                "MSKU9070323",
                "MSKU7803798"



            };


            //-----------------------------------
            //Bouton allow
            IWebElement btnall = driver.FindElement(By.ClassName("coi-banner__accept--fixed-margin"));
            btnall.Click();

            for (int i = 0; i < container.Count; i++)
            {

                driver.FindElement(By.Id("trackShipmentSearch")).Clear();

                IWebElement Tobox = driver.FindElement(By.Id("trackShipmentSearch"));
                Tobox.SendKeys(container[i]);

                IWebElement btnsearch = driver.FindElement(By.ClassName("button--primary"));
                btnsearch.Click();
                Thread.Sleep(5000);
                //Display dtails of containers
                IWebElement btn1 = driver.FindElement(By.ClassName("icon-chevron-down"));
                btn1.Click();
                for (int j = 0; j < container.Count; j++)
                {
                    //FROM / TO / Container number
                    string name1 = "From";
                    string name2 = "To";
                    string name3 = "Container number";

                    string csv0 = string.Format("{0};{1};{2}", name1, name2, name3);
                    stream.WriteLine(csv0);

                    var info1 = driver.FindElement(By.XPath("//*[@id='main']/section/div[2]/div/div[1]/dl[1]/dd"));
                    var info2 = driver.FindElement(By.XPath("//*[@id='main']/section/div[2]/div/div[1]/dl[2]/dd"));
                    var info3 = driver.FindElement(By.XPath("//*[@id='main']/section/div[2]/div/div[1]/dl[3]/dd"));

                    string csv1 = string.Format("{0};{1};{2}\n", info1.Text.ToString(), info2.Text.ToString(), info3.Text.ToString());
                    stream.WriteLine(csv1);



                    var ele1 = driver.FindElement(By.XPath("//*[@id='table_id']/tbody/tr/td[1]/span[4]"));
                    ///
                    var ele2 = driver.FindElement(By.XPath("//*[@id='table_id']/tbody/tr/td[2]/span[4]"));

                    ///
                    var ele3 = driver.FindElement(By.XPath("//*[@id='table_id']/tbody/tr/td[3]/span[4]"));

                    var ele4 = driver.FindElement(By.XPath("//*[@id='table_id']/tbody/tr/td[4]/span[4]"));

                    string csv5 = string.Format("{0};{1};{2};{3}\n", ele1.Text.ToString(), ele2.Text.ToString(), ele3.Text.ToString(), ele4.Text.ToString());
                    stream.WriteLine(csv5);

                    //-----------------------------------------------------------------------
                    var elements = driver.FindElements(By.ClassName("table--secondary"));

                    foreach (var item in elements)
                    {

                        string location = CheckField(item, "timeline__event-table__cell--heading");
                        string dateTime = CheckField(item, "timeline__event-table__cell--time");
                        string activity = CheckField(item, "timeline__event-table__cell--desc");
                        string csv = string.Format("{0} ;{1} ;{2}\n ", location, dateTime, activity);
                        stream.WriteLine(csv);

                    }
                    j++;
                }


            }
        }


        private static string CheckField(IWebElement item, string className)
        {
            try
            {
                return item.FindElement(By.ClassName(className)).Text.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

    }
}
