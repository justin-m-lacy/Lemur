using Lemur.Windows;
using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// ViewModel for constructing FileMatch operations.
	/// </summary>
	public class FileMatchBuilderVM : ViewModelBase {


		#region COMMANDS

		private RelayCommand _cmdAddCondition;

		/// <summary>
		/// Command to create a new Match Condition.
		/// </summary>
		/*public RelayCommand CmdAddCondition {

			get {
				return this._cmdAddCondition ??
					( this._cmdAddCondition = new RelayCommand( this.CreateNewTest ) );
			}

		}*/

		private RelayCommand<BaseCondition> _cmdRemoveCondition;
		/// <summary>
		/// Remove the Matching Condition specified.
		/// </summary>
		public RelayCommand<BaseCondition> CmdRemoveCondition {

			get {
				return this._cmdRemoveCondition ??
					( this._cmdRemoveCondition = new RelayCommand<BaseCondition>(
						( c ) => { this._matchConditions.Remove( c ); } )
						);
			}

		}

		/// <summary>
		/// Command to save the operation as a permanent object.
		/// </summary>
		private RelayCommand _cmdSaveOperation;
		/// <summary>
		/// TODO: Doesn't really belong in this model?
		/// </summary>
		public RelayCommand CmdSaveOperation {
			get {
				return this._cmdSaveOperation ?? ( this._cmdSaveOperation = new RelayCommand(
					this.DispatchRequestSave, this.HasConditions ) );
			}
		}

		#endregion

		#region PROPERTIES

		private ObservableCollection<BaseConditionVM> _conditionModels;
		public ObservableCollection<BaseConditionVM> ConditionModels {
			get => _conditionModels;
			set {

				if( this.SetProperty( ref this._conditionModels, value ) ) {
					//this._conditionModels.CollectionChanged += _conditionModels_CollectionChanged;
				}

			}

		}

		private void _conditionModels_CollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e ) {
		} //


		/// <summary>
		/// Event triggers when match conditions should be saved for
		/// future use.
		/// </summary>
		public event EventHandler OnRequestSave;
		private void DispatchRequestSave() {
			this.OnRequestSave?.Invoke( this, new EventArgs() );
		}

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
		/// </summary>
		private PlaceholderVM _placeholder;
		public PlaceholderVM Placeholder {
			get {
				return this._placeholder;
			}
			set {
				this.SetProperty( ref this._placeholder, value );
			}
		}

		/// <summary>
		/// Dispatched when the user selects to save the Match Condition list.
		/// </summary>
		//public event Action<FileMatchVM> OnRequestSave;

		private ObservableCollection<BaseCondition> _matchConditions;
		public ObservableCollection<BaseCondition> MatchConditions {
			get { return this._matchConditions; }
			set {
				if( value != this._matchConditions ) {
					this._matchConditions = new ObservableCollection<BaseCondition>();
				}
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

		protected bool HasConditions() {
			return this._matchConditions != null && this._matchConditions.Count > 0;
		}

		/// <summary>
		/// Removes a condition from the File Match operation.
		/// </summary>
		/// <param name="cond"></param>
		private void RemoveCondition( BaseCondition cond ) {

			this._matchConditions.Remove( cond );

		}

		public FileMatchBuilderVM() {

			this._placeholder = new PlaceholderVM();
			this._placeholder.OnConditionSelected += this._placeholder_OnConditionSelected;

		}

		private void _placeholder_OnConditionSelected( Type conditionType ) {

			if( conditionType is null ) {
				throw new ArgumentNullException( "Condition Type cannot be null." );
			}
			if( !typeof(BaseCondition).IsAssignableFrom( conditionType) ) {
				throw new ArgumentException( "Condition Type must be a subclass of FileMatching.BaseCondition." );
			}

			BaseCondition condition = (BaseCondition)Activator.CreateInstance( conditionType );

		} //

	} // class

} // namespace
