using System;
using System.Collections.Generic;
using System.Text;
using Lemur.Windows.MVVM;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// ViewModel for a simple DataObject whose display is primarily template-based.
	/// </summary>
	public class DataObjectVM : ViewModelBase {

		#region PROPERTIES

		#region DISPLAY TEXT

		/// <summary>
		/// Name of the data item being displayed.
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
		/// The underlying object being displayed.
		/// </summary>
		public object Data {
			get { return _data; }
			set {

				if( this.SetProperty( ref this._data, value ) ) {

					if( this._data != null ) {

						this.DataType = value.GetType();

					} else {
						this.DataType = null;
					}
					// update all property indexers on the condition.
					this.NotifyAllPropertiesChanged();

				}

			} //set

		}
		private object _data;

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
				Console.WriteLine( "GETTING PROPERTY" );
				if( this._dataType == null ) {
					return null;
				}

				TypeInfo info = this._dataType.GetTypeInfo();
				/// NOTE: BindingFlags are necessary for inclusion, not meant for implicit exclusion.
				PropertyInfo prop = info.GetProperty( propName,
					BindingFlags.Instance | BindingFlags.Public | BindingFlags.IgnoreCase );
				if( prop == null ) {
					Console.WriteLine( @"ERROR: Property '" + propName + @"' does not exist" );
					return null;
				}
				return prop.GetValue( this._data );

			}
			set {

				if( this._dataType == null ) {
					return;
				}

				TypeInfo info = this._dataType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName,
					BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase );
				if( prop == null ) {
					Console.WriteLine( @"ERROR: Property '" + propName + @"' does not exist" );
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

		#region PROPERTY CACHING

		public bool HasPropertyCache {
			get {
				return this._propCache != null;
			}
			set {
				if( value == true ) {
					this._propCache = new Dictionary<string, object>();
				} else {

					if( this._propCache != null ) {
						this._propCache.Clear();
						this._propCache = null;
					}

				}

			}

		} //

		/// <summary>
		/// Caches the property value, overriding any existing value.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void CacheProp( string prop, object value ) {
			this._propCache[prop] = value;
		}

		/// <summary>
		/// Caches the property value, overriding any existing value.
		/// </summary>
		/// <param name="prop"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected void CacheProp<T>( string prop, T value ) {
			this._propCache[prop] = value;
		}

		/// <summary>
		/// Optional Property cache for properties that are costly to retrieve or compute from the source.
		/// </summary>
		private Dictionary<string, object> _propCache;
		protected bool TryGetCache<T>( string prop, out T value ) {

			object readVal;
			if( this._propCache.TryGetValue( prop, out readVal ) ) {

				/// TODO: More options to avoid costly cast.
				if( readVal is T ) {
					value = (T)readVal;
					return true;
				}

			}

			value = default( T );
			return false;
		}

		/// <summary>
		/// Clears values in the cache without deleting the cache dictionary.
		/// </summary>
		protected void ClearCache() {

			if( this._propCache != null ) {
				this._propCache.Clear();
			}

		}

		#endregion

		public DataObjectVM() { }

		public DataObjectVM( object data ) {
			this.Data = data;
		}

		/// <summary>
		/// Creates a DataObject ViewModel with an option to use a backing property cache.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="usePropCache"></param>
		public DataObjectVM( object data, bool usePropCache ) : this( data ) {

			this.HasPropertyCache = usePropCache;

		}

	} // class

} // namespace