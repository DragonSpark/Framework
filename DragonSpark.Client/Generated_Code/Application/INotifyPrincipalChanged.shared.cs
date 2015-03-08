using System;
using System.Security.Principal;

namespace DragonSpark.Application
{
	public interface INotifyPrincipalChanged : IPrincipal
	{
		event EventHandler<PrincipalChangedEventArgs> PrincipalChanged;
	}
}