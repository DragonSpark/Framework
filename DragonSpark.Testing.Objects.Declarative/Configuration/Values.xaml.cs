using DragonSpark.Configuration;
using DragonSpark.Diagnostics.Exceptions;
using System.Composition;
using System.IO;

namespace DragonSpark.Testing.Objects.Declarative.Configuration
{
	/// <summary>
	/// Interaction logic for Values.xaml
	/// </summary>
	[Export( typeof(IValueStore) )]
	public partial class Values
	{
		public Values()
		{
			Retry.Execute<IOException>( InitializeComponent );
		}
	}
}
