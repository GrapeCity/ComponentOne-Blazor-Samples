using C1.DataCollection.AdoNet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace SQLServerRealTimeUpdates.Data
{
    /// <summary>
    /// Specialized SQLServer collection supporting notifications when the table changes.
    /// </summary>
    public class SQLServerDataCollection<T> : C1AdoNetCursorDataCollection<T>, IDisposable
        where T : class, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLServerDataCollection{T}"/> class.
        /// </summary>
        /// <param name="connection">The ado.net connection.</param>
        /// <param name="tableName">The table name.</param>
        /// <param name="comparer">Comparer used to determine the identity of the items.</param>
        /// <param name="fields">The fields that will be queried.</param>
        public SQLServerDataCollection(DbConnection connection, string tableName, IEqualityComparer<T> comparer = null, IEnumerable<string> fields = null)
            : base(connection, tableName, fields)
        {
            SynchronizationContext = SynchronizationContext.Current;
            Comparer = comparer ?? EqualityComparer<T>.Default;
            Notifier = new SqlTableDependency<T>(connection.ConnectionString, tableName);
            Notifier.OnChanged += OnTableDependencyChanged;
            Notifier.Start();
        }

        private SynchronizationContext SynchronizationContext { get; }
        private IEqualityComparer<T> Comparer { get; }
        private SqlTableDependency<T> Notifier { get; }


        private void OnTableDependencyChanged(object sender, RecordChangedEventArgs<T> e)
        {
            SynchronizationContext.Post(new SendOrPostCallback(o =>
            {

                switch (e.ChangeType)
                {
                    case ChangeType.Delete:
                        {
                            var index = InternalList.FindIndex(o => Comparer.Equals(o, e.Entity));
                            if (index >= 0)
                            {
                                var oldItem = InternalList[index];
                                InternalList.RemoveAt(index);
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
                            }
                        }
                        break;
                    case ChangeType.Insert:
                        {
                            InternalList.Add(e.Entity);
                            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.Entity, InternalList.Count - 1));
                        }
                        break;
                    case ChangeType.Update:
                        {
                            var index = InternalList.FindIndex(o => Comparer.Equals(o, e.Entity));
                            if (index >= 0)
                            {
                                var oldItem = InternalList[index];
                                InternalList[index] = e.Entity;
                                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.Entity, oldItem, index));
                            }
                        }
                        break;
                }
            }), e);
        }

#pragma warning disable CS1591

        public void Dispose()
        {
            Notifier.OnChanged -= OnTableDependencyChanged;
            Notifier.Stop();
        }

        protected override async Task<int> InsertAsyncOverride(int index, object item)
        {
            try
            {
                Notifier.Stop();
                return await base.InsertAsyncOverride(index, item);
            }
            finally
            {
                Notifier.Start();
            }
        }

        protected override async Task RemoveAsyncOverride(int index)
        {
            try
            {
                Notifier.Stop();
                await base.RemoveAsyncOverride(index);
            }
            finally
            {
                Notifier.Start();
            }
        }

        protected override async Task ReplaceAsyncOverride(int index, object item)
        {
            try
            {
                Notifier.Stop();
                await base.ReplaceAsyncOverride(index, item);
            }
            finally
            {
                Notifier.Start();
            }
        }
#pragma warning restore CS1591
    }
}
