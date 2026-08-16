using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Application.Environment.Development;

sealed class SequenceElementType : IAlteration<Type>
{
	public static SequenceElementType Default { get; } = new();

	SequenceElementType() : this(typeof(IEnumerable<>), typeof(IQueryable<>), typeof(IEnumerable<>)) {}

	readonly Type   _definition;
	readonly Type[] _definitions;

	public SequenceElementType(Type definition, params Type[] definitions)
	{
		_definition  = definition;
		_definitions = definitions;
	}

	public Type Get(Type parameter)
	{
		if (parameter.IsGenericType && _definitions.Contains(parameter.GetGenericTypeDefinition()))
		{
			return parameter.GetGenericArguments()[0];
		}

		var interfaces = parameter.GetInterfaces();
		for (int i = 0; i < interfaces.Length; i++)
		{
			if (interfaces[i].IsGenericType && interfaces[i].GetGenericTypeDefinition() == _definition)
			{
				return interfaces[i].GetGenericArguments()[0];
			}
		}

		return parameter;
	}
}