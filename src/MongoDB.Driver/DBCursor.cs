//COPYRIGHT

using System.Collections.Generic;
using System;
using System.Linq;
using MongoDB.Driver.Platform.Conditions;
namespace MongoDB.Driver
{

    /// <summary>
    /// 
    /// </summary>
    internal class DBCursor<TDoc> : IDBCursor<TDoc> where TDoc : class, IDocument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DBCursor"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public DBCursor(DBCursorOptions options)
        {
            Options = options;
        }

        void IDisposable.Dispose()
        {
        }

        public IEnumerator<IDocument> GetEnumerator()
        {
            return new DBCursorEnumerator<TDoc>(this);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        

        //        _lookForHints();

        //        DBQuery foo = Selector;
        //        if (hasSpecialQueryFields())
        //        {
        //            foo = new DBQuery();

        //        }

        //        int bs = _numWanted;
        //        if (_batchSize > 0)
        //        {
        //            if (_numWanted == 0)
        //                bs = _batchSize;
        //            else
        //                bs = Math.Min(bs, _batchSize);
        //        }
        //        _it = Collection.Find(foo, DesiredFieldSet, _skip, bs);
        //    }

        //    if (_it == null)
        //    {
        //        _it = new List<IDBObject>();
        //       // _fake = true;
        //        //TODO:still need _fake? What for?
        //    }
        //}

        ///**
        // * if there is a hint to use, use it
        // */
        //private void _lookForHints()
        //{

        //    if (_hint != null) // if someone set a hint, then don't do this
        //        return;

        //    if (Collection.IndexHintFieldSets == null)
        //        return;

        //    HashSet<string> mykeys = new HashSet<string>(Selector.Keys);

        //    foreach (DBFieldSet hintFieldSet in Collection.IndexHintFieldSets)
        //    {
        //        if (!mykeys.IsSupersetOf(hintFieldSet.Keys))
        //            continue;

        //        hint(hintFieldSet);
        //        return;
        //    }
        //}

        //bool hasSpecialQueryFields()
        //{
        //    if (OrderByFieldSet != null && OrderByFieldSet.Keys.Count > 0)
        //        return true;

        //    if (_hint != null)
        //        return true;

        //    return _explain;
        //}

        public DBCursorOptions Options { get; private set; }
        
        public IEnumerable<TDoc> DocumentsT
        {
            get { return this.Cast<TDoc>(); }
        }

        IEnumerator<IDocument> IEnumerable<IDocument>.GetEnumerator()
        {
            return new DBCursorEnumerator<TDoc>(this);
        }


        public IDBCursor Copy()
        {
            return new DBCursor<TDoc>(Options);
        }

        long? _CursorID = null;
        public long? CursorID
        {
            get { return _CursorID; }
            set { _CursorID = value; }
        }

        public bool HasMore
        {
            get { return !_CursorID.HasValue || _CursorID.Value != 0; } 
        }
    }

    internal class DBCursor : DBCursor<Document>
    {
        public DBCursor(DBCursorOptions options = null) : base(options)
        {
        }
        static Dictionary<string, List<long>> _DeadCursorLookup = new Dictionary<string, List<long>>();

        public static void CleanupCursors(IDBCollection collection)
        {
            //void _cleanCursors()
            //{
            //    //TODO:fix cross-class reference requiring internal specifier on _deadCursorList
            //    if (_Database._deadCursorList.Count == 0)
            //        return;

            //    if (_Database._deadCursorList.Count % 20 != 0 && _Database._deadCursorList.Count < MongoDB.Driver.Database.NUM_CURSORS_BEFORE_KILL)
            //        return;

            //    List<long> l = _Database._deadCursorList;
            //    _Database._deadCursorList = new List<long>();

            //    try
            //    {
            //        killCursors(l);
            //    }
            //    catch
            //    {
            //        _Database._deadCursorList.AddRange(l);
            //    }
            //}

            //void killCursors(List<long> all)
            //{
            //    if (all == null || all.Count < 1)
            //        return;

            //    Message.KillCursors killCursors = new Message.KillCursors(all);
            //    _Database.Say(killCursors, this.GetWriteConcern());
            //}
        }
    }
}