﻿using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MongoDB.Driver.Message
{
    internal class Reply<TDoc> : Header, IDBResponse<TDoc> where TDoc : class, IDocument
    {
        /// <summary>
        /// Gets or sets the response flag.
        /// </summary>
        /// <value>normally zero, non-zero on query failure</value>
        public int ResponseFlag { get; set; }

        /// <summary>
        /// Gets or sets the cursor ID.
        /// </summary>
        /// <value>id of the cursor created for this query response</value>
        public long CursorID { get; set; }

        /// <summary>
        /// Gets or sets the starting from.
        /// </summary>
        /// <value>indicates where in the cursor this reply is starting.</value>
        public int StartingFrom { get; set; }

        /// <summary>
        /// Gets or sets the number returned.
        /// </summary>
        /// <value>number of documents in the reply.</value>
        public int NumberReturned { get; set; }

        /// <summary>
        /// The documents in this reply
        /// </summary>
        /// <value>The documents T.</value>
        /// <returns></returns>
        public IEnumerable<TDoc> DocumentsT
        {
            get
            {
                return _documents.AsReadOnly();
            }
        }

        /// <summary>
        /// Gets the documents.
        /// </summary>
        /// <value>The documents.</value>
        public IEnumerable<IDocument> Documents
        {
            get
            {
                return _documents.AsReadOnly().Cast<IDocument>();
            }
        }

        List<TDoc> _documents = new List<TDoc>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Reply&lt;TDoc&gt;"/> class.
        /// </summary>
        /// <param name="partial">if set to <c>true</c> [partial].</param>
        public Reply(bool partial)
            : base(Operation.Reply)
        {
            Partial = partial;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Reply&lt;TDoc&gt;"/> is partial.
        /// </summary>
        /// <value><c>true</c> if partial; otherwise, <c>false</c>.</value>
        public bool Partial { get; private set; }

        /// <summary>
        /// Reads the message details via the specified reader. (Unsupported for Request Messages)
        /// </summary>
        /// <param name="reader">The reader.</param>
        public void Read(WireProtocolReader reader)
        {
            //Read in the header
            MessageLength = reader.ReadInt32();
            RequestID = reader.ReadInt32();
            ResponseTo = reader.ReadInt32();
            int op = reader.ReadInt32();
            if ((Operation)op != Operation.Reply)
                throw new InvalidDataException(string.Format("Unexpected OpCode: {0}", op));

            //Read Response fields
            ResponseFlag = reader.ReadInt32();
            CursorID = reader.ReadInt64();
            StartingFrom = reader.ReadInt32();
            NumberReturned = reader.ReadInt32();

            _documents = new List<TDoc>(NumberReturned);
            for (int i = 0; i < NumberReturned; i++)
            {
                _documents.Add(ReadDocument(reader));
            }
        }

        /// <summary>
        /// Reads the document.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        protected virtual TDoc ReadDocument(WireProtocolReader reader)
        {
            return reader.ReadDocument<TDoc>(Partial);
        }
    }
}
