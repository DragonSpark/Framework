using System;
using System.Linq.Expressions;

namespace DragonSpark.Application.Communication.Channels
{
    public partial class Channel<TSync>
    {
        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>
        /// <typeparam name="TOut">the type of result expected from the invokation of the operaton</typeparam>
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>        
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync<TOut>(Expression<Func<TSync, TOut>> requestInvokation, Action<TOut> responseAction)
        {
            return ExecuteAsync(requestInvokation, null, (result, status) => responseAction(result), null);
        }

        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>        
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync(Expression<Action<TSync>> requestInvokation, Action responseAction)
        {
            return ExecuteAsync(requestInvokation, null, status => responseAction(), null);
        }

        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>
        /// <typeparam name="TOut">the type of result expected from the invokation of the operaton</typeparam>
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>
        /// <param name="asyncStatus">an user defined object to transmit to the ascyn reply of the operation</param>
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync<TOut>(Expression<Func<TSync, TOut>> requestInvokation, object asyncStatus, Action<TOut, object> responseAction)
        {
            return ExecuteAsync(requestInvokation, asyncStatus, (result, status) => responseAction(result, status), null);
        }

        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>        
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>
        /// /// <param name="asyncStatus">an user defined object to transmit to the ascyn reply of the operation</param>
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync(Expression<Action<TSync>> requestInvokation, object asyncStatus, Action<object> responseAction)
        {
            return ExecuteAsync(requestInvokation, asyncStatus, status => responseAction(status), null);
        }

        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>
        /// <typeparam name="TOut">the type of result expected from the invokation of the operaton</typeparam>
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>        
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <param name="onException">invoked when exception occurs</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync<TOut>(Expression<Func<TSync, TOut>> requestInvokation, Action<TOut> responseAction, Action<Exception> onException)
        {
            return ExecuteAsync(requestInvokation, null, (result, status) => responseAction(result), (ex, status) => onException(ex));
        }

        /// <summary>
        /// Executes a synchronous operation defined on the Sync interface in an asynch fashon
        /// </summary>        
        /// <param name="requestInvokation">the request to perform on the channel expressed as Sync</param>
        /// <param name="responseAction">the action to perform when the reply arrives</param>
        /// <param name="onException">invoked when exception occurs</param>
        /// <returns>an IAsyncResult for the asyncronous operation, it could be useful to check when the operation is completed</returns>
        public IAsyncResult ExecuteAsync(Expression<Action<TSync>> requestInvokation, Action responseAction, Action<Exception> onException)
        {
            return ExecuteAsync(requestInvokation, null, status => responseAction(), (ex, status) => onException(ex));
        }
    }
}
