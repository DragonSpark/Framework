using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;

namespace DragonSpark.Server.Communication
{
	class ResponseState : ISelect<IRequestCookieCollection, Cookie>
	{
		readonly string                                 _name;
		readonly Func<IRequestCookieCollection, string> _state;

		public ResponseState(string name, Func<IRequestCookieCollection, string> state)
		{
			_name  = name;
			_state = state;
		}

		public Cookie Get(IRequestCookieCollection parameter) => new Cookie(_name, _state(parameter));
	}
}