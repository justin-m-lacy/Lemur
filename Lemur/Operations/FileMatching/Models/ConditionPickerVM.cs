using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Lemur.Windows;
using Lemur.Types;
using System.Linq;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// Placeholder before a specific condition is selected. A list of all available
	/// FileMatchConditions must be provided.
	/// 
	/// These controls are made a separate viewmodel so they can apppear in a list
	/// of existing Match Conditions as an item to be modified and added.
	/// </summary>
	public class ConditionPickerVM : ViewModelBase {

		#region COMMANDS

		private RelayCommand _cmdCreate;
		/// <summary>
		/// Command to create a MatchCondition of the currently selected type.
		/// </summary>
		public RelayCommand CmdCreateCondition {
			get {
				return this._cmdCreate ?? ( this._cmdCreate = new RelayCommand( this.DispatchCreate ) );
			}
		}


		private RelayCommand<TypeDescription> _cmdPickType;
		/// <summary>
		/// Type of condition picked.
		/// </summary>
		public RelayCommand<TypeDescription> CmdPickType {

			get {
				return this._cmdPickType ??
					( this._cmdPickType = new RelayCommand<TypeDescription>(
						( t ) => this.CreateType = t )
					);
			}

		}


		#endregion

		#region PROPERTIES

		/// <summary>
		/// Descriptions of available condition types.
		/// </summary>
		private static List< TypeDescription > _staticConditionTypes;

		private List<TypeDescription> _conditionTypes;
		/// <summary>
		/// Descriptions of available condition types.
		/// </summary>
		public List<TypeDescription> ConditionTypes {

			get {

				if( this._conditionTypes == null ) {

					if( ConditionPickerVM._staticConditionTypes == null ) {
						ConditionPickerVM._staticConditionTypes = BuildTypeDescs();
					}
					this.ConditionTypes = ConditionPickerVM._staticConditionTypes;
				}
				return this._conditionTypes;

			}
			set {
				if( this.SetProperty( ref this._conditionTypes, value ) ) {
					this.RefreshExclusions();
				}
			}

		}

		/// <summary>
		/// Condition type that will be created on the Create Command.
		/// </summary>
		private TypeDescription _createType;
		public TypeDescription CreateType {
			get {
				return this._createType;
			}
			set {

				if( !typeof( IMatchCondition ).IsAssignableFrom( value.Type ) ) {
					throw new ArgumentException( "Assigned value must implement IMatchCondition" );
				}
				this.SetProperty( ref this._createType, value );

			}
		}

		/// <summary>
		/// Types to exclude from being picked for new match conditions.
		/// </summary>
		private Type[] excludeTypes =
			new[] { typeof( DirectoryCondition ), typeof( BaseCondition ), typeof(ConditionEnumeration),
				typeof( ConditionList ) };

		public Type[] ExcludeTypes {

			get { return this.excludeTypes; }
			set {
				if( SetProperty( ref this.excludeTypes, value ) ) {
					/// search current list for excluded types.
					this.RefreshExclusions();
				}
			} //set

		}

		#endregion


		/// <summary>
		/// Event triggers when a condition has been selected.
		/// </summary>
		public event Action<Type> CreateRequested;
		protected void DispatchCreate() {

			//Console.WriteLine( "DISPATCHING CREATE" );
			this.CreateRequested?.Invoke( this._createType.Type );

		} //


		public ConditionPickerVM() { }

		private void RefreshExclusions() {

			if( this._conditionTypes == null || this.excludeTypes == null ) {
				return;
			}

			int len = this._conditionTypes.Count;
			if( len == 0 || this.excludeTypes.Length == 0 ) {
				return;
			}

			for( int i = this._conditionTypes.Count - 1; i >= 0; i-- ) {

				TypeDescription desc = this._conditionTypes[i];
				if( this.excludeTypes.Contains( desc.Type ) ) {
					this._conditionTypes.RemoveAt( i );
				}

			} // for-loop.


		} // RefreshExclusions()

		static private List<TypeDescription> BuildTypeDescs() {

			List<TypeDescription> descs = new List<TypeDescription>();
			List<Type> types = FindConditionTypes();

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

		static public List<Type> FindConditionTypes() {

			Type[] available = Assembly.GetCallingAssembly().GetTypes();

			Type conditionType = typeof( BaseCondition );
			List<Type> results = new List<Type>();

			foreach( Type t in available ) {

				if( conditionType.IsAssignableFrom(t) ) {
					results.Add( t );
				}

			} // foreach

			return results;

		}

    } // class

} // namespace
