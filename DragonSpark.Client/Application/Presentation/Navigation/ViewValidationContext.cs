using System;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class ViewValidationContext
	{
		readonly IRegion region;
		readonly Uri location;
		readonly object content;

		public ViewValidationContext( IRegion region, Uri location, object content )
		{
			this.region = region;
			this.location = location;
			this.content = content;
		}

		public IRegion Region
		{
			get { return region; }
		}

		public Uri Location
		{
			get { return location; }
		}

		public object Content
		{
			get { return content; }
		}
	}
}