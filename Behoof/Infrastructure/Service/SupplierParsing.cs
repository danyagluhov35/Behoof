using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using Microsoft.EntityFrameworkCore;
using Behoof.Infrastructure.Data;
using Behoof.Core.Entities;
using Behoof.Core.Services;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Behoof.Infrastructure.Service
{
    public abstract class SupplierParsing
    {

        private protected ApplicationContext db;
        private protected TrimProductName? TrimProductName { get; set; }
        private protected string? Url { get; set; }
        private protected string? ClassElementCard { get; set; }
        private protected string? ClassElementName { get; set; }
        private protected string? ClassElementPrice { get; set; }
        private protected string? ClassElementButtonMore { get; set; }
        private protected string _SupplierId { get; set; }

        private protected IWebDriver Driver { get; set; }
        private protected IJavaScriptExecutor JavaScriptExecutor { get; set; }
        private protected WebDriverWait WaitDriver { get; set; }
        private protected ChromeOptions Option { get; set; }

        public SupplierParsing(SupplierSetting supplierSetting, ApplicationContext db)
        {
            TrimProductName = new TrimProductName();
            Option = new ChromeOptions();
            Option.AddArgument("--disable-blink-features=AutomationControlled");
            Option.AddAdditionalOption("useAutomationExtension", false);
            Option.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/132.0.0.0 Safari/537.36");
            Driver = new ChromeDriver(Option);
            WaitDriver = new WebDriverWait(Driver, TimeSpan.FromSeconds(25));
            this.db = db;

            Option.AddArgument("--headless");
            Url = supplierSetting.Url;
            ClassElementCard = supplierSetting.ClassElementCard;
            ClassElementName = supplierSetting.ClassElementName;
            ClassElementPrice = supplierSetting.ClassElementPrice;
            _SupplierId = supplierSetting.SupplierId!;
            JavaScriptExecutor = (IJavaScriptExecutor)Driver;
            ClassElementButtonMore = supplierSetting.ClassElementBtnMore;
        }

        public virtual async Task<int> LoadFullPage()
        {
            Driver.Navigate().GoToUrl(Url);
            await Task.Delay(5000);
            JavaScriptExecutor?.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");
            await Task.Delay(5000);
            while (true)
            {
                await Task.Delay(5000);
                var btnNext = WaitDriver?.Until(d => d?.FindElements(By.XPath(ClassElementButtonMore)));
                if (btnNext?.Count > 0)
                    JavaScriptExecutor?.ExecuteScript("arguments[0].click();", btnNext[0]);
                else
                    break;
            }

            int scrollIndex = 0;
            do
            {
                JavaScriptExecutor?.ExecuteScript($"window.scrollTo(0, {scrollIndex});");
                scrollIndex += 1000;
            } while (Convert.ToInt32(JavaScriptExecutor?.ExecuteScript("return document.body.scrollHeight")) > scrollIndex);

            return scrollIndex;
        }

        public async virtual Task<ICollection<IWebElement>> LoadElements(int scrollHeight)
        {
            try
            {
                for (int i = 0; i < scrollHeight; i += 700)
                    JavaScriptExecutor?.ExecuteScript($"window.scrollTo(0, {i});");
            }
            catch (Exception ex)
            {
                throw;
            }
            
            return Driver?.FindElements(By.ClassName(ClassElementCard))!;
        }
        public virtual async Task SaveOnDb(ICollection<IWebElement> elements)
        {

            foreach (var item in elements)
            {
                var nameElement = WaitDriver?.Until(d => item.FindElements(By.CssSelector(ClassElementName)));
                var priceElement = WaitDriver?.Until(d => item.FindElements(By.CssSelector(ClassElementPrice)));
                if (nameElement?.Count > 0 && priceElement?.Count > 0)
                {
                    var name = TrimProductName?.TrimName(nameElement[0].Text);
                    var price = Convert.ToDecimal(priceElement[0].Text.Replace("&nbsp;", "").Replace("₽", ""));

                    var product = db.SupplierProduct
                            .Include(p => p.Supplier)
                            .Include(p => p.Product)
                            .Where(p => p.Product.Name == name && p.Supplier.Id == _SupplierId)
                            .FirstOrDefault();

                    if(product == null)
                    {
                        var productData = new Product()
                        {
                            Name = name,
                            Price = Convert.ToDecimal(price),
                            MinPrice = Convert.ToDecimal(price),
                            MaxPrice = Convert.ToDecimal(price),
                            CategoryId = "1",
                            BallAnswer = new Random().Next(1, 5),
                            BallBatery = new Random().Next(1, 5),
                            BallCamera = new Random().Next(1, 5),
                            BallDesign = new Random().Next(1, 5),
                            BallPortatable = new Random().Next(1, 5),
                        };
                        db.Product.Add(productData);
                        await db.SaveChangesAsync();

                        var supplierProduct = new SupplierProduct()
                        {
                            SupplierId = _SupplierId,
                            ProductId = productData?.Id
                        };
                        db.SupplierProduct.Add(supplierProduct);
                        await db.SaveChangesAsync();
                    }
                }
            }
        }
        public virtual async Task Update(ICollection<IWebElement> elements)
        {
            if (elements != null)
            {
                foreach (var item in elements)
                {
                    var nameElement = WaitDriver?.Until(d => item.FindElements(By.CssSelector("a.product-title__text")));
                    var priceElement = WaitDriver?.Until(d => item.FindElements(By.CssSelector("span.price__main-value")));

                    if (nameElement?.Count > 0 && priceElement?.Count > 0)
                    {
                        var name = TrimProductName?.TrimName(nameElement[0].Text);
                        var price = Convert.ToDecimal(priceElement[0].Text.Replace("&nbsp;", "").Replace("₽", ""));

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
