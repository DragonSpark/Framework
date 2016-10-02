using DragonSpark.Diagnostics.Exceptions;
using System.IO;

namespace DragonSpark.Testing.Objects.Configuration
{
	/// <summary>
	/// Interaction logic for Item.xaml
	/// </summary>
	public partial class Item
	{
		public Item()
		{
			Retry.Execute<IOException>( InitializeComponent );
		}
	}
}
