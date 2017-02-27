using System;
using System.Net;

namespace DownloadArtifacts.Helpers
{
	public class ResponseHelper
	{
		public static WebResponse GetWebResponse(string url)
		{
			WebResponse response = null;
			do
			{
				var client = System.Net.WebRequest.Create(url);
				try
				{
					client.Headers.Add("Authorization", "Basic " + CredentialHelper.GetCredentials());
					client.ContentType = "application/json";
					
					response = client.GetResponse();
				}
				catch (WebException exception)
				{
					if (exception.Message.Contains("401"))
					{
						Console.WriteLine("Bad Credentials, please enter them again.");
						//have them enter their credentials again.
						CredentialHelper.DeleteCredentialFile();
						client.Headers.Clear();
						client.Headers.Add("Authorization", "Basic " + CredentialHelper.GetCredentials());
					}
					else
					{
						//some other error occurred.
						Console.WriteLine("Unknown error occurred");
					}
				}
			} while (response == null);

			return response;
		}
		
	}
}
