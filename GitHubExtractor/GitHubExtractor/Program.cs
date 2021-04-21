using GitHubExtractor.Services;
using GitHubExtractor.Services.Connection;
using NLog;
using System;
using System.Text;

namespace GitHubExtractor
{
	class Program
	{
		public static Logger LOG = LogManager.GetCurrentClassLogger();

		static void Main(string[] args)
		{
			InitializeApp();
			LOG.Info("INIT");
			try
			{
				string gitUserName = args[0];
				string gitProject = args[1];

				string connectionKey = "basePathGitHubApi";
				GitHubApiConnectionService gitHubApiConnectionService = new GitHubApiConnectionService(connectionKey);
				GitHubPullRequestService gitHubPullRequestService = new GitHubPullRequestService(gitUserName, gitProject, gitHubApiConnectionService);
				GitHubService gitHubService = new GitHubService(gitHubPullRequestService);

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
