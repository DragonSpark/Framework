using System.Data.Entity;

namespace DragonSpark.Windows.Entity
{
	public interface IEntityInstallationStorage
	{
		IDbSet<InstallationEntry> Installations { get; }
	}
}