using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.TypeSystem;

namespace DragonSpark.ComponentModel
{
	public class HostedValueLocator<T> : FactoryBase<MemberInfo, T> where T : class
	{
		public static HostedValueLocator<T> Instance { get; } = new HostedValueLocator<T>();

		protected override T CreateItem( MemberInfo parameter )
		{
			var result = parameter.GetCustomAttributes<HostingAttribute>().Select( attribute => attribute.HostedValue ).FirstOrDefaultOfType<T>();
			return result;
		}
	}
}