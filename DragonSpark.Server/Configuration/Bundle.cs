using DragonSpark.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Server.Configuration
{
	[ContentProperty( "Directives" )]
	public abstract class Bundle
	{
		public string CdnPath { get; set; }

		public string Path { get; set; }

		public Collection<BundleDirective> Directives
		{
			get { return directives; }
		}	readonly Collection<BundleDirective> directives = new Collection<BundleDirective>();

		public System.Web.Optimization.Bundle Create()
		{
			var result = CreateInstance();
			Directives.Apply( x => x.Apply( result ) );
			return result;
		}

		protected abstract System.Web.Optimization.Bundle CreateInstance();
	}
}