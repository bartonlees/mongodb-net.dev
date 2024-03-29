﻿
namespace MongoDB.Driver
{
    /// <summary>
    /// Constants used in MongoDB.Driver namespace
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// The maximum allowable object size
        /// </summary>
        public const int MAX_OBJECT_SIZE = 1024 * 1024 * 4;
        /// <summary>
        /// The maximum connections allowed per host
        /// </summary>
        public const int CONNECTIONS_PER_HOST = 10;
        /// <summary>
        /// A hack to handle a special case (do we still need this?)
        /// </summary>
        public const string NO_REF_HACK = "_____nodbref_____";
        /// <summary>
        /// The Global Flag
        /// </summary>
        public const int GLOBAL_FLAG = 256;

        /// <summary>
        /// The maximum number of bytes allowed to be sent to the db at a time 
        /// </summary>
        public const int MAX_STRING = MAX_OBJECT_SIZE - 1024;

        /// <summary>
        /// Well-Known Collection Names
        /// </summary>
        public static class CollectionNames
        {
            /// <summary>
            /// Name of the virtual collection used for sending database commands to the server
            /// </summary>
            public const string Cmd = "$cmd";

            /// <summary>
            /// Name of the collection that houses user information
            /// </summary>
            public const string SystemUsers = "system.users";

            /// <summary>
            /// A collection that you can use to store JavaScript functions
            /// </summary>
            /// <remarks>
            /// There is a special system collection called system.js that can store JavaScript function to be re-used. To store a function, you would do:
            /// <code>
            /// db.system.js.save( { _id : "foo" , value : function( x , y ){ return x + y; } } );
            /// </code>
            /// _id is the name of the function, and is unique per database.
            /// Once you do that, you can use foo from any JavaScript context (db.eval, $where, map/reduce)
            /// See http://github.com/mongodb/mongo/tree/master/jstests/storefunc.js for a full example
            /// </remarks>
            public const string SystemJs = "system.js";
        }

        /// <summary>
        /// Well-Known, Special field names
        /// </summary>
        public static class SpecialFieldNames
        {
            /// <summary>
            /// The id of a referenced object
            /// </summary>
            public const string Id = "$id";

            /// <summary>
            /// A referenced collection's name
            /// </summary>
            public const string Ref = "$ref";

            /// <summary>
            /// Hints at what index to use for a query
            /// </summary>
            public const string Hint = "$hint";

            /// <summary>
            /// Requests a performance explanation for the query
            /// </summary>
            public const string Explain = "$explain";

            /// <summary>
            /// Requests that the operation occur against a snapshot of the data
            /// </summary>
            public const string Snapshot = "$snapshot";
        }

        
    }
}
