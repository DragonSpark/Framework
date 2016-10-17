using System.Data.Entity;

namespace DragonSpark.Windows.Legacy.Entity
{
	public interface IEntityInstallationStorage
	{
		IDbSet<InstallationEntry> Installations { get; }
	}
}