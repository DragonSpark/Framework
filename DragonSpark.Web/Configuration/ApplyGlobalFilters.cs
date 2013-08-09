using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Compilation;
using System.Web.Http;
using System.Web.Mvc;
using System.Windows.Markup;

namespace DragonSpark.Web.Configuration
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

			var type = assembly.GetTypes().FirstOrDefault( x => x.FullName == fullName );
			var propertyInfo = type.GetProperty( property, DragonSparkBindingOptions.AllProperties );
			var result = propertyInfo.GetValue( null, null );
			return result;
		}
	}



	public class ClientPathExtension : MarkupExtension
	{
		readonly string path;

		public ClientPathExtension( string path )
		{
			this.path = path;
		}

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = VirtualPathUtility.ToAbsolute( path );
			return result;
		}
	}

	[ContentProperty( "Filters" )]
	public class ApplyGlobalFilters : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			Filters.Apply( GlobalFilters.Filters.Add );
		}

		public Collection<object> Filters
		{
			get { return filters; }
		}	readonly Collection<object> filters = new Collection<object>();
	}

	[ContentProperty( "Filters" )]
	public class ApplyFilters : IHttpApplicationConfigurator
	{
		public void Configure( HttpConfiguration configuration )
		{
			Filters.Apply( configuration.Filters.Add );
		}

		public Collection<System.Web.Http.Filters.IFilter> Filters
		{
			get { return filters; }
		}	readonly Collection<System.Web.Http.Filters.IFilter> filters = new Collection<System.Web.Http.Filters.IFilter>();
	}
}