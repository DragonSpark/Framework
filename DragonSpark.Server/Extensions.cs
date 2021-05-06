using DragonSpark.Application;
using DragonSpark.Model;
using DragonSpark.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static View NewView(this Controller @this, Guid subject) => new (@this, subject);
		public static View<T> NewView<T>(this Controller @this, T subject) => new (@this, subject);

		public static Request<None> New(this ControllerBase @this, Guid identity) => @this.New(identity, None.Default);

		public static Request<T> New<T>(this ControllerBase @this, Guid identity, T subject)
			=> new(@this, new(@this.User.UserName(), identity, subject));

		public static Query Query(this ControllerBase @this, Guid subject) => new(@this, subject);
		public static Query<T> Query<T>(this ControllerBase @this, T subject) => new(@this, subject);

		public static Query<TOther> Subject<T, TOther>(this @Query<T> @this, TOther subject)
			=> new Query<TOther>(@this.Owner, subject);
	}
}
