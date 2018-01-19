using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Specialized;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// A more complicated GridView that allows showing and hiding columns by header or the ColumnName attached property.
	/// </summary>
	public class CustomGridView : GridView {

		/// <summary>
		/// TODO: Make this an Owner list so multiple grids can use the same GridViewColumn.
		/// This appears to be necessary to map column Visible changes to the CustomGridView that has to hide
		/// the column. The logistics of keeping track of all the owners, visible states is annoying.
		/// </summary>
		/*public static readonly DependencyProperty MyGridProperty = DependencyProperty.RegisterAttached( "MyGrid",
			typeof( object ), typeof( CustomGridView ) );*/

		/// <summary>
		/// NOTE: For some reason it's not possible to store a direct reference to the owning Grid, or list of owning grids.
		/// StackOverflowException will occur with no loops or recursion in the main program. Happens in the windows code.
		/// Might have something to do with storing an object as a DependencyProperty value while the xaml is still parsing.
		/// </summary>
		public static readonly DependencyProperty VisibleActionProperty = DependencyProperty.RegisterAttached( "VisibleAction",
			typeof( Action<GridViewColumn, bool> ), typeof( CustomGridView ) );

		private static void ColumnOwnerChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {
			Console.WriteLine( "COL OWNER CHANGED" );
		}

		public static readonly DependencyProperty VisibleProperty = DependencyProperty.RegisterAttached( "Visible",
			typeof( bool ), typeof( CustomGridView ), new PropertyMetadata( true, new PropertyChangedCallback(ColumnVisibleChanged) ) );

		public static readonly DependencyProperty ColumnNameProperty = DependencyProperty.RegisterAttached( "ColumnName",
			typeof( string ), typeof( GridViewColumn ) );

		/// <summary>
		/// If a Column Visible property changed, any owning CustomGridView must update the column visibility.
		/// </summary>
		/// <param name="d"></param>
		/// <param name="e"></param>
		private static void ColumnVisibleChanged( DependencyObject d, DependencyPropertyChangedEventArgs e ) {

			Console.WriteLine( "COL VISIBLE CHANGING" );

			Action<GridViewColumn, bool> visAction = d.GetValue( VisibleActionProperty ) as Action<GridViewColumn, bool>;
			if( visAction != null ) {
				visAction( d as GridViewColumn, (bool)e.NewValue );
			} else {
				Console.WriteLine( "ACTION NULL" );
			}
			/*CustomGridView owner = d.GetValue( MyGridProperty ) as CustomGridView;
			if( owner != null ) {

				/// show or hide the column.
				if( (bool)e.NewValue == true ) {
					Console.WriteLine( "SHOWING COLUMN" );
					owner.ShowColumn( d as GridViewColumn );
				} else {
					Console.WriteLine( "HIDING COLUMN" );
					owner.HideColumn( d as GridViewColumn );
				}

			}*/


		} //

		#region DEPENDENCY PROPERTY ACCESSORS

		public static Action<GridViewColumn, bool> GetVisibleAction( GridViewColumn c ) {
			return (Action<GridViewColumn, bool>)c.GetValue( VisibleActionProperty );
		}

		public static void SetVisibleAction( GridViewColumn c, Action<GridViewColumn, bool> action ) {
			c.SetValue( VisibleActionProperty, action );
		}

		/*public static void SetMyGrid( GridViewColumn c, object g ) {
			c.SetValue( MyGridProperty, g );
		}
		public static object GetMyGrid( GridViewColumn c ) {
			return c.GetValue( MyGridProperty );
		}*/

		public static void SetVisible( GridViewColumn c, bool b ) {
			Console.WriteLine( "SETTING VISIBILITY" );
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

		/*public class ColumnHiddenEventArgs: EventArgs {

			private string columnName;
			public ColumnHiddenEventArgs( string columnName ) {
				this.columnName = columnName;
			}

		}
		public event EventHandler<ColumnHiddenEventArgs> ColumnHidden;*/


		/// <summary>
		/// Columns cached but not currently displayed.
		/// </summary>
		private List<GridViewColumn> _hiddenColumns;

		public CustomGridView() : base() {

			this.Columns.CollectionChanged += Columns_CollectionChanged;

		}

		private void Columns_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) {
			
			Console.WriteLine( "COLLECITON CHANgED" );

			if( e.OldItems != null ) {

				foreach( GridViewColumn c in e.OldItems ) {

					Console.WriteLine( "looping old items" );
					if(
						(this._hiddenColumns == null || !this._hiddenColumns.Contains( c ) ) ) {
						// if column is in hidden items, owner doesn't change.
						Console.WriteLine( "REMOVING OWNER" );
						c.ClearValue( VisibleActionProperty );
					}

				}

				
			}

			if( e.NewItems != null ) {

				foreach( GridViewColumn c in e.NewItems ) {

					c.SetValue( VisibleActionProperty, new Action<GridViewColumn,bool>(this.UpdateVisibility) );
	 
					if( (bool)c.GetValue( VisibleProperty ) == false ) {
						Console.WriteLine( "hiding hidden column" );
						this.HideColumn( c );
					}


				}

			}


		} //

		#region COLUMN HIDING

		private void UpdateVisibility( GridViewColumn d, bool visible ) {

			Console.WriteLine( "UPDATING COLUMN VISIBILITY" );

			/// show or hide the column.
			if( visible ) {
				this.ShowColumn( d as GridViewColumn );
			} else {
				this.HideColumn( d as GridViewColumn );
			}

		} //

		public void HideColumn( GridViewColumn c ) {
			Console.WriteLine( "HIDE COL" );
			int index = this.Columns.IndexOf( c );
			if( index >= 0 ) {
				this.CacheColumn( index );
			}

		}

		public void ShowColumn( GridViewColumn c ) {

			Console.WriteLine( "SHOW COL" );
			int index = this._hiddenColumns.IndexOf( c );
			if( index >= 0 ) {
				Console.WriteLine( "ShowColumn calling RESTORING COLUMN" );
				this.RestoreColumn( index );
			}

		}

		public void HideColumn( object nameOrHeader ) {
			Console.WriteLine( "HIDE COL" );
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
			Console.WriteLine( "SHOW COL" );
			if( this._hiddenColumns == null ) {
				return;
			}

			int index;
			if( nameOrHeader is string ) {

				index = this.FindColumnIndex( this._hiddenColumns, nameOrHeader as string );

			} else {
				index = this.FindColumnIndex( this._hiddenColumns, nameOrHeader );
			}

			this.RestoreColumn( index );

		} //


		/// <summary>
		/// nameOrHeader can either be a CustomGridView.ColumnName or the Header content
		/// of the GridViewColumn.
		/// </summary>
		/// <param name="nameOrHeader"></param>
		/// <returns></returns>
		private int FindColumnIndex( IList<GridViewColumn> colList, string nameOrHeader ) {

			Console.WriteLine( "FINDING COL INDEX" );
			if( string.IsNullOrEmpty( nameOrHeader ) ) {
				return -1;
			}

			for( int i = colList.Count - 1; i >= 0; i-- ) {

				GridViewColumn c = colList[i];
				if( CustomGridView.GetColumnName( c ) == nameOrHeader ) {

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
		/// nameOrHeader can either be a CustomGridView.ColumnName or the Header content
		/// of the GridViewColumn.
		/// </summary>
		/// <param name="nameOrHeader"></param>
		/// <returns></returns>
		private int FindColumnIndex( IList<GridViewColumn> colList, object header ) {
			Console.WriteLine( "FINDING COL INDEX" );
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

		/// <summary>
		/// Cache a column no longer being displayed.
		/// </summary>
		/// <param name="c"></param>
		private void CacheColumn( int index ) {

			GridViewColumn c = this.Columns[index];
			this.Columns.RemoveAt( index );

			Console.WriteLine( "CACHING COLUMN" );

			if( this._hiddenColumns == null ) {
				this._hiddenColumns = new List<GridViewColumn>();
			}
			this._hiddenColumns.Add( c );

		}

		/// <summary>
		/// Restore a column that was previously hidden.
		/// </summary>
		/// <param name="c"></param>
		private void RestoreColumn( int index ) {

			GridViewColumn c = this._hiddenColumns[index];
			this._hiddenColumns.RemoveAt( index );

			Console.WriteLine( "RESTORING COLUMN" );

			this.Columns.Add( c );

		}

		#endregion


	} // class

} // namespace
