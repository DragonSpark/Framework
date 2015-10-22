using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Windows.Setup
{
	[ContentProperty( "Entries" )]
	public class ExceptionPolicyDefinition : MarkupExtension
	{
		public string PolicyName { get; set; }

		public Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry> Entries
		{
			get { return entries; }
		}	readonly Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry> entries = new Collection<Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyEntry>();

		public override object ProvideValue( IServiceProvider serviceProvider )
		{
			var result = new Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.ExceptionPolicyDefinition( PolicyName, Entries );
			return result;
		}
	}
}