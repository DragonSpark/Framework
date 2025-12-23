using System;
using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Maui.Presentation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Allocated;

namespace DragonSpark.Application.Mobile.Maui.Device.Security;

sealed class DeterminePasskeySupportViaBrowser : IAllocated<DeterminePasskeySupportViaBrowserInput>
{
    public static DeterminePasskeySupportViaBrowser Default { get; } = new();

    DeterminePasskeySupportViaBrowser() : this(PopModal.Default, PasskeyBrowserDetectionScript.Default) {}

    readonly IAllocated<bool> _pop;
    readonly string           _script;

    public DeterminePasskeySupportViaBrowser(IAllocated<bool> pop, string script)
    {
        _pop    = pop;
        _script = script;
    }

    public async Task Get(DeterminePasskeySupportViaBrowserInput parameter)
    {
        var (subject, source) = parameter;
        try
        {
            await subject.EvaluateJavaScriptAsync(_script).On();
            var evaluate = await subject.EvaluateJavaScriptAsync("window.result").Off();
            var result   = evaluate.Trim().Equals(bool.TrueString, StringComparison.OrdinalIgnoreCase);
            source.SetResult(result);
        }
        catch(Exception error)
        {
            source.SetException(error);
        }
        finally
        {
            await _pop.Main(false).Off();
        }
    }
}