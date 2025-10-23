using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Text;
using Refit;

namespace DragonSpark.Application.Communication.Http;

public sealed class FormUrlEncodedContentSerializer : IHttpContentSerializer
{
    public static FormUrlEncodedContentSerializer Default { get; } = new();

    FormUrlEncodedContentSerializer() : this(DefaultSerializer.Default.Get()) {}

    readonly IHttpContentSerializer           _previous;
    readonly IFormatter<ReadOnlyMemory<char>> _name;

    public FormUrlEncodedContentSerializer(IHttpContentSerializer previous) : this(previous, SnakeCase.Default) {}
    
    public FormUrlEncodedContentSerializer(IHttpContentSerializer previous, IFormatter<ReadOnlyMemory<char>> name)
    {
        _previous = previous;
        _name     = name;
    }

    public HttpContent ToHttpContent<T>(T item) => GetHttpContent<T>.Default.Get(item);

    public Task<T?> FromHttpContentAsync<T>(HttpContent content, CancellationToken cancellationToken = default)
        => _previous.FromHttpContentAsync<T>(content, cancellationToken);

    public string GetFieldNameForProperty(PropertyInfo propertyInfo) => _name.Get(propertyInfo.Name.AsMemory());
}