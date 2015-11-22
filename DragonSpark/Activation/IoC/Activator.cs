namespace DragonSpark.Activation.IoC
{
	/*public class Activator : IActivator
	{
		readonly IUnityContainer container;

		public Activator( IUnityContainer container )
		{
			this.container = container;
		}

		public bool CanActivate( Type type, string name )
		{
			var result = container.IsResolvable( type, name );
			return result;
		}

		public object Activate( Type type, string name = null )
		{
			var result = container.ResolveWithContext( type, name );
			return result;
		}

		public object Construct( Type type, params object[] parameters )
		{
			var result = container.Create( type, parameters );
			return result;
		}
	}*/
}