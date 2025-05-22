using System;
using System.Net.Http;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Communication;

public abstract class Configure : ICommand<HttpClient>
{
    readonly Uri _base;

    protected Configure(IResult<Uri> connection) : this(connection.Get()) {}

    protected Configure(Uri @base) => _base = @base;

    public void Execute(HttpClient parameter)
    {
        parameter.BaseAddress = _base;
    }
}