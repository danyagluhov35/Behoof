using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using Behoof.Infrastructure.Data;
using Behoof.Core.Entities;
using Behoof.Core.Services;

namespace Behoof.Infrastructure.Service
{
    public abstract class SupplierParsing
    {
        public string Name { get; set; }

        private protected ApplicationContext db;
        private protected TrimProductName? TrimProductName { get; set; }
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
            TrimProductName = new TrimProductName();
            this.db = db;

            Option = new ChromeOptions();
            //Option.AddArgument("--headless");
            Option.AddArgument("--disable-blink-features=AutomationControlled");

            Document = new HtmlDocument();
            Driver = new ChromeDriver(Option);
            WaitDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(25));
            Url = supplierSetting.Url;
            ClassElementCard = supplierSetting.ClassElementCard;
            ClassElementName = supplierSetting.ClassElementName;
            ClassElementPrice = supplierSetting.ClassElementPrice;
            _SupplierId = supplierSetting.SupplierId;
            JavaScriptExecutor = (IJavaScriptExecutor)Driver;
            ClassElementButtonMore = supplierSetting.ClassElementBtnMore;
        }

        public virtual async Task LoadPage()
        {
            Driver.Navigate().GoToUrl(Url);
            Thread.Sleep(8000);
            while (true)
            {
                try
                {
                    JavaScriptExecutor.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
                    Thread.Sleep(1000);
                    var loadMoreButton = WaitDriver.Until(d => d.FindElement(By.XPath(ClassElementButtonMore)));

                    JavaScriptExecutor.ExecuteScript("arguments[0].scrollIntoView({block: 'center'});", loadMoreButton);
                    Thread.Sleep(1500);
                    JavaScriptExecutor.ExecuteScript("arguments[0].click();", loadMoreButton);
                    Thread.Sleep(3000);
                }
                catch (Exception ex)
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
            var supplierItem2 = db.Product.ToList();


            var productNodes = Document.DocumentNode.SelectNodes(ClassElementCard);

            if (productNodes != null)
            {
                foreach (var item in productNodes)
                {
                    var name = TrimProductName.TrimName(item.SelectSingleNode(ClassElementName)?.InnerText.TrimStart());
                    var price = item.SelectSingleNode(ClassElementPrice)?.InnerText.Replace("&nbsp;", "").Replace("₽", "") ?? "";

                    if (name != "" && price != "")
                    {
                        var product = db.SupplierProduct
                            .Include(p => p.Supplier)
                            .Include(p => p.Product)
                            .Where(p => p.Product.Name == name && p.Supplier.Id == _SupplierId)
                            .FirstOrDefault();

                        if (product == null)
                        {
                            var productData = new Product()
                            {
                                Name = name,
                                Price = Convert.ToDecimal(price),
                                MinPrice = Convert.ToDecimal(price),
                                MaxPrice = Convert.ToDecimal(price),
                                CategoryId = "1",
                                BallAnswer = new Random().Next(1,5),
                                BallBatery = new Random().Next(1,5),
                                BallCamera = new Random().Next(1,5),
                                BallDesign = new Random().Next(1,5),
                                BallPortatable = new Random().Next(1,5),
                            };
                            db.Product.Add(productData);
                            db.SaveChanges();


                            var supplierProduct = new SupplierProduct()
                            {
                                SupplierId = _SupplierId,
                                ProductId = productData?.Id
                            };
                            db.SupplierProduct.Add(supplierProduct);
                            db.SaveChanges();
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
                    var name = item.SelectSingleNode(ClassElementName).InnerText.TrimStart() ?? "";
                    var price = item.SelectSingleNode(ClassElementPrice);

                    if (name != null && price != null)
                    {
                        var product = db.SupplierProduct
                            .Include(p => p.Supplier)
                            .Include(p => p.Product)
                            .Where(p => p.Product.Name == name && p.Supplier.Id == _SupplierId)
                            .FirstOrDefault();

                        if (product != null)
                        {

                            var productHistory = db.HistoryProduct
                                .Include(ph => ph.Product)
                                .Where(p => p.ProductId == product.Product.Id)
                                .ToList();

                            var newProductHistory = new HistoryProduct()
                            {
                                ProductId = product.Product.Id,
                                Price = product.Product.Price
                            };
                            db.HistoryProduct.Add(newProductHistory);
                            db.SaveChanges();


                            product.Product.Price = Convert.ToDecimal(price);
                            product.Product.MinPrice = productHistory.Min(p => p.Product.Price);
                            product.Product.MaxPrice = productHistory.Max(p => p.Product.Price);

                            db.Product.Update(product.Product);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }
    }
}
