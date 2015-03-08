using System.Collections.ObjectModel;

namespace DragonSpark.Application.Logging
{
	public interface ILoggingOutput
	{
		ReadOnlyObservableCollection<string> Output { get;  }
	}
}