namespace DragonSpark.Compose
{


	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		/*
		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this Query<_, T> @this, ISelect<T, TKey> key) => @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this Query<_, T> @this, ISelect<T, TKey> key, IEqualityComparer<TKey> comparer)
			=> @this.GroupMap(key.Get, comparer);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this Query<_, T> @this, Func<T, TKey> key)
			=> @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this Query<_, T> @this, Func<T, TKey> key,
		                                                                  IEqualityComparer<TKey> comparer)
			=> @this.Select(new GroupMap<T, TKey>(key, comparer));


		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this ISelect<_, T> @this, ISelect<T, TKey> key) => @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this ISelect<_, T> @this, ISelect<T, TKey> key, IEqualityComparer<TKey> comparer)
			=> @this.GroupMap(key.Get, comparer);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this ISelect<_, T> @this, Func<T, TKey> key)
			=> @this.GroupMap(key, EqualityComparer<TKey>.Default);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this ISelect<_, T> @this, Func<T, TKey> key,
		                                                                  IEqualityComparer<TKey> comparer)
			=> @this.Select(new GroupMapAdapter<T, TKey>(new GroupMap<T, TKey>(key, comparer)).Get);


/*Select(new GroupMapAdapter<,>(new GroupMap<Type, Type>(definition.Get().Get)))*/
	}
}