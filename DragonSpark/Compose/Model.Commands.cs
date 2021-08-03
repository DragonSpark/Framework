using DragonSpark.Compose.Model.Commands;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using DragonSpark.Model.Sequences.Collections;
using System;

// ReSharper disable TooManyArguments

namespace DragonSpark.Compose
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static System.Action<T> ToDelegate<T>(this ICommand<T> @this) => @this.Execute;

		public static System.Action<T> ToDelegateReference<T>(this ICommand<T> @this)
			=> Delegates<T>.Default.Get(@this);

		public static Action<TKey, TValue> ToAssignmentDelegate<TKey, TValue>(this IAssign<TKey, TValue> @this)
			=> @this.Assign;

		public static void Execute<T>(this ICommand<Array<T>> @this, params T[] parameters)
			=> @this.Execute(parameters);

		public static void Execute<T1, T2>(this ICommand<(T1, T2)> @this, T1 first, T2 second)
			=> @this.Execute((first, second));

		public static void Invoke<T1, T2>(this Action<T1, T2> @this, (T1, T2) parameter)
			=> @this.Invoke(parameter.Item1, parameter.Item2);

		public static void Execute<T1, T2, T3>(this ICommand<(T1, T2, T3)> @this, T1 first, T2 second, T3 third)
			=> @this.Execute((first, second, third));

		public static void Execute(this ICommand<None> @this)
		{
			@this.Execute(None.Default);
		}

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

		public static IAssign<TIn, TOut> ToAssignment<TIn, TOut>(this ISelect<TIn, IMembership<TOut>> @this)
			=> @this.Select(x => x.Add).Then().ToAssignment();

		public static void Assign<TKey, TValue>(this IAssign<TKey, TValue> @this, TKey key, TValue value)
			=> @this.Execute(Pairs.Create(key, value));

		public static void Assign<TKey, TValue>(this IAssign<TKey, TValue> @this, (TKey key, TValue value) parameter)
			=> @this.Execute(Pairs.Create(parameter.key, parameter.value));

		public static CommandContext<(T, T1)> Add<T, T1>(this ICommand<(T, T1)> @this, ICommand<T> other)
			=> @this.Then().Append(new SelectedParameterCommand<(T, T1), T>(other.Execute, x => x.Item1));

		public static CommandContext<(T, T2)> Add<T, T2>(this ICommand<(T, T2)> @this, ICommand<T2> other)
			=> @this.Then().Append(new SelectedParameterCommand<(T, T2), T2>(other.Execute, x => x.Item2));
	}
}