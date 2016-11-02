namespace DragonSpark.Sources.Parameterized
{
	public class AppliedAlteration<T> : DelegatedAlteration<T> where T : class
	{
		public AppliedAlteration( Alter<T> source ) : base( source ) {}

		public override T Get( T parameter ) => base.Get( parameter ) ?? parameter;
	}
}