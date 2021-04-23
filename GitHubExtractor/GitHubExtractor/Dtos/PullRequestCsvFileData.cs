using System;

namespace GitHubExtractor.Dtos
{
	public class PullRequestCsvFileData
	{
		public string PrNumber { get; internal set; }
		public DateTime? IssueClosedDate { get; internal set; }
		public string IssueAuthor { get; internal set; }
		public string IssueTitle { get; internal set; }
		public string IssueBody { get; internal set; }
		public DateTime? PrCloseData { get; internal set; }
		public string PrTitle { get; internal set; }
		public string PrBody { get; internal set; }
	}
}
