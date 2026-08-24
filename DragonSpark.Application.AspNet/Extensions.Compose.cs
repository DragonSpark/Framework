using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Compose.Entities;
using DragonSpark.Application.AspNet.Compose.Entities.Queries;
using DragonSpark.Application.AspNet.Compose.Entities.Queries.Composition.Runtime;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Configure;
using DragonSpark.Application.AspNet.Entities.Initialization;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Security.Data;
using DragonSpark.Application.AspNet.Security.Identity.State;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet;

// ReSharper disable once MismatchedFileName
public static partial class Extensions
{
	extension(StorageConfigurationBuilder @this)
	{
		public StorageConfigurationBuilder WithSqlServer(string name)
			=> @this.WithSqlServer(name, _ => {});

		public StorageConfigurationBuilder WithSqlServer(string name,
		                                                 Action<SqlServerDbContextOptionsBuilder> configure)
			=> @this.Append(new ConfigureSqlServer(name, configure));

		public StorageConfigurationBuilder WithSqlServer(string name,
		                                                 string migrations)
			=> @this.Append(new ConfigureSqlServerWithMigration(name, migrations));

		public StorageConfigurationBuilder WithSqlServer<T>()
			where T : DbContext => @this.WithSqlServer<T>(_ => {});

		public StorageConfigurationBuilder WithSqlServer<T>(Action<SqlServerDbContextOptionsBuilder> configure)
			where T : DbContext
			=> @this.Append(new ConfigureSqlServer<T>(configure));

		public StorageConfigurationBuilder WithSqlServer<T>(string name)
			where T : DbContext => @this.WithSqlServer<T>(name, _ => {});

		public StorageConfigurationBuilder WithSqlServer<T>(string name,
		                                                    Action<SqlServerDbContextOptionsBuilder> configure)
			where T : DbContext
			=> @this.Append(new ConfigureSqlServerWithMigration<T>(name, configure));

		public StorageConfigurationBuilder ApplySeeding()
			=> ApplySeeding(@this, ApplyMigrationRegistry.Default.Get);

		public StorageConfigurationBuilder ApplySeeding(Func<Stop<DbContext>, Task> configure)
			=> @this.Append(_ => new ApplySeeding(configure).Execute);

		public StorageConfigurationBuilder WithEnvironmentalConfiguration()
			=> @this.Append(EnvironmentalStorageConfiguration.Default);

		public StorageConfigurationBuilder WithModel(IModel model)
			=> @this.Append(new RuntimeModelConfiguration(model));
	}

	/**/

	extension(ApplicationProfileContext @this)
	{
		public IdentityStorage<T> WithIdentity<T>() where T : IdentityUser
			=> new(@this);

		public IdentityStorage<T> WithIdentity<T>(Action<IdentityOptions> configure)
			where T : IdentityUser
			=> @this.WithIdentity<T>(configure, _ => {});

		public IdentityStorage<T> WithIdentity<T>(Action<IdentityOptions> configure, Action<IdentityBuilder> builder)
			where T : IdentityUser
			=> new(@this, configure, builder);

		public AuthenticationContext WithAuthentication() => new(@this);

		public AuthenticationContext WithAuthentication(Action<AuthenticationBuilder> configure)
			=> new(@this, Start.A.Command(configure));

		public ApplicationProfileContext AuthorizeUsing(ICommand<AuthorizationOptions> policy)
			=> @this.AuthorizeUsing(policy.Execute);

		public ApplicationProfileContext AuthorizeUsing(Action<AuthorizationOptions> policy)
			=> @this.Append(new AuthorizeConfiguration(policy));

		public ApplicationProfileContext AuthorizeUsing<T>(Action<AuthorizationOptions, T> policy)
			where T : class
			=> @this.Append(new SelectedAuthorizeConfiguration<T>(policy));

		public ApplicationProfileContext WithEnvironmentalConfiguredSender()
			=> @this.Append(Messaging.Registrations.Default);

		public ApplicationProfileContext WithIdentityClaimsRelay()
			=> @this.Append(Security.Identity.Authentication.Persist.WithIdentityClaimsRelay.Default);
	}

