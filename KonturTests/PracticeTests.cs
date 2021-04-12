using System;
using System.Linq;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace KonturTests
{
    public class PracticeTests
    {
        public ChromeDriver driver;
        public WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            var options = new ChromeOptions();
            options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5)); //явные ожидания
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5); //неявные ожидания
        }

        private By emailInputLocator = By.Name("email");
        private By buttonLocator = By.Id("sendMe");
        private By radioButtonBoyLocator = By.Id("boy");
        private By radioButtonGirlLocator = By.Id("girl");
        private By emailResultLocator = By.ClassName("your-email");
        private By anotherEmailLinkLocator = By.Id("anotherEmail");
        private By formErrorLocator = By.ClassName("form-error");
        private By resultTextLocator = By.ClassName("result-text");
        private String expectedEmail = "test@mail.ru";
        private String url = "https://qa-course.kontur.host/selenium-practice";

        [Test]
        public void ParrotNameSite_FormFillWithEmail_CorrectEmail()
        {
            driver.Navigate().GoToUrl(url);

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultLocator).Text, "Сделали заявку не на тот e-mail");
        }
        [Test]
        public void ParrotNameSite_FillEmailChooseGirl_CorrectGender()
        {
            driver.Navigate().GoToUrl(url);

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(buttonLocator).Click();

            Assert.IsTrue(driver.FindElement(resultTextLocator).Text.Contains("девочки"), "Сделали заявку c другим полом");
        }

        [Test]
        public void ParrotNameSite_ClickAnotherEmailLink_EmailInputIsEmpty()
        {
            driver.Navigate().GoToUrl(url);

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле не очистилось");
        }
        [Test]
        public void ParrotNameSite_ClickAnotherEmailLink_RemovedAnotherEmailLink()
        {
            driver.Navigate().GoToUrl(url);

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.IsFalse(driver.FindElement(anotherEmailLinkLocator).Displayed, "Не исчезла ссылка для ввода другого e-mail");
        }

        [Test]
        public void ParrotNameSite_ChangeGenderBeforeClickAnotherEmailLink_GenderSaved()
        {
            driver.Navigate().GoToUrl(url);

            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(radioButtonBoyLocator).Click();
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            Assert.IsTrue(driver.FindElement(radioButtonGirlLocator).Selected, "Произошла смена пола");
        }

        [Test]
        public void ParrotNameSite_SendEmptyEmailField_ErrorOutput()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Введите email", driver.FindElement(formErrorLocator).Text, "При отправке пустого e-mail нет сообщения об ошибке");
        }

        [Test]
        public void ParrotNameSite_SendIncorrectEmail_ErrorOutput()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(radioButtonGirlLocator).Click();
            driver.FindElement(emailInputLocator).SendKeys("test");
            driver.FindElement(buttonLocator).Click();

            Assert.AreEqual("Некорректный email", driver.FindElement(formErrorLocator).Text, "При отправке некорректногоо e-mail нет сообщения об ошибке");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
