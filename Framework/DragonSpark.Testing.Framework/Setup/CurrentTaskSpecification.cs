using DragonSpark.Runtime.Specifications;
using DragonSpark.Windows;

namespace DragonSpark.Testing.Framework.Setup
{
	public class CurrentTaskSpecification : ProvidedSpecification
	{
		public CurrentTaskSpecification() : this( new TaskLocalValue( new object() ) )
		{}

		public CurrentTaskSpecification( TaskLocalValue local ) : base( () => local.Item != null )
		{}
	}
}