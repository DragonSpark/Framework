using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Windows.Markup;
using System.Xaml;

namespace DragonSpark.Windows.Markup
{
	[MarkupExtensionReturnType( typeof(int) )]
	public class NextExtension : MarkupExtension
	{
		readonly static Type[] Types = { typeof(DragonSpark.Runtime.DeclarativeCollection) };

		[Service, Required]
		public IIncrementer Incrementer { [return: Required]get; set; }
		
		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var context = DetermineContext( serviceProvider );
			var result = Incrementer.Next( context );
			return result;
		}

		protected virtual object DetermineContext( IServiceProvider serviceProvider )
		{
			var types = Types.Select( serviceProvider.Get<IXamlSchemaContextProvider>().SchemaContext.GetXamlType ).ToArray();
			var context = serviceProvider.Get<IAmbientProvider>().GetFirstAmbientValue( types );
			return context;
		}
	}
}