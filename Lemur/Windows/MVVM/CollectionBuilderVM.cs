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
	public class CollectionBuilderVM<T,VM> : ViewModelBase {

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
								this.Instantiate( c );
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
								this._conditionModels.Remove( c );
							} )
							);
				}

			}

			#endregion

			#region PROPERTIES

			private readonly ObservableCollection<VM> _conditionModels = new ObservableCollection<VM>();
			/// <summary>
			/// The models of the Conditions being displayed.
			/// </summary>
			public ObservableCollection<VM> ConditionModels {
				get => _conditionModels;
				/*set {

					if( this.SetProperty( ref this._conditionModels, value ) ) {
					}

				}*/

			} //

			/// <summary>
			/// A placeholder VM that allows selecting from available Conditions.
			/// NOTE: Not currently used.
			/// </summary>
			private readonly TypePickerVM<T> _picker = new TypePickerVM<T>();
			public TypePickerVM<T> Picker {
				get {
					return this._picker;
				}
			}

			private FileMatchOperation operation;

			/// <summary>
			/// The operation being displayed.
			/// </summary>
			public FileMatchOperation Operation {
				get { return this.operation; }
				set {
					if( operation != value ) {
						this.operation = value;
						this.NotifyPropertyChanged();
					}
				}

			}

			#endregion

			/// <summary>
			/// Creates a new FileMatchOperation for the current list of MatchConditions and settings.
			/// The method attempts to clone the MatchConditions so that altering them does not change
			/// the current ViewModel.
			/// 
			/// In order to clone the MatchConditions, the method first checks if the IMatchCondition
			/// implements ICloneable.
			/// If it does not, it checks for a constructor which takes an object of the same type.
			/// 
			/// If all options fail, the MatchCondition itself is returned.
			/// </summary>
			/// <returns></returns>
			public FileMatchOperation Create() {

				int len = this._conditionModels.Count;
				List<T> conditions = new List<T>( len );

				for( int i = 0; i < len; i++ ) {

					VM vm = this._conditionModels[i];
					T cond = vm.Condition;
					if( cond == null ) {
						continue;
					}
					ICloneable clonable = cond as ICloneable;
					if( clonable != null ) {

						conditions.Add( (T)clonable.Clone() );

					} else if( TypeUtils.CanCloneByConstructor( cond.GetType() ) ) {

						object clone = Activator.CreateInstance( cond.GetType(), new object[] { cond } );
						conditions.Add( (T)clone );

					} else {

						conditions.Add( cond );

					}

				} // for-loop.

				FileMatchOperation op = new FileMatchOperation( conditions );

				return op;

			} //

			/// <summary>
			/// Check if there are any non-empty conditions in the current operation build.
			/// </summary>
			/// <returns></returns>
			public bool HasConditions() {
				return this._conditionModels != null && this._conditionModels.Count > 0;
			}

			/// <summary>
			/// Removes a condition from the File Match operation.
			/// </summary>
			/// <param name="cond"></param>
			public void Remove( T cond ) {

				int len = this._conditionModels.Count;
				for( int i = 0; i < len; i++ ) {

					if( this._conditionModels[i].Condition == cond ) {
						this._conditionModels.RemoveAt( i );
						return;
					}

				}

			} //

			public void Remove( VM typeVM ) {
				this._conditionModels.Remove( typeVM );
			}

			public void Clear() {
				this._conditionModels.Clear();
			}

			/// <summary>
			/// Create and add a new Matching Condition of the given type.
			/// </summary>
			/// <param name="conditionType"></param>
			public void Instantiate( Type conditionType ) {

				if( conditionType is null ) {
					throw new ArgumentNullException( "Condition Type cannot be null." );
				}
				if( !typeof( T ).IsAssignableFrom( conditionType ) ) {
					throw new ArgumentException( "Condition Type must be a subclass of FileMatching.BaseCondition." );
				}

				T condition = (T)Activator.CreateInstance( conditionType );

				Console.WriteLine( "creating type: " + conditionType.Name );

				VM vm = new FileTestVM( condition );
				this.ConditionModels.Add( vm );

			}

			/// <summary>
			/// Add a match requirement to the match operation.
			/// </summary>
			/// <param name="cond"></param>
			public void Add( T cond ) {

				VM vm = new FileTestVM( cond );
				this.ConditionModels.Add( vm );

			} //

			public CollectionBuilderVM() {

				//this._picker = new ConditionPickerVM();
				//Console.WriteLine( "SETTING CREATE CALLBACK" );
				this._picker.CreateRequested += this.picker_CreateRequested;

			}

			private void picker_CreateRequested( Type createType ) {

				//Console.WriteLine( "Instantiating: " + createType.Name );
				this.Instantiate( createType );

			} //

		} // class

} // namespace