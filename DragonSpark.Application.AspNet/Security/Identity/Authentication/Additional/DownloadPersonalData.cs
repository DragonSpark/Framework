using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;


public class DownloadPersonalData<T> : IResulting<IResult> where T : class
{
    readonly IComposeUserDataInputs<T> _inputs;
    readonly IFormatUserData<T>        _format;

    public DownloadPersonalData(IComposeUserDataInputs<T> inputs) : this(inputs, FormatUserData<T>.Default) {}

    public DownloadPersonalData(IComposeUserDataInputs<T> inputs, IFormatUserData<T> format)
    {
        _inputs = inputs;
        _format = format;
    }

    public async ValueTask<IResult> Get()
    {
        using var inputs = await _inputs.Off();
        var (users, user, context) = inputs;
        return user is not null
                   ? TypedResults.File(await _format.Off(inputs), contentType: "application/json",
                                       fileDownloadName: "PersonalData.json")
                   : Results.NotFound($"Unable to load user with ID '{users.Subject.GetUserId(context.User)}'.");
    }
}