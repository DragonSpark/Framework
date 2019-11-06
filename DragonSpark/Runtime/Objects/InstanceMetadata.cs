using System.Reflection;
using DragonSpark.Model.Selection;
using DragonSpark.Reflection.Types;

namespace DragonSpark.Runtime.Objects
{
	sealed class InstanceMetadata<T> : Select<T, TypeInfo>
	{
		public static InstanceMetadata<T> Default { get; } = new InstanceMetadata<T>();

		InstanceMetadata() : base(InstanceType<T>.Default.Select(TypeMetadata.Default)) {}
	}
}