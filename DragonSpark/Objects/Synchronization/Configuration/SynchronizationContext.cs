using System;
using DragonSpark.Configuration;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public class SynchronizationContext : IInstanceSource<ISynchronizationContext>
	{
		public string Expression { get; set; }

		public string FirstExpression { get; set; }

		public string SecondExpression { get; set; }

		// [TypeConverter( typeof(TypeNameConverter) )]
		public Type TypeConverterType { get; set; }

		public ISynchronizationContext Instance
		{
			get { return instance ?? ( instance = Create() ); }
		}	Synchronization.SynchronizationContext instance;

		protected virtual Synchronization.SynchronizationContext Create()
		{
			var result = new Synchronization.SynchronizationContext( FirstExpression ?? Expression, SecondExpression ?? Expression,
			                                                                  TypeConverterType );
			return result;
		}
	}
}