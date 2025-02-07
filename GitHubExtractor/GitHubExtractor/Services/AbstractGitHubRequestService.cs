using GitHubExtractor.Dtos;
using GitHubExtractor.Dtos.Interfaces;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

		private void ExtractLastPageInfo(string linkHeader, ref int lastPage)
		{
			string[] links = linkHeader.Split(",");
			foreach (string link in links)
			{
				if (link.Contains("last"))
				{
					int pageIndex = link.IndexOf("&page=");
					int stopCharIndexIndex = link.IndexOf(">");
					int diffIndex = stopCharIndexIndex - pageIndex;

					string lastPageStr = link.Substring(pageIndex, diffIndex);

					string lastPageNumberAsStr = Regex.Match(lastPageStr, @"\d+").Value;

					int page = int.Parse(lastPageNumberAsStr);

					lastPage = page;
					break;
				}
			}
		}

		private bool IsLastPage(int lastPage, int page)
		{
			return lastPage <= page;
		}

		protected List<T> GetPaginatedData<T>(IPaginatedParamns paramns, string url)
		{
			bool isGettingPages = true;
			int lastPage = 0;

			GitHubApiConnectionService gitHubApiConnectionService = GitHubApiConnectionService;

			List<T> data = new List<T>();
			while (isGettingPages)
			{
				int page = paramns.Page;

				string response = gitHubApiConnectionService.AccessEndPoint(url, paramns, false, BasicAuth, "GitHub");
				List<T> partialData = UtilitiesObj.JsonDeserializeObject<List<T>>(response);

				LastRequestInfo lastRequestInfo = gitHubApiConnectionService.LastRequestInfo;
				string linkHeader = lastRequestInfo.Headers["Link"];
				if (lastPage == 0 && !string.IsNullOrEmpty(linkHeader))
				{
					ExtractLastPageInfo(linkHeader, ref lastPage);
				}

				if (IsLastPage(lastPage, page))
				{
					isGettingPages = false;
				}

				paramns.Page += 1;

				data.AddRange(partialData);
			}

			return data;
		}
	}
}
