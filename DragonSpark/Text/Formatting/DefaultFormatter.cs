using JetBrains.Annotations;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Text.Formatting
{
	sealed class DefaultFormatter : Adapter<object>, IActivateUsing<object>
	{
		[UsedImplicitly]
		public DefaultFormatter(object subject) : base(subject, TextSelector<object>.Default) {}
	}
}