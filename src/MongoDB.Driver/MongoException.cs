//COPYRIGHT

using System;
namespace MongoDB.Driver
{
    /// <summary>
    /// An MongoDB.Net Exception
    /// </summary>
    public class MongoException : Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public MongoException(string msg)
            : base(msg)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoException"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="t">The t.</param>
        public MongoException(string msg, Exception t)
            : base(msg, _massage(t))
        {
        }

        internal static Exception _massage(Exception t)
        {
            if (t is Network)
                return ((Network)t)._ioe;
            return t;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class Response : MongoException
        {
            /// <summary>
            /// Gets or sets the actual response.
            /// </summary>
            /// <value>The actual response.</value>
            public IDBResponse ActualResponse { get; private set; }
            internal Response(string msg, IDBResponse response)
                : base(msg)
            {
                ActualResponse = response;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class Authentication : MongoException
        {
            internal Authentication(string username)
                : base(string.Format("Cannot authenticate user \"{0}\".", username))
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class LastError : MongoException
        {
            /// <summary>
            /// Gets or sets the actual last error.
            /// </summary>
            /// <value>The actual last error.</value>
            public MongoDB.Driver.LastError ActualLastError { get; private set; }
            internal LastError(MongoDB.Driver.LastError lastError)
                : base(lastError.ErrorMessage)
            {
                ActualLastError = lastError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class Network : MongoException
        {

            internal Network(string msg, System.IO.IOException ioe)
                : base(msg, ioe)
            {
                _ioe = ioe;
            }

            internal Network(System.IO.IOException ioe)
                : this(ioe.Message, ioe)
            {
            }

            internal System.IO.IOException _ioe;
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class DuplicateKey : MongoException
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DuplicateKey"/> class.
            /// </summary>
            /// <param name="msg">The MSG.</param>
            public DuplicateKey(string msg)
                : base(msg)
            {
            }
        }
    }
}