using DragonSpark.Objects;
using System.Collections.ObjectModel;
using System.Windows.Markup;

namespace DragonSpark.Server.Client
{
	[ContentProperty( "Children" )]
	public class NavigationNode : ClientModule
	{
		public NavigationNode()
		{
			this.WithDefaults();
		}

		public string Route { get; set; }
		
		public string Title { get; set; }

		public string Type { get; set; }

		public string Tag { get; set; }

		[DefaultPropertyValue(true)]
		public bool IsVisible { get; set; }
		
		public Collection<NavigationNode> Children
		{
			get { return children; }
		}	readonly Collection<NavigationNode> children = new Collection<NavigationNode>();

		public Collection<IAuthorizer> Authorizers
		{
			get { return authorizers; }
		}	readonly Collection<IAuthorizer> authorizers = new Collection<IAuthorizer>();
	}
}