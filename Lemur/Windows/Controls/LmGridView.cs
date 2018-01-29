using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;
using System.Windows.Data;
using Lemur.Utils;
using System.Collections.ObjectModel;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// A more complicated GridView that allows showing and hiding columns by header or the ColumnName attached property.
	/// </summary>
	public class LMGridView : GridView {

		#region OLD PARENT REFERENCE
		/// <summary>
		/// This appears to be necessary to map column Visible changes to the LMGridView that has to hide
		/// the column. The logistics of keeping track of all the owners, visible states is annoying.
		/// </summary>
		/*public static readonly DependencyProperty MyGridProperty = DependencyProperty.RegisterAttached( "MyGrid",
			typeof( object ), typeof( LMGridView ) );
		public static void SetMyGrid( GridViewColumn c, object g ) {
			c.SetValue( MyGridProperty, g );
		}
		public static object GetMyGrid( GridViewColumn c ) {
			return c.GetValue( MyGridProperty );
		}*/
		#endregion

		#region DEPENDENCY PROPERTIES

		/// <summary>
		/// array of ColumnNames or column headers that should not be displayed in the grid.
		/// </summary>
		public static readonly DependencyProperty HideColumnsProperty = DependencyProperty.Register( "HideColumns",
			typeof( List<string> ), typeof( LMGridView ), new PropertyMetadata( HideColumnsChanged ) );

		/// <summary>
		/// Enabled the automatic creation of a Column Header context menu that allows selecting which columns are visible.
		/// </summary>
		public static readonly DependencyProperty AutoColumnMenuProperty = DependencyProperty.Register( "AutoColumnMenu",
			typeof( bool ), typeof( LMGridView ), new PropertyMetadata( false, AutoMenuChanged ) );

		/// <summary>
		/// Action to take when the column's Visible property is set to true.
		/// NOTE: For some reason it's not possible to store a reference to the owning Grid.
		/// StackOverflowException will occur with no loops or recursion in the main program. Happens in the underlying MS code.
		/// Might have something to do with storing an object as a DependencyProperty value while the xaml is still parsing.
		/// TODO: Make this an Action list so multiple grids can use the same GridViewColumn.
		/// </summary>
		private static readonly DependencyProperty VisibleActionProperty = DependencyProperty.RegisterAttached( "VisibleAction",
			typeof( Action<GridViewColumn, bool> ), typeof( LMGridView ) );

		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached( "Visible",
			typeof( bool ), typeof( LMGridView ), new PropertyMetadata( true, new PropertyChangedCallback(ColumnVisibleChanged) ) );

		public static readonly DependencyProperty ColumnNameProperty = DependencyProperty.RegisterAttached( "ColumnName",
			typeof( string ), typeof( GridViewColumn ) );

		private static void HideColumnsChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			LMGridView view = d as LMGridView;
			if( !view._updatingHideCols ) {

				// HideColumns might be updated from a Column Visible property changing; in which case
				// there's no reason to loop through all the columns again.
				view.SetHiddenColumns( (List<string>)e.NewValue );

			}

		}

		/// <summary>
		/// Property to automatically generate column ContextMenu items has changed.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void AutoMenuChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			LMGridView grid = d as LMGridView;
			if( (bool)e.NewValue == true ) {
				grid.BuildColumnMenu();
			} else {
				grid.RemoveColumnMenu();
			}

		}

		/// <summary>
		/// If a Column Visible property changed, any owning LMGridView must update the column visibility.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void ColumnVisibleChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			Console.WriteLine( "COLUMN VISIBLE CHANGED: " + ( (bool)e.NewValue ) );

			Action<GridViewColumn, bool> visAction = d.GetValue( VisibleActionProperty ) as Action<GridViewColumn, bool>;
			if( visAction != null ) {
				visAction( d as GridViewColumn, (bool)e.NewValue );
			} else {
				Console.WriteLine( "NO VISIBLE ACTION" );
			}

		} //

		#region DEPENDENCY PROPERTY ACCESSORS

		public bool AutoColumnMenu {
			get => (bool)this.GetValue( AutoColumnMenuProperty );
			set => this.SetValue( AutoColumnMenuProperty, value );
		}

		public List<string> HideColumns {
			get { return (List<string>)this.GetValue( HideColumnsProperty ); }
			set { this.SetValue( HideColumnsProperty, value ); }
		}

		private static Action<GridViewColumn, bool> GetVisibleAction( GridViewColumn c ) {
			return (Action<GridViewColumn, bool>)c.GetValue( VisibleActionProperty );
		}

		private static void SetVisibleAction( GridViewColumn c, Action<GridViewColumn, bool> action ) {
			c.SetValue( VisibleActionProperty, action );
		}

		public static void SetVisible( GridViewColumn c, bool b ) {
			c.SetValue( VisibleProperty, b );
		}
		public static bool GetVisible( GridViewColumn c ) {
			return (bool)c.GetValue( VisibleProperty );
		}

		public static void SetColumnName( GridViewColumn e, string name ) {
			e.SetValue( ColumnNameProperty, name );
		}
		public static string GetColumnName( GridViewColumn e ) {
			return (string)e.GetValue( ColumnNameProperty );
		}
		#endregion

		#endregion

		public class ColumnToggledEventArgs: EventArgs {

			private GridViewColumn column;
			private bool columnVisible;

			public ColumnToggledEventArgs( GridViewColumn c, bool visible ) {
				this.column = c;
				this.columnVisible = visible;
			}

		}
		public event EventHandler<ColumnToggledEventArgs> ColumnToggled;

		/// <summary>
		/// All columns, whether visible or not?
		/// While not necessary, having this collection simplifies some of the code.
		/// NOTE: doesn't help much because still have to listen to Columns callback to add to AllColumns...
		/// [TODO]
		/// </summary>
		//private readonly ObservableCollection<GridViewColumn> AllColumns = new ObservableCollection<GridViewColumn>();

		/// <summary>
		/// Columns cached but not currently displayed.
		/// </summary>
		private List<GridViewColumn> _hiddenColumns;

		public LMGridView() : base() {

			this.Columns.CollectionChanged += Columns_CollectionChanged;

		}

		/// <summary>
		/// Automatically generates ContextMenu items that allows selection of visible columns.
		/// </summary>
		private void BuildColumnMenu() {

			ContextMenu menu = this.ColumnHeaderContextMenu;
			if( menu == null ) {
				this.ColumnHeaderContextMenu = menu = new ContextMenu();
			}

			int numCols = this.Columns.Count;
			for( int i = 0; i < numCols; i++ ) {

				GridViewColumn c = this.Columns[i];
				MenuItem m = this.MakeColumnMenuItem( c );
				if( m != null ) {
					menu.Items.Add( m );
				}

			}


		} //

		private MenuItem MakeColumnMenuItem( GridViewColumn c ) {

			MenuItem item = new MenuItem();

			string nameStr = GetNameOrHeaderStr( c );
			if( string.IsNullOrEmpty( nameStr ) ) {
				return null;
			}

			item.DataContext = c;
			item.IsCheckable = true;
			PropertyPath path = new PropertyPath( LMGridView.VisibleProperty);
			Binding checkedBinding = new Binding();
			checkedBinding.Path = path;
			checkedBinding.Mode = BindingMode.TwoWay;
			checkedBinding.Source = c;
			item.SetBinding( MenuItem.IsCheckedProperty, checkedBinding );

			item.Header = nameStr;

			item.Name = this.MakeMenuName( nameStr );

			return item;

		}

		private bool HasColumnMenuItem( GridViewColumn c ) {

			ContextMenu menu = this.ColumnHeaderContextMenu;
			if( menu != null ) {
				foreach( MenuItem item in menu.Items ) {
					if( item.DataContext == c ) {
						return true;
					}
				}
			}
			return false;

		}

		private void RemoveColumnMenuItem( GridViewColumn c ) {

			ContextMenu menu = this.ColumnHeaderContextMenu;
			if( menu == null ) {
				return;
			}

			ItemCollection items = menu.Items;
			for( int i = items.Count-1; i >= 0; i-- ) {

				MenuItem item = items[i] as MenuItem;
				if( item != null && item.DataContext == c ) {
					items.RemoveAt( i );
					break;
				}

			}

		}


		/// <summary>
		/// Removes automatically generated ContextMenu items that allow selecting visible columns.
		/// </summary>
		private void RemoveColumnMenu() {

			ContextMenu menu = this.ColumnHeaderContextMenu;
			if( menu == null ) {
				return;
			}

			ItemCollection items = menu.Items;
			for( int i = items.Count - 1; i >= 0; i-- ) {

				MenuItem item = items[i] as MenuItem;
				if( item == null ) {
					continue;
				}
				if( item.Name.StartsWith( "lmrAuto" ) ) {
					
					items.RemoveAt( i );

				}

			}

		} // RemoveColumnMenu()

		private string MakeMenuName( string columnName ) {

			string prefix = "lmrAuto";
			return prefix + StringUtils.MakeVarName(columnName);

		}

		private void Columns_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) {

			if( e.OldItems != null ) {

				foreach( GridViewColumn c in e.OldItems ) {

					if(
						( this._hiddenColumns == null || !this._hiddenColumns.Contains( c ) ) ) {
						// if column is in hidden items, owner doesn't change.
						Console.WriteLine( "CLEARING COLUMN: " + c.Header );
						c.ClearValue( VisibleActionProperty );
					}

				}


			}

			if( e.NewItems != null ) {

				foreach( GridViewColumn c in e.NewItems ) {

					//Console.WriteLine( "SETTING COL VIS ACTION: " + c.Header );
					c.SetValue( VisibleActionProperty, new Action<GridViewColumn, bool>( this.UpdateVisibility ) );
					if( (bool)c.GetValue( VisibleProperty ) == false ) {
						this.HideColumn( c );
					}

					if( this.AutoColumnMenu && !this.HasColumnMenuItem(c) ) {

						MenuItem item = this.MakeColumnMenuItem( c );
						this.ColumnHeaderContextMenu.Items.Add( item );

					}


				}

			}

		} //

		#region COLUMN HIDING

		/// <summary>
		/// Prevent circular column hiding callbacks.
		/// </summary>
		private bool _updatingHideCols;

		/// <summary>
		/// Sets the listed columns, and only the listed columns, to hidden.
		/// </summary>
		/// <param name="hideKeys"></param>
		private void SetHiddenColumns( List<string> hideKeys ) {

			// prevent cyclic callbacks
			this._updatingHideCols = true;
			this.Columns.CollectionChanged -= this.Columns_CollectionChanged;

			int hiddenCount = this._hiddenColumns == null ? 0 : this._hiddenColumns.Count;

			GridViewColumnCollection columns = this.Columns;

			// loop the actual columns first so the column name/header lookup is only done once.
			for( int i = columns.Count - 1; i >= 0; i-- ) {

				string colName = this.GetNameOrHeaderStr( columns[i] );
				if ( hideKeys.Contains(colName) ) {

					/// column should be hidden.
					this.CacheColumn( i );

				}

			} // for-loop.

			// check for any previously hidden columns that should no longer be hidden.
			// start from the columns that were hidden before any added from this method.
			for( int i = hiddenCount; i >= 0; i-- ) {

				string colName = this.GetNameOrHeaderStr( this._hiddenColumns[i] );
				if( !hideKeys.Contains(colName) ) {

					// column should not be hidden.
					this.RestoreColumn( i );

				}

			} // for-loop.

			// restore collection callback.
			this.Columns.CollectionChanged += this.Columns_CollectionChanged;
			this._updatingHideCols = false;

		}

		/// <summary>
		/// Called by the Visible Attached Property change listener.
		/// </summary>
		/// <param name="c"></param>
		/// <param name="visible"></param>
		private void UpdateVisibility( GridViewColumn c, bool visible ) {

			if( this._updatingHideCols ) {
				// HideColumns is already updating the visibility of all columns.
				// Setting individual column visibility is superfluous.
				Console.WriteLine( "LMGridView: CANCELLING VISIBLITY UPDATE" );
				return;
			}

			/// show or hide the column.
			if( visible ) {

				//Console.WriteLine( "Show Visiblity changed" );
				this.ShowColumn( c );

			} else {
				//Console.WriteLine( "Hide Visiblity changed" );
				this.HideColumn( c );

			}

		} //

		public void HideColumn( GridViewColumn c ) {

			int index = this.Columns.IndexOf( c );
			if( index >= 0 ) {
				this.CacheColumn( index );
			}

		}

		public void ShowColumn( GridViewColumn c ) {

			int index = this._hiddenColumns.IndexOf( c );
			if( index >= 0 ) {
				this.RestoreColumn( index );
			}

		}

		public void HideColumn( object nameOrHeader ) {

			int index;
			if( nameOrHeader is string ) {

				index = FindColumnIndex( this.Columns, nameOrHeader as string );

			} else {

				index = FindColumnIndex( this.Columns, nameOrHeader );
			}

			if( index >= 0 ) {
				this.CacheColumn( index );
			}


		}

		public void ShowColumn( object nameOrHeader ) {

			if( this._hiddenColumns == null ) {
				return;
			}

			int index;
			if( nameOrHeader is string ) {
				index = this.FindColumnIndex( this._hiddenColumns, nameOrHeader as string );
			} else {
				index = this.FindColumnIndex( this._hiddenColumns, nameOrHeader );
			}

			if( index >= 0 ) {
				this.RestoreColumn( index );
			}

		} //


		/// <summary>
		/// Cache a column no longer being displayed.
		/// </summary>
		/// <param name="c"></param>
		private void CacheColumn( int index ) {

			GridViewColumn c = this.Columns[index];

			if( this._hiddenColumns == null ) {
				this._hiddenColumns = new List<GridViewColumn>();
			}
			c.SetValue( VisibleProperty, false );

			this._hiddenColumns.Add( c );

			/// MUST BE ADDED AFTER added to _hiddenColumns.
			/// TODO: way too messy.
			this.Columns.RemoveAt( index );

			if( !this._updatingHideCols ) {
				this.RemoveHideCol( c );
			}
	
			this.DispatchToggled( c, false );

		}

		/// <summary>
		/// Restore a column that was previously hidden.
		/// </summary>
		/// <param name="c"></param>
		private void RestoreColumn( int index ) {

			GridViewColumn c = this._hiddenColumns[index];
			c.SetValue( VisibleProperty, true );

			this._hiddenColumns.RemoveAt( index );

			this.Columns.Add( c );

			if( !this._updatingHideCols ) {
				this.AddHideCol( c );
			}

			this.DispatchToggled( c, true );


		}

		/// <summary>
		/// nameOrHeader can either be a LMGridView.ColumnName or the Header content
		/// of the GridViewColumn.
		/// </summary>
		/// <param name="nameOrHeader"></param>
		/// <returns></returns>
		private int FindColumnIndex( IList<GridViewColumn> colList, string nameOrHeader ) {

			if( string.IsNullOrEmpty( nameOrHeader ) ) {
				return -1;
			}

			for( int i = colList.Count - 1; i >= 0; i-- ) {

				GridViewColumn c = colList[i];
				if( LMGridView.GetColumnName( c ) == nameOrHeader ) {

					return i;

				}
				string header = c.Header as string;
				if( header == nameOrHeader ) {
					return i;
				}

			}

			return -1;

		}

		/// <summary>
		/// nameOrHeader can either be a LMGridView.ColumnName or the Header content
		/// of the GridViewColumn.
		/// </summary>
		/// <param name="nameOrHeader"></param>
		/// <returns></returns>
		private int FindColumnIndex( IList<GridViewColumn> colList, object header ) {

			if( header == null ) {
				return -1;
			}

			for( int i = colList.Count - 1; i >= 0; i-- ) {

				GridViewColumn c = colList[i];
				if( c.Header == header ) {
					return i;
				}

			}

			return -1;

		}

		private void AddHideCol( GridViewColumn c ) {

			string colKey = GetNameOrHeaderStr( c );
			List<string> hideCols = this.HideColumns;
			if( hideCols != null ) {

				if( !hideCols.Contains( colKey ) ) {
					hideCols.Add( colKey );
				}

			}

		}

		private void RemoveHideCol( GridViewColumn c ) {

			string colKey = GetNameOrHeaderStr( c );
			List<string> hideCols = this.HideColumns;
			if( hideCols != null ) {

				if( hideCols.Remove( colKey ) ) {
				}

			}

		}

		public object GetNameOrHeader( GridViewColumn c ) {

			string name = GetColumnName( c );
			if( !string.IsNullOrEmpty( name ) ) {
				return name;
			}
			return c.Header;

		}

		/// <summary>
		/// Gets the name or header as a string.
		/// </summary>
		/// <param name="c"></param>
		/// <returns></returns>
		public string GetNameOrHeaderStr( GridViewColumn c ) {

			string name = GetColumnName( c );
			if( string.IsNullOrEmpty( name ) ) {
				return c.Header as string;
			}

			return name;

		}

		private void DispatchToggled( GridViewColumn c, bool visible ) {
			this.ColumnToggled?.Invoke( this, new ColumnToggledEventArgs( c, visible ) );
		}

		#endregion


	} // class

} // namespace
