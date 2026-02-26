using System.Threading.Tasks;
using DragonSpark.Application.Mobile.Attestation;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Attestation;

sealed class AssertionToken : IAssertionToken
{
    public static AssertionToken Default { get; } = new();

    AssertionToken() {}

    public ValueTask<string> Get(Stop<string> parameter) => string.Empty.ToOperation();
}