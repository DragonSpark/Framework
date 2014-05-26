using System;
using System.ComponentModel;

namespace DragonSpark.Application.Communication.Entity
{
	[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, AllowMultiple=false)]
	public sealed class DefaultSortAttribute : Attribute
	{
		public DefaultSortAttribute( Priority order )
		{
			Order = order;
		}

		public ListSortDirection Direction { get; set; }

		public Priority Order { get; private set; }
	}
}