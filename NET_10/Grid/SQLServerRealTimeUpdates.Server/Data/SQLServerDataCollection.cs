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
    public class SQLServerDataCollection<T> : C1AdoNetCursorDataCollection<T>, IAsyncDisposable
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
        }

        private SynchronizationContext? SynchronizationContext { get; }
        private IEqualityComparer<T> Comparer { get; }
        private SqlTableDependency<T>? Notifier { get; set; }

        ///<inheritdoc/>
        protected override async Task ConnectAsyncOverride(CancellationToken cancellationToken)
        {
            await base.ConnectAsyncOverride(cancellationToken);
            Notifier = new SqlTableDependency<T>(Connection.ConnectionString, TableName);
            Notifier.OnChanged += OnTableDependencyChanged;
            Notifier.Start();
        }

        ///<inheritdoc/>
        protected override async Task DisconnectAsyncOverride(CancellationToken cancellationToken)
        {
            if (Notifier != null)
            {
                Notifier.OnChanged -= OnTableDependencyChanged;
                Notifier.Stop();
            }
            await base.DisconnectAsyncOverride(cancellationToken);
        }

        ///<inheritdoc/>
        public async ValueTask DisposeAsync()
        {
            await DisconnectAsync();
        }

        private void OnTableDependencyChanged(object sender, RecordChangedEventArgs<T> e)
        {
            RunInContext(o =>
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
                                OnCollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem, index));
                            }
                        }
                        break;
                    case ChangeType.Insert:
                        {
                            InternalList.Add(e.Entity);
                            OnCollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, e.Entity, InternalList.Count - 1));
                        }
                        break;
                    case ChangeType.Update:
                        {
                            var index = InternalList.FindIndex(o => Comparer.Equals(o, e.Entity));
                            if (index >= 0)
                            {
                                var oldItem = InternalList[index];
                                InternalList[index] = e.Entity;
                                OnCollectionChanged(sender, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, e.Entity, oldItem, index));
                            }
                        }
                        break;
                }
            }, e);
        }

        private void RunInContext(Action<object?> action, object? state)
        {
            if (SynchronizationContext != null)
                SynchronizationContext.Post(new SendOrPostCallback(action), state);
            else
                action(state);
        }

#pragma warning disable CS1591

        protected override async Task<int> InsertAsyncOverride(int index, T item, CancellationToken cancellationToken)
        {
            try
            {
                Notifier?.Stop();
                return await base.InsertAsyncOverride(index, item, cancellationToken);
            }
            finally
            {
                Notifier?.Start();
            }
        }

        protected override async Task RemoveAsyncOverride(int index, CancellationToken cancellationToken)
        {
            try
            {
                Notifier?.Stop();
                await base.RemoveAsyncOverride(index, cancellationToken);
            }
            finally
            {
                Notifier?.Start();
            }
        }

        protected override async Task<int> ReplaceAsyncOverride(int index, T item, CancellationToken cancellationToken)
        {
            try
            {
                Notifier?.Stop();
                return await base.ReplaceAsyncOverride(index, item, cancellationToken);
            }
            finally
            {
                Notifier?.Start();
            }
        }
#pragma warning restore CS1591
    }
}
