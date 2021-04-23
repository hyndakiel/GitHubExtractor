using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;

namespace GitHubExtractor.Services
{
	public class GitHubCommitRequestService : AbstractGitHubRequestService, IGitHubCommitRequestService
	{
		public GitHubCommitRequestService(GitHubRequestDto gitHubRequestDto, GitHubApiConnectionService gitHubApiConnectionService) : base(gitHubRequestDto, gitHubApiConnectionService)
		{
		}

		public Commit GetCommits(string sha)
		{
			string urlToUse = string.Format("/repos/{0}/{1}/commits/{2}", GitRepoUserName, GitProject, sha);

			BasicAuth basicAuth = BasicAuth;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;
			string response = gitHubApiConnectionService.AccessEndPoint(urlToUse, null, false, basicAuth, "GitHub");

			Commit commits = UtilitiesObj.JsonDeserializeObject<Commit>(response);

			return commits;
		}
	}
}
