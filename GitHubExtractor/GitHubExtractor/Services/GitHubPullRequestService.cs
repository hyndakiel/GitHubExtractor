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

		public IEnumerable<PullRequestComment> Comments(int pullRequestNumber)
		{
			string url = string.Format("/repos/{0}/{1}/pulls/{2}/comments", GitRepoUserName, GitProject, pullRequestNumber);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(url, null, false, basicAuth, "GitHub");

			List<PullRequestComment> pullRequests = UtilitiesObj.JsonDeserializeObject<List<PullRequestComment>>(response);

			return pullRequests;
		}

		public IList<PullRequestResponse> List()
		{
			string url = string.Format("/repos/{0}/{1}/pulls", GitRepoUserName, GitProject);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			PullRequestParamns pullRequestParamns = new PullRequestParamns();
			pullRequestParamns.Sort = "created";
			pullRequestParamns.State = "all";

			string response = gitHubApiConnectionService.AccessEndPoint(url, pullRequestParamns, false, basicAuth, "GitHub");

			List<PullRequestResponse> pullRequests = UtilitiesObj.JsonDeserializeObject<List<PullRequestResponse>>(response);

			return pullRequests;
		}
	}
}
