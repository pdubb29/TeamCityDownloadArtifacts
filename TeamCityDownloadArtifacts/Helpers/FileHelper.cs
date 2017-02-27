using System.IO;
using System.Text;

namespace DownloadArtifacts.Helpers
{
	public class FileHelper
	{
		
		private const string _debug = ""; //debug artifacts path
		private const string _release = ""; //release artifacts path 

		public static void DeleteOldZips()
		{
			//get all the zips in the temp directory
			DirectoryInfo directoryInfo = new DirectoryInfo(StringConstants.DownloadArtifactsDirectory);

			var files = directoryInfo.GetFiles("Build*.zip");

			foreach (var file in files)
			{
				file.Delete();
			}

		}

		public static string GetFileContents(string path, bool fullPath)
		{
			if (!fullPath)
			{
				path = StringConstants.DownloadArtifactsDirectory + path;
			}
			if (!File.Exists(path))
			{
				return null;
			}
			else
			{
				return File.ReadAllText(path);
			}
		}

		public static void WriteToFile(string path, string contents, bool fullPath)
		{
			if (!fullPath)
			{
				path = StringConstants.DownloadArtifactsDirectory + path;
			}
			var bytes = Encoding.ASCII.GetBytes(contents);
			File.WriteAllBytes(path, bytes);
		}


		public static void CleanPackages(string extractionDirectory)
		{
			if (Directory.Exists(extractionDirectory + _debug))
			{
				Directory.Delete(extractionDirectory + _debug, true);
			}
			if (Directory.Exists(extractionDirectory + _release))
			{
				Directory.Delete(extractionDirectory + _release, true);
			}
		}

		public static void CreateDirectoryIfNotExists()
		{
			if (!Directory.Exists(StringConstants.DownloadArtifactsDirectory))
			{
				Directory.CreateDirectory(StringConstants.DownloadArtifactsDirectory);
			}
		}
	}
}
