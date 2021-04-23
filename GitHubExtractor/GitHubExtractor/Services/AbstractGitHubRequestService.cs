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

		protected string GetNumberAfterStringFromUrl(string url, string stringToFind, bool isMiddleOfString = false)
		{
			int indexOfIssuesKeyWord = url.IndexOf(stringToFind);
			string afterIssuesKeyword = url.Substring(indexOfIssuesKeyWord);

			int actionDividerIndex = afterIssuesKeyword.IndexOf("/");
			string dataAsString = afterIssuesKeyword.Substring(actionDividerIndex + 1);

			if (isMiddleOfString)
			{
				int actionDividerIndexEnd = dataAsString.IndexOf("/");
				dataAsString = dataAsString.Substring(0, actionDividerIndexEnd);
			}

			return dataAsString;
		}
	}
}
