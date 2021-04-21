using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using System.Collections.Generic;

namespace GitHubExtractor.Services
{
	public class GitHubPullRequestService : IGitHubPullRequestService
	{
		public string GitUserName { get; set; }
		public string GitProject { get; set; }
		public GitHubApiConnectionService GitHubApiConnectionService { get; set; }
		public BasicAuth BasicAuth { get; private set; }

		public GitHubPullRequestService(string gitUserName, string gitProject, GitHubApiConnectionService gitHubApiConnectionService)
		{
			GitUserName = gitUserName;
			GitProject = gitProject;
			GitHubApiConnectionService = gitHubApiConnectionService;
			BasicAuth = CreateBasicAuth();
		}

		public IEnumerable<object> List()
		{
			string url = string.Format("/repos/{0}/{1}/pulls", GitUserName, GitProject);

			BasicAuth basicAuth = BasicAuth;


			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(url, null, false, basicAuth, "GitHub");

			return new List<object>();
		}

		private BasicAuth CreateBasicAuth()
		{
			return new BasicAuth("hyndakiel", "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");
		}
	}
}
