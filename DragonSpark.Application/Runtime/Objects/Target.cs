using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using System;
using System.Text.Json;

namespace DragonSpark.Application.Runtime.Objects;

sealed class Target<T> : ITarget<T> where T : notnull
{
	readonly JsonSerializerOptions _options;
	readonly Type                  _targetType;
	readonly IMutable<object?>     _target;

	public Target(JsonSerializerOptions options) : this(options, A.Type<T>()) {}

	public Target(JsonSerializerOptions options, Type targetType)
		: this(new Map(targetType).Get(options), targetType, AmbientTarget.Default) {}

	public Target(JsonSerializerOptions options, Type targetType, IMutable<object?> target)
	{
		_options    = options;
		_targetType = targetType;
		_target     = target;
	}

	public void Execute(TargetInput<T> parameter)
	{
		var (target, content) = parameter;
		using var _       = _target.Assigned(target);
		JsonSerializer.Deserialize(content, _targetType, _options);
	}
}