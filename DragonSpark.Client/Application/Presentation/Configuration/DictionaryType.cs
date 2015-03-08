using System;
using System.Collections.Generic;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class DictionaryType : Type
	{
		public string KeyTypeName { get; set; }

		protected override object ResolveValue( IServiceProvider serviceProvider, System.Type type )
		{
			var keyType = DetermineType( serviceProvider, KeyTypeName );
			var result = keyType.Transform( x => typeof(IDictionary<,>).MakeGenericType( x, type ) );
			return result;
		}
	}
}