using System.ComponentModel;

namespace DragonSpark.Application.Presentation.ComponentModel
{
	public interface IViewObject : INotifyPropertyChanged
	{
		bool IsNotifying { get; }

		void NotifyOfPropertyChange( string propertyName );

		void RefreshAllNotifications();
	}
}
