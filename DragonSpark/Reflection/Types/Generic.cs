using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Reflection.Types
{
	public class Generic<T> : Select<Array<Type>, Func<T>>, IGeneric<T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(GenericSingleton.Default) {}
		}
	}

	public class Generic<T1, T> : Select<Array<Type>, Func<T1, T>>, IGeneric<T1, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1)) {}
		}
	}

	public class Generic<T1, T2, T> : Select<Array<Type>, Func<T1, T2, T>>, IGeneric<T1, T2, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2)) {}
		}
	}

	public class Generic<T1, T2, T3, T> : Select<Array<Type>, Func<T1, T2, T3, T>>, IGeneric<T1, T2, T3, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T3, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2), typeof(T3)) {}
		}
	}

	public class Generic<T1, T2, T3, T4, T> : Select<Array<Type>, Func<T1, T2, T3, T4, T>>, IGeneric<T1, T2, T3, T4, T>
	{
		public Generic(Type definition) : base(new MakeGenericType(definition).Select(Delegates.Default.Get)) {}

		sealed class Delegates : ActivationDelegates<Func<T1, T2, T3, T4, T>>
		{
			public static Delegates Default { get; } = new Delegates();

			Delegates() : base(typeof(T1), typeof(T2), typeof(T3), A.Type<T4>()) {}
		}
	}
}