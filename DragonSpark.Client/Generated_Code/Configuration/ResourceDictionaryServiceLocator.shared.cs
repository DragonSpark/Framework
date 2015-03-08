using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Windows;
using DragonSpark.Extensions;
using Microsoft.Practices.ServiceLocation;

namespace DragonSpark.Configuration
{
	public class ResourceDictionaryServiceLocator : IServiceLocator
	{
		static readonly MethodInfo 
			GetInstanceMethod = typeof(ResourceDictionaryServiceLocator).GetMethod( "GetInstance", new[] { typeof(string) } ),
			GetAllInstancesMethod = typeof(ResourceDictionaryServiceLocator).GetMethod( "GetAllInstances", Type.EmptyTypes );

		readonly ResourceDictionary dictionary;

		public ResourceDictionaryServiceLocator( ResourceDictionary dictionary )
		{
			Contract.Requires( dictionary != null );
			this.dictionary = dictionary;
		}

		/*[ContractInvariantMethod]
		void Invariant()
		{
			Contract.Invariant( dictionary != null );
		}*/

		public object GetService( Type serviceType )
		{
			var result = GetInstance( serviceType );
			return result;
		}

		public object GetInstance( Type serviceType )
		{
			var result = GetInstance( serviceType, null );
			return result;
		}

		public object GetInstance( Type serviceType, string key )
		{
			var result = GetInstanceMethod.MakeGenericMethod( serviceType ).Invoke( this, new object[] { key } );
			return result;
		}

		public IEnumerable<object> GetAllInstances( Type serviceType )
		{
			var result = GetAllInstancesMethod.MakeGenericMethod( serviceType ).Invoke( this, null ).To<IEnumerable>().Cast<object>();
			return result;
		}

		public TService GetInstance<TService>()
		{
			var result = GetInstance<TService>( null );
			return result;
		}

		public TService GetInstance<TService>( string key )
		{
			var keys = from name in dictionary.Keys.Cast<string>()
					   where name == key && dictionary[name] is TService
			           select (TService)dictionary[name];

			var all = GetAllInstances<TService>();
			var set = keys.Concat( all );

			var result = set.NotNull().FirstOrDefault();
			return result;
		}

		public IEnumerable<TService> GetAllInstances<TService>()
		{
			Contract.Ensures( Contract.Result<IEnumerable<TService>>() != null );
			var result = from key in dictionary.Keys.Cast<string>()
						 let creator = dictionary[key] as IInstanceSource<TService>
						 let item = creator == null && !Equals( dictionary[ key ], default(TService) ) && typeof(TService).IsInstanceOfType(dictionary[ key ]) ? (TService)dictionary[key] : default(TService)
			             let created = creator.Transform( i => i.Instance, () => item )
						 where !Equals( created, default(TService) )
			             select created;
			return result;		
		}
	}
}