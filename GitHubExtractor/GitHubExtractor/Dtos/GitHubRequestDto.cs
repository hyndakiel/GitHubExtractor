using GitHubExtractor.Configs;
using GitHubExtractor.Services.Connection;

namespace GitHubExtractor.Dtos
{
	public class GitHubRequestDto
	{
		public string GitRepoUserName { get; }
		public string GitProject { get; }
		public BasicAuth BasicAuth { get; }

		public GitHubRequestDto(BasicAuth basicAuth)
		{
			AppConfig instance = AppConfig.Instance;
			GitRepoUserName = instance.GetConfig("RepoOwner");
			GitProject = instance.GetConfig("RepoName");
			BasicAuth = basicAuth;
		}
	}
}
