using GitHubExtractor.Dtos.Interfaces;
using Newtonsoft.Json;
using System;

namespace GitHubExtractor.Models
{
	public class PullRequestResponse : ILogNumber
	{

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("closed_at")]
		public DateTime? CloseDate { get; set; }

		[JsonProperty("issue_url")]
		public string IssueUrl { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }

		[JsonProperty("comments_url")]
		public string CommentsUrl { get; set; }

		[JsonProperty("review_comments_url")]
		public string PullRequestComments { get; set; }

		[JsonProperty("commits_url")]
		public string CommitsUrl { get; set; }

		[JsonProperty("number")]
		public int Number { get; set; }

		[JsonProperty("user")]
		public Creator User { get; set; }

		[JsonProperty("head")]
		public Head Head { get; internal set; }

		public int LogNumber => Number;
	}
}
