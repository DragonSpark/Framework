using DragonSpark.Reflection;
using DragonSpark.Runtime.Activation;

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static TTo To<T, TTo>(this T @this, I<TTo> select) where TTo : IActivateUsing<T> => select.From(@this);

		public static TTo New<TFrom, TTo>(this I<TTo> _, TFrom parameter)
			=> Runtime.Activation.New<TFrom, TTo>.Default.Get(parameter);

		public static T From<TIn, T>(this I<T> _, TIn parameter) where T : IActivateUsing<TIn>
			=> Activations<TIn, T>.Default.Get(parameter);
	}
}