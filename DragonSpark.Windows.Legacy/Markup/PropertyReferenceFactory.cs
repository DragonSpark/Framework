using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.TypeSystem;
using System;
using System.Xaml;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class PropertyReferenceFactory : ParameterizedSourceBase<IServiceProvider, PropertyReference>
	{
		public static PropertyReferenceFactory Default { get; } = new PropertyReferenceFactory();

		readonly IExpressionEvaluator evaluator;
		readonly Func<object, PropertyReference> create;

		public PropertyReferenceFactory() : this( ExpressionEvaluator.Default ) {}

		public PropertyReferenceFactory( IExpressionEvaluator evaluator )
		{
			this.evaluator = evaluator;
			create = Create;
		}

		public override PropertyReference Get( IServiceProvider parameter ) => parameter.Get<IXamlNameResolver>()?.GetFixupToken( Items<string>.Default ).With( create ) ?? default(PropertyReference);

		PropertyReference Create( object token )
		{
			var type = evaluator.Evaluate<Type>( token, "TargetContext.GrandParentType.UnderlyingType" );
			var property = evaluator.Evaluate<Type>( token, "TargetContext.GrandParentProperty.Type.UnderlyingType" );
			var name = evaluator.Evaluate<string>( token, "TargetContext.GrandParentProperty.Name" );
			var result = new PropertyReference( type, property, name );
			return result;
		}
	}
}