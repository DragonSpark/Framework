namespace DragonSpark.Coercion
{
	public class Coercer<T> : CoercerBase<T>
	{
		public static Coercer<T> Default { get; } = new Coercer<T>();
		protected Coercer() {}

		protected override T Apply( object parameter ) => default(T);
	}
}