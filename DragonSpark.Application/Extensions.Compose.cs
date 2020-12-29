using AsyncUtilities;
using AutoBogus;
using Bogus.Extensions;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Communication;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Compose.Entities.Generation;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Entities;
using DragonSpark.Application.Entities.Generation;
using DragonSpark.Compose;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using System;
using System.Linq;

namespace DragonSpark.Application
{
	// ReSharper disable once MismatchedFileName
	public static partial class Extensions
	{
		public static StorageConfigurationBuilder WithSqlServer<T>(this StorageConfigurationBuilder @this)
			where T : DbContext
			=> @this.Append(ConfigureSqlServer<T>.Default.Execute);

		public static StorageConfigurationBuilder WithEnvironmentalConfiguration(this StorageConfigurationBuilder @this)
			=> @this.Append(EnvironmentalStorageConfiguration.Default);

		/**/

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this)
			=> @this.WithIdentity(_ => {});

		public static IdentityContext WithIdentity(this ApplicationProfileContext @this,
		                                           System.Action<IdentityOptions> configure)
			=> new IdentityContext(@this, configure);

		public static AuthenticationContext WithAuthentication(this ApplicationProfileContext @this)
			=> new AuthenticationContext(@this);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       ICommand<AuthorizationOptions> policy)
			=> @this.AuthorizeUsing(policy.Execute);

		public static ApplicationProfileContext AuthorizeUsing(this ApplicationProfileContext @this,
		                                                       System.Action<AuthorizationOptions> policy)
			=> @this.Then(new AuthorizeConfiguration(policy));

		/**/
		public static StoreContext<TIn, TOut> Store<TIn, TOut>(this Selector<TIn, TOut> @this)
			=> new StoreContext<TIn, TOut>(@this);

		public static Compose.Store.Operations.StoreContext<TIn, TOut> Store<TIn, TOut>(
			this OperationResultSelector<TIn, TOut> @this)
			=> new Compose.Store.Operations.StoreContext<TIn, TOut>(@this);

		public static Slide Slide(this TimeSpan @this) => new Slide(@this);

		public static OperationResultSelector<_, IQueryable<T>> Protected<_, T>(
			this OperationResultSelector<_, IQueryable<T>> @this, AsyncLock @lock) where T : class
			=> @this.Protecting(@lock).Select(new ProtectedQueries<T>(@lock));

		/**/

		public static ProjectionContext<_, TOut> Then<_, TOut>(this Selector<_, IQueryable<TOut>> @this)
			=> @this.Get().Then();

		public static ProjectionContext<_, TOut> Then<_, TOut>(
			this ISelect<_, IQueryable<TOut>> @this) => new ProjectionContext<_, TOut>(@this);

		/**/

		public static GeneratorContext<T> Generator<T>(this ModelContext _, in uint? seed = null)
			where T : class
		{
			return Generator<T>(_, new Configuration(seed));
		}

		public static GeneratorContext<T> Generator<T>(this ModelContext _,
		                                               System.Action<IAutoGenerateConfigBuilder> configure)
			where T : class
		{
			return Generator<T>(_, new Configuration(null, configure));
		}

		public static GeneratorContext<T> Generator<T>(this ModelContext _, Configuration configuration)
			where T : class => new GeneratorContext<T>(configuration);

		public static IncludeMany<T, TOther> Between<T, TOther>(this IncludeMany<T, TOther> @this, Range range) where TOther : class
			=> @this.Generate((faker, _) => faker.GenerateBetween(range.Start.Value, range.End.Value));
		public static IncludeMany<T, TOther> Empty<T, TOther>(this IncludeMany<T, TOther> @this) where TOther : class
			=> @this.Generate((faker, _) => faker.Generate(0));
		/**/

		public static RegisterApiContext<T> Api<T>(this IServiceCollection @this, IContentSerializer serializer)
			where T : class
			=> Api<T>(@this, new RefitSettings(serializer));

		public static RegisterApiContext<T> Api<T>(this IServiceCollection @this, RefitSettings? settings = null)
			where T : class
			=> new RegisterApiContext<T>(@this, settings);
	}
}
