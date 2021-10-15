using DragonSpark.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace DragonSpark.Composition.Construction;

sealed class CanLocateDependency : ICondition<ParameterInfo>
{
	readonly Func<Type, string, bool> _specification;
	readonly bool                     _enableOptionalArguments;

	public CanLocateDependency(Func<Type, string, bool> specification, bool enableOptionalArguments = false)
	{
		_specification           = specification;
		_enableOptionalArguments = enableOptionalArguments;
	}

	public bool Get(ParameterInfo parameter) => _specification(parameter.ParameterType, string.Empty)
	                                            ||
	                                            !string.IsNullOrEmpty(parameter.Name)
	                                            &&
	                                            _specification(parameter.ParameterType, parameter.Name)
	                                            ||
	                                            _enableOptionalArguments && parameter.HasDefaultValue;
}