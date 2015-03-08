using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Extensions;

namespace Common.EntityModel
{
	class EntityServiceProxy :  IEntityService
	{
		readonly Type serviceType;
		readonly object instance;

		public EntityServiceProxy( Type serviceType, object instance )
		{
			this.serviceType = serviceType;
			this.instance = instance;
		}

		#region Implementation of IEntityService
		void IEntityService.Add( object entity )
		{
			Invoke( EntityServiceOperation.Add, entity );
		}

		IEnumerable<object> IEntityService.Query( object query )
		{
			var result =
				ResolveMethod( EntityServiceOperation.Retrieve ).Translate(
					item => item.Invoke( instance, query.ToEnumerable().ToArray() ) ).As<IEnumerable>().Cast<object>();
			return result;
		}

		MethodInfo ResolveMethod( EntityServiceOperation operation )
		{
			var query = from method in serviceType.GetMethods()
			            where
			            	method.GetAttribute<EntityServiceOperationAttribute>().Translate( item => item.Operation == operation )
			            select method;
			var result = query.SingleOrDefault();
			return result;
		}

		object IEntityService.Retrieve( IDictionary<string, object> key )
		{
			var result =
				ResolveMethod( EntityServiceOperation.Retrieve ).Translate(
					item => item.Invoke( instance, ( from k in key.Keys select key[ k ] ).ToArray() ) );
			return result;
		}

		void IEntityService.Update( object entity )
		{
			Invoke( EntityServiceOperation.Update, entity );
		}

		void Invoke( EntityServiceOperation operation, object entity )
		{
			ResolveMethod( operation ).NotNull( item => item.Invoke( instance, entity.ToEnumerable().ToArray() ) );
		}

		void IEntityService.Delete( object entity )
		{
			Invoke( EntityServiceOperation.Delete, entity );
		}
		#endregion
	}
}