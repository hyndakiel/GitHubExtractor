using Newtonsoft.Json;

namespace GitHubExtractor.Utils
{
	public static class UtilitiesObj
	{
		public static T JsonDeserializeObject<T>(string json)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(json);
			}
			catch (JsonReaderException e)
			{
				string msg = string.Format("Error on convert value to json: \n----\n{0}\n----\n", json);
				throw new JsonReaderException(msg, e);
			}
		}

		public static object JsonDeserializeObject(string json)
		{
			if (json == null)
			{
				return null;
			}
			return JsonDeserializeObject<object>(json);
		}

		public static string JsonSerializeObject(object obj)
		{
			if (obj == null)
			{
				return null;
			}

			return JsonConvert.SerializeObject(obj);
		}
	}


}
