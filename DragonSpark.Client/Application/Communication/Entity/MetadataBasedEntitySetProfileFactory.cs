using System;
using System.Collections.Generic;
using System.Linq;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using DragonSpark.Objects.Synchronization;

namespace DragonSpark.Application.Communication.Entity
{
	public class MetadataBasedEntitySetProfileFactory : Factory<IEnumerable<IEntitySetProfile>>
	{
		public static MetadataBasedEntitySetProfileFactory Instance
		{
			get { return InstanceField; }
		}	static readonly MetadataBasedEntitySetProfileFactory InstanceField = new MetadataBasedEntitySetProfileFactory();

		protected override IEnumerable<IEntitySetProfile> CreateItem( object source )
		{
			var result = AppDomain.CurrentDomain.GetAllTypesWith<EntitySetAttribute>().OrderBy( x => x.Item1.Order ).Select( CreateDescriptor ).ToArray();
			return result;
		}

		static IEntitySetProfile CreateDescriptor( Tuple<EntitySetAttribute,Type> input )
		{
			var result = new EntitySetProfile { EntityType = input.Item2, AuthorizedRoles = input.Item1.AuthorizedRoles.ToStringArray() }.SynchronizeFrom( input.Item1, "EntityType" );
			var operations = input.Item2.GetAttributes<EntitySetOperationAttribute>().Select( x => new EntitySetOperationDescriptor { AuthorizedRoles = x.AuthorizedRoles.ToStringArray() }.SynchronizeFrom( x ) );
			operations.Apply( result.OperationDescriptors.Add );
			
			return result;
		}
	}
}