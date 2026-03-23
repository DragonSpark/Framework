using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Reflection;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Grok.Chat;

sealed class FunctionParameters<T> : IResult<FunctionParameters>
{
    public static FunctionParameters<T> Default { get; } = new();

    FunctionParameters() : this([..typeof(T).GetProperties()]) {}

    readonly ImmutableArray<PropertyInfo> _properties;

    public FunctionParameters(ImmutableArray<PropertyInfo> properties) => _properties = properties;

    public FunctionParameters Get()
    {
        var properties = new Dictionary<string, ParameterSchema>();
        var required   = new List<string>();

        foreach (var p in _properties)
        {
            var (jsonType, itemsSchema) = GetJsonTypeAndItems(p.PropertyType);

            var isRequired = !p.PropertyType.IsGenericType ||
                             p.PropertyType.GetGenericTypeDefinition() != typeof(Nullable<>);

            if (isRequired)
                required.Add(p.Name);

            properties[p.Name] =
                new(jsonType, p.GetCustomAttribute<DescriptionAttribute>()?.Description ?? p.Name, itemsSchema);
        }

        return new(properties, required.ToArray());
    }

    static (string JsonType, ParameterSchema? Items) GetJsonTypeAndItems(Type type)
    {
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            type = type.GetGenericArguments()[0]; // unwrap nullable

        if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
        {
            var elementType = type.IsArray ? type.GetElementType().Verify() : type.GetGenericArguments()[0];
            var (elementJsonType, items) = GetJsonTypeAndItems(elementType);
            return ("array", new(elementJsonType, items));
        }

        var jsonType = type switch
        {
            {} t when t == typeof(string) => "string",
            {} t when t == typeof(int) || t == typeof(long) || t == typeof(short) => "integer",
            {} t when t == typeof(decimal) || t == typeof(double) || t == typeof(float) => "number",
            {} t when t == typeof(bool) => "boolean",
            {} t when t.IsClass && t != typeof(string) => "object",
            _ => "string" // fallback
        };

        return (jsonType, null);
    }
}