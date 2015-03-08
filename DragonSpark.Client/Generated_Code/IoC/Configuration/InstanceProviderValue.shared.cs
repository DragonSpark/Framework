using System;
using System.Reflection;
using DragonSpark.Configuration;
using DragonSpark.Extensions;

namespace DragonSpark.IoC.Configuration
{
	public class InstanceProviderValue : InstanceValue
	{
		readonly static MethodInfo ResolveMethod = typeof(InstanceProviderValue).GetMethod( "Resolve", DragonSparkBindingOptions.AllProperties );

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type CreatorItemType { get; set; }

		TResult Resolve<TResult>()
		{
			var result = Instance.As<IInstanceSource<TResult>>().Transform( item => item.Instance );
			return result;
		}

		protected override object ResolveInstance()
		{
			var method = ResolveMethod.MakeGenericMethod( CreatorItemType ?? ParameterType );
			var result = method.Invoke( this, null );
			return result;
		}
	}
}