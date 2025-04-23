using System;
using DragonSpark.Application.Runtime.Process;
using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Mobile.Uno.Security.Authentication;

sealed class ResumeSession : ISelect<ResumeSessionInput, Uri?>
{
    readonly uint                                _process;
    readonly ISelect<Uri, AuthenticationSession> _session;
    public static ResumeSession Default { get; } = new();

    ResumeSession() : this(GetProcessIdentity.Default, ParseSession.Default) {}

    public ResumeSession(uint process, ISelect<Uri, AuthenticationSession> session)
    {
        _process = process;
        _session = session;
    }

    public Uri? Get(ResumeSessionInput parameter)
    {
        var (identifier, address) = parameter;
        var (process, id, state)  = _session.Get(address);
        if (_process == process && identifier == id)
        {
            var builder = new UriBuilder(address);
            var query   = System.Web.HttpUtility.ParseQueryString(address.Query);
            query["state"] = state;
            builder.Query  = query.ToString();
            return builder.Uri;
        }

        return null;
    }
}