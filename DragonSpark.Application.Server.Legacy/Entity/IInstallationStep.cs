namespace DragonSpark.Server.Legacy.Entity
{
	public interface IInstallationStep
	{
		void Execute( DbContext context );
	}
}