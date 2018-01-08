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
	public class CheckListModel<T> : CollectionVMBase,
		INotifyPropertyChanged, INotifyCollectionChanged where T : IEquatable<T> {

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
		private ListItemModel<T> selectedItem;
		/// <summary>
		/// Allows a binding for an item clicked or selected in a list,
		/// instead of one being marked with a checkbox.
		/// TODO: Allow multiselection.
		/// </summary>
		public ListItemModel<T> SelectedItem {

			get { return this.selectedItem; }
			set {
				this.SetProperty( ref this.selectedItem, value, SelectedPropertyName );
			}

		}

		private readonly ObservableCollection<ListItemModel<T>> items = new ObservableCollection<ListItemModel<T>>();
		/// <summary>
		/// The items being displayed.
		/// </summary>
		public ObservableCollection<ListItemModel<T>> Items {
			get { return this.items; }
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
		/// Returns a list of all items that have been checked.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<ListItemModel<T>> GetCheckedItems() {
			return this.Items.Where( ( item ) => { return item.IsChecked; } ).ToArray();
		}

		/// <summary>
		/// Called when a ListItemModel's checked property changes.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="nowChecked"></param>
		private void ItemCheckedChanged( ListItemModel<T> item, bool nowChecked ) {

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

		/// <summary>
		/// Removes items from the list.
		/// </summary>
		/// <param name="remove_items"></param>
		public void Remove( IEnumerable<ListItemModel<T>> remove_items ) {

			foreach( ListItemModel<T> item in remove_items ) {
				this.items.Remove( item );
			}

		}

		/// <summary>
		/// Remove all items from the list.
		/// </summary>
		public void Clear() {

			this.items.Clear();
			this.CheckedItems.Clear();

			this.CmdDelete.RaiseCanExecuteChanged();

		} //

		public CheckListModel( IEnumerable<T> start_files=null ) {

			this.items = new ObservableCollection<ListItemModel<T>>();
			this.items.CollectionChanged += this.Items_CollectionChanged;

			if( start_files != null ) {

				int count = start_files.Count();

				foreach( T data in start_files ) {

					ListItemModel<T> listItem = new ListItemModel<T>( data );
					this.items.Add( listItem );

				} // for

			}

		} // FileListModel()

		/// <summary>
		/// Property of an item within the list has changed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ListItem_PropertyChanged( object sender, PropertyChangedEventArgs e ) {

			if( e.PropertyName == ListItemModel<T>.IsCheckedPropertyName ) {
				this.ItemCheckedChanged( (ListItemModel<T>)sender, ( (ListItemModel<T>)sender).IsChecked );
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

				foreach( ListItemModel<T> item in e.NewItems ) {

					if( item.IsChecked ) {
						this.ItemCheckedChanged( item, true );
					}
					item.PropertyChanged += this.ListItem_PropertyChanged;

				}

			}
			if( e.OldItems != null ) {

				foreach( ListItemModel<T> listItem in e.OldItems ) {

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
