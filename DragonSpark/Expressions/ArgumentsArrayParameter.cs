using System.Linq.Expressions;
using System.Reflection;

namespace DragonSpark.Expressions
{
	class ArgumentsArrayParameter
	{
		public ArgumentsArrayParameter( MethodBase method, ParameterExpression parameter )
		{
			Method = method;
			Parameter = parameter;
		}

		public MethodBase Method { get; }
		public ParameterExpression Parameter { get; }
	}
}