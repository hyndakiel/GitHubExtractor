using Newtonsoft.Json;
using System;

namespace GitHubExtractor.Models
{
	public class PullRequestResponse
	{
		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("closed_at")]
		public DateTime? CloseTime { get; set; }

		[JsonProperty("issue_url")]
		public string IssueUrl { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("body")]
		public string Body { get; set; }

		[JsonProperty("comments_url")]
		public string CommentsUrl { get; set; }

		[JsonProperty("commits_url")]
		public string CommitsUrl { get; set; }

	}
}
