using System.ComponentModel.DataAnnotations;

namespace DragonSpark.Application.AspNet.Entities.Configuration;

public sealed class Setting
{
	[MaxLength(32)]
	public string Id { get; init; } = default!;

	[MaxLength(256)]
	public string? Value { get; set; }
}

/*public class ConfigurationContext : DbContext
{
	public ConfigurationContext(DbContextOptions<ConfigurationContext> options) : base(options) {}
}

public class EntityConfigurationProvider : ConfigurationProvider
{
	readonly Func<DbContext> _context;

	public EntityConfigurationProvider(Func<DbContext> context) => _context = context;

	public override void Load()
	{
		using var context = _context();
		Data = context.Set<Setting>().ToDictionary(c => c.Id, c => c.Value, StringComparer.OrdinalIgnoreCase);
	}
}

public sealed class EntityConfigurationSource : IConfigurationSource
{
	readonly Func<DbContext> _create;

	public EntityConfigurationSource(Func<DbContext> create) => _create = create;

	public IConfigurationProvider Build(IConfigurationBuilder builder) => new EntityConfigurationProvider(_create);
}

sealed class CreateSqlContext : IResult<DbContext>
{
	readonly string? _connectionString;

	public CreateSqlContext(string? connectionString) => _connectionString = connectionString;

	public DbContext Get()
	{
		var builder = new DbContextOptionsBuilder<ConfigurationContext>().UseSqlServer(_connectionString);
		return new ConfigurationContext(builder.Options);
	}
}

public static class ConfigurationBuilderExtensions
{
	public static BuildHostContext WithEntityConfiguration<T>(this BuildHostContext @this) where T : DbContext
		=> @this.Select(Configuration.WithEntityConfiguration<T>.Default);
}

sealed class WithEntityConfiguration<T> : IAlteration<IHostBuilder> where T : DbContext
{
	public static WithEntityConfiguration<T> Default { get; } = new();

	WithEntityConfiguration() : this(ApplyEntityConfiguration<T>.Default.Execute) {}

	readonly Action<HostBuilderContext, IConfigurationBuilder> _configure;

	public WithEntityConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configure)
		=> _configure = configure;

	public IHostBuilder Get(IHostBuilder parameter) => parameter.ConfigureAppConfiguration(_configure);
}

sealed class ApplyEntityConfiguration<T> : ICommand<(HostBuilderContext Context, IConfigurationBuilder Builder)>
	where T : DbContext
{
	public static ApplyEntityConfiguration<T> Default { get; } = new();

	ApplyEntityConfiguration() {}

	public void Execute((HostBuilderContext Context, IConfigurationBuilder Builder) parameter)
	{
		var (_, builder) = parameter;
		var connectionString = builder.Build().GetConnectionString(A.Type<T>().Name);
		var context          = new CreateSqlContext(connectionString);
		var source           = new EntityConfigurationSource(context.Get);
		builder.Add(source);
	}
}*/