using DragonSpark.Compose;
using DragonSpark.Model.Selection.Conditions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public class Assignment<T> : Condition<Claim>, IAssignment<T> where T : IdentityUser
	{
		readonly Action<T, string> _assign;

		protected Assignment(Action<T, string> assign, string type) : base(Start.A.Selection<Claim>()
		                                                                        .By.Calling(x => x.Type)
		                                                                        .Select(Is.EqualTo(type)))
			=> _assign = assign;

		public void Execute((T User, string Value) parameter)
		{
			_assign(parameter.User, parameter.Value);
		}
	}
}