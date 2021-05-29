using CsvHelper.Configuration;
using GitHubExtractor.Configs;
using GitHubExtractor.Dtos;
using GitHubExtractor.Dtos.Interfaces;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Files;
using GitHubExtractor.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace GitHubExtractor.Services
{
	public class GitHubService : IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }

		public IGitHubIssuesRequestService GitHubIssuesRequestService { get; set; }

		public IGitHubCommitRequestService GitHubCommitRequestService { get; set; }
		public IFileCreator FileCreator { get; set; }

		public static readonly string GIT_HUB_EXCEEDED_RATE_LIMIT_MESSAGE = "rate limit exceeded";

		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		private readonly string PULL_REQUEST_FILE_PATH_KEY = "PullRequestFilePathKey";
		private readonly string COMMIT_FILE_PATH_KEY = "CommitFilePathKey";
		private readonly string ISSUE_FILE_PATH_KEY = "IssueFilePathKey";

		private readonly int DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE = 10;

		private readonly IDictionary<string, Commit> CommitByShaDict = new Dictionary<string, Commit>();

		private static readonly int SLEEP_TIME = 60 * 60 * 1000;

		public GitHubService(IGitHubPullRequestService gitHubPullRequestService, IGitHubIssuesRequestService gitHubissuesRequestService, IGitHubCommitRequestService gitHubCommitRequestService, IFileCreator fileCreator)
		{

			GitHubPullRequestService = gitHubPullRequestService;
			GitHubIssuesRequestService = gitHubissuesRequestService;
			GitHubCommitRequestService = gitHubCommitRequestService;
			FileCreator = fileCreator;
		}

		public void CreateFiles()
		{
			bool needToGetData = true;

			List<PullRequestResponse> pullRequests = new List<PullRequestResponse>();
			List<IssueResponse> issues = new List<IssueResponse>();
			while (needToGetData)
			{
				try
				{
					LOG.Info("INIT - GET PULL REQUESTS");
					IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;
					IList<PullRequestResponse> pullRequestsLocal = gitHubPullRequestService.List();
					pullRequests.AddRange(pullRequestsLocal);

					int pullRequestsCount = pullRequests.Count;
					LOG.Info("END - GET PULL REQUESTS - RESULT {0} PULL REQUESTS", pullRequestsCount);

					LOG.Info("INIT - GET ISSUES");
					IGitHubIssuesRequestService gitHubIssuesRequestService = GitHubIssuesRequestService;
					IList<IssueResponse> issuesResponseLocal = gitHubIssuesRequestService.List();
					issues.AddRange(issuesResponseLocal);

					int issuesCount = issues.Count;
					LOG.Info("END - GET ISSUES - RESULT {0} ISSUES", issuesCount);
					needToGetData = false;
				}
				catch (Exception e)
				{
					LOG.Error("Error getting essential data: {0} {1}", e.Message, e.StackTrace);
					pullRequests.Clear();
					issues.Clear();
				}
			}

			CreatePullRequestCSVFile(pullRequests);
			CreateCommitsCSVFile(pullRequests);
			CreateIssuesCSVFile(issues);
		}

		public void CreateIssuesCSVFile(IList<IssueResponse> issues)
		{
			Action<List<IssueFileCsvData>, IssueResponse> action = (data, issue) =>
			{
				IGitHubIssuesRequestService gitHubIssuesRequestService = GitHubIssuesRequestService;

				GetIssuesData(gitHubIssuesRequestService, issue, data);
			};

			string filePathKey = ISSUE_FILE_PATH_KEY;
			string filePath = GetFilePath(filePathKey);

			const string name = "issues";
			string fileName = GetFileName(name);

			GetData<IssueFileCsvData, IssueResponseMap, IssueResponse>(issues, action, filePath, fileName);
		}

		private void GetIssuesData(IGitHubIssuesRequestService gitHubIssuesRequestService, IssueResponse issue, List<IssueFileCsvData> data)
		{

			IEnumerable<IssueCommentResponse> comments = GetIssueComment(gitHubIssuesRequestService, issue);

			IssueFileCsvData item = TransformIntoCsvFormat(issue, comments);
			data.Add(item);
		}

		public void CreatePullRequestCSVFile(IList<PullRequestResponse> pullRequests)
		{
			Action<List<PullRequestCsvFileData>, PullRequestResponse> action = (data, pullRequestResponse) =>
			{
				IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;

				GetPullRequestData(gitHubPullRequestService, data, pullRequestResponse);
			};

			string filePathKey = PULL_REQUEST_FILE_PATH_KEY;
			string filePath = GetFilePath(filePathKey);

			const string name = "pull-request";
			string fileName = GetFileName(name);

			GetData<PullRequestCsvFileData, PullRequestCsvFileDataMap, PullRequestResponse>(pullRequests, action, filePath, fileName);
		}

		public void CreateCommitsCSVFile(IList<PullRequestResponse> pullRequests)
		{
			Action<List<CommitCsvFileData>, PullRequestResponse> action = (data, pullRequest) =>
			{
				GetCommitsData(data, pullRequest);
			};

			string filePathKey = COMMIT_FILE_PATH_KEY;
			string filePath = GetFilePath(filePathKey);

			const string name = "commit";
			string fileName = GetFileName(name);

			GetData<CommitCsvFileData, CommitCsvFileDataMap, PullRequestResponse>(pullRequests, action, filePath, fileName);
		}

		private void GetCommitsData(List<CommitCsvFileData> data, PullRequestResponse pullRequestResponse)
		{
			Commit commit = GetCommitData(pullRequestResponse);

			TransformIntoCsvFormat(pullRequestResponse, commit, data);
		}

		private Commit GetCommitData(PullRequestResponse pullRequestResponse)
		{
			bool didGetValue = CommitByShaDict.TryGetValue(pullRequestResponse.Head.Sha, out Commit commit);
			if (!didGetValue)
			{
				LOG.Info("INIT - GET COMMITS FROM PULL REQUEST {0}", pullRequestResponse.Number);
				IGitHubCommitRequestService gitHubCommitRequestService = GitHubCommitRequestService;
				commit = gitHubCommitRequestService.GetCommits(pullRequestResponse.Head.Sha);

				CommitByShaDict.Add(pullRequestResponse.Head.Sha, commit);
			}

			LOG.Info("END - GET COMMITS PULL REQUEST {0}", pullRequestResponse.Number);

			return commit;
		}

		private void GetData<T, TClassMap, K>(IList<K> dataItem, Action<List<T>, K> action, string filePath, string fileName)
			where TClassMap : ClassMap
			where K : ILogNumber
		{
			int dataCount = dataItem.Count();

			AppConfig instance = AppConfig.Instance;
			bool debugMode = instance.GetConfigBool("DebbugMode");
			int runLimit = (debugMode && DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE < dataCount)
				? DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE : dataCount;

			int count = 0;
			const int logCoeficient = 10;

			if (dataItem.Any())
			{
				List<T> data = new List<T>();


				for (; count < runLimit; count++)
				{
					if (count == 0 || (count % logCoeficient == 0) || (count == dataCount - 1))
					{
						LOG.Info("GETTING DATA FROM REQUEST {0} OF {1}", count, dataCount);
					}

					GetData(dataItem, action, count, data);
				}

				FileCreator.FilePath = filePath;
				FileCreator.FileName = fileName;
				LOG.Info("INIT - CREATING CSV");
				FileCreator.CreateFile<T, TClassMap>(data);
				LOG.Info("END - CREATING CSV");
			}
			else
			{
				LOG.Info("No pull requests available to get");
			}
		}

		private void GetData<T, K>(IList<K> dataList, Action<List<T>, K> action, int count, List<T> data) where K : ILogNumber
		{
			bool needToGetData = true;
			while (needToGetData)
			{
				K dataItem = dataList[count];

				try
				{
					action(data, dataItem);
					needToGetData = false;
				}
				catch (WebException webException)
				{
					if (typeof(WebException) == webException.InnerException.GetType())
					{
						TreatGitHubLimitRateError(dataItem, webException);
					}
					else
					{
						DefaultErrorBehavior(dataItem, webException);
					}
				}
				catch (Exception e)
				{
					DefaultErrorBehavior(dataItem, e);
				}
			}
		}

		private void TreatGitHubLimitRateError<K>(K pullRequestResponse, WebException webException) where K : ILogNumber
		{
			try
			{
				WebException innerException = (WebException)webException.InnerException;
				HttpWebResponse response = (HttpWebResponse)innerException.Response;
				HttpStatusCode statusCode = response.StatusCode;
				string statusDescription = response.StatusDescription;
				if (statusCode == HttpStatusCode.Forbidden && GIT_HUB_EXCEEDED_RATE_LIMIT_MESSAGE.Equals(statusDescription))
				{
					LOG.Error("Could not get data for {0} {1}, sleeping and trying again. Error: {2}", pullRequestResponse.LogName, pullRequestResponse.LogNumber, webException);
					Thread.Sleep(SLEEP_TIME);
				}
				else
				{
					DefaultErrorBehavior(pullRequestResponse, webException);
				}
			}
			catch (Exception e)
			{
				LOG.Error("Unknown web exception. moving on. Error: {1}", pullRequestResponse.LogNumber, e);
			}
		}

		private void DefaultErrorBehavior<K>(K pullRequestResponse, Exception e) where K : ILogNumber
		{
			LOG.Error("Could not get data for {0} {1}, moving on. Error: {2}", pullRequestResponse.LogName, pullRequestResponse.LogNumber, e);
		}

		private void GetPullRequestData(IGitHubPullRequestService gitHubPullRequestService, List<PullRequestCsvFileData> data, PullRequestResponse pullRequestResponse)
		{

			IGitHubIssuesRequestService gitHubIssuesRequestService = GitHubIssuesRequestService;

			LOG.Info("INIT - GET COMMENTS FROM PULL REQUEST {0}", pullRequestResponse.Number);
			IEnumerable<PullRequestComment> pullRequestComments = gitHubPullRequestService.Comments(pullRequestResponse.Number);
			LOG.Info("END - GET COMMENTS FROM PULL REQUEST {0}", pullRequestResponse.Number);

			IssueResponse issue = GetIssueResponse(pullRequestResponse, gitHubIssuesRequestService);
			IEnumerable<IssueCommentResponse> issueComments = GetIssueComment(gitHubIssuesRequestService, issue);

			Commit commit = GetCommitData(pullRequestResponse);

			TransformIntoCsvFormat(pullRequestResponse, issue, pullRequestComments, issueComments, commit, data);
		}

		private static IEnumerable<IssueCommentResponse> GetIssueComment(IGitHubIssuesRequestService gitHubIssuesRequestService, IssueResponse issue)
		{
			LOG.Info("INIT - GET COMMENTS FROM ISSUE {0}", issue.Number);
			IEnumerable<IssueCommentResponse> issueComments = gitHubIssuesRequestService.GetIssueComments(issue.Number);
			LOG.Info("END - GET COMMENTS FROM ISSUE {0}", issue.Number);
			return issueComments;
		}

		private IssueResponse GetIssueResponse(PullRequestResponse pullRequestResponse, IGitHubIssuesRequestService gitHubIssuesRequestService)
		{
			LOG.Info("INIT - GET ISSUE FROM PULL REQUEST {0}", pullRequestResponse.Number);
			IssueResponse issue = gitHubIssuesRequestService.Get(pullRequestResponse.Number);
			LOG.Info("END - GET ISSUE FROM PULL REQUEST {0}", pullRequestResponse.Number);
			return issue;
		}

		private void TransformIntoCsvFormat(PullRequestResponse pullRequestResponse, IssueResponse issue, IEnumerable<PullRequestComment> pullRequestComments, IEnumerable<IssueCommentResponse> issueComments, Commit commit, List<PullRequestCsvFileData> data)
		{
			PullRequestCsvFileData item = new PullRequestCsvFileData();
			item.PrNumber = pullRequestResponse.Number;
			item.IssueClosedDate = issue.ClosedAt;
			item.IssueAuthor = issue.Milestone?.Creator?.Login;
			item.IssueTitle = issue.Title;
			item.IssueBody = issue.Body;
			item.IssueComments = IssueCommentResponse.CreateIssueCommentsField(issueComments);
			item.PrCloseData = pullRequestResponse.CloseDate;
			item.PrAuthor = pullRequestResponse.User?.Login;
			item.PrTitle = pullRequestResponse.Title;
			item.PrBody = pullRequestResponse.Body;
			item.PrComments = item.CreatePullRequestCommentsField(pullRequestComments);
			item.CommitAuthor = commit.CommitInfo?.Author?.Name;
			item.CommitDate = commit.CommitInfo?.Author?.Date;
			item.CommitMessage = commit.CommitInfo?.Message;
			item.IsPr = true;

			data.Add(item);
		}

		private void TransformIntoCsvFormat(PullRequestResponse pullRequestResponse, Commit commit, List<CommitCsvFileData> data)
		{
			CommitCsvFileData item = new CommitCsvFileData();
			item.PrNumber = pullRequestResponse.Number;
			item.AuthorLogin = commit.CommitInfo.Author.Name;
			item.CommiterLogin = commit.CommitInfo.Committer.Name;
			item.SHA = pullRequestResponse.Head.Sha;
			item.CommitMessage = commit.CommitInfo.Message;
			item.PrepareChangesInfo(commit);

			data.Add(item);
		}

		private IssueFileCsvData TransformIntoCsvFormat(IssueResponse issue, IEnumerable<IssueCommentResponse> comments)
		{
			IssueFileCsvData item = new IssueFileCsvData();
			item.IssueNumber = issue.Number;
			item.IssueClosedDate = issue.ClosedAt;
			item.IssueTitle = issue.Title;
			item.IssueAuthor = issue.Milestone?.Creator?.Login;
			item.IssueBody = issue.Body;
			item.IssueComments = IssueCommentResponse.CreateIssueCommentsField(comments);

			return item;
		}

		private string GetFileName(string name)
		{
			string fileName = String.Format("{0:yyyy-MM-dd_HH-mm-ss}-{1}.csv", DateTime.Now, name);
			return fileName;
		}

		private string GetFilePath(string key)
		{
			AppConfig appConfig = AppConfig.Instance;
			string filePath = appConfig.GetConfig(key);
			return filePath;
		}
	}
}
