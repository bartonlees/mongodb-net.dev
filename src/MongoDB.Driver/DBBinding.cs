//COPYRIGHT

using System.Collections.Generic;
using System.Net;
using System;
using System.Net.Sockets;
using System.Diagnostics.Contracts;
using MongoDB.Driver.Platform.Util;
using MongoDB.Driver.Platform.Conditions;
using System.Linq;
using System.Transactions;
using System.IO;
using MongoDB.Driver.Command;
using System.Security;
using System.Text;

namespace MongoDB.Driver
{
    /// <summary>
    /// A Simple database binding.
    /// Some construction options:
    /// <code>
    /// //BASIC
    /// DBBinding basic = new DBBinding("mongo://localhost/db");
    /// 
    /// //FROM TEMPLATE BINDING
    /// DBBinding template = new DBBinding("mongo://localhost/db");
    /// DBBinding templated = new DBBinding(template, "db2");
    /// //Equivalent to "mongo://localhost/db2"
    /// 
    /// //HOSTNAME AND DB NAME
    /// DBBinding withnames = new DBBinding("localhost", "db");
    /// //Equivalent to "mongo://localhost/db"
    /// 
    /// //WITH PORT
    /// DBBinding withport = new DBBinding("localhost", 1910, "db");
    /// //Equivalent to "mongo://localhost:1910/db"
    /// </code>
    /// </summary>
    public class DBBinding : IInternalDBBinding
    {
        /// <summary>
        /// Gets this pool's options.
        /// </summary>
        /// <value>The options.</value>
        public DBConnectionOptions ConnectionOptions { get; private set; }

        Queue<IDBConnection> _IdleConnections = new Queue<IDBConnection>();

        SortedDictionary<string, IDBConnection> _transactionConnectionLookup = new SortedDictionary<string, IDBConnection>();

        /// <summary>
        /// Requires that a connection is pulled from the available pool and is initialized for the specified binding
        /// </summary>
        /// <param name="binding">The binding.</param>
        /// <returns></returns>
        public IDBConnection RequireConnection(Transaction transaction)
        {
            IDBConnection connection = null;
            string transID = transaction.TransactionInformation.LocalIdentifier;

            //Loop until we find or create a workable connection
            while (true)
            {
                //If there is an existing transaction, use it
                if (!_transactionConnectionLookup.TryGetValue(transID, out connection))
                {
                    //Otherwise, get an available connection
                    connection = GetIdleConnection() ?? CreateConnection();
                    transaction.TransactionCompleted += new TransactionCompletedEventHandler(TransactionCompleted);
                    _transactionConnectionLookup[transID] = connection;
                }
                if (connection.CanRequest) //Connection is open and usable
                    break;
            }
            return connection;
        }
        
        void TransactionCompleted(object sender, TransactionEventArgs e)
        {
            IDBConnection connection = null;
            string transID = e.Transaction.TransactionInformation.LocalIdentifier;
            if (_transactionConnectionLookup.TryGetValue(transID, out connection))
            {
                switch (e.Transaction.TransactionInformation.Status)
                {
                    case TransactionStatus.Committed:
                        IdleConnection(connection);
                        return; //If everything went ok, idle for reuse...don't dispose
                    case TransactionStatus.Aborted:
                        //Fallthrough
                    case TransactionStatus.InDoubt:
                        break;
                }
                _transactionConnectionLookup.Remove(transID);
                connection.Dispose();
            }
        }

        private void IdleConnection(IDBConnection connection)
        {
            _IdleConnections.Enqueue(connection); //finished, so push into the idle queue
        }

        private IDBConnection CreateConnection()
        {
            return ConnectionOptions.ConnectionFactory(EndPoint, ConnectionOptions);
        }

        private IDBConnection GetIdleConnection()
        {
            if (!_IdleConnections.Any())
                return null;
            return _IdleConnections.Dequeue();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBBinding"/> class based on an existing server binding
        /// </summary>
        /// <code>
        /// ServerBinding template = new ServerBinding("mongo://localhost");
        /// DBBinding templated = new DBBinding(template, "db");
        /// //result would be "mongo://localhost/db"
        /// </code>
        /// <param name="serverBinding">The server binding.</param>
        /// <param name="databaseUri">Name of the database.</param>
        public DBBinding(IServerBinding serverBinding, Uri databaseUri, bool readOnly = false)
        {
            Condition.Requires(serverBinding, "serverBinding")
                .IsNotNull();
            Condition.Requires(databaseUri, "name").IsNotNull();
            ReadOnly = readOnly;
            Uri relative = databaseUri.IsAbsoluteUri ? serverBinding.ToUri().MakeRelativeUri(databaseUri) : databaseUri;
            Uri = new Uri(serverBinding.ToUri(), relative);
            Address = serverBinding.Address;
            ConnectionOptions = new DBConnectionOptions();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DBBinding"/> class based on an existing server binding
        /// </summary>
        /// <code>
        /// ServerBinding template = new ServerBinding("mongo://localhost");
        /// DBBinding templated = new DBBinding(template, "db");
        /// //result would be "mongo://localhost/db"
        /// </code>
        /// <param name="serverBinding">The server binding.</param>
        /// <param name="databaseUri">Name of the database.</param>
        public DBBinding(IServerBinding serverBinding, string databaseUri, bool readOnly = false)
            : this(serverBinding, new Uri(databaseUri, UriKind.RelativeOrAbsolute))
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether modifications to the database is allowed.
        /// </summary>
        /// <value><c>true</c> if binding is read only; otherwise, <c>false</c>.</value>
        public bool ReadOnly
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            int hash = 17;
            hash = hash * 23 + EndPoint.GetHashCode();
            hash = hash * 23 + DatabaseName.GetHashCode();  
            return hash;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="other">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object other)
        {
            DBBinding a = other as DBBinding;
            if (a != null)
            {
                return a.EndPoint.Equals(EndPoint) && a.DatabaseName.Equals(DatabaseName);
            }
            return false;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Uri.ToString();
        }

        /// <summary>
        /// Gets a binding to another database on the same host
        /// </summary>
        /// <param name="databaseUri">The name of the database</param>
        /// <returns></returns>
        public IDBBinding GetSisterBinding(Uri databaseUri)
        {
            try
            {
                return new DBBinding(Server.Binding, databaseUri);
            }
            catch (SocketException uh)
            {
                throw new MongoInternalException("shouldn't be possible", uh);
            }
        }

        /// <summary>
        /// Gets the end point represented by this binding.
        /// </summary>
        /// <value>The end point.</value>
        public IPEndPoint EndPoint
        {
            get { return new IPEndPoint(Address, Port); }
        }

        /// <summary>
        /// Gets the name of the host this binding is bound to.
        /// </summary>
        /// <value>The name of the host.</value>
        public string HostName
        {
            get { return Uri.DnsSafeHost; }
        }

        /// <summary>
        /// Gets the port this binding is bound to
        /// </summary>
        /// <value>The port.</value>
        public int Port
        {
            get { return Uri.IsDefaultPort ? DefaultPort : Uri.Port; }
        }

        /// <summary>
        /// Gets the name of the database this binding is bound to
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get { return Uri.GetDatabaseName(); }
        }

