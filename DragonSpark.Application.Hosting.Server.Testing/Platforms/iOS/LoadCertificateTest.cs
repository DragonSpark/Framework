using System.Threading.Tasks;
using DragonSpark.Server.Mobile.Platforms.iOS;
using JetBrains.Annotations;
using Xunit;

namespace DragonSpark.Application.Hosting.Server.Testing.Platforms.iOS;

[TestSubject(typeof(RootCertificate))]
public class RootCertificateTest
{

    [Fact]
    public Task Verify()
    {
        /*var factory = new ServiceCollection().AddHttpClient().BuildServiceProvider().GetRequiredService<IHttpClientFactory>();
        var bytes = await new RootCertificate(factory).Get(CancellationToken.None);
        bytes.Open().Should().NotBeEmpty();*/
        return Task.CompletedTask;
    }
}