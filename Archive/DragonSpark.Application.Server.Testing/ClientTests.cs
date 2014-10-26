using DragonSpark.Extensions;
using DragonSpark.Server.ClientHosting;
using DragonSpark.Testing.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading;
using Navigation = DragonSpark.Application.Server.Testing.TestObjects.Navigation;

namespace DragonSpark.Application.Server.Testing
{
	[TestClass, DeploymentItem( "Client", "Client" )]
	public class ClientTests
	{
		[TestMethod, Owner( TestAuthors.MichaelDeMond )]
		public void Ensure_Configuration_Serializes_As_Expected()
		{
			var subject = new TestObjects.ClientConfiguration().Instance.GetInstance<ClientApplicationConfiguration>();
			Assert.IsNotNull( subject );
			Assert.IsNotNull( subject.Navigation ); 
			Assert.IsNotNull( subject.ApplicationDetails );
			Assert.IsNotNull( subject.Commands );
			Assert.IsNotNull( subject.Widgets );
			Assert.IsNotNull( subject.Logo );
		}

		[TestMethod]
		public void Ensure_Widgets_Are_Resolved_As_Expected()
		{
			var builder = new WidgetsBuilder();
			var modules = builder.Create().ToArray();

			Assert.AreEqual( 1, modules.Count() );

			Assert.AreEqual( "operations", modules.First().Path );
			Assert.AreEqual( "durandal/widgets/operations/controller.initialize", modules.First().Path );
		}

		[TestMethod]
		public void Ensure_Commands_Are_Resolved_As_Expected()
		{
			var builder = new CommandsBuilder();
			var modules = builder.Create().ToArray();

			Assert.AreEqual( 3, modules.Count() );

			Assert.AreEqual( "alert", modules.First().Path );
			Assert.AreEqual( "commands/alert", modules.First().Path );

			Assert.AreEqual( "wait", modules.Last().Path );
			Assert.AreEqual( "commands/wait", modules.Last().Path );
		}

		[TestMethod]
		public void Ensure_Routes_Are_Resolved_As_Expected()
		{
			var builder = new NavigationBuilder( Thread.CurrentPrincipal );
			var navigation = builder.Create( new Navigation() );

			Assert.IsNotNull( navigation.Shell );
			Assert.AreEqual( "viewModels/shell", navigation.Shell.Path );

			var items = new[]
				{
					new RouteInfo { Route = string.Empty, Title = "Hello World", Path = "viewModels/hello/index", IsVisible = true },
					new RouteInfo { Route = "view-composition", Path = "viewModels/viewComposition/index", Title = "View Composition", IsVisible = true },
					new RouteInfo { Route = "modal", Path = "viewModels/modal/index", Title = "Modal Dialogs", IsVisible = true },
					new RouteInfo { Route = "event-aggregator", Path = "viewModels/eventAggregator/index", Title = "Events", IsVisible = true },
					new RouteInfo { Route = "widgets", Path = "viewModels/widgets/index", Title = "Widgets", IsVisible = true },
					new RouteInfo { Route = "master-detail", Path = "viewModels/masterDetail/index", Title = "Master Detail", IsVisible = true },
					new RouteInfo { Route = "knockout-samples*details", Title = "Knockout Samples", Path = "viewModels/ko/index", Tag = "#knockout-samples", IsVisible = true,
						Children = new[]
							{
								new RouteInfo { Route = string.Empty, Path = "helloWorld/index", Title = "Hello World", Type = "intro" },
								new RouteInfo { Route = "helloWorld", Path = "helloWorld/index", Title = "Hello World", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "clickCounter", Path = "clickCounter/index", Title = "Click Counter", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "simpleList", Path = "simpleList/index", Title = "Simple List", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "betterList", Path = "betterList/index", Title = "Better List", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "controlTypes", Path = "controlTypes/index", Title = "Control Types", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "collections", Path = "collections/index", Title = "Collection", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "pagedGrid", Path = "pagedGrid/index", Title = "Paged Grid", Type = "intro", IsVisible = true },
								new RouteInfo { Route = "animatedTrans", Path = "animatedTrans/index", Title = "Animated Transition", Type = "intro", IsVisible = true },
								
								new RouteInfo { Route = "contactsEditor", Path = "contactsEditor/index", Title = "Contacts Editor", Type = "detailed", IsVisible = true },
								new RouteInfo { Route = "gridEditor", Path = "gridEditor/index", Title = "Grid Editor", Type = "detailed", IsVisible = true },
								new RouteInfo { Route = "shoppingCart", Path = "shoppingCart/index", Title = "Shopping Cart", Type = "detailed", IsVisible = true },
								new RouteInfo { Route = "twitterClient", Path = "twitterClient/index", Title = "Twitter Client", Type = "detailed", IsVisible = true },
							}
					}
				}.ToList();

			foreach ( var expected in items )
			{
				var index = items.IndexOf( expected );
				var actual = navigation.Routes.ElementAt( index );
				AssertItem( expected, actual );
			}
		}

		static void AssertItem( RouteInfo expected, RouteInfo actual )
		{
			Assert.AreEqual( expected.Route, actual.Route );
			Assert.AreEqual( expected.Title, actual.Title );
			Assert.AreEqual( expected.Path, actual.Path );
			Assert.AreEqual( expected.IsVisible, actual.IsVisible );
			Assert.AreEqual( expected.Type, actual.Type );
			Assert.AreEqual( expected.Tag, actual.Tag );
			if ( expected.Children.Transform( x => x.Any() ) )
			{
				Assert.AreEqual( expected.Children.Length, actual.Children.Length );
				foreach ( var info in actual.Children )
				{
					var item = expected.Children[actual.Children.ToList().IndexOf( info )];
					AssertItem( info, item );
				}
			}
		}
	}
}