	/**/

	public static BuildHostContext WithDataSecurity(this BuildHostContext @this)
		=> @this.Configure(Application.Security.Data.Registrations.Default).Configure(Registrations.Default);

	/**/

	/**/

	extension(ModelContext _)
	{
		public QueryComposer<T> Query<T>() where T : class => Set<T>.Default.Then();

		public ComposeComposer<T> Compose<T>() where T : class => new();
	}

	public static ContextsComposer<T> Then<T>(this INewContext<T> @this) where T : DbContext => new(@this);

	public static ScopesComposer Then(this IScopes @this) => new(@this);

	public static QueryComposer<TIn, T> Then<TIn, T>(this IQuery<TIn, T> @this) => new(@this);

	public static TrackingComposer<TIn, T> Tracking<TIn, T>(this QueryComposer<TIn, T> @this) where T : class
		=> new(@this);

	public static QueryComposer<T> Then<T>(this IQuery<None, T> @this) => new(@this);

	public static IQuery<T> Out<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());

	public static PlaceholderParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, None, T>> @this)
		=> new(@this);

	public static ElidedParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, T>> @this) => new(@this);

	public static In<None> Subject<T>(this In<T> @this) => new(@this.Context, None.Default);

	public static In<TTo> Subject<T, TTo>(this In<T> @this, TTo subject) => new(@this.Context, subject);

	public static QueryComposer<TIn, T?> Account<TIn, T>(this QueryComposer<TIn, T> @this) where T : struct
		=> @this.Select(x => new T?(x));

	public static QueryComposer<TIn, TEntity> Include<TIn, TEntity, TOther>(this QueryComposer<TIn, TEntity> source,
	                                                                        Expression<Func<TEntity, TOther>> path)
		where TEntity : class
		=> source.Select(q => q.Include(path));

	public static QueryComposer<TIn, TEntity> Include<TIn, TEntity>(this QueryComposer<TIn, TEntity> source,
	                                                                string include)
		where TEntity : class
		=> source.Select(q => q.Include(include));

	public static QueryComposer<TIn, TEntity> Includes<TIn, TEntity>(this QueryComposer<TIn, TEntity> source,
	                                                                 params string[] includes)
		where TEntity : class
		=> includes.Aggregate(source, (current, include) => current.Include(include));

	public static IQueryable<T> Includes<T>(this IQueryable<T> source, params string[] includes) where T : class
		=> includes.Aggregate(source, (current, include) => current.Include(include));

	/**/
	/*public static Compose.OperationResultComposer<_, T> Then<_, T>(this DragonSpark.Compose.Model.Operations.OperationResultComposer<_,T> @this)
		=> new(@this.Out());*/

	public static InstanceComposer<TIn, T> Then<TIn, T>(this IInstance<TIn, T> @this) => new(@this);

	public static InstanceComposer<T> Then<T>(this IInstance<T> @this) => new(@this);

	public static IQuery<T> Then<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());

	public static OperationResultComposer<T?> Handle<T>(this OperationResultComposer<T?> @this,
	                                                    IExceptions exceptions, Type? reportedType = null)
		=> new(new ExceptionAwareResult<T>(@this, exceptions, reportedType));

	extension(BuildHostContext @this)
	{
		public BuildHostContext WithFrameworkConfigurations()
			=> Configure.Default.Get(@this);

		public ApplicationProfileContext Apply(IApplicationProfile profile)
			=> new(@this, profile);
	}

	extension(IServiceCollection @this)
	{
		public IServiceCollection AddHttpIdentity() => Communication.Http.Registrations.Default.Parameter(@this);

		public IDataProtectionBuilder ApplyServerSettings<T>()
			where T : SystemServerSettings
			=> Security.Identity.State.ApplyServerSettings<T>.Default.Get(@this);
	}

	

	extension(BuildHostContext @this)
	{
		public BuildHostContext WithHostedConfiguration()
			=> @this.Configure(Configuration.Registrations.Default);

		public BuildHostContext WithIssuedTokens()
			=> @this.Configure(Security.Tokens.Registrations.Default);
	}

	/**/
}