using System;
using System.Globalization;
using System.Security.Principal;
using DragonSpark.Application.Presentation.Converters;
using DragonSpark.Application.Presentation.Security;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Launch
{
	public class SecurityConverter : BooleanConverter
	{
		public new static SecurityConverter Instance
		{
			get { return InstanceField; }
		}	static readonly SecurityConverter InstanceField = new SecurityConverter();

		[Dependency]
		public ISecurityManager Manager { get; set; }

		[Dependency]
		public IPrincipal Principal { get; set; }

		protected override bool Resolve( object value, Type targetType, object parameter, CultureInfo culture )
		{
			this.BuildUpOnce();
			var result = value.AsTo<string, bool>( x => Manager.Verify( Principal, x ) );
			return result;
		}
	}
}