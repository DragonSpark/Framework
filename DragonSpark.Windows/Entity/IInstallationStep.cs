using DragonSpark.Extensions;
using DragonSpark.Runtime;
using System.Data.Entity;
using System.Windows.Markup;

namespace DragonSpark.Windows.Entity
{
	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}

	[ContentProperty( "Attach" )]
	public class EntityInstallationStep : IInstallationStep
	{
		public void Execute( DbContext context )
		{
			Remove.Apply( y => context.Get( y ).NotNull( x => context.Remove( x ) ) );

			Attach.Apply( y => context.ApplyChanges( y ) );
		}

		public Collection Attach { get; } = new Collection();
		
		public Collection Remove { get; } = new Collection();
	}
}