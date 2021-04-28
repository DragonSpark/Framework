using DragonSpark.Application;
using DragonSpark.Model;
using DragonSpark.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static Request<None> New(this ControllerBase @this, Guid identity) => @this.New(identity, None.Default);

		public static Request<T> New<T>(this ControllerBase @this, Guid identity, T subject)
			=> new(@this, new(@this.User.UserName(), identity, subject));

		public static Query<T> New<T>(this ControllerBase @this, T subject) => new(@this, subject);
	}
}
