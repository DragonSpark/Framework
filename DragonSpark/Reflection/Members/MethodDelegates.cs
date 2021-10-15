using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using System;
using System.Reflection;

namespace DragonSpark.Reflection.Members;

sealed class MethodDelegates<T> : Select<MethodInfo, T> where T : Delegate
{
	public static MethodDelegates<T> Default { get; } = new MethodDelegates<T>();

	MethodDelegates() : base(x => (T)x.CreateDelegate(A.Type<T>())) {}
}