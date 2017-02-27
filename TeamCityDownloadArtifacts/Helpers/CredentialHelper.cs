using System;
using System.IO;
using System.Text;

namespace DownloadArtifacts.Helpers
{
	public class CredentialHelper
	{
		private const string credentialsPath = "C:\\temp\\downloadartifacts\\.credentials";

		public static string GetCredentials()
		{
			if (!File.Exists(credentialsPath))
			{
				if (!Directory.Exists(StringConstants.DownloadArtifactsDirectory))
				{
					Directory.CreateDirectory(StringConstants.DownloadArtifactsDirectory);
				}
				Console.WriteLine("Enter your team city username: ");
				var teamcityUsername = Console.ReadLine();
				Console.WriteLine("Enter your password: ");
				ConsoleKeyInfo key;
				string password = "";
				do
				{
					key = Console.ReadKey(true);
					if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
					{
						password += key.KeyChar;
						Console.Write("*");
					}
					else if (key.Key == ConsoleKey.Backspace)
					{
						Console.Write("\b");
					}
				} while (key.Key != ConsoleKey.Enter);
				Console.WriteLine();
				//take the teamcity username and the password
				var unEncodedCredentials = teamcityUsername + ":" + password;
				var bytes = Encoding.ASCII.GetBytes(unEncodedCredentials);
				var encodedCredentials = Convert.ToBase64String(bytes);

				FileHelper.WriteToFile(credentialsPath, encodedCredentials, true);
				return encodedCredentials;
			}
			var credentials = FileHelper.GetFileContents(credentialsPath, true);

			return credentials;
		}

		public static bool DeleteCredentialFile()
		{
			if (File.Exists(credentialsPath))
			{
				File.Delete(credentialsPath);

				return true;
			}
			return false;
		}
	}
}
