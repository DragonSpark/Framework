using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DragonSpark.Extensions;
using DragonSpark.Objects;

namespace DragonSpark.Application.Communication.Entity
{
    public abstract class FilterExpressionFactoryBase<TItem> : Factory<string,Expression<Func<TItem, bool>>> where TItem : class
    {
        readonly static IDictionary<Type,IEnumerable<string>> Properties = new Dictionary<Type, IEnumerable<string>>();

        protected override Expression<Func<TItem,bool>> CreateItem(string source)
        {
            if ( !string.IsNullOrEmpty( source ) )
            {
                var terms = source.ToStringArray( ' ' );
                var parameter = System.Linq.Expressions.Expression.Parameter( typeof(TItem), "x" );
                var properties = Properties.Ensure( typeof(TItem), ResolveProperties );
                var first = properties.FirstOrDefault().Transform( x => CreateMethod( parameter, x, terms ) );
                var expression = properties.Skip( 1 ).Aggregate( first, ( current, name ) => System.Linq.Expressions.Expression.Or( current, CreateMethod( parameter, name, terms ) ) );
                var result = System.Linq.Expressions.Expression.Lambda<Func<TItem, bool>>( expression, parameter );
                return result;
            }
            return null;
        }

        protected abstract IEnumerable<string> ResolveProperties( Type arg );

        static System.Linq.Expressions.Expression CreateMethod( System.Linq.Expressions.Expression parameter, string name, IEnumerable<string> terms )
        {
            var property = System.Linq.Expressions.Expression.Property( parameter, name );
            var notNull = System.Linq.Expressions.Expression.NotEqual( property, System.Linq.Expressions.Expression.Constant( null ) );
            var first = terms.FirstOrDefault().Transform( x => ResolveContains( property, x ) );
            var expression = terms.Skip( 1 ).Aggregate( first, ( current, term ) =>
            {
                var methodCallExpression = ResolveContains( property, term );
                var binaryExpression = System.Linq.Expressions.Expression.Or( current, methodCallExpression );
                return binaryExpression;
            } );
            // var contains = System.Linq.Expressions.Expression.Call( property, "Contains", null, System.Linq.Expressions.Expression.Constant( filter ) );
            var result = System.Linq.Expressions.Expression.And( notNull, expression );
            return result;
        }

        static System.Linq.Expressions.Expression ResolveContains( MemberExpression property, string x )
        {
            var result = System.Linq.Expressions.Expression.Call( property, "Contains", null, System.Linq.Expressions.Expression.Constant( x ) );
            return result;
        }
    }
}