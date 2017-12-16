using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Collections {

	/// <summary>
	/// Tracks a current item from a list.
	/// NOTE: Maybe this shouldn't directly be a list, but just give access to the underlying list?
	/// </summary>
	public class ListVisitor<T> : IList<T>, IListVisitor<T>, INotifyCollectionChanged, INotifyPropertyChanged {

		public event NotifyCollectionChangedEventHandler CollectionChanged;
		public event PropertyChangedEventHandler PropertyChanged;

		private IList<T> list;
		/// <summary>
		/// Gives access to the underlying list.
		/// </summary>
		public IList<T> List {
			get {
				return this.list;
			}
		}

		public string CurrentPropertyName = "Current";
		private int curIndex;
		public int CurrentIndex {
			get => this.curIndex;
			set {
	
				if( value != this.curIndex ) {
					Console.WriteLine( "INDEX CHANGE: " + curIndex + " -> " + value );
					this.curIndex = value;
					this.NotifyPropertyChanged();
					this.NotifyPropertyChanged( CurrentPropertyName );
				}

			}
		}

		public T Current {
			get {
				if( this.curIndex < 0 || this.curIndex >= this.list.Count ) {
					return default( T );
				}
				return this.list[this.curIndex];
			}
		}

		public int Count => this.list.Count;
		public bool IsReadOnly => this.list.IsReadOnly;

		private bool _looping = true;
		/// <summary>
		/// Whether the Current item should loop when it reaches the end of the list
		/// ( or the start of the list, if visiting in reverse. )
		/// </summary>
		public bool Looping { get { return this._looping; }
			set {
				if( this._looping != value ) {
					this._looping = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		public T this[int index] { get => this.list[index];

			set {

				T oldItem = this.list[index];

				if( oldItem.Equals( value ) ) {
					// No change.
					return;
				}
				this.list[index] = value;
				this.NotifyCollectionChanged( index, oldItem, value );

			}

		}

		public ListVisitor( IList<T> list ) {

			this.list = list;
			this.CurrentIndex = -1;

		} //

		#region NOTIFICATION EVENTS

		protected void NotifyPropertyChanged( [CallerMemberName] string propertyName = "" ) {

			if( this.PropertyChanged != null ) {
				PropertyChangedEventHandler handler = this.PropertyChanged;
				handler( this, new PropertyChangedEventArgs( propertyName ) );
			}

		}

		protected void NotifyCollectionAdded( T item, int index ) {

			this.CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, item, index ) );

		}

		protected void NotifyCollectionRemoved( T item, int index ) {

			this.CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Remove, item, index ) );

		}

		protected void NotifyCollectionChanged( int index, T oldItem, T newItem ) {

			this.CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Replace, newItem, oldItem, index ) );

		}

		#endregion

		/// <summary>
		/// Set the Current item to the previous element of the list.
		/// </summary>
		public void Prev() {

			Console.WriteLine( "Moving previous entry" );
			int newIndex = this.curIndex - 1;
			if ( newIndex < 0 ) {

				if( this.Looping ) {
					newIndex = this.list.Count - 1;
				} else {
					newIndex = 0;
				}

			}
			this.CurrentIndex = newIndex;

		}

		/// <summary>
		/// Set the Current item to the next element of the list.
		/// </summary>
		public void Next() {

			int newIndex = this.curIndex + 1;
			if ( newIndex >= this.list.Count ) {

				if( this.Looping ) {
					newIndex = 0;
				} else {
					newIndex = this.list.Count - 1;
				}

			}
			this.CurrentIndex = newIndex;

		}

		#region LIST METHODS
	
		public int IndexOf( T item ) {
			return this.list.IndexOf( item );
		}

		virtual public void Insert( int index, T item ) {
	
			this.list.Insert( index, item );

			this.NotifyCollectionAdded( item, index );

		}

		virtual public void Add( T item ) {

			int len = this.list.Count;
			this.list.Add( item );

			this.NotifyCollectionAdded( item, len );

		}

		public void AddItems<S>( S items ) where S : IEnumerable<T> {

			foreach( T item in items ) {
				this.list.Add( item );
			}

			this.CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Add, items ) );

		}

		virtual public void Clear() {

			this.list.Clear();
			this.CurrentIndex = -1;

			this.CollectionChanged?.Invoke( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );

		}

		/// <summary>
		/// !!! NOTE: WARNING: Must move back the currentIndex if a previous item from
		/// the list is removed.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		virtual public bool Remove( T item ) {

			int index = this.list.IndexOf( item );
			if( index < 0 || index >= this.Count ) {
				return false;
			}

			this.RemoveAt( index );

			return true;

		}

		public void RemoveAt( int index ) {

			if( index < 0 || index >= this.Count ) {
				return;
			}

			T item = this.list[index];
			Console.WriteLine( "Directory Removing: " + index );
			this.list.RemoveAt( index );

			if( index == this.CurrentIndex ) {

				int new_index = this.CurrentIndex - 1;
				if( new_index < 0 ) {
					new_index = this.Count - 1;     // no items => new_index = -1
				}
				this.CurrentIndex = new_index;

			} else if( index < this.CurrentIndex ) {
				// CURRENT INDEX CHANGES BECAUSE ITEM WAS REMOVED.
				this.Prev();
			}

			this.NotifyCollectionRemoved( item, index );

		}

		virtual public bool Contains( T item ) {
			return this.list.Contains( item );
		}

		public void CopyTo( T[] array, int arrayIndex ) {
			this.list.CopyTo( array, arrayIndex );
		}

		public IEnumerator<T> GetEnumerator() {
			return this.list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return this.list.GetEnumerator();
		}

		#endregion

	} // class

} // namespace