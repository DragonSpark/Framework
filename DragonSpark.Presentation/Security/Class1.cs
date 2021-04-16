﻿using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Security
{
	class Class1 {}

	public sealed class ApplyPolicy : ApplyHeader
	{
		public ApplyPolicy(RequestDelegate next, string value) : base(next, "Content-Security-Policy", value) {}

		public Task Invoke(HttpContext parameter) => Get(parameter); // Sigh...
	}

	public class ApplyHeader : IAllocated<HttpContext>
	{
		readonly RequestDelegate _next;
		readonly string          _name;
		readonly string          _value;

		protected ApplyHeader(RequestDelegate next, string name, string value)
		{
			_next  = next;
			_name  = name;
			_value = value;
		}

		public Task Get(HttpContext parameter)
		{
			parameter.Response.Headers.Add(_name, _value);
			return _next(parameter);
		}
	}
}