using System;
using DragonSpark.Application.Runtime.Objects;
using DragonSpark.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Properties;
using DragonSpark.Text;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Runtime;

public class SessionVariable<T> : IProperty<HttpContext, T?> where T : notnull
{
    readonly string          _key;
    readonly Func<T, byte[]> _formatter;
    readonly Func<byte[], T> _parser;

    protected SessionVariable(string key) : this(key, DefaultSerializer<T>.Default) {}

    protected SessionVariable(string key, ISerializer<T> serializer)
        : this(key, serializer.Then().Select(TextAsBinary.Default), serializer.Get) {}

    protected SessionVariable(string key, Func<T, byte[]> formatter, Func<byte[], T> parser)
    {
        _key       = key;
        _formatter = formatter;
        _parser    = parser;
    }

    public T? Get(HttpContext parameter)
        => parameter.Session.TryGetValue(_key, out var value) ? _parser(value) : default;

    public void Execute(Pair<HttpContext, T?> parameter)
    {
        var (subject, value) = parameter;

        if (value is not null)
        {
            subject.Session.Set(_key, _formatter(value));
        }
        else
        {
            subject.Session.Remove(_key);
        }
    }
}

public class SessionVariable : SessionVariable<string>
{
    protected SessionVariable(string key) : base(key, Serializer.Default) {}

    sealed class Serializer : Serializer<string>
    {
        public static Serializer Default { get; } = new();

        Serializer() : base(Formatter.Instance, SelfParser.Instance, SelfTarget.Instance) {}

        sealed class Formatter : Formatter<string>
        {
            public static Formatter Instance { get; } = new();

            Formatter() : base(x => x) {}
        }

        sealed class SelfParser : Parser<string>
        {
            public static SelfParser Instance { get; } = new();

            SelfParser() : base(x => x) {}
        }

        sealed class SelfTarget : ITarget<string>
        {
            public static SelfTarget Instance { get; } = new();

            SelfTarget() {}

            public void Execute(TargetInput<string> parameter) {}
        }
    }
}