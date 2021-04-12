using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace KonturTests
{
    public class LessonTests
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

        [Test]
        public void Wikipedia_Search_TitleIsCorrect()
        {
            //перешли на страницу
            driver.Navigate().GoToUrl("https://ru.wikipedia.org/");

            //ищем элементы
            var search = driver.FindElement(By.Name("search"));
            var searchButton = driver.FindElement(By.Name("go"));

            //взаимодействуем с элементами
            search.SendKeys("Selenium");
            searchButton.Click();

            //проверки в тесте
            Assert.IsTrue(driver.Title.Contains("Selenium — Википедия"), "Неверный заголовок страницы при переходе из поиска");
        }

   
        private By emailInputLocator = By.ClassName("email");
        private By buttonLocator = By.Id("write-to-me");
        private By emailResultLocator = By.Name("result-email");
        private By anotherEmailLinkLocator = By.LinkText("указать другой e-mail");

        [Test]
        public void ComputerSite_FormFillWithEmail_Success()
        {
            //перешли на страницу
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-lesson");

            //заполнить форму и кликнуть
            var expectedEmail = "test@mail.ru";
            driver.FindElement(emailInputLocator).SendKeys(expectedEmail);
            driver.FindElement(buttonLocator).Click();        

            //проверки
            Assert.AreEqual(expectedEmail, driver.FindElement(emailResultLocator).Text, "Сделали заявку не на тот e-mail");
        }

        [Test]
        public void ComputerSite_ClickAnotherEmail_EmailInputIsEmpty()
        {
            //перешли на страницу
            driver.Navigate().GoToUrl("https://qa-course.kontur.host/selenium-lesson");

            //заполнить форму и кликнуть
            driver.FindElement(emailInputLocator).SendKeys("test@mail.ru");
            driver.FindElement(buttonLocator).Click();
            driver.FindElement(anotherEmailLinkLocator).Click();

            //проверки
            Assert.AreEqual(string.Empty, driver.FindElement(emailInputLocator).Text, "После клика по ссылке поле не очистилось");
            Assert.IsTrue(driver.FindElements(anotherEmailLinkLocator).Count == 0, "Не исчезла ссылка для ввода другого e-mail");
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }
    }
}
