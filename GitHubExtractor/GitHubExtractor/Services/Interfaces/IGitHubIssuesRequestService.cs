using GitHubExtractor.Models;
using System.Collections.Generic;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubIssuesRequestService
	{
		public IssueResponse Get(int number);

		public IEnumerable<IssueCommentResponse> GetIssueComments(int issueNumber);
	}
}
