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
using System.Security.Policy;
using System.Reflection.Metadata;
using System.Collections.ObjectModel;
using static System.Net.Mime.MediaTypeNames;

namespace YTVideoBot
{
	public partial class Form1 : Form
	{
		private UndetectedChromeDriver driver;
		private string folder;
		List<(string Email, string Password, string Proxy, string Login, string LoginPassword)> ReadAccountData(string filePath)
		{
			var accounts = new List<(string, string, string, string, string)>();
			foreach (var line in File.ReadAllLines(filePath))
			{
				var parts = line.Split(';');
				var account = (parts[0], parts[1], parts.Length > 2 ? parts[2] : "", parts.Length > 3 ? parts[3] : "", parts.Length > 4 ? parts[4] : "");
				accounts.Add(account);
			}
			return accounts;
		}
		public Form1()
		{
			InitializeComponent();
			folder = Properties.Settings.Default.Folder;
			textBox1.Text = Properties.Settings.Default.VideoName;
		}

		private async void InitializeSeleniumWebDriver(string? proxy)
		{
			ChromeOptions chromeOptions = new ChromeOptions();
			if (!string.IsNullOrEmpty(proxy))
			{
				chromeOptions.AddArgument($"--proxy-server={proxy}");
			}
			chromeOptions.PageLoadStrategy = PageLoadStrategy.None;
			chromeOptions.AddArgument("--disable-blink-features=AutomationControlled");
			chromeOptions.AddArgument("--mute-audio");
			driver = UndetectedChromeDriver.Create(chromeOptions, null, driverExecutablePath: await new ChromeDriverInstaller().Auto());
		}

		private async Task OpenVideoAsync(string videoName)
		{
			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(300));

			driver.Navigate().GoToUrl("https://www.youtube.com");
			await Task.Delay(2000); // Используем асинхронную задержку вместо Thread.Sleep
			var searchBox = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("search_query")));
			searchBox.Click();
			searchBox.SendKeys(videoName);
			await Task.Delay(1000); // Используем асинхронную задержку вместо Thread.Sleep

			var search = wait.Until(ExpectedConditions.ElementExists(By.Id("search-icon-legacy")));
			await Task.Delay(2000); // Используем асинхронную задержку вместо Thread.Sleep
			search.Click();

			var videoResult = wait.Until(ExpectedConditions.ElementExists(By.CssSelector("ytd-video-renderer,ytd-grid-video-renderer")));
			if (videoResult != null)
			{
				videoResult.Click(); // Кликаем по первому видео в результате
				Thread.Sleep(3000);
				if (checkBox1.Checked && GetVideoLike(driver) == "I like this")
				{
					var like = wait.Until(ExpectedConditions.ElementExists(By.ClassName("YtLikeButtonViewModelHost")));
					like.Click();
				}
				if (checkBox2.Checked && GetVideoSub(driver))
				{
					var sub = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("subscribe-button-shape")));
					sub.Click();
				}
				int durationInWholeSeconds = GetVideoDurationInSeconds(driver);
				await Task.Delay((durationInWholeSeconds * 1000) + 5000); // Используем асинхронную задержку
			}
			else
			{
				Console.WriteLine("Видео не найдено.");
			}
			driver?.Quit();
		}
		public int GetVideoDurationInSeconds(IWebDriver driver)
		{
			IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
			string script = "return document.querySelector('video').duration;";
			var durationInSeconds = jsExecutor.ExecuteScript(script);

			return Convert.ToInt32(Math.Round(Convert.ToDouble(durationInSeconds)));
		}

		public string GetVideoLike(IWebDriver driver)
		{
			IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
			string script = @"
			var parentElement = document.querySelector('.yt-spec-button-view-model');
			var children = parentElement.children;
			var titles = [];
			for (var i = 0; i < children.length; i++)
			{
				if (children[i].title) 
				{
					titles.push(children[i].title);
				}
			}
			return titles;
			";
			var titles = (ReadOnlyCollection<object>)jsExecutor.ExecuteScript(script);
			string tile = "";
			// Выводим полученные заголовки
			foreach (var title in titles)
			{
				tile = title.ToString();
			}
			return tile;
		}
		public bool GetVideoSub(IWebDriver driver)
		{
			bool hasSubscribe = false; 
			IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
			string script = @"
			var elements = document.querySelectorAll('.yt-core-attributed-string.yt-core-attributed-string--white-space-no-wrap');
			var texts = [];
			for (var i = 0; i < elements.length && i < 4; i++)
			{
			    texts.push(elements[i].textContent.trim());
			}
			return texts;
			";

			// Выполнение JavaScript скрипта и получение результатов
			var texts = (ReadOnlyCollection<object>)jsExecutor.ExecuteScript(script);

			// Вывод текстов найденных элементов
			foreach (var text in texts)
			{
				if (text.ToString() == "Subscribe")
				{
					hasSubscribe = true;
				}
			}
			return hasSubscribe;
		}
		private async void Auth(string mail, string password)
		{
			WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(300));

			bool isPageLoadedSuccessfully;
			do
			{
				driver.Navigate().GoToUrl("http://accounts.google.com/");
				Thread.Sleep(2000); // Задержка для загрузки страницы, лучше использовать явные ожидания

				// Проверяем, есть ли на странице контейнер с ошибкой
				var errorElements = driver.FindElements(By.Id("af-error-container"));
				isPageLoadedSuccessfully = !errorElements.Any(); // Страница загружена успешно, если элемент с ошибкой не найден

				if (!isPageLoadedSuccessfully)
				{
					Console.WriteLine("Обнаружена ошибка 400, пытаемся перезагрузить страницу...");
					// Можно добавить задержку перед повторной попыткой, если это необходимо
					Thread.Sleep(5000);
				}
			}
			while (!isPageLoadedSuccessfully);

			// Когда страница загружена успешно, выполняем дальнейшие действия
			var Auth = wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("identifierId")));
			Auth.Click();
			Auth.SendKeys(mail + OpenQA.Selenium.Keys.Enter);

			Thread.Sleep(2000); // Лучше использовать явные ожидания

			var Pass = wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("Passwd")));
			Pass.Click();
			Pass.SendKeys(password + OpenQA.Selenium.Keys.Enter);
			Thread.Sleep(5000); // Лучше использовать явные ожидания


		}

		private async void buttonSearch_Click(object sender, EventArgs e)
		{
			string videoName = textBox1.Text;
			var accounts = ReadAccountData($"{folder}");
			if (!string.IsNullOrWhiteSpace(videoName))
			{
				for (int i = 0; i < accounts.Count; i++)
				{
					InitializeSeleniumWebDriver(accounts[i].Proxy);
					Auth(accounts[i].Email, accounts[i].Password);
					await OpenVideoAsync(videoName);
				}
			}
			else
			{
				MessageBox.Show("Введите название видео.");
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				if (folder != "") openFileDialog.InitialDirectory = folder;
				else openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "txt files (*.txt)|*.txt";
				openFileDialog.FilterIndex = 2;
				openFileDialog.RestoreDirectory = true;

				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					string filePath = openFileDialog.FileName;
					folder = filePath;
				}
				Properties.Settings.Default.Folder = folder;
				Properties.Settings.Default.VideoName = textBox1.Text;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
			driver?.Quit();
		}
	}
}