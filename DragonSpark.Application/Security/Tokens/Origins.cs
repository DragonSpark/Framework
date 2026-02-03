using System;
using System.Net.Http;
using DragonSpark.Compose;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Application.Security.Tokens;

sealed class Origins : ReferenceValueStore<HttpRequestMessage, Uri>
{
    public static Origins Default { get; } = new();

    Origins() : base(x => new(x.RequestUri.Verify().GetLeftPart(UriPartial.Authority))) {}
}