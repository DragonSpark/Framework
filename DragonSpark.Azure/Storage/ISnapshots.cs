using DragonSpark.Model.Operations.Selection.Stop;
using System;

namespace DragonSpark.Azure.Storage;

public interface ISnapshots : IStopAware<ReadOnlyMemory<string>, ISnapshotEntry>;