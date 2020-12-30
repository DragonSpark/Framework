using DragonSpark.Application;
using DragonSpark.Server.Requests;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server
{
	public static class Extensions
	{
		public static Request<T> New<T>(this ControllerBase @this, Guid identity, T subject)
			=> new(@this, new(@this.User.UserName(), identity, subject));
	}
}