        /// <summary>
        /// Gets the underlying IP address of this binding.
        /// </summary>
        /// <value>The address.</value>
        public IPAddress Address { get; private set; }

        public Uri Uri { get; private set;}

        /// <summary>
        /// Returns the default database host.
        /// </summary>
        /// <value>the db_ip setting from the .config, or "127.0.0.1" as a default</value>
        public static string DefaultHost
        {
            get
            {
                return Properties.Settings.Default.DefaultHost;
            }
        }

        /// <summary>
        /// Gets the default port that the database runs on.
        /// </summary>
        /// <value>the db_port setting from the .config, or 27017 as a default.</value>
        public static int DefaultPort
        {
            get
            {
                return Properties.Settings.Default.DefaultPort;
            }
        }

        void IInternalDBBinding.Initialize(IServer server)
        {
            Server = server;
        }

        public void Say(IDBCollection cmdCollection, IDBRequest request, bool checkError = false)
        {
            _Say(cmdCollection, request, checkError, false);

            //If we got an error back (and have the appropriate write concern) throw our exception
            if (LastException != null)
            {
                throw LastException;
            }
        }

        public bool CredentialsSpecified { get { return !string.IsNullOrWhiteSpace(Username); }}
        public bool Authenticated { get; private set; }

        public bool _Say(IDBCollection cmdCollection, IDBRequest request, bool checkError, bool suppressException)
        {
            LastException = null;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                //Not explicitly disposing connection...its lifespan is longer than just this message
                IDBConnection connection = RequireConnection(Transaction.Current);
                
                if (CredentialsSpecified)
                {
                    Authenticated = connection.TryAuthenticate(cmdCollection, Username, _UsernamePasswordHash);
                    if (!Authenticated && !suppressException)
                    {
                        this.LastException = new MongoException.Authentication(Username);
                        _UsernamePasswordHash = null;
                        return false;
                    }
                }

                try
                {
                    connection.Say(request);
                    if (checkError)
                    {
                        LastError le = Server.GetDatabase(this.DatabaseName).GetLastError();
                        if (le.Code != DBError.Code.NoError)
                            this.LastException = new MongoException.LastError(le);
                    }
                } 
                catch (Exception x)
                {
                    if (!suppressException)
                        throw;
                    else
                    {
                        this.LastException = x;
                        return false;
                    }
                }

                //If we got this far, our transaction succeeded.
                transactionScope.Complete();
                return LastException != null;
            }
        }

        public bool TrySay(IDBCollection cmdCollection, IDBRequest request, bool checkError)
        {
            return _Say(cmdCollection, request, checkError, true);
        }

        public IDBResponse<TDoc> Call<TDoc>(IDBCollection cmdCollection, IDBRequest request) where TDoc : class, IDocument
        {        
            IDBResponse<TDoc> response = null;
            string err = null;
            using (TransactionScope transactionScope = new TransactionScope())
            {
                //Not explicitly disposing connection...its lifespan is longer than just this message
                IDBConnection connection = RequireConnection(Transaction.Current);
                response = connection.Call<TDoc>(request);         

                //If we got this far, our transaction succeeded.
                transactionScope.Complete();

                //Did we get an error message back from server?
                err = response.GetError();

                if (err != null)
                {
                    throw new MongoException.Response(err, response);
                }
                return response;
            }
        }


        /// <summary>
        /// Gets or sets the hosting <see cref="Server"/>.
        /// </summary>
        /// <value>The server.</value>
        public IServer Server { get; private set; }


        public Exception LastException
        {
            get;
            private set;
        }


        public string Username
        {
            get;
            private set;
        }

        SecureString _UsernamePasswordHash;

        public void SetCredentials(string username, SecureString password)
        {
            Username = username;
            _UsernamePasswordHash = new SecureString();
            _UsernamePasswordHash.Append(username);
            _UsernamePasswordHash.Append(password);
        }
    }
}