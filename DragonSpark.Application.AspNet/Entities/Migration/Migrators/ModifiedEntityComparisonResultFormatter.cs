using DragonSpark.Application.AspNet.Entities.Migration.Planning.Comparison;
using DragonSpark.Text;
using System.Text;

namespace DragonSpark.Application.AspNet.Entities.Migration.Migrators;

sealed class ModifiedEntityComparisonResultFormatter : IFormatter<ModifiedEntityComparisonResult>
{
	public static ModifiedEntityComparisonResultFormatter Default { get; } = new();

	ModifiedEntityComparisonResultFormatter() {}

	public string Get(ModifiedEntityComparisonResult parameter)
	{
		var (from, to, modifications) = parameter;

		var builder = new StringBuilder();

		builder.AppendLine($"Entity '{from.Name}' differs from '{to.Name}':");
		builder.AppendLine();

		AppendKeyChanges(builder, modifications.Keys);
		AppendPropertyChanges(builder, modifications.Properties);
		AppendNavigationChanges(builder, modifications.Navigations);

		builder.AppendLine($"Total changes: {modifications.Changes}");

		return builder.ToString();
	}

	static void AppendKeyChanges(StringBuilder b, KeyChanges keys)
	{
		if (keys.Changes == 0) return;

		b.AppendLine("Keys:");

		if (keys.Added.Length > 0)
		{
			b.AppendLine("  Added:");
			foreach (var k in keys.Added)
				b.AppendLine($"    + {k.Signature}");
		}

		if (keys.Removed.Length > 0)
		{
			b.AppendLine("  Removed:");
			foreach (var k in keys.Removed)
				b.AppendLine($"    - {k.Signature}");
		}

		if (keys.Modified.Length > 0)
		{
			b.AppendLine("  Modified:");
			foreach (var k in keys.Modified)
				b.AppendLine($"    * {k.Signature}");
		}

		b.AppendLine();
	}

	static void AppendPropertyChanges(StringBuilder b, PropertyChanges props)
	{
		if (props.Changes == 0) return;

		b.AppendLine("Properties:");

		if (props.Added.Length > 0)
		{
			b.AppendLine("  Added:");
			foreach (var p in props.Added)
				b.AppendLine($"    + {p.Name}: {p.Type.Name}");
		}

		if (props.Removed.Length > 0)
		{
			b.AppendLine("  Removed:");
			foreach (var p in props.Removed)
				b.AppendLine($"    - {p.Name}: {p.Type.Name}");
		}

		if (props.Modified.Length > 0)
		{
			b.AppendLine("  Modified:");
			foreach (var p in props.Modified)
				b.AppendLine($"    * {p.Name}: {p.Type.Name}");
		}

		b.AppendLine();
	}

	static void AppendNavigationChanges(StringBuilder b, NavigationChanges navs)
	{
		if (navs.Changes == 0) return;

		b.AppendLine("Navigations:");

		if (navs.Added.Length > 0)
		{
			b.AppendLine("  Added:");
			foreach (var n in navs.Added)
				b.AppendLine($"    + {NavigationRecordFormatter.Default.Get(n)}");
		}

		if (navs.Removed.Length > 0)
		{
			b.AppendLine("  Removed:");
			foreach (var n in navs.Removed)
				b.AppendLine($"    - {NavigationRecordFormatter.Default.Get(n)}");
		}

		if (navs.Modified.Length > 0)
		{
			b.AppendLine("  Modified:");
			foreach (var n in navs.Modified)
				b.AppendLine($"    * {NavigationRecordFormatter.Default.Get(n)}");
		}

		b.AppendLine();
	}
}