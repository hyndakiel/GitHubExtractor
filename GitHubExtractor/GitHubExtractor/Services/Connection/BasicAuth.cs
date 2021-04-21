namespace GitHubExtractor.Services.Connection
{
	public class BasicAuth
	{
		public string Authorization { get; set; }
		public string Token { get; set; }

		public BasicAuth()
		{

		}

		public BasicAuth(string Authorization, string token)
		{
			this.Authorization = Authorization;
			this.Token = token;
		}

	}
}
