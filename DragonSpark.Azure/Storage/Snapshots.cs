using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Runtime;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Storage;

sealed class Snapshots : ISnapshots
{
	readonly ISnapshot _previous;

	public Snapshots(ISnapshot previous) => _previous = previous;

	public async ValueTask<ISnapshotEntry> Get(Stop<ReadOnlyMemory<string>> parameter)
	{
		var (subject, stop) = parameter;
		using var builder = ArrayBuilder.New<ISnapshotEntry>(subject.Length);
		for (byte i = 0; i < subject.Span.Length; i++)
		{
			var name = subject.Span[i];
			builder.UncheckedAdd(await _previous.Off(new(name, stop)));
		}

		return new SnapshotsEntry(builder.AsLease());
	}
}