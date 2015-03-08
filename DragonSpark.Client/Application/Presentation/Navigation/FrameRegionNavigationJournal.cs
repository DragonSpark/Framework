using System;
using DragonSpark.Extensions;
using Microsoft.Practices.Prism.Regions;

namespace DragonSpark.Application.Presentation.Navigation
{
	public class FrameRegionNavigationJournal : IRegionNavigationJournal
	{
		readonly FrameRegionNavigationService service;

		public FrameRegionNavigationJournal( FrameRegionNavigationService service )
		{
			this.service = service;
		}

		public Boolean CanGoBack
		{
			get { return service.Frame.CanGoBack; }
		}

		public Boolean CanGoForward
		{
			get { return service.Frame.CanGoForward; }
		}

		public void Clear()
		{
			throw new NotSupportedException();
		}

		public IRegionNavigationJournalEntry CurrentEntry
		{
			get
			{
				var result = service.Frame.CurrentSource.Transform( x => new FrameRegionNavigationJournalEntry { Uri = service.MapUri( x ) } );
				return result;
			}
		}

		public void GoBack()
		{
			service.Frame.GoBack();
		}

		public void GoForward()
		{
			service.Frame.GoForward();
		}

		public INavigateAsync NavigationTarget
		{
			get { return service; }
			set { throw new NotSupportedException(); }
		}

		public void RecordNavigation( IRegionNavigationJournalEntry entry )
		{
			throw new NotSupportedException();
		}

		class FrameRegionNavigationJournalEntry : IRegionNavigationJournalEntry
		{
			public Uri Uri { get; set; }
		}
	}
}
