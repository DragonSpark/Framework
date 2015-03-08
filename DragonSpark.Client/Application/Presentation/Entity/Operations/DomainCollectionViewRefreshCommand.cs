using DragonSpark.Application.Presentation.Commands;
using DragonSpark.IoC;
using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
	[Singleton]
	public class DomainCollectionViewRefreshCommand : CommandBase<DomainCollectionView>
	{
		protected override void Execute( DomainCollectionView parameter )
		{
			parameter.Reload();
		}
	}
}