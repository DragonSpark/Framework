using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DragonSpark.Application.Communication.Channels
{
    /// <summary>
    /// Performs assignemnt back to the out and ref parameters
    /// </summary>
    /// <typeparam name="T">Interface type on wich the assignment operation on out and ref parameters occurs</typeparam>
    public class Assigner<T>
    {
        /// <summary>
        /// performs assigmnet on out and ref parameters
        /// </summary>
        /// <param name="exp">expression to evaluate</param>
        /// <param name="values">the array of values to assign back</param>
        public void Assign( Expression<Action<T>> exp, params object[] values )
        {
            Assign( (System.Linq.Expressions.Expression)exp, values );
        }

        /// <summary>
        /// performs assigmnet on out and ref parameters
        /// </summary>
        /// <typeparam name="TResult">the result type of the invoked method (automatically inferred)</typeparam>
        /// <param name="exp">expression to evaluate</param>
        /// <param name="values">the array of values to assign back</param>
        public void Assign<TResult>( Expression<Func<T, TResult>> exp, params object[] values )
        {
            Assign( (System.Linq.Expressions.Expression)exp, values );
        }

        /// <summary>
        /// performs assignation
        /// </summary>
        /// <param name="exp">expression to evaluate</param>
        /// <param name="values">values to return</param>
        static void Assign( System.Linq.Expressions.Expression exp, object[] values )
        {
            var lambda = exp as LambdaExpression;
            var method = lambda.Body as MethodCallExpression;

            var assigmentToPerform = method.Method.GetParameters().Zip( method.Arguments,
                        ( p, a ) =>
                        new {
                                parameter = p,
                                argument = a,
                                isByRef = p.ParameterType.IsByRef
                            } )
                    .Where( x => x.isByRef );

            var statements = new List<System.Linq.Expressions.Expression>();

            foreach ( var assignment in assigmentToPerform )
            {
                System.Linq.Expressions.Expression value;
                if ( values == null )
                {
                    value = System.Linq.Expressions.Expression.Default( assignment.argument.Type );
                }
                else
                {
                    if ( assignment.parameter.Position < values.Length )
                    {
                        value =
                            System.Linq.Expressions.Expression.Convert(
                                System.Linq.Expressions.Expression.Constant(
                                    values[ assignment.parameter.Position ] ),
                                assignment.argument.Type );
                    }
                    else
                    {
                        value = System.Linq.Expressions.Expression.Default( assignment.argument.Type );
                    }
                }

                statements.Add( System.Linq.Expressions.Expression.Assign( assignment.argument, value ) );
            }

            if ( statements.Count != 0 )
            {
                var assigner = System.Linq.Expressions.Expression.Lambda( System.Linq.Expressions.Expression.Block( statements ) );

                assigner.Compile().DynamicInvoke();
            }
        }
    }
}
