using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamCityDownloadArtifacts
{
	public class RestApiProjectDoc
	{
		public string Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Href { get; set; }
		public string WebUrl { get; set; }
	}
}
