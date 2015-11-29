using System.Data.Entity;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( "Attach" )]
	public class EntityInstallationStep : IInstallationStep
	{
		public void Execute( DbContext context )
		{
			Remove.Apply( y => context.Get( y ).NotNull( x => context.Remove<object>( x ) ) );

			Attach.Apply( y => context.ApplyChanges( y ) );
		}

		public Collection Attach { get; } = new Collection();
		
		public Collection Remove { get; } = new Collection();
	}
}