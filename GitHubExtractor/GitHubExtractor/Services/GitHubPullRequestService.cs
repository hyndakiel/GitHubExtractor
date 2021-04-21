using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;
using System.Collections.Generic;

namespace GitHubExtractor.Services
{
	public class GitHubPullRequestService : AbstractGitHubRequestService, IGitHubPullRequestService
	{
		public GitHubPullRequestService(GitHubRequestDto gitHubRequestDto, GitHubApiConnectionService gitHubApiConnectionService) : base(gitHubRequestDto, gitHubApiConnectionService)
		{
		}

		public IEnumerable<PullRequestResponse> List()
		{
			string url = string.Format("/repos/{0}/{1}/pulls", GitRepoUserName, GitProject);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(url, null, false, basicAuth, "GitHub");

			List<PullRequestResponse> pullRequests = UtilitiesObj.JsonDeserializeObject<List<PullRequestResponse>>(response);

			return pullRequests;
		}
	}
}
