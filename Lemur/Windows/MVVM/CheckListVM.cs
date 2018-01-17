using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Represents a list where items can be marked as "Checked", usually
	/// with a checkbox.
	/// This is opposed to an item simply being selected (clicked on) in a list,
	/// though the functions can be combined.
	/// 
	/// TODO: Automate the sub-object watching better.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class CheckListVM<T> : CollectionVMBase,
		INotifyPropertyChanged, INotifyCollectionChanged {

		#region COMMANDS

		private RelayCommand _cmdDeleteChecked;

		/// <summary>
		/// Command to delete all checked items.
		/// </summary>
		public RelayCommand CmdDelete {

			get {
				return this._cmdDeleteChecked ?? ( this._cmdDeleteChecked = new RelayCommand(
			  null, this.HasCheckedItems
			  ) );
			}
	
			set {
				this.SetProperty( ref this._cmdDeleteChecked, value );
			} //

		} // CmdDelete

		private RelayCommand _cmdAddItem;
		public RelayCommand CmdAddItem {

			get {
				return this._cmdAddItem ?? ( this._cmdAddItem = new RelayCommand(

			  null

			  ) );
			}

			set {
				this.SetProperty( ref this._cmdAddItem, value );
			} //

		} // CmdOpen

		#endregion

		#region PROPERTIES

		private bool _multiCheck = true;
		/// <summary>
		/// Indicates multiple items from the list can be checked at the same time.
		/// </summary>
		public bool MultiCheck {
			get { return this._multiCheck; }
			set {
				this.SetProperty( ref this._multiCheck, value );
			}
		}

		public const string SelectedPropertyName = "SelectedItem";
		private ListItemVM<T> selectedItem;
		/// <summary>
		/// Allows a binding for an item clicked or selected in a list,
		/// instead of one being marked with a checkbox.
		/// TODO: Allow multiselection.
		/// </summary>
		public ListItemVM<T> SelectedItem {

			get { return this.selectedItem; }
			set {
				this.SetProperty( ref this.selectedItem, value, SelectedPropertyName );
			}

		}

		private readonly ObservableCollection<ListItemVM<T>> _itemModels = new ObservableCollection<ListItemVM<T>>();
		/// <summary>
		/// The items being displayed.
		/// </summary>
		public ObservableCollection<ListItemVM<T>> Items {
			get { return this._itemModels; }
		} //

		private readonly ObservableCollection<T> _checkedItems = new ObservableCollection<T>();
		/// <summary>
		/// Collection of items that have been checked in the list.
		/// </summary>
		public ObservableCollection<T> CheckedItems {
			get { return this._checkedItems; }
		}

		#endregion

		/// <summary>
		/// Returns an array of ListItem ViewModels for the checked items.
		/// </summary>
		/// <returns></returns>
		protected IEnumerable<ListItemVM<T>> GetCheckedVMs() {
			return this._itemModels.Where( ( item ) => item.IsChecked ).ToArray();
		}

		/// <summary>
		/// Returns a list of all items that have been checked.
		/// </summary>
		/// <returns></returns>
		public IEnumerable< T > GetCheckedItems() {
			return this._checkedItems.ToArray();
		}

		/// <summary>
		/// Called when a ListItemModel's checked property changes.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="nowChecked"></param>
		private void ItemCheckedChanged( ListItemVM<T> item, bool nowChecked ) {

			if( nowChecked ) {

				if( this.CheckedItems.Contains( item.Item ) ) {
					return;
				}
				this.CheckedItems.Add( item.Item );

			} else {

				// item unchecked.
				if( !this.CheckedItems.Remove( item.Item ) ) {
					return;
				}
	
			} //
			
			this.CmdDelete.RaiseCanExecuteChanged();
	
		}

		/// <summary>
		/// Returns whether any items in the list are selected (not checked.)
		/// </summary>
		/// <returns></returns>
		public bool HasSelectedItems() {

			return this.SelectedItem != null;

		}

		/// <summary>
		/// Check if any items in the list have had their items 'checked.'
		/// </summary>
		/// <returns></returns>
		public bool HasCheckedItems() {
			return this.CheckedItems.Count > 0;
		}

		public void Add( T item, bool isChecked = false ) {

			this._itemModels.Add( new ListItemVM<T>( item, isChecked ) );

		}

		/// <summary>
		/// Removes all checked items from the list, and returns an array
		/// of the items removed.
		/// </summary>
		/// <returns></returns>
		public T[] RemoveCheckedItems() {

			T[] items = this.CheckedItems.ToArray();

			int len = this._itemModels.Count;
			for( int i = len - 1; i >= 0; i-- ) {

				if( _itemModels[i].IsChecked ) {
					this._itemModels.RemoveAt( i );
				}

			} // for-loop.

			return items;

		}

		/// <summary>
		/// Removes items from the list.
		/// </summary>
		/// <param name="remove_items"></param>
		public void Remove( IEnumerable<ListItemVM<T>> remove_items ) {

			foreach( ListItemVM<T> item in remove_items ) {
				this._itemModels.Remove( item );
			}

		}

		/// <summary>
		/// Remove all items from the list.
		/// </summary>
		public void Clear() {

			this._itemModels.Clear();
			this.CheckedItems.Clear();

			this.CmdDelete.RaiseCanExecuteChanged();

		} //

		public CheckListVM( IEnumerable<T> start_files=null ) {

			this._itemModels = new ObservableCollection<ListItemVM<T>>();
			this._itemModels.CollectionChanged += this.Items_CollectionChanged;

			if( start_files != null ) {

				int count = start_files.Count();

				foreach( T data in start_files ) {

					ListItemVM<T> listItem = new ListItemVM<T>( data );
					this._itemModels.Add( listItem );

				} // for

			}

		} // FileListModel()

		/// <summary>
		/// Property of an item within the list has changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListItem_PropertyChanged( object sender, PropertyChangedEventArgs e ) {

			if( e.PropertyName == ListItemVM<T>.IsCheckedPropertyName ) {
				this.ItemCheckedChanged( (ListItemVM<T>)sender, ( (ListItemVM<T>)sender).IsChecked );
			}

		} // ListItem_PropertyChanged()

		/// <summary>
		/// When an Item is added to the collection, listeners need to be added for when their properties
		/// change, so notifications for changes can be triggered.
		/// TODO: Weak references on listeners?
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Items_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) {

			//Console.WriteLine( "ITEMS CHANGED: " + e.Action.ToString() );

			if( e.NewItems != null ) {

				foreach( ListItemVM<T> item in e.NewItems ) {

					if( item.IsChecked ) {
						this.ItemCheckedChanged( item, true );
					}
					item.PropertyChanged += this.ListItem_PropertyChanged;

				}

			}
			if( e.OldItems != null ) {

				foreach( ListItemVM<T> listItem in e.OldItems ) {

					if( listItem.IsChecked ) {
						this.ItemCheckedChanged( listItem, false );
					}

					/// NOTE: this works since ListItemModel<T> is a class.
					if( listItem == this.SelectedItem ) {
						this.SelectedItem = null;
					}

					///!!!!
					listItem.PropertyChanged -= this.ListItem_PropertyChanged;

				}

			} // ( OldItems != null )

		} // Files_CollectionChanged

	} // class

} // namespace
