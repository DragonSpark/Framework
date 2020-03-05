using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components {
	public delegate Task Callback<in T>(T parameter);
}