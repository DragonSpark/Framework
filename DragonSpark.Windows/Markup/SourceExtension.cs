using DragonSpark.Sources;
using PostSharp.Patterns.Contracts;
using System.Windows.Markup;

namespace DragonSpark.Windows.Markup
{
	[ContentProperty( nameof(Instance) )]
	public class SourceExtension : MarkupExtensionBase
	{
		public SourceExtension() {}

		public SourceExtension( ISource instance )
		{
			Instance = instance;
		}

		[NotNull]
		public ISource Instance { [return: NotNull]get; set; }

		protected override object GetValue( MarkupServiceProvider serviceProvider ) => Instance.Get();
	}
}