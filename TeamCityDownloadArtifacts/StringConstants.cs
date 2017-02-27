using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadDlls
{
	public static class StringConstants
	{
		public const string UnableToAccess = "Unable to access artifacts in your services directory. Please close down Visual Studio or any other program that may lock this folder.";
		public const string NoBuildsFound = "No Builds found on teamcity for specified branch: ";
		public const string Downloading = "Downloading zipped artifacts....";
		public const string Unzipping = "Unzipping files...";
		public const string Removing = "Removing unnecessary artifacts....";
		public const string LastUsedBranch = "\\.lastUsedBranch";
		public const string ExtractionDirectory = "\\.extractionDirectory";
		public const string HelpResponse = @"***************
Usage
***************
downloadartifacts.exe --help 
-Display this

downloadartifacts.exe [branchName] [extractionDirectory]

ExtractionDirectoy - this argument is where you have your git repository for xm8Services.  
	If you don't provide an argument it will default to C:\Temp\downloadartifacts\teamcityArtifacts
";

		public const string Cleaning = "Cleaning old packages...";
		public const string DirectoryNotFound = "Extraction directory not found. Using default place to build: ";
		public const string DownloadArtifactsDirectory = "C:\\temp\\downloadartifacts";
		public const string DownloadComplete = "Download Complete";
		public const string MissionAccomplished = "Mission accomplished.";
		public const string DefaultExtractionDirectory = "C:\\temp\\downloadartifacts\\teamcityArtifacts\\";

	}
}
