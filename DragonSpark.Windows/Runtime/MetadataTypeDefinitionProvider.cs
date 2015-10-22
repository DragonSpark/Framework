﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using DynamicExpresso;

namespace DragonSpark.Windows.Runtime
{
	public class MetadataTypeDefinitionProvider : ITypeDefinitionProvider
	{
		public TypeInfo GetDefinition( TypeInfo info )
		{
			var result = info.FromMetadata<MetadataTypeAttribute, TypeInfo>( item => item.MetadataClassType.GetTypeInfo() );
			return result;
		}
	}

	public class ExpressionEvaluator : IExpressionEvaluator
	{
		const string Target = "___target___";

		public object Evaluate( object context, string expression )
		{
			var interpreter = new Interpreter().SetVariable( Target, context );

			var result = interpreter.Eval( string.Concat( Target, ".", expression.TrimStart( '.' ) ) );

			return result;
		}
	}
}
