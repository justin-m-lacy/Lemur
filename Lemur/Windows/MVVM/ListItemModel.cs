using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Defined as class instead of struct to facilitate MVVM
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ListItemModel<T> : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Name (string) of the Item property.
		/// </summary>
		public const string ItemPropertyName = "Item";
		private T _item;
		public T Item {
			get { return this._item; }
			set {
				if( !_item.Equals( value ) ) {
					this._item = value;
					this.DispatchChange( ItemPropertyName, value );
				}
			}

		}

		/// <summary>
		/// Name (string) of the IsChecked property.
		/// </summary>
		public const string IsCheckedPropertyName = "IsChecked";
		/// <summary>
		/// Whether the item is checked in the view.
		/// </summary>
		private bool _checked;
		/// <summary>
		/// Whether the item has been checked in the view.
		/// </summary>
		public bool IsChecked {
			get { return this._checked; }
			set {

				if( this._checked != value ) {
					this._checked = value;
					DispatchChange( IsCheckedPropertyName, value );
				}

			}
		}

		private void DispatchChange<TProp>( string prop, TProp data ) {

			this.PropertyChanged?.Invoke( this, new ChangeEventArgs<TProp>( prop, data ) );

		}

		public ListItemModel( T data ) {

			this._item = data;
			this._checked = false;

		}


		public ListItemModel( T data, bool isChecked ) {

			this._item = data;
			this._checked = isChecked;

		}

	} // class

} // namespace
