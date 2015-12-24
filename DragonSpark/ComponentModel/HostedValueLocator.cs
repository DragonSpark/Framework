using DragonSpark.Activation.FactoryModel;
using DragonSpark.TypeSystem;
using System.Linq;
using System.Reflection;

namespace DragonSpark.ComponentModel
{
	public class HostedValueLocator<T> : FactoryBase<MemberInfo, T[]> where T : class
	{
		public static HostedValueLocator<T> Instance { get; } = new HostedValueLocator<T>();

		protected override T[] CreateItem( MemberInfo parameter )
		{
			var result = parameter.GetCustomAttributes<HostingAttribute>().Select( attribute => attribute.Item ).OfType<T>().ToArray();
			return result;
		}
	}
}