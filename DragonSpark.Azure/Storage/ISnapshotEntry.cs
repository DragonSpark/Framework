using DragonSpark.Model.Operations.Stop;
using System;

namespace DragonSpark.Azure.Storage;

public interface ISnapshotEntry : IStopAware, IAsyncDisposable;