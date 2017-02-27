using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCityDownloadArtifacts
{
	public class BuildDoc
	{
		public int Id {get; set;}
		public string BuildTypeId {get; set;}
		public int Number {get; set;}
		public string Status {get; set;}
		public string State {get; set;}
		public string BranchName {get; set;}
		public string DefaultBranch {get; set;}
		public string Href {get; set;}
		public string WebUrl { get; set; }
	}
}
