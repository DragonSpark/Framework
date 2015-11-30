namespace DragonSpark.Activation.FactoryModel
{
	public class ActivateFactory<TResult> : ActivateFactory<ActivateParameter, TResult> where TResult : class
	{
		public static ActivateFactory<TResult> Instance { get; } = new ActivateFactory<TResult>();

		public ActivateFactory()
		{}

		public ActivateFactory( IActivator activator ) : base( activator )
		{}

		/*protected override ActivateParameter QualifyParameter( object parameter )
		{
			return base.QualifyParameter( parameter ) ?? new ActivateParameter( typeof(TResult) );
		}*/
	}
}