using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Runtime;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace DragonSpark.Markup
{
	[ContentProperty( "Attributes" )]
	public class FaultContractExceptionHandler : Singleton<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.FaultContractExceptionHandler>
	{
		public string ExceptionMessageResourceType { get; set; }

		public string ExceptionMessageResourceName { get; set; }

		public Type FaultContractType { get; set; }

		public string ExceptionMessage { get; set; }

		public Collection<KeyValuePair> Attributes
		{
			get { return attributes; }
		}	readonly Collection<KeyValuePair> attributes = new Collection<KeyValuePair>();

		protected override Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.FaultContractExceptionHandler Create()
		{
			var items = new NameValueCollection();
			Attributes.Apply( x => items.Add( x.Key.To<string>(), x.Value.To<string>() ) );
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.FaultContractExceptionHandler( new ResourceStringResolver( ExceptionMessageResourceType ?? string.Empty, ExceptionMessageResourceName, ExceptionMessage ), FaultContractType, items );
			return result;
		}
	}
}