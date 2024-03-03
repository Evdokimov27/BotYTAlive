using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;
using SeleniumStealth.NET.Clients;
using SeleniumStealth.NET.Clients.Extensions;
using SeleniumStealth.NET.Clients.Models;
using SeleniumStealth.NET.Clients.Enums;
using static System.Windows.Forms.Design.AxImporter;
using OpenQA.Selenium.Support.Extensions;
using SeleniumUndetectedChromeDriver;
using OpenQA.Selenium.Chrome.ChromeDriverExtensions;
using OpenQA.Selenium.DevTools.V120.FedCm;

namespace YTVideoBot
{
	public partial class Form1 : Form
	{
		private UndetectedChromeDriver driver;
		private WebDriverWait wait;
		string[] mail = {

	"sanjeevibeird@gmail.com",
	"reappknowramistephanie@gmail.com",
	"foleymegan47@gmail.com",
	"lerimadonevans@gmail.com",
	"sporhuacadubabu@gmail.com",
	"troykeleher67@gmail.com",
	"cremestasip1986@gmail.com",
	"serscarrerenleslie@gmail.com",
	"mieletkeysepam@gmail.com",
	"exombronsouthmario@gmail.com",
	"locarecgajaime@gmail.com",
	"jimmypittman411@gmail.com",
	"checkponcoger1979@gmail.com",
	"enasobcyctiffany@gmail.com",
	"lisahibbard110@gmail.com",
	"allennina86@gmail.com",
	"brocterssicna1979@gmail.com",
	"mclainjack292@gmail.com",
	"carranzasarah463@gmail.com",
	"c3ifekasi@gmail.com",
};

		string[] password = {
	"rQDuAWNaiyf",
	"4bbYfILXMcoJ",
	"IQGa1zPbwwiwR",
	"wmhNeR7izhw3",
	"KCxgqAiLpIJ64a",
	"x64p6NZNq7Uj",
	"PSeKFpFBrwRK",
	"WIxJ9MSQTCDsQq",
	"fKZzX3Ur50dJMS",
	"44XN5DZ1GXgn",
	"ghtya2LXFw9Q",
	"UKZ97reyIxP",
	"MMa7qCUneJSdh",
	"lPL6mKwPh4hTd",
	"Owwdn8QyMoE",
	"XqHW39JQ4IuXk9",
	"gf6zSwpJJNz",
	"zfRr0xWVsrpA3g",
	"cpXiTqrJVCG",
	"QTQ8qhsnj47lt6",
};

		string[] proxy = { "65.109.112.245:9877", "51.195.51.101:3129" };
		public Form1()
		{
			InitializeComponent();
		}
		private async void InitializeSeleniumWebDriver(string proxy)
		{
			ChromeOptions chromeOptions = new ChromeOptions();
			chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
			chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
			chromeOptions.AddArgument($"--proxy-server=http://{proxy}");
			driver = UndetectedChromeDriver.Create(chromeOptions, null, driverExecutablePath: await new ChromeDriverInstaller().Auto());
			driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
			driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
			Thread.Sleep(5000);
		}

		private void OpenVideo(string videoName)
		{
			driver.Navigate().GoToUrl("https://www.youtube.com");
			Thread.Sleep(5000);
			((IJavaScriptExecutor)driver).ExecuteScript("window.stop();");
			var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("search_query")));
			searchBox.Click();
			searchBox.SendKeys(videoName + OpenQA.Selenium.Keys.Enter);


			var videoResult = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("ytd-video-renderer,ytd-grid-video-renderer")));
			if (videoResult != null)
			{
				videoResult.Click(); //  ликаем по первому видео в результате
				Thread.Sleep(5000);
				var like = wait.Until(ExpectedConditions.ElementExists(By.ClassName("YtLikeButtonViewModelHost")));
				like.Click();
				Thread.Sleep((int.Parse(textBox2.Text) * 1000) + 5000);
			}
			else
			{
				Console.WriteLine("¬идео не найдено.");
			}
			driver?.Quit();
		}
		private async void Auth(string mail, string password)
		{
			wait = new WebDriverWait(driver, TimeSpan.FromSeconds(1));
			driver.Navigate().GoToUrl("http://accounts.google.com/");
			Thread.Sleep(5000);
			var Auth = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("identifierId")));
			Auth.Click();
			Auth.SendKeys(mail + OpenQA.Selenium.Keys.Enter);
			Thread.Sleep(5000);
			var Pass = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Passwd")));
			Pass.Click();
			Pass.SendKeys(password + OpenQA.Selenium.Keys.Enter);
			Thread.Sleep(5000);
		}
		private void buttonSearch_Click(object sender, EventArgs e)
		{
			string videoName = textBox1.Text;
			if (!string.IsNullOrWhiteSpace(videoName))
			{
				for (int i = 0; i < mail.Length; i++)
				{
					InitializeSeleniumWebDriver(proxy[i]);
					Auth(mail[i], password[i]);
					OpenVideo(videoName);
				}
			}
			else
			{
				MessageBox.Show("¬ведите название видео.");
			}
		}
	}
}