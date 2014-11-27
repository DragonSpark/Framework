namespace DragonSpark.Client
{
	using System;

	/*[ContentProperty( "Launcher" )]
	public class Application : System.Windows.Application
	{
		public ApplicationLauncher Launcher { get; set; }

		protected override void OnStartup( StartupEventArgs e )
		{
			base.OnStartup( e );

			Launcher.With( x => x.Launch( e.Args ) );
		}
	}*/

	public class FatalApplicationException : Exception
	{
		public FatalApplicationException()
		{}

		public FatalApplicationException( string message ) : base( message )
		{}

		public FatalApplicationException( string message, Exception inner ) : base( message, inner )
		{}
	}
}