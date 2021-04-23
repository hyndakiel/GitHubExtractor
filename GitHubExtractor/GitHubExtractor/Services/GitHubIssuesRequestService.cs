using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;
using System.Collections.Generic;

namespace GitHubExtractor.Services
{
	public class GitHubIssuesRequestService : AbstractGitHubRequestService, IGitHubIssuesRequestService
	{
		public GitHubIssuesRequestService(GitHubRequestDto gitHubRequestDto, GitHubApiConnectionService gitHubApiConnectionService) : base(gitHubRequestDto, gitHubApiConnectionService)
		{
		}

		public IssueResponse Get(int number)
		{
			string urlToUse = string.Format("/repos/{0}/{1}/issues/{2}", GitRepoUserName, GitProject, number);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(urlToUse, null, false, basicAuth, "GitHub");

			IssueResponse issue = UtilitiesObj.JsonDeserializeObject<IssueResponse>(response);

			return issue;
		}

		public IEnumerable<IssueCommentResponse> GetIssueComments(int issueNumber)
		{
			string urlToUse = string.Format("/repos/{0}/{1}/issues/{2}/comments", GitRepoUserName, GitProject, issueNumber);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(urlToUse, null, false, basicAuth, "GitHub");

			IEnumerable<IssueCommentResponse> issueComments = UtilitiesObj.JsonDeserializeObject<IEnumerable<IssueCommentResponse>>(response);

			return issueComments;
		}
	}
}
