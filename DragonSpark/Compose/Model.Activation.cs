using DragonSpark.Compose.Extents;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static TTo To<T, TTo>(this T @this, Extent<TTo> select) where TTo : IActivateUsing<T>
			=> select.From(@this);

		public static TTo New<TFrom, TTo>(this Extent<TTo> _, TFrom parameter)
			=> Runtime.Activation.New<TFrom, TTo>.Default.Get(parameter);

		public static T From<TIn, T>(this Extent<T> _, TIn parameter) where T : IActivateUsing<TIn>
			=> Activations<TIn, T>.Default.Get(parameter);
	}
}