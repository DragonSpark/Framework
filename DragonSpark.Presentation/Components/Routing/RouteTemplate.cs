using System.Diagnostics;
using System.Linq;

namespace DragonSpark.Presentation.Components.Routing
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/ShaunCurtis/CEC.Routing/tree/master/CEC.Routing/Routing
	/// </summary>
	[DebuggerDisplay("{TemplateText}")]
	class RouteTemplate
	{
		public RouteTemplate(string templateText, TemplateSegment[] segments)
		{
			TemplateText          = templateText;
			Segments              = segments;
			OptionalSegmentsCount = segments.Count(template => template.IsOptional);
		}

		public string TemplateText { get; }

		public TemplateSegment[] Segments { get; }

		public int OptionalSegmentsCount { get; }
	}
}