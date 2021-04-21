using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;
using System.Collections.Generic;

namespace GitHubExtractor.Services
{
	public class GitHubPullRequestService : IGitHubPullRequestService
	{
		public string GitUserName { get; set; }
		public string GitProject { get; set; }
		public string GitToken { get; set; }
		public GitHubApiConnectionService GitHubApiConnectionService { get; set; }
		public BasicAuth BasicAuth { get; private set; }

		public GitHubPullRequestService(string gitUserName, string gitProject, BasicAuth basicAuth, GitHubApiConnectionService gitHubApiConnectionService)
		{
			GitUserName = gitUserName;
			GitProject = gitProject;
			GitHubApiConnectionService = gitHubApiConnectionService;
			BasicAuth = basicAuth;
		}

		public IEnumerable<object> List()
		{
			string url = string.Format("/repos/{0}/{1}/pulls", GitUserName, GitProject);

			BasicAuth basicAuth = BasicAuth;


			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(url, null, false, basicAuth, "GitHub");

			List<dynamic> pullRequests = UtilitiesObj.JsonDeserializeObject<List<dynamic>>(response);

			return pullRequests;
		}
	}
}
