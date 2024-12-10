using System.Runtime.CompilerServices;
using DragonSpark.Model;
using DragonSpark.Model.Results;
using JetBrains.Annotations;

namespace DragonSpark.Compose;

// ReSharper disable once MismatchedFileName
public static partial class ExtensionMethods
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Or<T>(this Assigned<T> @this, T defaultValue) where T : struct
        => @this.IsAssigned ? @this.Instance : defaultValue;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Or<T>(this Assigned<T> @this, in T defaultValue) where T : struct
        => @this.IsAssigned ? @this.Instance : defaultValue;

    /*public static TOut Get<T, TOut>(this ISelect<Assigned<T>, TOut> @this) where T : struct
		=> @this.Get(Assigned<T>.Unassigned);

	public static TOut Invoke<T, TOut>(this Func<Assigned<T>, TOut> @this) where T : struct
		=> @this(Assigned<T>.Unassigned);*/

    [MustDisposeResource]
    public static Assignment<T> Assigned<T>(this IMutable<T?> @this, T value)
    {
        @this.Pass(value);
        return new(@this);
    }
}
