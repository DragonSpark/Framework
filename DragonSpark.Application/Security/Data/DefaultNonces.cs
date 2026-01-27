using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Security.Data;

sealed class DefaultNonces : FixedSelection<byte, string>, IText
{
    public static DefaultNonces Default { get; } = new();

    DefaultNonces() : base(Nonces.Default, 24) {}
}