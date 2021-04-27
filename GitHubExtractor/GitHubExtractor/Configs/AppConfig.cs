using Microsoft.Extensions.Configuration;

namespace GitHubExtractor.Configs
{
	public class AppConfig
	{
		private static AppConfig _instance;
		public static AppConfig Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new AppConfig();
				}
				return _instance;
			}
			private set
			{
				//does nothing
			}
		}

		private IConfiguration Configuration { get; set; }

		private AppConfig()
		{
			IConfiguration configuration = new ConfigurationBuilder()
				.AddJsonFile("Configs/appsettings.json", optional: false, reloadOnChange: true)
				.Build();

			Configuration = configuration;
		}

		public string GetConfig(string key)
		{
			string config = Configuration[key];
			return config;
		}

		public bool GetConfigBool(string key)
		{
			string valueAsString = GetConfig(key);
			return bool.Parse(valueAsString);
		}
	}
}
