﻿using System;
using System.Collections.Generic;
using System.Text;
using Lemur.Windows.MVVM;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// ViewModel for a simple DataObject whose display is primarily template-based.
	/// CURRENTLY UNUSED.
	/// </summary>
	public class DataObjectVM<T> : ViewModelLite {

		#region PROPERTIES

		#region DISPLAY TEXT

		/// <summary>
		/// Name of the data item being displayed to the user.
		/// </summary>
		public string DisplayName { get => _displayName; set => this.SetProperty( ref this._displayName, value ); }
		private string _displayName;

		/// <summary>
		/// Description of the data item.
		/// </summary>
		public string Desc { get => _desc; set => this.SetProperty( ref this._desc, value ); }
		private string _desc;

		#endregion DISPLAY TEXT

		/// <summary>
		/// The underlying condition being displayed.
		/// </summary>
		public T Data {
			get { return _data; }
			set {

				if( this.SetProperty( ref this._data, value ) ) {

					if( this._data != null ) {

						this.DataType = value.GetType();

					} else {
						this.DataType = null;
					}
					// update all property indexers on the condition.
					this.NotifyPropertyChanged( "Property[]" );

				}

			} //set

		}
		private T _data;

		/// <summary>
		/// Type of the data being displayed
		/// Use in xaml to change the display Template based on Type.
		/// </summary>
		public Type DataType {
			get {
				return this._dataType;
			}
			set {
				this.SetProperty( ref this._dataType, value );
			}
		}
		private Type _dataType;

		/// <summary>
		/// Gives access to properties of the underlying condition.
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		[IndexerName( "Property" )]
		public object this[string propName] {

			get {

				if( this._dataType == null ) {
					return null;
				}

				TypeInfo info = this._dataType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return null;
				}
				return prop.GetValue( this._data );

			}
			set {

				if( this._dataType == null ) {
					return;
				}

				TypeInfo info = this._dataType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return;
				}

				// check if property has changed.
				object current = prop.GetValue( this._data );
				if( !Object.Equals( current, value ) ) {

					prop.SetValue( this._data, value );
					/// unfortunately refreshes ALL indexers on Property.
					this.NotifyPropertyChanged( "Property[]" );
				}

			} // set

		}

		#endregion

		public DataObjectVM() { }

		public DataObjectVM( T data ) {
			this.Data = data;
		}

	} // class

} // namespace