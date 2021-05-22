using GitHubExtractor.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace GitHubExtractor.Dtos
{
	public class PullRequestCsvFileData
	{
		public bool IsPr { get; set; }
		public int PrNumber { get; internal set; }
		public DateTime? IssueClosedDate { get; internal set; }
		public string IssueAuthor { get; internal set; }
		public string IssueTitle { get; internal set; }
		public string IssueBody { get; internal set; }
		public DateTime? PrCloseData { get; internal set; }
		public string PrTitle { get; internal set; }
		public string PrBody { get; internal set; }
		public string PrComments { get; internal set; }
		public string IssueComments { get; internal set; }
		public string PrAuthor { get; internal set; }
		public string CommitAuthor { get; internal set; }
		public DateTime? CommitDate { get; internal set; }
		public string CommitMessage { get; internal set; }

		public string CreatePullRequestCommentsField(IEnumerable<PullRequestComment> pullRequestComments)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(" =||= ");
			foreach (PullRequestComment comment in pullRequestComments)
			{
				builder.Append(comment.Body);
				builder.Append(" =||= ");
			}

			return builder.ToString();
		}
	}
}
