using System.Data.Entity;

namespace DragonSpark.Windows.Legacy.Entity
{
	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}
}