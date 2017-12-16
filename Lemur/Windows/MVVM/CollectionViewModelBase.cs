using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;

namespace Lemur.Windows.MVVM {

	public class CollectionViewModelBase : ViewModelBase, INotifyCollectionChanged {

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		protected void NotifyReset<T>( IList<T> new_files = null ) {

			if( CollectionChanged != null ) {
				if( new_files != null ) {
					this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset, new_files ) );
				} else {
					this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( NotifyCollectionChangedAction.Reset ) );
				}
			}

		} // NotifyReset()

		protected void NotifyCollectionChanged<T>( NotifyCollectionChangedAction action, T item, int index = -1 ) {

			if( CollectionChanged != null ) {

				if( index >= 0 ) {
					this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( action, item, index ) );
				} else {
					this.CollectionChanged( this, new NotifyCollectionChangedEventArgs( action, item ) );
				}

			}

		} // NotifyCollectionChanged()

	} // class

} // namespace
