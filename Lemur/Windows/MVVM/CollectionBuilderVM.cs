using System;
using System.Collections.Generic;
using System.Text;
using Lemur.Windows;
using Lemur.Windows.MVVM;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using Lemur.Utils;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// ViewModel for constructing a collection of objects all derived
	/// from a single base type.
	/// </summary>
	public class CollectionBuilderVM<T> : ViewModelBase {

		#region COMMANDS

		private RelayCommand<Type> _cmdAdd;
		/// <summary>
		/// Add a given MatchCondition of the given type.
		/// </summary>
		public RelayCommand<Type> CmdAdd {

			get {
				return this._cmdAdd ??
					( this._cmdAdd = new RelayCommand<Type>(
						( c ) => {
							Console.WriteLine( "Attempting to add condition: " + c.Name );
							this.InstantiateType( c );
						} )
					);
			}

		} // CmdAddCondition

		private RelayCommand<DataObjectVM> _cmdRemove;
		/// <summary>
		/// Remove the Matching Condition specified.
		/// </summary>
		public RelayCommand<DataObjectVM> CmdRemove {

			get {
				return this._cmdRemove ??
					( this._cmdRemove = new RelayCommand<DataObjectVM>(
						( c ) => {
							this._viewModels.Remove( c );
						} )
					);
			}

		}

		#endregion

		#region PROPERTIES

		private readonly ObservableCollection<DataObjectVM> _viewModels = new ObservableCollection<DataObjectVM>();
		/// <summary>
		/// The models displaying the current collection of instantiated items.
		/// </summary>
		public ObservableCollection<DataObjectVM> DataModels {
			get => _viewModels;
		} //

		/// <summary>
		/// A placeholder VM that allows selecting from the available Types
		/// in order to instantiate one.
		/// </summary>
		private readonly TypePickerVM<T> _picker = new TypePickerVM<T>();
		public TypePickerVM<T> Picker {
			get {
				return this._picker;
			}
		}

		#endregion

		/// <summary>
		/// Check if there are any non-empty conditions in the current operation build.
		/// </summary>
		/// <returns></returns>
		public bool HasItems() {
			return this._viewModels != null && this._viewModels.Count > 0;
		}

		/// <summary>
		/// Add a match requirement to the match operation.
		/// </summary>
		/// <param name="item"></param>
		public void Add( T item ) {

			DataObjectVM vm = new DataObjectVM( item );
			this.DataModels.Add( vm );

		} //

		  /// <summary>
		  /// Removes a data item from the Collection.
		  /// </summary>
		  /// <param name="data"></param>
		public void Remove( T data ) {

			int len = this._viewModels.Count;
			for( int i = 0; i < len; i++ ) {

				if( this._viewModels[i].Data.Equals( data ) ) {
					this._viewModels.RemoveAt( i );
					return;
				}

			}

		} //

		/// <summary>
		/// Removes the view model and its associated data item
		/// from the collection.
		/// </summary>
		/// <param name="typeVM"></param>
		public void Remove( DataObjectVM typeVM ) {
			this._viewModels.Remove( typeVM );
		}

		public void Clear() {
			this._viewModels.Clear();
		}

		/// <summary>
		/// Attempts to clone the displayed list of data items into a new list
		/// and return it.
		/// Objects that cannot be cloned will be included in the list as direct references.
		/// </summary>
		/// <returns></returns>
		public List<T> CloneCollection() {

			int len = this._viewModels.Count;
			List<T> items = new List<T>( len );

			for( int i = 0; i < len; i++ ) {

				DataObjectVM vm = this._viewModels[i];
				T src = (T)vm.Data;
				if( src == null ) {
					continue;
				}
				ICloneable clonable = src as ICloneable;
				if( clonable != null ) {

					items.Add( (T)clonable.Clone() );

				} else if( TypeUtils.CanCloneByConstructor( src.GetType() ) ) {

					object clone = Activator.CreateInstance( src.GetType(), new object[] { src } );
					items.Add( (T)clone );

				} else {

					try {

						T clone = DataUtils.DeepClone( src );
						items.Add( clone );
	
					} catch( Exception ) {
						items.Add( src );
					}

				}

			} // for-loop.

			return items;

		} // CloneCollection()

		/// <summary>
		/// Create and add a new Matching Condition of the given type.
		/// </summary>
		/// <param name="dataType"></param>
		public void InstantiateType( Type dataType ) {

			Console.WriteLine( "Instantiating: " + dataType.Name );
			if( dataType is null ) {
				throw new ArgumentNullException( "Type cannot be null." );
			}
			if( !typeof( T ).IsAssignableFrom( dataType ) ) {
				throw new ArgumentException( "Type must be a subclass of " + typeof( T ).Name );
			}

			T instance = (T)Activator.CreateInstance( dataType );

			Console.WriteLine( "creating type: " + dataType.Name );

			DataObjectVM vm = new DataObjectVM( instance );
			this.DataModels.Add( vm );

		}

		public CollectionBuilderVM() {
			this._picker.CreateRequested += this.InstantiateType;
		}

	}// class

} // namespace