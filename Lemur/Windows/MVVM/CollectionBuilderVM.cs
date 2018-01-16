using System;
using System.Collections.Generic;
using System.Text;
using Lemur.Windows;
using Lemur.Windows.MVVM;
using System.Linq;
using System.Collections.ObjectModel;
using System.Reflection;
using Lemur.Utils;
using System.Collections.Specialized;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// ViewModel for constructing a collection of objects all derived
	/// from a single base type.
	/// </summary>
	/// <typeparam name="T">
	/// The base type or interface for all objects in the collection.
	/// NOTE: The derived types must all contain a parameterless constructor so they can be
	/// instantiated.
	/// </typeparam>
	/// <typeparam name="VM">The ViewModel to use for each created object.</typeparam>
	public class CollectionBuilderVM<T, VM> : ViewModelBase
		where VM : DataObjectVM, new() {

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

		private RelayCommand<VM> _cmdRemove;
		/// <summary>
		/// Remove the Matching Condition specified.
		/// </summary>
		public RelayCommand<VM> CmdRemove {

			get {
				return this._cmdRemove ??
					( this._cmdRemove = new RelayCommand<VM>(
						( c ) => {
							this._viewModels.Remove( c );
						} )
					);
			}

		}

		#endregion

		#region PROPERTIES

		private readonly ObservableCollection<VM> _viewModels = new ObservableCollection<VM>();
		/// <summary>
		/// The models displaying the current collection of instantiated items.
		/// </summary>
		public ObservableCollection<VM> DataModels {
			get => _viewModels;
		} //

		/// <summary>
		/// If SourceCollection is set, ViewModels added to the DataModels list will have their
		/// corresponding Data items added to the SyncCollection.
		/// However, items added to the SourceCollection will not be automatically
		/// reflected in the DataModels collection without an explicit SyncSource() call.
		/// 
		/// Setting the SourceCollection to a non-null source will clear all items in DataModels
		/// and create new DataModels to reflect the new Collection
		/// </summary>
		public ICollection<T> SourceCollection {
			get => this._sourceCollection;
			set {

				bool prevValNull = ( this._sourceCollection == null );
				if( !prevValNull ) {
					// Disable DataModel collection updates while the items are added
					// to prevent circular Adds to the Source.
					this.DataModels.CollectionChanged -= this.DataModels_CollectionChanged;
				}

				if( this.SetProperty( ref this._sourceCollection, value ) ) {

					if( value != null ) {

						this.SetItems( value );
						this.DataModels.CollectionChanged += DataModels_CollectionChanged;
					}

				}

			}

		} // SourceCollection
		private ICollection<T> _sourceCollection;

		private void DataModels_CollectionChanged( object sender, NotifyCollectionChangedEventArgs e ) {

			// Reflect any changes in the SourceCollection.
			if( e.OldItems != null ) {

				foreach( VM vm in e.OldItems ) {
					this._sourceCollection.Remove( (T)vm.Data );
				}

			}
			if( e.NewItems != null ) {

				foreach( VM vm in e.NewItems ) {
					this._sourceCollection.Add( (T)vm.Data );
				}

			}

		}

		

		/// <summary>
		/// A placeholder VM that allows selecting from the available Types
		/// in order to instantiate one.
		/// </summary>
		private readonly TypePickerVM<T> _picker = new TypePickerVM<T>( true );

		public TypePickerVM<T> Picker {
			get {
				return this._picker;
			}
		}

		#endregion

		public CollectionBuilderVM() {

			this._picker.CreateRequested += this.InstantiateType;
			this.OnProviderChanged += SetDefaultCreators;

		}

		public CollectionBuilderVM( IServiceProvider provider ) : base( provider ) {

			this.SetDefaultCreators();
			this._picker.CreateRequested += this.InstantiateType;

		}

		virtual protected void SetDefaultCreators() {

			ViewModelBuilder builder = this.GetService<ViewModelBuilder>();

			/// look for a custom creator for the data type.
			if( builder != null ) {
				// associate the base type with a default ViewModel creator.
				builder.SetCreator<T>( this.CreateVM );
			} //

		} //

		/// <summary>
		/// Check if there are any non-empty conditions in the current operation build.
		/// </summary>
		/// <returns></returns>
		public bool HasItems() {
			return this._viewModels != null && this._viewModels.Count > 0;
		}

		public void SetItems( IEnumerable<T> items ) {

			this._viewModels.Clear();
			foreach( T item in items ) {

				this.Add( item );

			} // for

		}

		/// <summary>
		/// If a SourceCollection has been set, ensures that all items in the SourceCollection
		/// are reflected in the DataModels collection.
		/// The Sync is from Source to DataModels, since items in the DataModels collection
		/// are already automatically reflected in the SourceCollection.
		/// If the SourceCollection is null, no changes occur.
		/// </summary>
		public void SyncSource() {

			if( this.SourceCollection != null ) {

				/// Turn off CollectionChanged events to prevent circular adding.
				this.DataModels.CollectionChanged -= this.DataModels_CollectionChanged;

				foreach( T item in this.SourceCollection ) {

					if( this.DataModels.Count(
						( vm ) => EqualityComparer<T>.Default.Equals( item, (T)vm.Data )
						) == 0 ) {

						this.Add( item );
					}
	
				}

				this.DataModels.CollectionChanged += this.DataModels_CollectionChanged;

			}

		} //

		/// <summary>
		/// Add a match requirement to the match operation.
		/// </summary>
		/// <param name="item"></param>
		public void Add( T item ) {

			ViewModelBuilder builder = this.GetService<ViewModelBuilder>();

			/// look for a custom creator for the data type.
			if( builder != null ) {

				Console.WriteLine( "Item type: " + item.GetType().Name );

				var creator = builder.GetCreator( item );
				if( creator != null ) {
					VM model = (VM)creator( item );
					this.DataModels.Add( model );
					return;
				}

			}

			VM vm = new VM();
			vm.Data = item;
			this.DataModels.Add( vm );

		} //

		private VM CreateVM( object data, object view=null ) {

			VM vm = new VM();
			vm.Data = data;
			return vm;

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
		public void Remove( VM typeVM ) {
			this._viewModels.Remove( typeVM );
		}

		public void Clear() {
			this._viewModels.Clear();
		}

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
			this.Add( instance );

		}

		/// <summary>
		/// Returns a collection of items currently displayed in the view models.
		/// Unless the items are Value Types, these are references to the actual
		/// items displayed, and altering them may change the collection itself.
		/// </summary>
		/// <returns></returns>
		public List<T> GetCollection() {

			int count = this._viewModels.Count;
			List<T> items = new List<T>( count );

			for( int i = 0; i < count; i++ ) {

				T data = (T)this._viewModels[i].Data;
				if( data != null ) {
					items.Add( data );
				}

			} //

			return items;

		}

		/// <summary>
		/// Attempts to clone the displayed list of data items into a new list
		/// and return it.
		/// 
		/// For each Data item in the collection the following cloning methods are attempted:
		/// 1) If the item implements ICloneable, ICloneable.Clone() is called to clone the object.
		/// 2) The item Type is checked for a constructor that can be initialized with by an instance
		/// of the same type.
		/// 3) A Deep Clone is attempted using the DataUtils.DeepClone method.
		/// 
		/// If cloning fails, a direct reference to the item in the collection is made.
		/// 
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

	}// class

} // namespace