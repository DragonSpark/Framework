using System.Collections.Generic;

namespace Common.EntityModel
{
	public interface IEntityService
	{
		void Add( object entity );
		IEnumerable<object> Query( object query );
		object Retrieve( IDictionary<string,object> key );
		void Update( object entity );
		void Delete( object entity );
	}
}