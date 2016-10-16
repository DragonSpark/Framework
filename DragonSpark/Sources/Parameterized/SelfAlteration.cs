namespace DragonSpark.Sources.Parameterized
{
	public class SelfAlteration<T> : AlterationBase<T>
	{
		public static SelfAlteration<T> Default { get; } = new SelfAlteration<T>();
		protected SelfAlteration() {}

		public override T Get( T parameter ) => parameter;
	}
}