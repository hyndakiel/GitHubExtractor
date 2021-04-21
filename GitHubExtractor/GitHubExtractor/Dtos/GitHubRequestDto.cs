using GitHubExtractor.Services.Connection;

namespace GitHubExtractor.Dtos
{
	public class GitHubRequestDto
	{
		public string GitRepoUserName { get; }
		public string GitProject { get; }
		public BasicAuth BasicAuth { get; }

		public GitHubRequestDto(string gitRepoUserName, string gitProject, BasicAuth basicAuth)
		{
			GitRepoUserName = gitRepoUserName;
			GitProject = gitProject;
			BasicAuth = basicAuth;
		}
	}
}
