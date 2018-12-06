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
using System.IO;

namespace ParserFSS
{
    public partial class Form1 : Form
    {
        public IWebDriver browser;
        public delegate void textChanger(string nameSSD); //Делегат принимающий аргумент типа стринг
        public textChanger txtChanger;
        public StreamWriter txtOutput;

        public Form1()
        {
            InitializeComponent();
        }

        void ListChanger(string NameOfSSD) //метод для делегата(для инвока)
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
            if (textBox2.Text != null && textBox2.Text != "Введите URL" && textBox2.Text != "")
            {
                if (textBox1.Text != "" && textBox1.Text != null)
                {
                    if (textBox3.Text != "" && textBox3.Text != "Имя файла вывода без .txt")
                    {
                        browser = new OpenQA.Selenium.Chrome.ChromeDriver();
                        browser.Manage().Window.Maximize();
                        browser.Navigate().GoToUrl(textBox2.Text); //url https://www.dns-shop.ru/catalog/8a9ddfba20724e77/ssd-nakopiteli/
                        txtOutput = new StreamWriter(textBox3.Text);
                        List<IWebElement> pages = browser.FindElements(By.CssSelector("span[data-page-number]")).ToList();
                        int firstPage = int.Parse(pages[2].Text);
                        pages = null;
                        for (int k = firstPage; k < int.Parse(textBox1.Text) + firstPage; k++)
                        {
                            pages = browser.FindElements(By.CssSelector("span[data-page-number]")).ToList();
                            if (k != firstPage)
                                for (int j = 0; j < pages.Count; j++)
                                {
                                    if (pages[j].Text == k.ToString())
                                    {
                                        pages[j].Click();
                                        break;
                                    }
                                }
                            Thread.Sleep(3000);
                            List<IWebElement> products = browser.FindElements(By.CssSelector("h3")).ToList();
                            List<IWebElement> costs = browser.FindElements(By.CssSelector("span[data-value]")).ToList();
                            for (int i = 0; i < products.Count; i++)
                            {
                                txtOutput.WriteLine(products[i].Text + " cost:" + costs[i].Text.Replace(" ", "") + " /cost");
                                //Invoke(txtChanger, productsSSD[i].Text); для работы с графическими элементами из неосновного потока
                                //textBox1.AppendText(productsSSD[i].Text + Environment.NewLine);
                            }
                            pages = null;
                        }
                        txtOutput.Close();
                        MessageBox.Show("Сбор товаров завершен");
                    }
                    else { MessageBox.Show("Введите имя файла вывода без .txt", "System.Error"); textBox3.Text = "Введите имя файла вывода без .txt"; }
                }
                else { MessageBox.Show("Введите количество страниц для получения списка товаров", "System.Error"); }
            }
            else { MessageBox.Show("Введите URL Любой страницы магазина DNS", "System.Error"); }
        }

        private void textBox2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Text = null;
        }

        private void textBox1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox1.Text = null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            switch (textBox1.Text)
            {
                case "1":break; case "2":break; case "3":break; case "4":break; case "5":break; case "6":break;
                default: textBox1.Text = ""; MessageBox.Show("Введите положительное числовое значение от 1 до 6", "System.Error"); break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (browser != null)
            browser.Quit();
        }

        private void textBox3_MouseDown(object sender, MouseEventArgs e)
        {
            textBox3.Text = null;
        }
    }
}
