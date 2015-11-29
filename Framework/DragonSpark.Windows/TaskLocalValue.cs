using System.Threading;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Windows
{
	public class TaskLocalValue : Value<object>
	{
		readonly AsyncLocal<object> local = new AsyncLocal<object>();

		public TaskLocalValue( object item )
		{
			local.Value = item;
		}

		public override object Item => local.Value;
	}
}