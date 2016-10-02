using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Data.Entity;
using System.Windows.Markup;

namespace DragonSpark.Windows.Entity
{
	[ContentProperty( nameof(Attach) )]
	public class EntityInstallationStep : IInstallationStep
	{
		public void Execute( DbContext context )
		{
			Remove.Each( y => context.Get( y ).With( x => context.Remove( x ) ) );

			Attach.Each( context.ApplyChanges );
		}

		public DeclarativeCollection Attach { get; } = new DeclarativeCollection();
		
		public DeclarativeCollection Remove { get; } = new DeclarativeCollection();
	}
}