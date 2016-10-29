using DragonSpark.Activation.Location;

namespace DragonSpark.Activation
{
	public sealed class Activator : CompositeActivator
	{
		public static IActivator Default { get; } = new Activator();
		Activator() : base( SingletonLocator.Default, Constructor.Default ) {}

		/*public override object Get( Type parameter )
		{
			var item = base.Get( parameter );
			var result = item != null ? SourceAccountedAlteration.Defaults.Get( parameter ).Invoke( item ) : null;
			return result;
		}*/
	}
}