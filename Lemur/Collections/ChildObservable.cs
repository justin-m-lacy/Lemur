using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;

namespace Lemur.Collections {

	/// <summary>
	/// Observable collection that sends notifications for Child property changed events.
	/// 
	/// TODO: weak references?
	///
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ChildObservable<T> : ICollection<T>, INotifyPropertyChanged, INotifyCollectionChanged {

		/// <summary>
		/// Underlying collection.
		/// </summary>
		private readonly ObservableCollection<T> _collection;

		public int Count => _collection.Count;

		public bool IsReadOnly => ( (ICollection<T>)_collection ).IsReadOnly;

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Wraps the underlying ObservableCollection event.
		/// </summary>
		public event NotifyCollectionChangedEventHandler CollectionChanged {
			add { this._collection.CollectionChanged += value; }
			remove { this._collection.CollectionChanged -= value; }
		}

		public ChildObservable( IEnumerable<T> items=null ) {

			if( items != null ) {
				this._collection = new ObservableCollection<T>( items );
			} else {
				this._collection = new ObservableCollection<T>();
			}

			this.InitCollection();


		} //

		private void InitCollection() {

			this._collection.CollectionChanged += this._collection_CollectionChanged;

		}

		private void _collection_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) {

			if( e.OldItems != null ) {
				this.RemoveRange( e.OldItems );
			}
			if( e.NewItems != null ) {
				this.AddRange( e.NewItems );
			}

		}

		private void AddRange( IList items ) {

			foreach( INotifyPropertyChanged item in items ) {
				item.PropertyChanged += this.Item_PropertyChanged;
			}

		} //

		private void Item_PropertyChanged( object sender, PropertyChangedEventArgs e ) {

			// bubble up.
			if( this.PropertyChanged != null ) {
				this.PropertyChanged( sender, e );
			}

		}

		private void RemoveRange( IList items ) {

			foreach( INotifyPropertyChanged item in items ) {
				item.PropertyChanged -= this.Item_PropertyChanged;
			}

		} //

		#region COLLECTION<T> IMPLEMENTATION

		public void Add( T item ) {
			this._collection.Add( item );
		}

		public void Clear() {
			this._collection.Clear();
		}

		public bool Contains( T item ) {
			return this._collection.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex ) {
			this._collection.CopyTo( array, arrayIndex );
		}

		public IEnumerator<T> GetEnumerator() {
			return this._collection.GetEnumerator();
		}

		public bool Remove( T item ) {
			return _collection.Remove( item );
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this._collection.GetEnumerator();
		}

#endregion

	} // class

} // namespace