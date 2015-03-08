using System;
using System.Security.Principal;

namespace DragonSpark.Application
{
	public class PrincipalChangedEventArgs : EventArgs
	{
		public PrincipalChangedEventArgs( IPrincipal principal )
		{
			Principal = principal;
		}

		public IPrincipal Principal { get; private set; }
	}
}