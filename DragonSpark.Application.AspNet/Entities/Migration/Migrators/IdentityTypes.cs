using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class IdentityTypes : Instances<Type>
{
	public static IdentityTypes Default { get; } = new();

	IdentityTypes() : base(typeof(uint), typeof(int), typeof(long), typeof(ulong), typeof(byte), typeof(sbyte),
	                       typeof(ushort), typeof(short)) {}
}