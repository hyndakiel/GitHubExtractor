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
				string gitRepoUserName = args[0];
				string gitProject = args[1];
				string gitRequestUser = args[2];
				string gitRequestToken = args[3];

				BasicAuth basicAuth = new BasicAuth(gitRequestUser, gitRequestToken);

				string connectionKey = "basePathGitHubApi";
				GitHubApiConnectionService gitHubApiConnectionService = new GitHubApiConnectionService(connectionKey);
				GitHubPullRequestService gitHubPullRequestService = new GitHubPullRequestService(gitRepoUserName, gitProject, basicAuth, gitHubApiConnectionService);
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
