using System;
using System.Windows.Media;
using DragonSpark.Application.Forms.ComponentModel;
using DragonSpark.Extensions;
using Xamarin.Forms;
using DeviceInfo = DragonSpark.Application.Forms.ComponentModel.DeviceInfo;
using EventTrigger = System.Windows.EventTrigger;
using ExpressionSearch = DragonSpark.Application.Forms.ComponentModel.ExpressionSearch;

namespace DragonSpark.Application.Forms
{
	public interface IInitializer
	{
		void Initialize();
	}

	public class FormsInitializer : IInitializer
	{
		public SolidColorBrush Accent { get; set; }

		public void Initialize()
		{
			new EventTrigger();
			
			Accent.Transform( brush =>  Xamarin.Forms.Color.FromRgba( brush.Color.R, brush.Color.G, brush.Color.B, brush.Color.A ) ).With( color =>
			{
				Xamarin.Forms.Color.Accent = color;
			} );
				
			Log.Listeners.Add( new DelegateLogListener( ( c, m ) => Console.WriteLine( @"[{0}] {1}", m, c ) ) );
			Device.OS = TargetPlatform.Other;
			Device.PlatformServices = new PlatformServices();
			Device.Info = new DeviceInfo();
			Device.Idiom = TargetIdiom.Desktop;
			Xamarin.Forms.Ticker.Default = new Xamarin.Forms.Ticker();
			Xamarin.Forms.ExpressionSearch.Default = new ExpressionSearch();

			var handlers = GetHandlers();
			Registrar.RegisterAll( handlers );
		}

		protected virtual Type[] GetHandlers()
		{
			return new[] { typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute) };
		}
	}
}