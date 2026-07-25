using DragonSpark.Model.Sequences;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class IdentityTypes : Instances<Type>
{
	public static IdentityTypes Default { get; } = new();

	IdentityTypes() : base(typeof(uint), typeof(int), typeof(long), typeof(ulong), typeof(byte), typeof(sbyte),
	                       typeof(ushort), typeof(short)) {}
}