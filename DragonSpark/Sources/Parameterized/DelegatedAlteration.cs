namespace DragonSpark.Sources.Parameterized
{
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