namespace DragonSpark.Sources.Parameterized
{
	public abstract class AlterationBase<T> : ParameterizedSourceBase<T, T>, IAlteration<T> {}

	public class DelegatedAlteration<T> : AlterationBase<T>
	{
		readonly Alter<T> source;

		public DelegatedAlteration( Alter<T> source )
		{
			this.source = source;
		}

		public override T Get( T parameter ) => source( parameter );
	}
}