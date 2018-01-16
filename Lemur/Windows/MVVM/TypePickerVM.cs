using System;
using System.Collections.Generic;
using System.Text;
using Lemur.Windows.MVVM;
using System.Reflection;
using Lemur.Windows;
using Lemur.Types;
using System.Linq;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// ViewModel for selecting and instantiating subtypes of a base type T.
	/// All types must have a constructor with no arguments.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class TypePickerVM<T> : ViewModelBase {

		#region COMMANDS

		/// <summary>
		/// Command to request the instantiation of a given Type.
		/// </summary>
		public RelayCommand CmdInstantiate {
			get {
				return this._cmdInstantiate ?? ( this._cmdInstantiate = new RelayCommand( this.DispatchCreate, this.CreateTypeSelected ) );
			}
		}
		private RelayCommand _cmdInstantiate;

		private RelayCommand<TypeDescription> _cmdPickType;
		/// <summary>
		/// Type of condition picked.
		/// </summary>
		public RelayCommand<TypeDescription> CmdPickType {

			get {
				return this._cmdPickType ??
					( this._cmdPickType = new RelayCommand<TypeDescription>(

						( t ) => {
							this.CreateType = t; this._cmdInstantiate.RaiseCanExecuteChanged(); }  )

					);
			}

		}


		#endregion

		#region PROPERTIES

		/// <summary>
		/// Descriptions of available Types derived from the base type.
		/// </summary>
		private static List<TypeDescription> _staticTypes;

		private List<TypeDescription> _availableTypes;
		/// <summary>
		/// Descriptions of available condition types.
		/// </summary>
		public List<TypeDescription> AvailableTypes {

			get {

				if( this._availableTypes == null ) {

					if( TypePickerVM<T>._staticTypes == null ) {
						TypePickerVM<T>._staticTypes = BuildTypeDescs();
					}
					this.AvailableTypes = TypePickerVM<T>._staticTypes;
				}
				return this._availableTypes;

			}
			set {
				if( this.SetProperty( ref this._availableTypes, value ) ) {
					this.RefreshExclusions();
				}
			}

		}

		/// <summary>
		/// Type that will be instantiated on the next Create Command.
		/// </summary>
		private TypeDescription _createType;
		public TypeDescription CreateType {
			get {
				return this._createType;
			}
			set {
				Console.WriteLine( "ATTEMPTING TO SET NEW VALUE" );
				if( value == null ) {
					this._createType = null;
				} else if ( !typeof( T ).IsAssignableFrom( value.Type ) ) {
					throw new ArgumentException( "Assigned value must be of type: " + typeof( T ).Name );
				}

				if( this.SetProperty( ref this._createType, value ) ) {

					this.CmdInstantiate.RaiseCanExecuteChanged();
				}
				/// Check even if property hasn't changed. This allows repeating the same selection.
				/// NOTE: ComboBox's will not allow repeat selection of the same item by default,
				/// you have to override PreviewMouseDown code to unset the selected item manually.
				if( this.CreateOnSelect ) {
					this.CmdInstantiate.Execute( null );
				}

			}

		}

		/// <summary>
		/// Types to exclude from selection.
		/// </summary>
		public Type[] ExcludeTypes {

			get { return this.excludeTypes; }
			set {
				if( SetProperty( ref this.excludeTypes, value ) ) {
					/// search current list for excluded types.
					this.RefreshExclusions();
				}
			} //set

		}

		public bool CreateOnSelect {
			get => _createOnSelect;
			set => this.SetProperty( ref this._createOnSelect, value );
		}
		private bool _createOnSelect;

		private Type[] excludeTypes;

		#endregion

		public bool CreateTypeSelected() => this.CreateType != null && CreateType.Type != null;

		/// <summary>
		/// Event triggers when a specific type has been selected.
		/// </summary>
		public event Action<Type> CreateRequested;
		protected void DispatchCreate() {

			Console.WriteLine( "CREATE CALLED" );
			if( CreateTypeSelected() ) {
				this.CreateRequested?.Invoke( this._createType.Type );
			}

		} //


		public TypePickerVM( bool createOnSelect=false ) {
			this.CreateOnSelect = createOnSelect;
		}

		private void RefreshExclusions() {

			if( this._availableTypes == null || this.excludeTypes == null ) {
				return;
			}

			int len = this._availableTypes.Count;
			if( len == 0 || this.excludeTypes.Length == 0 ) {
				return;
			}

			for( int i = this._availableTypes.Count - 1; i >= 0; i-- ) {

				TypeDescription desc = this._availableTypes[i];
				if( this.excludeTypes.Contains( desc.Type ) ) {
					this._availableTypes.RemoveAt( i );
				}

			} // for-loop.


		} // RefreshExclusions()

		static private List<TypeDescription> BuildTypeDescs() {

			List<TypeDescription> descs = new List<TypeDescription>();
			List<Type> types = FindDerivedTypes();

			foreach( Type t in types ) {

				NameDescAttribute nameDesc = t.GetCustomAttribute<NameDescAttribute>();
				if( nameDesc != null ) {

					descs.Add( new TypeDescription( t, nameDesc.Name, nameDesc.Desc ) );

				} else {

					descs.Add( new TypeDescription( t, t.Name ) );

				}

			} //

			return descs;

		}

		static public List<Type> FindDerivedTypes() {

			Type[] available = Assembly.GetCallingAssembly().GetTypes();

			Type baseType = typeof( T );
			List<Type> results = new List<Type>();

			foreach( Type t in available ) {

				if( t.IsAbstract || t.IsInterface ) {
					continue;
				}
				if( baseType.IsAssignableFrom( t ) ) {
					results.Add( t );
				}

			} // foreach

			return results;

		}

	} // class

} // namespace
