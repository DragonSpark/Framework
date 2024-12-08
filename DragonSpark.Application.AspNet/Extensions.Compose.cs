using DragonSpark.Application.AspNet.Compose;
using DragonSpark.Application.AspNet.Compose.Entities;
using DragonSpark.Application.AspNet.Compose.Entities.Queries;
using DragonSpark.Application.AspNet.Compose.Entities.Queries.Composition.Runtime;
using DragonSpark.Application.AspNet.Entities;
using DragonSpark.Application.AspNet.Entities.Configure;
using DragonSpark.Application.AspNet.Entities.Queries.Composition;
using DragonSpark.Application.AspNet.Security.Data;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Linq;
using System.Linq.Expressions;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Application.AspNet;

// ReSharper disable once MismatchedFileName
public static partial class Extensions
{
	public static StorageConfigurationBuilder WithSqlServer(this StorageConfigurationBuilder @this, string name)
		=> @this.WithSqlServer(name, _ => {});

	public static StorageConfigurationBuilder WithSqlServer(this StorageConfigurationBuilder @this, string name,
	                                                        Action<SqlServerDbContextOptionsBuilder> configure)
		=> @this.Append(new ConfigureSqlServer(name, configure));

	public static StorageConfigurationBuilder WithSqlServer(this StorageConfigurationBuilder @this, string name,
	                                                        string migrations)
		=> @this.Append(new ConfigureSqlServerWithMigration(name, migrations));

	public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this)
		where T : DbContext => @this.WithSqlServer<T>(_ => {});

	public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this,
	                                                           Action<SqlServerDbContextOptionsBuilder> configure)
		where T : DbContext
		=> @this.Append(new ConfigureSqlServer<T>(configure));

	public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this, string name)
		where T : DbContext
		=> @this.Append(new ConfigureSqlServerWithMigration<T>(name));

	public static StorageConfigurationBuilder WithEnvironmentalConfiguration(this StorageConfigurationBuilder @this)
		=> @this.Append(EnvironmentalStorageConfiguration.Default);

	public static StorageConfigurationBuilder WithModel(this StorageConfigurationBuilder @this, IModel model)
		=> @this.Append(new RuntimeModelConfiguration(model));

	/**/

	public static IdentityStorage<T> WithIdentity<T>(this ApplicationProfileContext @this) where T : IdentityUser
		=> new(@this);

	public static IdentityStorage<T> WithIdentity<T>(this ApplicationProfileContext @this,
	                                                 Action<IdentityOptions> configure)
		where T : IdentityUser
		=> new(@this, configure);

	public static AuthenticationContext WithAuthentication(this ApplicationProfileContext @this) => new(@this);

	public static AuthenticationContext WithAuthentication(this ApplicationProfileContext @this,
	                                                       Action<AuthenticationBuilder> configure)
		=> new(@this, Start.A.Command(configure));

	public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
	                                                       ICommand<AuthorizationOptions> policy)
		=> @this.AuthorizeUsing(policy.Execute);

	public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
	                                                       Action<AuthorizationOptions> policy)
		=> @this.Append(new AuthorizeConfiguration(policy));

	public static ApplicationProfileContext AuthorizeUsing<T>(this ApplicationProfileContext @this,
	                                                          Action<AuthorizationOptions, T> policy)
		where T : class
		=> @this.Append(new SelectedAuthorizeConfiguration<T>(policy));
	/**/

	public static BuildHostContext WithDataSecurity(this BuildHostContext @this)
		=> @this.Configure(Application.Security.Data.Registrations.Default).Configure(Registrations.Default);

	/*public static BuildHostContext WithInitializationLogging<T>(this BuildHostContext @this)
		=> new(new InitializationAwareHostBuilder<T>(@this));*/

	/**/

	public static ApplicationProfileContext WithEnvironmentalConfiguredSender(this ApplicationProfileContext @this)
		=> @this.Append(Messaging.Registrations.Default);

	/**/

	public static QueryComposer<T> Query<T>(this ModelContext _) where T : class => Set<T>.Default.Then();

	public static ComposeComposer<T> Compose<T>(this ModelContext _) where T : class => new();

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

	/**/
	/*public static Compose.OperationResultComposer<_, T> Then<_, T>(this DragonSpark.Compose.Model.Operations.OperationResultComposer<_,T> @this)
		=> new(@this.Out());*/

	public static InstanceComposer<TIn, T> Then<TIn, T>(this IInstance<TIn, T> @this) => new(@this);

	public static InstanceComposer<T> Then<T>(this IInstance<T> @this) => new(@this);

	public static IQuery<T> Then<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());

	public static OperationResultComposer<T?> Handle<T>(this OperationResultComposer<T?> @this,
	                                                    IExceptions exceptions, Type? reportedType = null)
		=> new(new ExceptionAwareResult<T>(@this, exceptions, reportedType));

}