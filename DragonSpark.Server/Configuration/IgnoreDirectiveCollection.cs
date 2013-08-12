using System.Collections.ObjectModel;

namespace DragonSpark.Server.Configuration
{
	public class IgnoreDirectiveCollection : Collection<IgnoreDirective>
	{
		public IgnoreDirectiveCollection()
		{
			ClearList = true;
		}

		public bool ClearList { get; set; }
	}
}