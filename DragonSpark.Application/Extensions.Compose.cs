using AutoBogus;
using Bogus.Extensions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Communication;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Compose.Entities.Generation;
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Compose.Entities.Queries.Composition.Runtime;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Diagnostics;
using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Configure;
using DragonSpark.Application.Entities.Queries.Composition;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Compose.Model.Selection;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Runtime;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Linq.Expressions;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application;

// ReSharper disable once MismatchedFileName
public static partial class Extensions
{
	public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this, string name)
		where T : DbContext
		=> @this.Append(new ConfigureSqlServerWithMigration<T>(name));

	public static StorageConfigurationBuilder WithEnvironmentalConfiguration(this StorageConfigurationBuilder @this)
		=> @this.Append(EnvironmentalStorageConfiguration.Default);

	public static StorageConfigurationBuilder WithModel(this StorageConfigurationBuilder @this, IModel model)
		=> @this.Append(new RuntimeModelConfiguration(new ThreadAwareModel(model)));

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
	/**/

	public static BuildHostContext WithDataSecurity(this BuildHostContext @this)
		=> @this.Configure(Security.Data.Registrations.Default);

	public static BuildHostContext WithInitializationLogging<T>(this BuildHostContext @this)
		=> new(new InitializationAwareHostBuilder<T>(@this));

	/**/
	public static StoreContext<TIn, TOut> Store<TIn, TOut>(this Selector<TIn, TOut> @this) => new(@this);

	public static Compose.Store.Operations.StoreContext<TIn, TOut> Store<TIn, TOut>(
		this DragonSpark.Compose.Model.Operations.OperationResultSelector<TIn, TOut> @this)
		=> new(@this);

	public static Slide Slide(this TimeSpan @this) => new(@this);

	public static IWindow WithinLast(this ITime @this, TimeSpan within) => new Ago(@this, within);

	public static IWindow FromNow(this ITime @this, TimeSpan window) => new FromNow(@this, window);

	public static IWindow Outside(this ITime @this, TimeSpan window) => new Outside(@this, window);

	/**/

	public static QueryComposer<T> Query<T>(this ModelContext _) where T : class => Set<T>.Default.Then();

	public static ComposeComposer<T> Compose<T>(this ModelContext _) where T : class => new();

	public static ContextsComposer<T> Then<T>(this IContexts<T> @this) where T : DbContext => new(@this);

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

	//

	public static IQuery<T> Then<T>(this QueryComposer<None, T> @this) => new Query<T>(@this.Instance());

	public static OperationResultSelector<T?> Handle<T>(this OperationResultSelector<T?> @this,
	                                                    IExceptions exceptions, Type? reportedType = null)
		=> new(new ExceptionAwareResult<T>(@this, exceptions, reportedType));

	/**/

	public static GeneratorContext<T> Generator<T>(this ModelContext _, in uint? seed = null)
		where T : class => _.Generator<T>(new Entities.Generation.Configuration(seed));

	public static GeneratorContext<T> Generator<T>(this ModelContext _,
	                                               Action<IAutoGenerateConfigBuilder> configure)
		where T : class => _.Generator<T>(new Entities.Generation.Configuration(null, configure));

	public static GeneratorContext<T> Generator<T>(this ModelContext _, Entities.Generation.Configuration configuration)
		where T : class => new(configuration);

	public static IncludeMany<T, TOther> Between<T, TOther>(this IncludeMany<T, TOther> @this, Range range)
		where TOther : class
		=> @this.Generate((faker, _) => faker.GenerateBetween(range.Start.Value, range.End.Value));

	public static IncludeMany<T, TOther> Empty<T, TOther>(this IncludeMany<T, TOther> @this) where TOther : class
		=> @this.Generate((faker, _) => faker.Generate(0));
	/**/

	public static StartApiContext<T> Api<T>(this IServiceCollection @this) where T : class => new(@this);
}