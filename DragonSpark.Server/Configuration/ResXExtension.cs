using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Configuration
{
	public class ResXExtension : MarkupExtension
	{
		readonly Assembly applicationAssembly;
		readonly string path;

		public ResXExtension( string path ) : this( null, path )
		{}

		public ResXExtension( Assembly applicationAssembly, string path )
		{
			this.applicationAssembly = applicationAssembly;
			this.path = path;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var parts = new Stack<string>( path.ToStringArray( '.' ) );
			var property = parts.Pop();
			var assembly = applicationAssembly ?? BuildManager.GetGlobalAsaxType().BaseType.Assembly;
			var fullName = string.Join( ".", parts.Reverse() );

			var type = assembly.GetValidTypes().FirstOrDefault( x => x.FullName == fullName );
			var propertyInfo = type.GetProperty( property, DragonSparkBindingOptions.AllProperties );
			var result = propertyInfo.GetValue( null, null );
			return result;
		}
	}
}