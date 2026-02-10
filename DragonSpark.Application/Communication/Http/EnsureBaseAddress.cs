using System;
using System.Net.Http;
using DragonSpark.Model.Commands;

namespace DragonSpark.Application.Communication.Http;

sealed class EnsureBaseAddress : ICommand<HttpClient>
{
    readonly Uri _address;

    public EnsureBaseAddress(string address) : this(new Uri(address)) {}

    public EnsureBaseAddress(Uri address) => _address = address;

    public void Execute(HttpClient parameter)
    {
        parameter.BaseAddress ??= _address;
    }
}