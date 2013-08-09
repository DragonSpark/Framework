namespace DragonSpark.Runtime
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces", Justification = "Denied." )]
    public static class Threading
	{
		public static IDelegateWorker Application
		{
			get { return ServiceLocation.With<IDelegateWorkerProvider, IDelegateWorker>( x => x.Primary ); }
		}

		public static IDelegateWorker Background
		{
			get { return ServiceLocation.With<IDelegateWorkerProvider, IDelegateWorker>( x => x.Secondary ); }
		}
	}
}