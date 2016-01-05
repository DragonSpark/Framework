using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Windows.Markup;

namespace DragonSpark.Setup
{
	[ContentProperty( nameof(Commands) )]
	public class Setup<TParameter> : CompositeCommand<TParameter>, ISetup where TParameter : ISetupParameter
	{
		public Collection<object> Items { get; } = new Collection<object>();

		public virtual void Run( ISetupParameter parameter )
		{
			parameter.Register<ISetup>( this );

			CommandExtensions.Apply( this, (object)parameter );
		}
	}
}
