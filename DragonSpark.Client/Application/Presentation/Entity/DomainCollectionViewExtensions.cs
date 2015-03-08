using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity
{
    public static class DomainCollectionViewExtensions
    {
        public static DomainCollectionView Reload( this DomainCollectionView target )
        {
            using ( target.DeferRefresh() )
            {
                target.SetTotalItemCount( -1 );
                target.MoveToFirstPage();
            }
            return target;
        }
    }
}