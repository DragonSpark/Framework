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
			Remove.Each( y => context.Get( y ).With( x => context.Remove( x ) ) );

			Attach.Each( context.ApplyChanges );
		}

		public Collection Attach { get; } = new Collection();
		
		public Collection Remove { get; } = new Collection();
	}
}