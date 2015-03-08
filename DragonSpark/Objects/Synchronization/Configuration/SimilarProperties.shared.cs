using System.Collections.ObjectModel;

namespace DragonSpark.Objects.Synchronization.Configuration
{
	public class SimilarProperties : SynchronizationExpressionResolverBase
	{
		public string ContextExpressionFirst { get; set; }

		public string ContextExpressionSecond { get; set; }

		public ObservableCollection<string> PropertiesToIgnore
		{
			get { return propertiesToIgnore ?? ( propertiesToIgnore = new ObservableCollection<string>() ); }
		}	ObservableCollection<string> propertiesToIgnore;

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "NonPublic", Justification = "It is what it is." )]
		public bool IncludeNonPublicProperties { get; set; }

		protected override ISynchronizationExpressionResolver Create()
		{
			var result = new Synchronization.SimilarProperties( ContextExpressionFirst, ContextExpressionSecond, PropertiesToIgnore, IncludeNonPublicProperties );
			return result;
		}
	}
}