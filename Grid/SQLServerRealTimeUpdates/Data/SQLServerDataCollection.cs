using C1.DataCollection.AdoNet;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.Threading;
using TableDependency.SqlClient;
using TableDependency.SqlClient.Base.Enums;
using TableDependency.SqlClient.Base.EventArgs;

namespace SQLServerRealTimeUpdates.Data
{
    public class SQLServerDataCollection<T> : C1AdoNetCursorDataCollection<T>, IDisposable
        where T : class, new()
    {
        public SQLServerDataCollection(DbConnection connection, string tableName, IEqualityComparer<T> comparer = null, IEnumerable<string> fields = null)
            : base(connection, tableName, fields)
        {
            SynchronizationContext = SynchronizationContext.Current;
            Comparer = comparer ?? EqualityComparer<T>.Default;
            Notifier = new SqlTableDependency<T>(connection.ConnectionString, tableName);
            Notifier.OnChanged += OnTableDependencyChanged;
            Notifier.Start();
        }

        public void Dispose()
        {
            Notifier.OnChanged -= OnTableDependencyChanged;
            Notifier.Stop();
        }

        public SynchronizationContext SynchronizationContext { get; }
        public IEqualityComparer<T> Comparer { get; }
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
    }
}
