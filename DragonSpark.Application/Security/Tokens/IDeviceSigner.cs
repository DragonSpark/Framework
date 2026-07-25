using DragonSpark.Model.Operations.Selection.Stop;

namespace DragonSpark.Application.Security.Tokens;

public interface IDeviceSigner : IAltering<ReadOnlyMemory<byte>>;