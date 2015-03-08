using System;
using System.Collections.ObjectModel;
using System.Windows.Markup;
using DragonSpark.Application.Presentation.Security;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	[ContentProperty( "Rules" )]
	public class RegisterSecurityRules : ViewRegistration
	{
		protected override void Process( IUnityContainer container, Type viewType )
		{
			var manager = container.Resolve<ISecurityManager>();
			manager.Register( viewType.FullName, Rules );
		}

		public Collection<ISecurityRule> Rules
		{
			get { return rules; }
		}	readonly Collection<ISecurityRule> rules = new Collection<ISecurityRule>();
	}
}