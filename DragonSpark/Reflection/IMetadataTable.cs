using System.Reflection;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Reflection
{
	public interface IMetadataTable<TMetadata, TValue> : ITable<TMetadata, TValue> where TMetadata : MemberInfo {}
}