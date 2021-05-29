using GitHubExtractor.Dtos.Interfaces;
using Newtonsoft.Json;
using System;

namespace GitHubExtractor.Models
{
	public class IssueResponse : ILogNumber
	{
		[JsonProperty("closed_at")]
		public DateTime? ClosedAt { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }


		[JsonProperty("body")]
		public string Body { get; set; }


		[JsonProperty("comments_url")]
		public string CommentsUrl { get; set; }

		[JsonProperty("milestone")]
		public Milestone Milestone { get; set; }


		[JsonProperty("number")]
		public int Number { get; set; }

		public int LogNumber => Number;

		public string LogName => "Issues";
	}
}
