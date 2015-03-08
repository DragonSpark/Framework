using System;
using System.Windows.Threading;

namespace DragonSpark.Application.Communication.Channels
{
    public partial class Channel<TSync>
    {
        /// <summary>
        /// Returns a typed object requested, if present, from the appropriate layer in the channel stack.
        /// </summary>
        /// <typeparam name="T">The typed object for which the method is querying.</typeparam>
        /// <returns>The typed object T requested if it is present or null if it is not.</returns>
        public T GetProperty<T>() where T : class
        {
            return channel.GetProperty<T>();
        }

        /// <summary>
        /// Causes a communication object to transition immediately from its current state into the closed state.
        /// </summary>
        public void Abort()
        {
            channel.Abort();
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state.
        /// </summary>
        public void Open()
        {
            channel.Open();
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state within a specified interval of time.
        /// </summary>
        /// <param name="timeout">The Timespan that specifies how long the send operation has to complete before timing out.</param>
        public void Open(TimeSpan timeout)
        {
            channel.Open(timeout);
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Open(Action responseAction)
        {
            return Open(null, (asyncState) => responseAction(), null);
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="asyncState">a status object that will be passed to the response action when the operation is completed</param>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Open(object asyncState, Action<object> responseAction)
        {
            return Open(asyncState, responseAction, null);
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <param name="onException">exception handler</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Open(Action responseAction, Action<Exception> onException)
        {
            return Open(null, (asyncState) => responseAction(), (ex, asyncState) => onException(ex));
        }

        /// <summary>
        /// Causes a communication object to transition from the created state into the opened state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="asyncState">status object passed to the responseAction and onException when the operation completes</param>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <param name="onException">exception handler</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Open(object asyncState, Action<object> responseAction, Action<Exception, object> onException)
        {
            var dispatcher = System.Windows.Deployment.Current.Dispatcher;

            AsyncCallback callback = asyncResult =>
            {
                try
                {
                    channel.EndOpen(asyncResult);
                    dispatcher.BeginInvoke(() =>
                    {
                        if (responseAction == null) return;
                        responseAction(asyncResult.AsyncState);
                    });
                }
                catch (Exception ex)
                {
                    if (onException == null) return;
                    Exception exception = ex.InnerException ?? ex;
                    dispatcher.BeginInvoke(() => onException(exception, asyncResult.AsyncState));
                }
            };

            IAsyncResult invokationResult = channel.BeginOpen(callback, asyncState);

            return invokationResult;
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state.
        /// </summary>
        public void Close()
        {
            channel.Close();
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state.
        /// </summary>
        /// <param name="timeout">The Timespan that specifies how long the send operation has to complete before timing out.</param>
        public void Close(TimeSpan timeout)
        {
            channel.Close(timeout);
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Close(Action responseAction)
        {
            return Close(null, (asyncState) => responseAction(), null);
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="asyncState">a status object that will be passed to the response action when the operation is completed</param>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Close(object asyncState, Action<object> responseAction)
        {
            return Close(asyncState, (state) => responseAction(state), null);
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <param name="onException">exception handler</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Close(Action responseAction, Action<Exception> onException)
        {
            return Close(null, (asyncState) => responseAction(), (ex, asyncState) => onException(ex));
        }

        /// <summary>
        /// Causes a communication object to transition from its current state into the closed state asynchronously.
        /// when it is opened it continues with the responseAction
        /// </summary>
        /// <param name="asyncState">status object passed to the responseAction and onException when the operation completes</param>
        /// <param name="responseAction">the action to perform when the channel is successfully opened</param>
        /// <param name="onException">exception handler</param>
        /// <returns>IAsyncResult that describes the status of the asynchrnous operation</returns>
        public IAsyncResult Close(object asyncState, Action<object> responseAction, Action<Exception, object> onException)
        {
            Dispatcher dispatcher = System.Windows.Deployment.Current.Dispatcher;

            AsyncCallback callback = asyncResult =>
            {
                try
                {
                    channel.EndClose(asyncResult);
                    dispatcher.BeginInvoke(() =>
                    {
                        if (responseAction == null) return;
                        responseAction(asyncResult.AsyncState);
                    });
                }
                catch (Exception ex)
                {
                    if (onException == null) return;
                    Exception exception = ex.InnerException ?? ex;
                    dispatcher.BeginInvoke(() => onException(ex, asyncResult.AsyncState));
                }
            };

            IAsyncResult invokationResult = channel.BeginClose(callback, asyncState);

            return invokationResult;
        }
    }
}
