﻿using AutoBogus;
using Bogus.Extensions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Communication;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Compose.Entities.Generation;
using DragonSpark.Application.Compose.Entities.Queries;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Diagnostics.Time;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Configure;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Application.Entities.Queries;
using DragonSpark.Compose;
using DragonSpark.Compose.Model.Operations;
using DragonSpark.Composition.Compose;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
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

namespace DragonSpark.Application
{
	// ReSharper disable once MismatchedFileName
	public static partial class Extensions
	{
		public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this)
			where T : DbContext
			=> @this.Append(ConfigureSqlServer<T>.Default.Execute);

		public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this, string name)
			where T : DbContext
			=> @this.Append(new ConfigureSqlServer<T>(name).Execute);

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
			=> @this.Then(new AuthorizeConfiguration(policy));
		/**/

		public static BuildHostContext WithDataSecurity(this BuildHostContext @this)
			=> @this.Configure(Security.Data.Registrations.Default);

		public static BuildHostContext WithInitializationLogging<T>(this BuildHostContext @this)
			=> new(new InitializationAwareHostBuilder<T>(@this));

		/**/
		public static StoreContext<TIn, TOut> Store<TIn, TOut>(
			this DragonSpark.Compose.Model.Selection.Selector<TIn, TOut> @this)
			=> new(@this);

		public static Compose.Store.Operations.StoreContext<TIn, TOut> Store<TIn, TOut>(
			this OperationResultSelector<TIn, TOut> @this)
			=> new(@this);

		public static Slide Slide(this TimeSpan @this) => new(@this);

		public static IWindow WithinLast(this ITime @this, TimeSpan within) => new Ago(@this, within);

		public static IWindow FromNow(this ITime @this, TimeSpan window) => new FromNow(@this, window);

		/**/

		/*public static QuerySelector<TIn, T> Then<TIn, T>(this Entities.Queries.IQuery<TIn, T> @this) where T : class
			=> new(@this);*/

		public static ContextsAdapter<T> Then<T>(this IContexts<T> @this) where T : DbContext => new(@this);

		public static QueryComposer<TIn, T> Then<TIn, T>(this IQuery<TIn, T> @this) => new(@this);

		public static QueryComposer<T> Then<T>(this IQuery<T> @this) => new(@this);

		/*public static ElidedParameterExpressionComposer<T> Then<T>(this IQuery<T> @this) => @this.Get().Then();*/

		public static PlaceholderParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, None, IQueryable<T>>> @this)
			=> new(@this);

		public static ElidedParameterExpressionComposer<T> Then<T>(this Expression<Func<DbContext, IQueryable<T>>> @this) => new(@this);

		// Scoped:

		public static ScopedQuerySelector<TIn, T> Then<TIn, T>(this Entities.Queries.Scoped.IQuery<TIn, T> @this)
			=> new(@this);

		public static Entities.Queries.Scoped.IQuery<TIn, T> Out<TIn, T>(this ISelect<TIn, IQueryable<T>> @this)
			=> new Entities.Queries.Scoped.Adapter<TIn, T>(@this);

		public static Entities.Queries.Scoped.IQuery<TIn, T> Out<TIn, T>(
			this DragonSpark.Compose.Model.Selection.Selector<TIn, IQueryable<T>> @this)
			=> @this.Get().Out();

		/**/

		public static GeneratorContext<T> Generator<T>(this ModelContext _, in uint? seed = null)
			where T : class => _.Generator<T>(new Configuration(seed));

		public static GeneratorContext<T> Generator<T>(this ModelContext _,
		                                               Action<IAutoGenerateConfigBuilder> configure)
			where T : class => _.Generator<T>(new Configuration(null, configure));

		public static GeneratorContext<T> Generator<T>(this ModelContext _, Configuration configuration)
			where T : class => new(configuration);

		public static IncludeMany<T, TOther> Between<T, TOther>(this IncludeMany<T, TOther> @this, Range range)
			where TOther : class
			=> @this.Generate((faker, _) => faker.GenerateBetween(range.Start.Value, range.End.Value));

		public static IncludeMany<T, TOther> Empty<T, TOther>(this IncludeMany<T, TOther> @this) where TOther : class
			=> @this.Generate((faker, _) => faker.Generate(0));
		/**/

		/*public static RegisterApiContext<T> Api<T>(this IServiceCollection @this, IHttpContentSerializer serializer)
			where T : class
			=> Api<T>(@this, new RefitSettings(serializer));*/

		public static StartApiContext<T> Api<T>(this IServiceCollection @this)
			where T : class
			=> new(@this);
	}
}