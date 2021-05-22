using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;

namespace GitHubExtractor.Models
{
	public class IssueCommentResponse
	{
		[JsonProperty("body")]
		public string Body { get; set; }

		public static string CreateIssueCommentsField(IEnumerable<IssueCommentResponse> issueComments)
		{
			StringBuilder builder = new StringBuilder();

			builder.Append(" =||= ");
			foreach (IssueCommentResponse comment in issueComments)
			{
				builder.Append(comment.Body);
				builder.Append(" =||= ");
			}

			return builder.ToString();
		}
	}
}
