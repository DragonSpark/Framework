using System.Collections.ObjectModel;

namespace DragonSpark.Web.Configuration
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