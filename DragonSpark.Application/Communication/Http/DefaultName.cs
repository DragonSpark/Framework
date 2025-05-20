using System;
using DragonSpark.Compose;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http;

sealed class DefaultName<T> : IText
{
    public static implicit operator string(DefaultName<T> instance) => instance.Get();

    public static DefaultName<T> Default { get; } = new();

    DefaultName() : this(A.Type<T>()) {}

    readonly Type _type;

    public DefaultName(Type type) => _type = type;

    public string Get()
    {
        var name   = _type.Name;
        var result = _type.IsInterface ? name[1..] : name;
        return result;
    }
}