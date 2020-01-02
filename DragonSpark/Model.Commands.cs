using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using DragonSpark.Runtime;
using System;

// ReSharper disable TooManyArguments

namespace DragonSpark
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static Action ToDelegate(this ICommand<None> @this) => @this.Execute;

		public static Action<T> ToDelegate<T>(this ICommand<T> @this) => @this.Execute;

		public static Action<T> ToDelegateReference<T>(this ICommand<T> @this)
			=> Model.Commands.Delegates<T>.Default.Get(@this);

		public static void Execute<T>(this ICommand<Array<T>> @this, params T[] parameters)
			=> @this.Execute(parameters);

		public static void Execute<T1, T2>(this ICommand<(T1, T2)> @this, T1 first, T2 second)
			=> @this.Execute((first, second));

		public static void Execute<T1, T2, T3>(this ICommand<(T1, T2, T3)> @this, T1 first, T2 second, T3 third)
			=> @this.Execute((first, second, third));

		public static IAssign<TIn, TOut> ToAssignment<TIn, TOut>(this ISelect<TIn, IMembership<TOut>> @this)
			=> @this.Select(x => x.Add).Then().ToAssignment();

		public static void Assign<TKey, TValue>(this IAssign<TKey, TValue> @this, TKey key, TValue value)
			=> @this.Execute(Pairs.Create(key, value));

		public static ICommand<T> Pass<T>(this ICommand<T> @this, T parameter)
		{
			@this.Execute(parameter);
			return @this;
		}

		public static T Parameter<T>(this ICommand @this, T parameter) => @this.Parameter().Return(parameter);

		public static T Parameter<T>(this ICommand<T> @this, T parameter) => @this.Pass(parameter).Return(parameter);

		public static T Parameter<T>(this T @this) where T : class, ICommand
		{
			@this.Execute();
			return @this;
		}

		public static void Execute(this ICommand<None> @this)
		{
			@this.Execute(None.Default);
		}

		public static ISelect<T, None> ToSelect<T>(this ICommand<T> @this)
			=> new Model.Selection.Adapters.Action<T>(@this.Execute);
	}
}