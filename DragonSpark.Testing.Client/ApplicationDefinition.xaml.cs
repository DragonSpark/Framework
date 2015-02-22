using DragonSpark.Activation.IoC.Commands;
using Xamarin.Forms;

namespace DragonSpark.Testing.Client
{
	[RegisterAs( typeof(Application) )]
	public partial class ApplicationDefinition
	{
		public ApplicationDefinition()
		{
			InitializeComponent();
		}
	}
}
