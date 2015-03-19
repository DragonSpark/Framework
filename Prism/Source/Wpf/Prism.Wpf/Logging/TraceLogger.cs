// Copyright (c) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.

using System.Diagnostics;

namespace Prism.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILoggerFacade"/> that logs to .NET <see cref="Trace"/> class.
    /// </summary>
    public class TraceLogger : ILoggerFacade
    {
        readonly string format;

        public TraceLogger() : this( "[Priority: {0}]: {1}" )
        {}

        public TraceLogger( string format )
        {
            this.format = format;
        }

        /// <summary>
        /// Write a new log entry with the specified category and priority.
        /// </summary>
        /// <param name="message">Message body to log.</param>
        /// <param name="category">Category of the entry.</param>
        /// <param name="priority">The priority of the entry.</param>
        public void Log(string message, Category category, Priority priority)
        {
            var line = string.Format( format, priority, message, category );
            switch ( category )
            {
                case Category.Exception:
                    Trace.TraceError( line );
                    break;
                case Category.Warn:
                    Trace.TraceWarning( line );
                    break;
                default:
                    Trace.TraceInformation( line );
                    break;

            }
        }
    }
}