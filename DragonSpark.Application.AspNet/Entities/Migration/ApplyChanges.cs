using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ApplyChanges : ICommand<DbContext>
{
	public static ApplyChanges Default { get; } = new();

	ApplyChanges() : this(new byte[8]) {}

	readonly Func<PropertyEntry, bool> _where;
	readonly Array<byte>               _timestamp;

	public ApplyChanges(byte[] timestamp)
		: this(x => x.Metadata.ClrType == typeof(byte[]) && !x.Metadata.IsNullable && x.CurrentValue is null,
		       timestamp) {}

	public ApplyChanges(Func<PropertyEntry, bool> where, byte[] timestamp)
	{
		_where     = where;
		_timestamp = timestamp;
	}

	public void Execute(DbContext parameter)
	{
		foreach (var entry in parameter.ChangeTracker.Entries().Where(x => x.State is EntityState.Added))
		{
			foreach (var property in entry.Properties.Where(_where))
			{
				property.CurrentValue = _timestamp.Open();
			}
		}
	}
}