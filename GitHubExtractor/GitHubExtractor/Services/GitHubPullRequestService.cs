using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Connection;
using GitHubExtractor.Services.Interfaces;
using GitHubExtractor.Utils;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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
			pullRequestParamns.State = "closed";
			const int maxPerPageAllowed = 100;
			pullRequestParamns.PerPage = maxPerPageAllowed;
			pullRequestParamns.Page = 1;

			List<PullRequestResponse> pullRequests = new List<PullRequestResponse>();
			bool isGettingPages = true;
			int lastPage = 0;
			while (isGettingPages)
			{
				int page = pullRequestParamns.Page;
				string response = gitHubApiConnectionService.AccessEndPoint(url, pullRequestParamns, false, basicAuth, "GitHub");
				List<PullRequestResponse> data = UtilitiesObj.JsonDeserializeObject<List<PullRequestResponse>>(response);

				LastRequestInfo lastRequestInfo = gitHubApiConnectionService.LastRequestInfo;
				string linkHeader = lastRequestInfo.Headers["Link"];
				if (lastPage == 0)
				{
					ExtractLastPageInfo(linkHeader, ref lastPage);
				}

				if (IsLastPage(lastPage, page))
				{
					isGettingPages = false;
				}

				pullRequestParamns.Page += 1;

				pullRequests.AddRange(data);
			}


			return pullRequests;
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
			bool ret = false;
			if (lastPage == page)
			{
				ret = true;
			}

			return ret;
		}
	}
}
