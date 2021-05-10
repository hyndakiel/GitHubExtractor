using GitHubExtractor.Configs;
using GitHubExtractor.Dtos;
using GitHubExtractor.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace GitHubExtractor.Services.Connection
{
	public abstract class AbstractConnectionService
	{
		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		/// <summary>
		/// Timeout in miliseconds
		/// </summary>
		public const int DEFAULT_TIMEOUT = 15 * 60 * 1000;

		public string ApiUrl { get; set; }

		public LastRequestInfo LastRequestInfo { get; set; }

		protected AbstractConnectionService(string key) : base()
		{
			AppConfig appConfig = AppConfig.Instance;
			string baseApiUrl = appConfig.GetConfig(key);
			this.ApiUrl = baseApiUrl;
			LastRequestInfo = new LastRequestInfo();
		}

		public string AccessEndPoint(string url)
		{
			return this.AccessEndPoint(url, null);
		}

		public Tuple<T, string> AccessEndPoint<T>(string path, object request, bool isPost, BasicAuth auth, string requestOrigin)
		{
			string rawResponse = AccessEndPoint(path, request, isPost, auth, requestOrigin);
			T responseObj = UtilitiesObj.JsonDeserializeObject<T>(rawResponse);
			return new Tuple<T, string>(responseObj, rawResponse);
		}//func

		public virtual string AccessEndPoint(string baseUrl, string json, params Tuple<string, string>[] headers)
		{
			string apiUrl = this.ApiUrl;
			string url = apiUrl + baseUrl;
			try
			{
				string finalUrl = url;
				if (json != null)
				{
					Dictionary<string, dynamic> data = UtilitiesObj.JsonDeserializeObject<Dictionary<string, dynamic>>(json);

					finalUrl += "?";
					StringBuilder builder = new StringBuilder();
					builder.Append(finalUrl);
					int i = 1;
					foreach (string key in data.Keys)
					{
						builder.Append(String.Format("{0}={1}", key, data[key]));

						if (i < data.Count)
						{
							builder.Append("&");
							i++;
						}
					}
					finalUrl = builder.ToString();

				}


				LOG.Debug("INIT - url: {0}", finalUrl);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(finalUrl);
				request.ContentType = "application/json";
				request.Method = "GET";
				request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

				foreach (Tuple<string, string> header in headers)
				{
					request.Headers.Add(header.Item1, header.Item2);
				}

				string responseString = ReadResponse(request);

				LOG.Debug("END - url: {0}", finalUrl);

				return responseString;
			}
			catch (Exception e)
			{
				string msg = string.Format("Erro on accesss url='{0}' with json={1}", url, json);
				throw new WebException(msg, e);
			}

		}

		public string AccessEndPoint(string path, object request, bool isPost, BasicAuth auth, string requestDestiny, int? timeout = null)
		{
			string json = UtilitiesObj.JsonSerializeObject(request);
			LOG.Info(string.Format("INIT - {3} request '{0}' - isPost: {1} - request: {2}", path, isPost, json, requestDestiny));
			try
			{
				Tuple<string, string>[] headers = new Tuple<string, string>[]
				{
					new Tuple<string, string>("User-Agent", auth.Authorization),
					new Tuple<string, string>("Authorization", "Bearer " + auth.Token),
				};

				string response;
				if (isPost)
				{
					response = AccessEndPointPost(path, json, timeout, headers);
				}//if
				else
				{
					response = AccessEndPoint(path, json, headers);
				}//else
				LOG.Info(string.Format("END - {3} request '{0}' - isPost: {1} - response: {2}", path, isPost, response, requestDestiny));

				return response;
			}//try
			catch (Exception e)
			{
				LOG.Error(string.Format("END - {3} request '{0}' - isPost: {1} - with error: {2}", path, isPost, e, requestDestiny));
				throw;
			}//catch

		}//func

		protected string ReadResponse(HttpWebRequest request, int? timeout = null)
		{
			request.Timeout = timeout.HasValue ? timeout.Value : DEFAULT_TIMEOUT;
			request.KeepAlive = true;

			WebResponse webResponse = request.GetResponse();

			using Stream stream = webResponse.GetResponseStream();

			StreamReader reader = new StreamReader(stream, Encoding.UTF8);

			string responseString = reader.ReadToEnd();

			LastRequestInfo.Headers = webResponse.Headers;
			LastRequestInfo.Response = responseString;

			return responseString;
		}

		public string AccessEndPointPost(string baseUrl, object obj, int? timeout = null)
		{
			string json = UtilitiesObj.JsonSerializeObject(obj);
			return AccessEndPointPost(baseUrl, json, timeout);
		}

		public virtual string AccessEndPointPost(string baseUrl, string json, int? timeout = null, params Tuple<string, string>[] headers)
		{
			string apiUrl = this.ApiUrl;
			string url = apiUrl + baseUrl;
			try
			{

				LOG.Debug("INIT - url: {0}\nJson: {1}", url, json);

				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.ProtocolVersion = HttpVersion.Version10;
				request.UseDefaultCredentials = false;
				request.PreAuthenticate = false;
				request.ContentType = "application/json";
				request.Method = "POST";
				request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

				using (StreamWriter streamWriter = new StreamWriter(request.GetRequestStream()))
				{
					streamWriter.Write(json);
				}

				foreach (Tuple<string, string> header in headers)
				{
					request.Headers.Add(header.Item1, header.Item2);
				}

				string responseString = ReadResponse(request, timeout);

				LOG.Debug("END - url: {0}", url);

				return responseString;
			}
			catch (Exception e)
			{
				string msg = string.Format("Erro on accesss url='{0}' with json={1}", url, json);
				throw new IOException(msg, e);
			}
		}
	}
}
