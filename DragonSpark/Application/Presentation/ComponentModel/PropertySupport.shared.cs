using System;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.ComponentModel
{
    public static class PropertySupport
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#", Justification = "Used as convenience." ), System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Used to extract expression." )]
        public static bool SetProperty<TItem>( ref TItem current, TItem assignment, System.Linq.Expressions.Expression expression, Action<string> notify )
        {
            var result = !Equals( current, assignment );
            if ( result )
            {
                current = assignment;
                notify( expression.GetMemberInfo().Name );
                // NotifyOfPropertyChange( expression );
            }
            return result;
        }
    }
}