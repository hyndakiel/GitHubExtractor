using GitHubExtractor.Dtos;
using GitHubExtractor.Services.Connection;

namespace GitHubExtractor.Services
{
	public abstract class AbstractGitHubRequestService
	{
		protected string GitRepoUserName { get; set; }
		protected string GitProject { get; set; }
		protected GitHubApiConnectionService GitHubApiConnectionService { get; set; }
		protected BasicAuth BasicAuth { get; set; }

		protected AbstractGitHubRequestService(GitHubRequestDto gitHubRequestDto, GitHubApiConnectionService gitHubApiConnectionService)
		{
			GitRepoUserName = gitHubRequestDto.GitRepoUserName;
			GitProject = gitHubRequestDto.GitProject;
			BasicAuth = gitHubRequestDto.BasicAuth;
			GitHubApiConnectionService = gitHubApiConnectionService;
		}
	}
}
