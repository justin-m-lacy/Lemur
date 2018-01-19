using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lemur.Windows.Controls {

	/// <summary>
	/// A more complicated GridView that allows showing and hiding columns by header or the ColumnName attached property.
	/// </summary>
	public class CustomGridView : GridView {

		public static readonly DependencyProperty ColumnNameProperty =
			DependencyProperty.RegisterAttached( "ColumnName",
			typeof( string ), typeof( GridViewColumn ) );

		public static void SetColumnName( GridViewColumn e, string name ) {
			e.SetValue( ColumnNameProperty, name );
		}

		public static string GetColumnName( GridViewColumn e ) {
			return (string)e.GetValue( ColumnNameProperty );
		}

		public class ColumnRemovedEventArgs : EventArgs {

			private string columnName;
			public ColumnRemovedEventArgs( string columnName ) {
				this.columnName = columnName;
			}

		}
		public event EventHandler<ColumnRemovedEventArgs> ColumnRemoved;


		/// <summary>
		/// Columns cached but not currently displayed.
		/// </summary>
		private List<GridViewColumn> _hiddenColumns;

		public CustomGridView() {

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

			this.RestoreColumn( index );

		} //


		/// <summary>
		/// nameOrHeader can either be a CustomGridView.ColumnName or the Header content
		/// of the GridViewColumn.
		/// </summary>
		/// <param name="nameOrHeader"></param>
		/// <returns></returns>
		private int FindColumnIndex( IList<GridViewColumn> columns, string nameOrHeader ) {

			if( string.IsNullOrEmpty( nameOrHeader ) ) {
				return -1;
			}

			for( int i = columns.Count - 1; i >= 0; i-- ) {

				GridViewColumn c = columns[i];
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
		private int FindColumnIndex( IList<GridViewColumn> columns, object header ) {

			if( header == null ) {
				return -1;
			}

			for( int i = columns.Count - 1; i >= 0; i-- ) {

				GridViewColumn c = columns[i];
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

    } // class

} // namespace
