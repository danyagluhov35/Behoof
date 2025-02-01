﻿using Behoof.Domain.Entity.Context;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Behoof.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Behoof.Domain.Parsing2
{
    public abstract class SupplierParsing
    {
        public string Name { get; set; }

        private protected ApplicationContext db;
        private protected string? Url { get; set; }
        private protected string? ClassElementCard { get; set; }
        private protected string? ClassElementName { get; set; }
        private protected string? ClassElementPrice { get; set; }
        private protected string? ClassElementButtonMore { get; set; }
        private protected string _SupplierId { get; set; }

        private protected string Page { get; set; }
        private protected HtmlDocument Document { get; set; }
        private protected IWebDriver Driver { get; set; }
        private protected IJavaScriptExecutor JavaScriptExecutor { get; set; }
        private protected WebDriverWait WaitDriver { get; set; }
        private protected ChromeOptions Option { get; set; }

        public SupplierParsing(SupplierSetting supplierSetting, ApplicationContext db)
        {
            Option = new ChromeOptions();
            Option.AddArgument("--headless");
            Option.AddArgument("--disable-blink-features=AutomationControlled");

            Document = new HtmlDocument();
            Driver = new ChromeDriver(Option);
            WaitDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
            Url = supplierSetting.Url;
            ClassElementCard = supplierSetting.ClassElementCard;
            ClassElementName = supplierSetting.ClassElementName;
            ClassElementPrice = supplierSetting.ClassElementPrice;
            _SupplierId = supplierSetting.SupplierId;
            JavaScriptExecutor = (IJavaScriptExecutor)Driver;
            ClassElementButtonMore = supplierSetting.ClassElementBtnMore;
            this.db = db;
        }

        public virtual async Task LoadPage()
        {
            Driver.Navigate().GoToUrl(Url);
            while (true)
            {
                try
                {
                    JavaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    Thread.Sleep(3000);
                    var loadMoreButton = WaitDriver.Until(d => d.FindElement(By.XPath(ClassElementButtonMore)));

                    JavaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", loadMoreButton);
                    Thread.Sleep(500);
                    JavaScriptExecutor.ExecuteScript("arguments[0].click();", loadMoreButton);
                    Thread.Sleep(3000);
                }
                catch (NoSuchElementException ex)
                {
                    break;
                }
            }
            Thread.Sleep(10000);

            Page = Driver.PageSource;
            Document.LoadHtml(Page);
        }

        public virtual async Task SaveOnDb()
        {
            var supplierItem2 = db.SupplierItem.Include(s => s.Products).ToList();


            var productNodes = Document.DocumentNode.SelectNodes(ClassElementCard);

            if (productNodes != null)
            {
                foreach (var item in productNodes)
                {
                    var name = item.SelectSingleNode(ClassElementName);
                    var price = item.SelectSingleNode(ClassElementPrice);

                    if (name != null && price != null)
                    {
                        var product = db.Product.FirstOrDefault(p => p.Name == name.InnerText);
                        if (product == null)
                        {
                            var productData = new Product() { Name = name.InnerText, Price = Convert.ToDecimal(price.InnerText), CategoryId = "1" };
                            await db.Product.AddAsync(productData);
                            await db.SaveChangesAsync();

                            SupplierItem supplierItem = new SupplierItem()
                            {
                                Products = new List<Product> { productData },
                                SupplierId = _SupplierId,
                                MinPrice = Convert.ToDecimal(price.InnerText),
                                MaxPrice = Convert.ToDecimal(price.InnerText),
                            };
                            await db.SupplierItem.AddAsync(supplierItem);
                            await db.SaveChangesAsync();

                            var supplierProduct = new SupplierProduct()
                            {
                                SupplierId = _SupplierId,
                                ProductId = productData?.Id
                            };
                            await db.SupplierProduct.AddAsync(supplierProduct);
                            await db.SaveChangesAsync();
                        }
                    }
                }
            }
        }
        public virtual async Task Update()
        {
            var productNodes = Document.DocumentNode.SelectNodes(ClassElementCard);

            if (productNodes != null)
            {
                foreach (var item in productNodes)
                {
                    var name = item.SelectSingleNode(ClassElementName);
                    var price = item.SelectSingleNode(ClassElementPrice);

                    if (name != null && price != null)
                    {
                        var product = db.Product.FirstOrDefault(p => p.Name == name.InnerText);
                        if (product != null)
                        {
                            var supplierItem = db.SupplierItem.Where(s => s.Products.Select(u => u.Id).FirstOrDefault() == product.Id).Include(s => s.Products).FirstOrDefault();
                            if (supplierItem != null)
                            {
                                var newProduct = new Product();
                                supplierItem.Products.Add(newProduct);
                                db.Entry(supplierItem).State = EntityState.Modified;
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
        }
    }
}
