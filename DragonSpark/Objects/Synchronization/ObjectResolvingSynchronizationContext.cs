using System;
using System.Collections.Generic;
using DragonSpark.Extensions;
using System.Linq;

namespace DragonSpark.Objects.Synchronization
{
	public class ObjectResolvingSynchronizationContext : SynchronizationContext
	{
		readonly Dictionary<SynchronizationContainer,SynchronizationObjectResolver> resolvers = new Dictionary<SynchronizationContainer, SynchronizationObjectResolver>();
		readonly IObjectResolver resolver;
		readonly string mappingName;
		readonly bool includeBasePolicies;

		public ObjectResolvingSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression ) : this( resolver, firstExpression, secondExpression, null )
		{}

		public ObjectResolvingSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression, string mappingName ) : this( resolver, firstExpression, secondExpression, mappingName, false )
		{}

		public ObjectResolvingSynchronizationContext( IObjectResolver resolver, string firstExpression, string secondExpression, string mappingName, bool includeBasePolicies ) : base( firstExpression, secondExpression, null )
		{
			this.resolver = resolver;
			this.mappingName = mappingName;
			this.includeBasePolicies = includeBasePolicies;
		}

		protected override PropertyContext CreateContext( SynchronizationContextInformation context )
		{
			var value = ResolveTarget( context.Context.Container, context.Source.Value, context.Target.Property.PropertyType );
			var result = new PropertyContext( context.Target.Container, context.Target.Property, value,
			                                         context.Target.Index.ToArray() );
			return result;
		}

		protected object ResolveTarget( SynchronizationContainer container, object source, Type targetType )
		{
			var result = resolver.Transform( item => ResolveItem( container, source, targetType ) );
			return result;
		}

		object ResolveItem( SynchronizationContainer container, object source, Type targetType )
		{
			var mappingResolver = resolvers.Ensure( container, item => new SynchronizationObjectResolver( item, resolver, MappingName, IncludeBasePolicies ) );
			var resolved = mappingResolver.Resolve( source );
			var result = resolved != null && targetType.IsAssignableFrom( resolved.GetType() ) ? resolved : null;
			return result;
		}

		protected override ISynchronizationContext CreateMirror()
		{
			var result = new ObjectResolvingSynchronizationContext( Resolver, SecondExpression, FirstExpression, MappingName, IncludeBasePolicies );
			return result;
		}

		public string MappingName
		{
			get { return mappingName; }
		}

		public IObjectResolver Resolver
		{
			get { return resolver; }
		}

		public bool IncludeBasePolicies
		{
			get { return includeBasePolicies; }
		}
	}
}