using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenQA.Selenium;

namespace ParserFSS
{
    public partial class Form1 : Form
    {
        public IWebDriver browser;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            browser.Manage().Window.Maximize();
            browser.Navigate().GoToUrl("http://google.com");

            IWebElement search = browser.FindElement(By.Name("q"));
            search.SendKeys("как поднять ммр" + OpenQA.Selenium.Keys.Enter); //for git
        }
    }
}
