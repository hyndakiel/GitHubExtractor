using CsvHelper;
using CsvHelper.Configuration;
using GitHubExtractor.Configs;
using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace GitHubExtractor.Services
{
	public class GitHubService : IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }

		public IGitHubIssuesRequestService GitHubIssuesRequestService { get; set; }

		public IGitHubCommitRequestService GitHubCommitRequestService { get; set; }

		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		private readonly string DELIMETER = "\a";

		private readonly string FILE_PATH_KEY = "PullRequestFilePathKey";

		private readonly int DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE = 10;

		public GitHubService(IGitHubPullRequestService gitHubPullRequestService, IGitHubIssuesRequestService gitHubissuesRequestService, IGitHubCommitRequestService gitHubCommitRequestService)//, IFileCreator fileCreator)
		{

			GitHubPullRequestService = gitHubPullRequestService;
			GitHubIssuesRequestService = gitHubissuesRequestService;
			GitHubCommitRequestService = gitHubCommitRequestService;
			//FileCreator = fileCreator;
		}

		public void CreateFiles()
		{
			LOG.Info("INIT - GET PULL REQUESTS");
			IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;
			IList<PullRequestResponse> pullRequests = gitHubPullRequestService.List();

			int pullRequestsCount = pullRequests.Count();
			LOG.Info("END - GET PULL REQUESTS - RESULT {0} PULL REQUESTS", pullRequestsCount);

			CreatePullRequestCSVFile(pullRequests);
			CreateCommitsCSVFile(pullRequests);
		}

		public void CreatePullRequestCSVFile(IList<PullRequestResponse> pullRequests)
		{
			Action<List<PullRequestCsvFileData>, PullRequestResponse> action = (data, pullRequestResponse) =>
			{
				IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;

				GetPullRequestData(gitHubPullRequestService, data, pullRequestResponse);
			};

			GetPullRequestsData<PullRequestCsvFileData, PullRequestCsvFileDataMap>(pullRequests, action);
		}

		public void CreateCommitsCSVFile(IList<PullRequestResponse> pullRequests)
		{
			Action<List<CommitCsvFileData>, PullRequestResponse> action = (data, pullRequest) =>
			{
				GetCommitsData(data, pullRequest);
			};

			GetPullRequestsData<CommitCsvFileData, CommitCsvFileDataMap>(pullRequests, action);
		}

		private void GetCommitsData(List<CommitCsvFileData> data, PullRequestResponse pullRequest)
		{
			throw new NotImplementedException();
		}

		private void GetPullRequestsData<T, TClassMap>(IList<PullRequestResponse> pullRequests, Action<List<T>, PullRequestResponse> action) where TClassMap : ClassMap
		{
			int pullRequestsCount = pullRequests.Count();

			AppConfig instance = AppConfig.Instance;
			bool debugMode = instance.GetConfigBool("DebbugMode");
			int runLimit = (debugMode && DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE < pullRequestsCount)
				? DEBUG_MODE_PULL_REQUEST_MAX_RUN_VALUE : pullRequestsCount;

			int count = 0;
			const int logCoeficient = 10;

			if (pullRequests.Any())
			{
				List<T> data = new List<T>();


				for (; count < runLimit; count++)
				{
					PullRequestResponse pullRequestResponse = pullRequests[count];
					if (count == 0 || (count % logCoeficient == 0) || (count == pullRequestsCount - 1))
					{
						LOG.Info("GETTING DATA FROM REQUEST {0} OF {1}", count, pullRequestsCount);
					}
					try
					{
						action(data, pullRequestResponse);
					}
					catch (Exception e)
					{
						LOG.Error("Could not get data for pullRequest {0}, moving on. Error: {1}", pullRequestResponse.Number, e);
					}
				}


				LOG.Info("INIT - CREATING CSV");
				CreateCsvFile<T, TClassMap>(data);
				LOG.Info("END - CREATING CSV");
			}
			else
			{
				LOG.Info("No pull requests available to get");
			}
		}

		private void GetPullRequestData(IGitHubPullRequestService gitHubPullRequestService, List<PullRequestCsvFileData> data, PullRequestResponse pullRequestResponse)
		{

			IGitHubIssuesRequestService gitHubIssuesRequestService = GitHubIssuesRequestService;

			LOG.Info("INIT - GET COMMENTS FROM PULL REQUEST {0}", pullRequestResponse.Number);
			IEnumerable<PullRequestComment> pullRequestComments = gitHubPullRequestService.Comments(pullRequestResponse.Number);
			LOG.Info("INIT - GET COMMENTS FROM PULL REQUEST {0}", pullRequestResponse.Number);

			LOG.Info("INIT - GET ISSUE FROM PULL REQUEST {0}", pullRequestResponse.Number);
			IssueResponse issue = gitHubIssuesRequestService.Get(pullRequestResponse.Number);
			LOG.Info("END - GET ISSUE FROM PULL REQUEST {0}", pullRequestResponse.Number);

			LOG.Info("INIT - GET COMMENTS FROM ISSUE {0}", issue.Number);
			IEnumerable<IssueCommentResponse> issueComments = gitHubIssuesRequestService.GetIssueComments(issue.Number);
			LOG.Info("END - GET COMMENTS FROM ISSUE {0}", issue.Number);

			LOG.Info("INIT - GET COMMITS FROM PULL REQUEST {0}", pullRequestResponse.Number);
			IGitHubCommitRequestService gitHubCommitRequestService = GitHubCommitRequestService;
			Commit commit = gitHubCommitRequestService.GetCommits(pullRequestResponse.Head.Sha);
			LOG.Info("END - GET COMMITS PULL REQUEST {0}", pullRequestResponse.Number);

			TransformIntoCsvFormat(pullRequestResponse, issue, pullRequestComments, issueComments, commit, data);
		}

		private void TransformIntoCsvFormat(PullRequestResponse pullRequestResponse, IssueResponse issue, IEnumerable<PullRequestComment> pullRequestComments, IEnumerable<IssueCommentResponse> issueComments, Commit commit, List<PullRequestCsvFileData> data)
		{
			PullRequestCsvFileData item = new PullRequestCsvFileData();
			item.PrNumber = pullRequestResponse.Id;
			item.IssueClosedDate = issue.ClosedAt;
			item.IssueAuthor = issue.Milestone?.Creator?.Login;
			item.IssueTitle = issue.Title;
			item.IssueBody = issue.Body;
			item.IssueComments = CreateIssueCommentsField(issueComments);
			item.PrCloseData = pullRequestResponse.CloseDate;
			item.PrAuthor = pullRequestResponse.User?.Login;
			item.PrTitle = pullRequestResponse.Title;
			item.PrBody = pullRequestResponse.Body;
			item.PrComments = CreatePullRequestCommentsField(pullRequestComments);
			item.CommitAuthor = commit.CommitInfo?.Author?.Name;
			item.CommitDate = commit.CommitInfo?.Author?.Date;
			item.CommitMessage = commit.CommitInfo?.Message;
			//item.IsPr = pullRequestResponse.

			data.Add(item);

		}

		private string CreateIssueCommentsField(IEnumerable<IssueCommentResponse> issueComments)
		{
			StringBuilder builder = new StringBuilder();

			foreach (IssueCommentResponse comment in issueComments)
			{
				builder.Append(comment.ToString());
			}

			return builder.ToString();
		}

		private string CreatePullRequestCommentsField(IEnumerable<PullRequestComment> pullRequestComments)
		{
			StringBuilder builder = new StringBuilder();

			foreach (PullRequestComment comment in pullRequestComments)
			{
				builder.Append(comment.ToString());
			}

			return builder.ToString();
		}

		private void CreateCsvFile<T, TClassMap>(IEnumerable<T> data) where TClassMap : ClassMap
		{
			string filePath = GetFilePath();

			Directory.CreateDirectory(filePath);

			string fileName = GetFileName();

			string completePath = filePath + fileName;
			StreamWriter writer = new StreamWriter(completePath);

			CsvConfiguration configuration = new CsvConfiguration(new CultureInfo("en-us"));

			configuration.Delimiter = DELIMETER;
			configuration.HasHeaderRecord = true;

			CsvWriter csvWriter = new CsvWriter(writer, configuration);
			csvWriter.Context.RegisterClassMap<TClassMap>();
			SetupData(csvWriter, data);

			LOG.Info("INIT - SAVING CSV TO PATH: {0} WITH NAME {1}", filePath, fileName);
			writer.Flush();
			LOG.Info("INIT - SAVING CSV TO PATH: {0} WITH NAME {1}", filePath, fileName);
		}

		private string GetFileName()
		{
			string fileName = String.Format("{0:yyyy-MM-dd_HH-mm-ss}-pull_request.csv", DateTime.Now);
			return fileName;
		}

		private string GetFilePath()
		{
			AppConfig appConfig = AppConfig.Instance;
			string filePath = appConfig.GetConfig(FILE_PATH_KEY);
			return filePath;
		}

		private void SetupData<T>(CsvWriter csvWriter, IEnumerable<T> data)
		{
			csvWriter.WriteRecords(data);
		}
	}
}
