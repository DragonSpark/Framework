using System;
using System.Diagnostics.Contracts;
using System.ServiceModel;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Communication.Configuration
{
	public class CurrentOperationContextCallbackFactory : GenericItemFactoryBase
	{
		protected override TItem Create<TItem>( IUnityContainer container, string buildName )
		{
			Contract.Assume( OperationContext.Current != null );

			try
			{
				var result = OperationContext.Current.GetCallbackChannel<TItem>();
				return result;
			}
			catch ( NullReferenceException )
			{}
			catch ( InvalidCastException )
			{}
			return default(TItem);
		}
	}
}