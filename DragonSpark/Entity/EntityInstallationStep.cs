using System.Data.Entity;
using DragonSpark.Extensions;
using System.Collections.ObjectModel;

namespace DragonSpark.Entity
{
	/*public class ObjectContextInstallationStep : IInstallationStep
	{
		/*readonly SecurityDataObjectContextFactory factory = new SecurityDataObjectContextFactory();#1#

		public Type FactoryType { get; set; }

		public Type ObjectContextType { get; set; }

		public void Execute( DbContext context )
		{
			using ( var instance = CreateContext() )
			{
				Attach.Apply( instance.AddOrAttach );
				Remove.Apply( instance.DeleteObject );
				var result = instance.SaveChanges();
				DragonSpark.Runtime.Logging.Information( string.Format( "{0} saved '{1}' entities.", GetType().AssemblyQualifiedName, result ) );
			}
		}

		ObjectContext CreateContext()
		{
			var result = FactoryType.Transform( x => Activator.CreateInstance<IFactory>( x ).Create<ObjectContext>() ) ?? Activator.Create<ObjectContext>( ObjectContextType );
			return result;
		}

		public Collection<object> Attach
		{
			get { return attach ?? ( attach = new Collection<object>() ); }
		}	Collection<object> attach;

		public Collection<object> Remove
		{
			get { return remove ?? ( remove = new Collection<object>() ); }
		}	Collection<object> remove;
	}*/

	public class EntityInstallationStep : IInstallationStep
	{
		public void Execute( DbContext context )
		{
			Remove.Apply( y => context.Get( y ).NotNull( context.Remove ) );

			Attach.Apply( y => context.ApplyChanges( y ) );
		}
		
		public Collection<object> Attach
		{
			get { return attach ?? ( attach = new Collection<object>() ); }
		}	Collection<object> attach;

		public Collection<object> Remove
		{
			get { return remove ?? ( remove = new Collection<object>() ); }
		}	Collection<object> remove;
	}
}