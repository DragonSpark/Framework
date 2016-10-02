using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public struct MethodMapping
	{
		public MethodMapping( MethodInfo interfaceMethod, MethodInfo mappedMethod )
		{
			InterfaceMethod = interfaceMethod;
			MappedMethod = mappedMethod;
		}

		public MethodInfo InterfaceMethod { get; }
		public MethodInfo MappedMethod { get; }
	}
}