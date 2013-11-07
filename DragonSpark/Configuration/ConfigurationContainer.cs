using Microsoft.Practices.ServiceLocation;
using System.Diagnostics.Contracts;
using System.Windows;

namespace DragonSpark.Configuration
{
	public class ConfigurationDictionary : ConfigurationDictionary<IServiceLocator>
	{
		protected override IServiceLocator ResolveInstance()
		{
			return Locator;
		}
	}

	public abstract class ConfigurationDictionary<TInstance> : ResourceDictionary, IInstanceSource<TInstance> where TInstance : class 
	{
		protected virtual IServiceLocator ResolveLocator()
		{
			Contract.Ensures( Contract.Result<IServiceLocator>() != null );
			var result = new ResourceDictionaryServiceLocator( this );
			return result;
		}

		public TInstance Instance
		{
			get { return instance ?? ( instance = ResolveInstance() ); }
		}	TInstance instance;

		protected virtual TInstance ResolveInstance()
		{
			var result = Locator.GetInstance<TInstance>();
			return result;
		}

		protected IServiceLocator Locator
		{
			get { return locator ?? ( locator = ResolveLocator() ); }
		}	IServiceLocator locator;
	}
}
