using DragonSpark.Application.Client.Forms.ComponentModel;
using DragonSpark.Application.Setup;
using DragonSpark.Extensions;
using Microsoft.Practices.Unity;
using Prism;
using Prism.Events;
using Prism.Unity;
using System;
using System.Windows.Media;
using Xamarin.Forms;
using EventTrigger = System.Windows.EventTrigger;

namespace DragonSpark.Application.Client.Forms
{
	public class SetupApplicationCommand : Client.SetupApplicationCommand
	{
		public SetupApplicationCommand()
		{
			Handlers = new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) };
		}

		public SolidColorBrush Accent { get; set; }

		public Type[] Handlers { get; set; }

		protected override void Execute( SetupContext context )
		{
			base.Execute( context );

			new EventTrigger();
			
			Accent.Transform( brush => Xamarin.Forms.Color.FromRgba( brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A ) ).With( color =>
			{
				Xamarin.Forms.Color.Accent = color;
			} );
				
			Log.Listeners.Add( new DelegateLogListener( ( c, m ) => Console.WriteLine( @"[{0}] {1}", m, c ) ) );
			Device.OS = TargetPlatform.Other;
			Device.PlatformServices = new PlatformServices();
			Device.Info = new ComponentModel.DeviceInfo();
			Device.Idiom = TargetIdiom.Desktop;
			Xamarin.Forms.Ticker.Default = new Xamarin.Forms.Ticker();
			Xamarin.Forms.ExpressionSearch.Default = new ComponentModel.ExpressionSearch();

			Registrar.RegisterAll( Handlers ?? new Type[0] );

			// TODO: Needs to be done from shell:
			var container = context.Container();
			container.Resolve<IEventAggregator>().With( aggregator => aggregator.ExecuteWhenStatusIs( SetupStatus.Configured, async () =>
			{
				var navigation = container.Resolve<INavigation>();
				var application = container.Resolve<Xamarin.Forms.Application>();
				await navigation.PushAsync( application.MainPage );
			} ) );
		}
	}
}
