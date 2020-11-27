using AsyncUtilities;
using DragonSpark.Application.Compose;
using DragonSpark.Application.Compose.Entities;
using DragonSpark.Application.Compose.Store;
using DragonSpark.Application.Entities;
using DragonSpark.Compose.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
			=> @this.WithIdentity(options => {});

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
	}
}
