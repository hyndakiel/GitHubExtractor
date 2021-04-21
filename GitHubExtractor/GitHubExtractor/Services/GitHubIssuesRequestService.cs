using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;

namespace GitHubExtractor.Services
{
	public class GitHubIssuesRequestService : AbstractGitHubRequestService, IGitHubIssuesRequestService
	{
		public GitHubIssuesRequestService(GitHubRequestDto gitHubRequestDto, GitHubApiConnectionService gitHubApiConnectionService) : base(gitHubRequestDto, gitHubApiConnectionService)
		{
		}

		public IssueResponse Get(string url)
		{
			string issueNumberAsString = GetIssueNumberFromUrl(url);

			string urlToUse = string.Format("/repos/{0}/{1}/issues/{2}", GitRepoUserName, GitProject, issueNumberAsString);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(urlToUse, null, false, basicAuth, "GitHub");

			IssueResponse issue = UtilitiesObj.JsonDeserializeObject<IssueResponse>(response);

			return issue;
		}

		private string GetIssueNumberFromUrl(string url)
		{
			int indexOfIssuesKeyWord = url.IndexOf("issues");
			string afterIssuesKeyword = url.Substring(indexOfIssuesKeyWord);

			int actionDividerIndex = url.IndexOf("/");
			string issueNumberAsString = afterIssuesKeyword.Substring(actionDividerIndex + 1);

			return issueNumberAsString;
		}
	}
}
