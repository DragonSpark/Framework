using System.Data.Entity;

namespace DragonSpark.Windows.Entity
{
	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}
}