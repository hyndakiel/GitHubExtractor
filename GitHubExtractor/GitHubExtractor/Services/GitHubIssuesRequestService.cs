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

			IssueCommentsRequestParamns issuesRequestParamns = new IssueCommentsRequestParamns();
			const int maxPerPageAllowed = 100;
			issuesRequestParamns.PerPage = maxPerPageAllowed;
			issuesRequestParamns.Page = 1;

			List<IssueCommentResponse> issueComments = GetPaginatedData<IssueCommentResponse>(issuesRequestParamns, urlToUse);

			return issueComments;
		}

		public IList<IssueResponse> List()
		{
			string url = string.Format("/repos/{0}/{1}/issues", GitRepoUserName, GitProject);

			IssuesRequestParamns issuesRequestParamns = new IssuesRequestParamns();
			issuesRequestParamns.Sort = "created";
			issuesRequestParamns.State = "closed";
			const int maxPerPageAllowed = 100;
			issuesRequestParamns.PerPage = maxPerPageAllowed;
			issuesRequestParamns.Page = 1;

			List<IssueResponse> issues = GetPaginatedData<IssueResponse>(issuesRequestParamns, url);

			return issues;
		}
	}
}
