using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DragonSpark.Client.Windows.Forms.Rendering
{
	internal sealed class AsyncValue<T> : INotifyPropertyChanged
	{
		private readonly Task<T> valueTask;
		private readonly T defaultValue;
		private bool isRunning = true;
		public event PropertyChangedEventHandler PropertyChanged;
		public bool IsRunning
		{
			get
			{
				return this.isRunning;
			}
			set
			{
				if (this.isRunning == value)
				{
					return;
				}
				this.isRunning = value;
				this.OnPropertyChanged("IsRunning");
			}
		}
		public T Value
		{
			get
			{
				if (this.valueTask.Status != TaskStatus.RanToCompletion)
				{
					return this.defaultValue;
				}
				return this.valueTask.Result;
			}
		}
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
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
