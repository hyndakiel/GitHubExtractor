using System;

namespace GitHubExtractor.Dtos
{
	public class IssueFileCsvData
	{
		public int IssueNumber { get; internal set; }
		public DateTime? IssueClosedDate { get; internal set; }
		public string IssueAuthor { get; internal set; }
		public string IssueTitle { get; internal set; }
		public string IssueBody { get; internal set; }
		public string IssueComments { get; internal set; }
	}
}
