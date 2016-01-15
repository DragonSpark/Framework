using System;
using System.Reflection;
using AutoMapper.Internal;
using DragonSpark.Aspects;
using DragonSpark.Extensions;

namespace DragonSpark.TypeSystem
{
	public static class Default<T>
	{
		public static Func<T, T> Self => t => t;

		[Freeze]
		public static T Item => DefaultFactory<T>.Instance.Create();

		[Freeze]
		public static T[] Items => DefaultFactory<T[]>.Instance.Create();
	}

	public static class Type
	{
		public static System.Type From( object item ) => item.AsTo<ParameterInfo, System.Type>( info => info.ParameterType ) ?? item.AsTo<MemberInfo, System.Type>( info => info.GetMemberType() ) ?? item as System.Type;
	}
}