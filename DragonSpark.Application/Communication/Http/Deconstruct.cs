using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http;

sealed class Deconstruct : IDeconstruct
{
    public static Deconstruct Default { get; } = new();

    Deconstruct() {}

    public IEnumerable<KeyValuePair<string, string>> Get(IDictionary<string, object?> parameter)
    {
        foreach (var (key, value) in parameter)
        {
            yield return new(key, value as string ?? value?.ToString() ?? string.Empty);
        }
    }
}

sealed class Deconstruct<T> : IDeconstruct<T>
{
    public static Deconstruct<T> Default { get; } = new();

    Deconstruct() : this(SnakeCase.Default) {}

    readonly IFormatter<ReadOnlyMemory<char>> _name;

    public Deconstruct(IFormatter<ReadOnlyMemory<char>> name) => _name = name;

    public IEnumerable<KeyValuePair<string, string>> Get(T parameter)
    {
        if (parameter is not null)
        {
            foreach (var info in parameter.GetType()
                                          .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                          .Where(p => p.CanRead))
            {
                var value = info.GetValue(parameter);
                yield return new(_name.Get(info.Name.AsMemory()), value as string ?? value?.ToString() ?? string.Empty);
            }
        }
    }
}