using GitHubExtractor.Dtos;
using GitHubExtractor.Services;
using GitHubExtractor.Services.Connection;
using NLog;
using System;
using System.Text;

namespace GitHubExtractor
{
	class Program
	{
		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		protected Program() { }

		static void Main(string[] args)
		{
			InitializeApp();
			LOG.Info("INIT");
			try
			{
				string gitRepoUserName = args[0];
				string gitProject = args[1];
				string gitRequestUser = args[2];
				string gitRequestToken = args[3];

				BasicAuth basicAuth = new BasicAuth(gitRequestUser, gitRequestToken);

				GitHubRequestDto gitHubRequestDto = new GitHubRequestDto(gitRepoUserName, gitProject, basicAuth);

				string connectionKey = "basePathGitHubApi";
				GitHubApiConnectionService gitHubApiConnectionService = new GitHubApiConnectionService(connectionKey);

				GitHubPullRequestService gitHubPullRequestService = new GitHubPullRequestService(gitHubRequestDto, gitHubApiConnectionService);
				GitHubIssuesRequestService gitHubIssuesRequestService = new GitHubIssuesRequestService(gitHubRequestDto, gitHubApiConnectionService);

				GitHubService gitHubService = new GitHubService(gitHubPullRequestService, gitHubIssuesRequestService);

				gitHubService.CreatePullRequestCSVFile();
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
