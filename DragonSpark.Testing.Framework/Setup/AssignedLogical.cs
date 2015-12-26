using DragonSpark.Windows.Runtime;

namespace DragonSpark.Testing.Framework.Setup
{
	public class AssignedLogical<T> : LogicalValue<T>
	{
		public AssignedLogical() : base( typeof(T).AssemblyQualifiedName )
		{ }
	}
}