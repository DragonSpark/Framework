using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Dragonspark.Extensions
{
	public static class NotifyPropertyChangedExtensions
	{
		public static bool SetProperty<TValue>(this PropertyChangedEventHandler @this,
			INotifyPropertyChanged sender, TValue newValue, ref TValue oldValueField, [CallerMemberName] string propertyName = null)
		{
			var propertyChanged = !object.Equals(oldValueField, newValue);
			if (propertyChanged)
			{
				oldValueField = newValue;
				@this.RaisePropertyChanged(sender, propertyName);
			}
			return propertyChanged;
		}

		public static bool SetProperty<TSender, TValue>(this PropertyChangedEventHandler @this,
			TSender sender, TValue newValue, ref TValue oldValueField, params string[] propertyNames)
			where TSender : INotifyPropertyChanged
		{
			var propertyChanged = !object.Equals(oldValueField, newValue);
			if (propertyChanged)
			{
				oldValueField = newValue;
				@this.RaisePropertyChanged(sender, propertyNames);
			}
			return propertyChanged;
		}

		public static void RaisePropertyChanged(this PropertyChangedEventHandler @this,
			INotifyPropertyChanged sender, string propertyName)
		{
			if (@this != null && propertyName != null)
			{
				@this.Invoke(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static void RaisePropertyChanged(this PropertyChangedEventHandler @this,
			INotifyPropertyChanged sender, params string[] propertyNames)
		{
			if (@this != null && propertyNames != null)
			{
				var length = propertyNames.Length;
				for (var index = 0; index < length; ++index)
				{
					@this.Invoke(sender, new PropertyChangedEventArgs(propertyNames[index]));
				}
			}
		}
	}
}
