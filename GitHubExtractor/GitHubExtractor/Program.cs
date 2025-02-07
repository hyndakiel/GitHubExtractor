using GitHubExtractor.Configs;
using GitHubExtractor.Dtos;
using GitHubExtractor.Services;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Files;
using NLog;
using System;
using System.Text;

namespace GitHubExtractor
{
	class Program
	{
		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		protected Program() { }

		static void Main()
		{
			InitializeApp();
			LOG.Info("INIT");
			try
			{
				var config = AppConfig.Instance;
				string gitRequestToken = config.GetConfig("gitHubToken");

				BasicAuth basicAuth = new BasicAuth(gitRequestToken);

				GitHubRequestDto gitHubRequestDto = new GitHubRequestDto(basicAuth);

				string connectionKey = "basePathGitHubApi";
				GitHubApiConnectionService gitHubApiConnectionService = new GitHubApiConnectionService(connectionKey);

				GitHubPullRequestService gitHubPullRequestService = new GitHubPullRequestService(gitHubRequestDto, gitHubApiConnectionService);
				GitHubIssuesRequestService gitHubIssuesRequestService = new GitHubIssuesRequestService(gitHubRequestDto, gitHubApiConnectionService);
				GitHubCommitRequestService gitHubCommitRequestService = new GitHubCommitRequestService(gitHubRequestDto, gitHubApiConnectionService);

				CsvFileCreator fileCreator = new CsvFileCreator();

				GitHubService gitHubService = new GitHubService(gitHubPullRequestService, gitHubIssuesRequestService, gitHubCommitRequestService, fileCreator);

				gitHubService.CreateFiles();

			}
			catch (Exception e)
			{
				LOG.Error(String.Format("Unexpected error: {0}", e));
			}
			LOG.Info("END");
		}

		private static void InitializeApp()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
		}
	}
}
