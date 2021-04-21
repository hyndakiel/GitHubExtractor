using System.Collections.Generic;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubPullRequestService
	{
		public IEnumerable<object> List();
	}
}
