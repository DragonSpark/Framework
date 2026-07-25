using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results.Stop;
using Microsoft.AspNetCore.Http;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Additional;


public class DownloadPersonalData<T> : IStopAware<IResult> where T : class
{
    readonly IComposeUserDataInputs<T> _inputs;
    readonly IFormatUserData<T>        _format;

    protected DownloadPersonalData(IComposeUserDataInputs<T> inputs) : this(inputs, Properties.Default) {}

    protected DownloadPersonalData(IComposeUserDataInputs<T> inputs, IProperties properties)
        : this(inputs, new FormatUserData<T>(properties)) {}

    protected DownloadPersonalData(IComposeUserDataInputs<T> inputs, IFormatUserData<T> format)
    {
        _inputs = inputs;
        _format = format;
    }

    public async ValueTask<IResult> Get(CancellationToken parameter)
    {
        using var inputs = await _inputs.Off(parameter);
        var (users, user, context) = inputs;
        return user is not null
                   ? TypedResults.File(await _format.Off(inputs), contentType: "application/json",
                                       fileDownloadName: "PersonalData.json")
                   : Results.NotFound($"Unable to load user with ID '{users.Subject.GetUserId(context.User)}'.");
    }
}