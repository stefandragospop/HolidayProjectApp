using System;
using System.Drawing.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Assert = NUnit.Framework.Assert;


namespace Holiday.Test
{


    [TestClass]
    public class UnitTest1
    {
        [Test]
        public void TC001_OpenHolidaySite_Smoke_Testing()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.Manage().Window.Maximize();
        }

        [Test]

        public void TC002_BehaviourOfLinkAllHolidayIfTheUserIsNotLogin()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.XPath("/html/body/header/nav/div/div/ul[2]/li/a")).Click();//verify and put ID 
            Assert.AreEqual("Log in - Holidays App", driver.Title);
        }

        [Test]

        public void TC003_BehaviourOfLinkMyHolidayIfTheUserIsNotLogin()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.ClassName("navbar-brand")).Click();
            Assert.AreEqual("Log in - Holidays App",driver.Title);
        }

        [Test]

        public void TC004_ForgotPassword()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.Id("forgot-password")).Click();
            driver.FindElement(By.Name("Input.Email")).SendKeys("anamaria1@bt.com");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            IWebElement paragraph = driver.FindElement(By.TagName("p"));
            Assert.IsTrue(paragraph.Text.Contains("Please check your email to reset your password."));
            Console.WriteLine("The Page contains the text 'Please check your email to reset your password.' "); 
        }

        [Test]

        public void TC005_EmptyEmailFieldForgotPassword()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.Id("forgot-password")).Click();
            driver.FindElement(By.ClassName("btn-primary")).Click();
            IWebElement errormessage = driver.FindElement(By.Id("Input_Email-error"));
            Assert.IsTrue(errormessage.Text.Contains("The Email field is required."));
            ITakesScreenshot screenshotdriver2 = driver.FindElement(By.ClassName("pb-3")) as ITakesScreenshot;
            Screenshot screenshot1 = screenshotdriver2.GetScreenshot();
            screenshot1.SaveAsFile(fileName: @"C:\Users\manue\Desktop\ForgotPassword.png", ScreenshotImageFormat.Png);
        }

        [Test]

        public void TC006_IncorrectEmailOnForgotPasswordField()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.Id("forgot-password")).Click();
            driver.FindElement(By.Name("Input.Email")).SendKeys("111");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            IWebElement errormessage = driver.FindElement(By.Id("Input_Email-error"));
            Assert.IsTrue(errormessage.Text.Contains("The Email field is not a valid e-mail address."));
            ITakesScreenshot screenshotdriver2 = driver.FindElement(By.ClassName("pb-3")) as ITakesScreenshot;
            Screenshot screenshot1 = screenshotdriver2.GetScreenshot();
            screenshot1.SaveAsFile(fileName: @"C:\Users\manue\Desktop\ForgotPasswordIncorrectEmailField.png", ScreenshotImageFormat.Png);
        }

        [Test]

        public void TC007_RegisterNewUser()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.XPath("//*[@id='account']/div[6]/p[2]/a")).Click();
            IWebElement fullname = driver.FindElement(By.Name("Input.FullName"));
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            IWebElement confirmpassword = driver.FindElement(By.Name("Input.ConfirmPassword"));

            Random rnd = new Random(DateTime.Now.Millisecond);
            string nume = String.Format($"AnaMaria{rnd.Next(1, 1000)}");
            string email = String.Format($"ana{rnd.Next(10000)}maria{rnd.Next(10000)}@gmail.com");
            string pass = String.Format($"AnaMaria?{rnd.Next(DateTime.Now.Millisecond)}");
            string confirm = pass;

            fullname.SendKeys(nume);
            emailfield.SendKeys(email);
            password.SendKeys(pass);
            confirmpassword.SendKeys(confirm);

            driver.FindElement(By.ClassName("btn-primary")).Click();
            Assert.AreEqual("My Holidays - Holidays App", driver.Title);
            ITakesScreenshot screenshotdriver2 = driver.FindElement(By.ClassName("pb-3")) as ITakesScreenshot;
            Screenshot screenshot1 = screenshotdriver2.GetScreenshot();
            screenshot1.SaveAsFile(fileName: @"C:\Users\manue\Desktop\RegisterUser.png", ScreenshotImageFormat.Png);
         
        }

        [Test]

        public void TC008__CreateAccountIncorrectEmailFormat()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/Identity/Account/Login?ReturnUrl=%2F";
            driver.FindElement(By.XPath("//*[@id='account']/div[6]/p[2]/a")).Click();
            IWebElement fullname = driver.FindElement(By.Name("Input.FullName"));
            fullname.Click();
            fullname.SendKeys("AnaMaria Bob");
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("anabt.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            IWebElement confirmpassword = driver.FindElement(By.Name("Input.ConfirmPassword"));

            Random rnd = new Random(DateTime.Now.Millisecond);
            string pass = String.Format($"AnaMaria?{rnd.Next(DateTime.Now.Millisecond)}");
            string confirm = pass;

            password.SendKeys(pass);
            confirmpassword.SendKeys(confirm);

            driver.FindElement(By.ClassName("btn-primary")).Click();
            IWebElement errormessage = driver.FindElement(By.Id("Input_Email-error"));
            Assert.IsTrue(errormessage.Text.Contains("The Email field is not a valid e-mail address."));
        }

        [Test]

        public void TC009_CreateHolidaywithInvalidYearOnStartDate()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/";
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("manuelaporfire@gmail.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            password.Click();
            password.SendKeys("VY!N3vt9usCSAF5");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            driver.FindElement(By.ClassName("fa-plus-circle")).Click();
            IWebElement startdate = driver.FindElement(By.Id("StartDate"));
            startdate.Click();
            startdate.SendKeys("06/03/20202");
            IWebElement enddate = driver.FindElement(By.Id("EndDate"));
            enddate.Click();
            enddate.SendKeys("06/01/2020");
            var type = driver.FindElement(By.Id("Type"));
            var selectelement = new SelectElement(type);
            selectelement.SelectByValue("2");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            IWebElement errormessage = driver.FindElement(By.ClassName("field-validation-error"));
            Assert.IsTrue(errormessage.Text.Contains("The value '20202-06-03' is not valid for Start Date."));

        }

        [Test]

        public void TC010_CreateHolidayWithInvalidYearOnEndDate()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/";
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("manuelaporfire@gmail.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            password.Click();
            password.SendKeys("VY!N3vt9usCSAF5");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            driver.FindElement(By.ClassName("fa-plus-circle")).Click();
            IWebElement startdate = driver.FindElement(By.Id("StartDate"));
            startdate.Click();
            startdate.SendKeys("06/01/2020");
            IWebElement enddate = driver.FindElement(By.Id("EndDate"));
            enddate.Click();
            enddate.SendKeys("06/03/20202");
            var type = driver.FindElement(By.Id("Type"));
            var selectelement = new SelectElement(type);
            selectelement.SelectByText("Medical");
            driver.FindElement(By.XPath("/html/body/div/main/div[2]/div/form/div[4]/input")).Click();
            IWebElement errormessage = driver.FindElement(By.ClassName("field-validation-error"));
            Assert.IsTrue(errormessage.Text.Contains("The value '20202-06-03' is not valid for End Date."));
        }

        [Test]

        public void TC011_CreateHolidayWithEmptyField()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/";
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("manuelaporfire@gmail.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            password.Click();
            password.SendKeys("VY!N3vt9usCSAF5");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            driver.FindElement(By.ClassName("fa-plus-circle")).Click();
            driver.FindElement(By.XPath("/html/body/div/main/div[2]/div/form/div[4]/input")).Click();
            //de completat
        }
        [Test]

        public void TC012_TheBehaviourOfBackToListButtonOnCreatePage()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/";
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("manuelaporfire@gmail.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            password.Click();
            password.SendKeys("VY!N3vt9usCSAF5");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            driver.FindElement(By.ClassName("fa-plus-circle")).Click();
            driver.FindElement(By.ClassName("fa-arrow-circle-left")).Click();
            Assert.AreEqual("My Holidays - Holidays App", driver.Title);
        }

        [Test]

        public void TC013_EditHolidayRequest()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Url = "https://localhost:44382/";
            IWebElement emailfield = driver.FindElement(By.Name("Input.Email"));
            emailfield.Click();
            emailfield.SendKeys("manuelaporfire@gmail.com");
            IWebElement password = driver.FindElement(By.Name("Input.Password"));
            password.Click();
            password.SendKeys("VY!N3vt9usCSAF5");
            driver.FindElement(By.ClassName("btn-primary")).Click();
            driver.FindElement(By.ClassName("fa-edit")).Click();
            IWebElement editstartdate = driver.FindElement(By.Id("StartDate"));
            editstartdate.Click();
            editstartdate.SendKeys("04/06/2020");
            IWebElement editenddate = driver.FindElement(By.Id("EndDate"));
            editenddate.Click();
            editenddate.SendKeys("04/10/2020");
            var type = driver.FindElement(By.Id("Type"));
            var selectelement = new SelectElement(type);
            selectelement.SelectByText("Paid");
            driver.FindElement(By.CssSelector("body > div > main > div:nth-child(5) > div > form > div:nth-child(7) > input")).Click();
            Assert.AreEqual("My Holidays - Holidays App", driver.Title);
            IWebElement startdateedit = driver.FindElement(By.Id("holidaysTable"));
            Assert.IsTrue(startdateedit.Text.Contains("4/6/2020"));
        }

        [Test]

        public void TC014_TheBehaviourOfModifiedOnColumnAfterEditAHolidayRequest()
        {

        }
    }
    
}
