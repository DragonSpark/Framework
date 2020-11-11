﻿using DragonSpark.Application.Entities;
using DragonSpark.Application.Runtime;
using DragonSpark.Application.Security;
using DragonSpark.Application.Security.Identity;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityUser = DragonSpark.Application.Security.Identity.IdentityUser;

namespace DragonSpark.Application
{
	partial class Extensions
	{
		public static async ValueTask<T> Latest<T>(this DbContext @this, T entity) where T : notnull
		{
			await @this.Entry(entity).ReloadAsync().ConfigureAwait(false);
			return entity;
		}

		public static IQueryable<TEntity> Includes<TEntity>(this IQueryable<TEntity> source, params string[] includes)
			where TEntity : class => includes.Aggregate(source, (entities, include) => entities.Include(include));


		public static IQueryable<T> Protected<T>(this IQueryable<T> @this) where T : class
			=> ProtectedQueries<T>.Default.Get(@this);

		public static IQuerying<T> Querying<T>(this IQueryable<T> @this)
			=> @this as IQuerying<T> ?? new Querying<T>(@this);
		/**/

		public static string UniqueId(this ExternalLoginInfo @this) => Security.Identity.UniqueId.Default.Get(@this);

		public static ProviderIdentity Identity(this IdentityUser @this) => Identities.Default.Get(@this.UserName);

		public static async ValueTask<T> GetUser<T>(this UserManager<T> @this, ExternalLoginInfo login)
			where T : IdentityUser
		{
			var id     = login.UniqueId();
			var result = await @this.Users.SingleAsync(x => x.UserName == id);
			return result;
		}

		public static bool HasClaim(this ClaimsPrincipal @this, IResult<string> claim)
		{
			var type   = claim.Get();
			var result = @this.HasClaim(x => x.Type == type);
			return result;
		}

		public static Claim Claim(this Text.Text @this, string value) => new Claim(@this, value);

		public static string DisplayName(this ClaimsPrincipal @this) => @this.DisplayName(Anonymous.Default);

		public static string DisplayName(this ClaimsPrincipal @this, string anonymous)
			=> @this.FindFirstValue(Security.Identity.DisplayName.Default) ?? @this.UserName(anonymous);

		public static string UserName(this ClaimsPrincipal @this) => @this.UserName(Anonymous.Default);

		public static string UserName(this ClaimsPrincipal @this, string anonymous) => @this.Identity?.Name ?? anonymous;

		public static IUserMapping Promote<T>(this IAccessor<T> @this, string key, bool required = false)
			where T : IdentityUser
			=> new UserMapping<T>(@this, key, required);

		public static string? Value(this ModelBindingContext @this, IResult<string> key)
			=> @this.ValueProvider.Get(key);

		public static string? Get(this IValueProvider @this, IResult<string> key)
		{
			var name   = key.Get();
			var value  = @this.GetValue(name);
			var result = value != ValueProviderResult.None ? value.FirstValue : null;
			return result;
		}

		public static async Task<AuthenticationState<T>> Promote<T>(this Task<AuthenticationState> @this)
			where T : class
		{
			var state  = await @this;
			var result = state.To<AuthenticationState<T>>();
			return result;
		}

		public static T User<T>(this AuthenticationState @this) where T : class
			=> @this.To<AuthenticationState<T>>().Profile.Verify();

		/**/

		public static Array<T> ApplyOrder<T>(this Array<T> @this) where T : class, IOrderAware
			=> Ordered<T>.Default.Get(@this);

		public static SelectedCollection<T> ToSelectedCollection<T>(this IEnumerable<T> @this) where T : class
			=> new SelectedCollection<T>(@this);

		/**/

		public static (Type Owner, string Name) Key(this FieldIdentifier @this)
			=> (@this.Model.GetType(), @this.FieldName);
	}
}
