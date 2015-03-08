using System;
using System.Collections.Generic;
using System.ServiceModel.DomainServices.Client;

namespace DragonSpark.Application.Communication.Entity
{
	[AttributeUsage( AttributeTargets.Class )]
	public class QueryNameAttribute : Attribute
	{
		readonly string name;
		readonly string viewName;

		public QueryNameAttribute( string name ) : this( name, null )
		{}

		public QueryNameAttribute( string name, string viewName )
		{
			this.name = name;
			this.viewName = viewName;
		}

		public string ViewName
		{
			get { return viewName; }
		}

		public string Name
		{
			get { return name; }
		}
	}

}