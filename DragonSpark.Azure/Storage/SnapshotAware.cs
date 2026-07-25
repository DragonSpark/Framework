using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using NetFabric.Hyperlinq;
using System.Buffers;

namespace DragonSpark.Azure.Storage;

public class SnapshotAware<T> : IStopAware<T>
{
	readonly IStopAware<T>                 _previous;
	readonly ISnapshots                    _snapshot;
	readonly Func<T, IEnumerable<string?>> _paths;

	protected SnapshotAware(IStopAware<T> previous, IContainer container, Func<T, IEnumerable<string?>> paths)
		: this(previous, container.Snapshots(), paths) {}

	public SnapshotAware(IStopAware<T> previous, ISnapshots snapshot, Func<T, IEnumerable<string?>> paths)
	{
		_previous = previous;
		_snapshot = snapshot;
		_paths    = paths;
	}

	public async ValueTask Get(Stop<T> parameter)
	{
		var (subject, stop) = parameter;
		using var paths = _paths(subject)
		                  .AsValueEnumerable()
		                  .Where(x => x is not null)
		                  .Select(x => x.Verify())
		                  .ToArray(ArrayPool<string>.Shared);
		await using var snapshot = await _snapshot.Off(new(paths.Memory, stop));
		try
		{
			await _previous.Off(parameter);
		}
		catch
		{
			await snapshot.Off(CancellationToken.None);
			throw;
		}
	}
}