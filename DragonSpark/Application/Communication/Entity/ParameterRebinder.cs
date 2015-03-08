using System.Collections.Generic;
using System.Linq.Expressions;

namespace DragonSpark.Application.Communication.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Rebinder" )]
    public class ParameterRebinder : ExpressionVisitor
    {
        readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder( Dictionary<ParameterExpression, ParameterExpression> map = null )
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters( Dictionary<ParameterExpression, ParameterExpression> map, Expression exp )
        {
            return new ParameterRebinder( map ).Visit( exp );
        }

        protected override Expression VisitParameter( ParameterExpression node )
        {
            ParameterExpression replacement;

            if ( map.TryGetValue( node, out replacement ) )
            {
                node = replacement;
            }

            return base.VisitParameter( node );
        }
 
    }
}