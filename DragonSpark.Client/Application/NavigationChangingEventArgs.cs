using System;
using System.ComponentModel;

namespace DragonSpark.Application
{
    public class NavigationChangingEventArgs : CancelEventArgs
	{
		readonly Uri currentLocation;
		readonly Uri newLocation;

		public NavigationChangingEventArgs( Uri currentLocation, Uri newLocation )
		{
			this.currentLocation = currentLocation;
			this.newLocation = newLocation;
		}

		public Uri CurrentLocation
		{
			get { return currentLocation; }
		}

		public Uri NewLocation
		{
			get { return newLocation; }
		}
	}
}