using Lemur.Windows;
using Lemur.Windows.MVVM;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Reflection;
using Lemur.Utils;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// ViewModel for constructing FileMatch operations.
	/// </summary>
	public class FileMatchBuilderVM : ViewModelBase {

		#region COMMANDS

		private RelayCommand<Type> _cmdAddCondition;
		/// <summary>
		/// Add a given MatchCondition of the given type.
		/// </summary>
		public RelayCommand<Type> CmdAddCondition {

			get {
				return this._cmdAddCondition ??
					( this._cmdAddCondition = new RelayCommand<Type>(
						( c ) => {
							Console.WriteLine( "Attempting to add condition: " + c.Name );
							this.CreateCondition( c );
						} )
						);
			}

		} // CmdAddCondition

		private RelayCommand<FileTestVM> _cmdRemoveCondition;
		/// <summary>
		/// Remove the Matching Condition specified.
		/// </summary>
		public RelayCommand<FileTestVM> CmdRemoveCondition {

			get {
				return this._cmdRemoveCondition ??
					( this._cmdRemoveCondition = new RelayCommand<FileTestVM>(
						( c ) => {
							Console.WriteLine( "Attempting to remove condition: " + c.DisplayName );
							this._conditionModels.Remove( c );
						} )
						);
			}

		}

		#endregion

		#region PROPERTIES

		private readonly ObservableCollection<FileTestVM> _conditionModels = new ObservableCollection<FileTestVM>();
		/// <summary>
		/// The models of the Conditions being displayed.
		/// </summary>
		public ObservableCollection<FileTestVM> ConditionModels {
			get => _conditionModels;
			/*set {

				if( this.SetProperty( ref this._conditionModels, value ) ) {
				}

			}*/

		} //

		private FileMatchSettings _settings;
		/// <summary>
		/// Settings for the matching operation.
		/// </summary>
		public FileMatchSettings Settings {
			get {
				return this._settings;
			}
			set {
				this.SetProperty( ref this._settings, value );
			}
		}

		/// <summary>
		/// A placeholder VM that allows selecting from available Conditions.
		/// NOTE: Not currently used.
		/// </summary>
		private readonly ConditionPickerVM _picker = new ConditionPickerVM();
		public ConditionPickerVM ConditionPicker {
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
			List<IMatchCondition> conditions = new List<IMatchCondition>( len );

			for( int i = 0; i < len; i++ ) {

				FileTestVM vm = this._conditionModels[i];
				IMatchCondition cond = vm.MatchCondition;
				if( cond == null ) {
					continue;
				}
				ICloneable clonable = cond as ICloneable;
				if( clonable != null ) {

					conditions.Add( (IMatchCondition)clonable.Clone() );

				} else if( TypeUtils.CanCloneByConstructor( cond.GetType() ) ) {

					object clone = Activator.CreateInstance( cond.GetType(), new object[] { cond } );
					conditions.Add( (IMatchCondition)clone );

				} else {

					conditions.Add( cond );

				}

			} // for-loop.

			FileMatchOperation op = new FileMatchOperation( conditions, new FileMatchSettings( this.Settings ) );

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
		public void RemoveCondition( BaseCondition cond ) {

			int len = this._conditionModels.Count;
			for( int i = 0; i < len; i++ ) {

				if( this._conditionModels[i].MatchCondition == cond ) {
					this._conditionModels.RemoveAt( i );
					return;
				}

			}

		} //

		public void RemoveCondition( FileTestVM conditionView ) {
			this._conditionModels.Remove( conditionView );
		}

		public void Clear() {
			this._conditionModels.Clear();
		}

		/// <summary>
		/// Create and add a new Matching Condition of the given type.
		/// </summary>
		/// <param name="conditionType"></param>
		public void CreateCondition( Type conditionType ) {

			if( conditionType is null ) {
				throw new ArgumentNullException( "Condition Type cannot be null." );
			}
			if( !typeof( IMatchCondition ).IsAssignableFrom( conditionType ) ) {
				throw new ArgumentException( "Condition Type must be a subclass of FileMatching.BaseCondition." );
			}

			BaseCondition condition = (BaseCondition)Activator.CreateInstance( conditionType );

			FileTestVM vm = new FileTestVM( condition );
			this.ConditionModels.Add( vm );

		}

		/// <summary>
		/// Add a match requirement to the match operation.
		/// </summary>
		/// <param name="cond"></param>
		public void AddCondition( IMatchCondition cond ) {

			FileTestVM vm = new FileTestVM( cond );
			this.ConditionModels.Add( vm );

		} //

		public FileMatchBuilderVM() {

			//this._picker = new ConditionPickerVM();
			//Console.WriteLine( "SETTING CREATE CALLBACK" );
			this._picker.CreateRequested += this.picker_CreateRequested;

		}

		private void picker_CreateRequested( Type conditionType ) {

			//Console.WriteLine( "Creating condition: " + conditionType.Name );
			this.CreateCondition( conditionType );
		} //

	} // class

} // namespace
