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
using System.Threading;

namespace ParserFSS
{
    public partial class Form1 : Form
    {
        public IWebDriver browser;
        public delegate void textChanger(string nameSSD); //Делегат принимающий аргумент типа стринг
        public textChanger txtChanger;

        public Form1()
        {
            InitializeComponent();
        }

        void ListChanger(string NameOfSSD) //метод для делегата
        {
            textBox1.Text = textBox1.Text + NameOfSSD + Environment.NewLine;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread catalogReaderThr = new Thread(new ThreadStart(CatalogReader));
            txtChanger = new textChanger(ListChanger);
            catalogReaderThr.Start();
        }

        void CatalogReader()
        {
            browser = new OpenQA.Selenium.Chrome.ChromeDriver();
            browser.Manage().Window.Maximize();

            browser.Navigate().GoToUrl(textBox2.Text); //url https://www.dns-shop.ru/catalog/8a9ddfba20724e77/ssd-nakopiteli/
            //IWebElement search = browser.FindElement(By.CssSelector("h3"));
            //textBox1.Text = search.Text; //for git

            List<IWebElement> productsSSD = browser.FindElements(By.CssSelector("h3")).ToList();
            for (int i = 0; i < productsSSD.Count; i++)
            {
                Invoke(txtChanger, productsSSD[i].Text);
                //textBox1.Invoke(new Action<string>((s) textBox1.Text = productsSSD[i].Text + Environment.NewLine, n));
                //textBox1.AppendText(productsSSD[i].Text + Environment.NewLine);
            }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Text = null;
        }
    }
}
