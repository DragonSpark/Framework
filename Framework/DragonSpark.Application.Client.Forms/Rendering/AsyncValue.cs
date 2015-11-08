using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Application.Client.Forms.Rendering
{
	/*public interface IValueProvider<out T> : INotifyPropertyChanged
	{
		T Value { get; }
	}*/

	sealed class AsyncValue<T> : INotifyPropertyChanged
	{
		private readonly Task<T> valueTask;
		private readonly T defaultValue;
		private bool isRunning = true;
		public event PropertyChangedEventHandler PropertyChanged = delegate {};

		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
			set
			{
				if ( this.isRunning != value )
				{
					this.isRunning = value;
					this.OnPropertyChanged();
				}
			}
		}
		public T Value
		{
			get { return valueTask.Status != TaskStatus.RanToCompletion ? defaultValue : valueTask.Result; }
		}

		/*public static implicit operator T( AsyncValue<T> container )
		{
			return container.Value;
		}*/

		/*public static explicit operator T( AsyncValue<T> container )
		{
			return container.Value;
		}*/

		public AsyncValue(Task<T> valueTask, T defaultValue)
		{
			if (valueTask == null)
			{
				throw new ArgumentNullException("valueTask");
			}
			this.valueTask = valueTask;
			this.defaultValue = defaultValue;
			TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();
			this.valueTask.ContinueWith(delegate(Task<T> t)
			{
				this.IsRunning = false;
			}, scheduler);
			this.valueTask.ContinueWith(delegate(Task<T> t)
			{
				this.OnPropertyChanged("Value");
			}, CancellationToken.None, TaskContinuationOptions.OnlyOnRanToCompletion, scheduler);
		}
		
		void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
