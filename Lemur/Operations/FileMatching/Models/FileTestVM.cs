using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// A view model for a File Match Condition with custom information messages.
	/// 
	/// The class implements <see cref="IMatchCondition"/> by wrapping the underlying IMatchCondition
	/// being displayed. This is done so parent models can easily switch between displaying
	/// Collections of FileTestVM objects or directly displaying IMatchCondition objects with custom DataTemplates.
	/// 
	/// This way the decision can be changed with minimal refactoring, though I don't know how useful it is yet.
	///
	/// </summary>
	public class FileTestVM : ViewModelLite, IMatchCondition {

		#region STATIC PROPERTY DEFAULTS

		// STATIC STRINGS SO ALL VALUES CAN BE SET AT ONCE.
		//static public readonly string DefaultExcludeToolTip;

		#endregion

		#region PROPERTIES

		#region DISPLAY TEXT

		/// <summary>
		/// Name of Test condition to display to the user.
		/// </summary>
		public string DisplayName { get => _displayName; set => this.SetProperty( ref this._displayName, value ); }
		private string _displayName;

		/// <summary>
		/// Tooltip to display when hovering over the exclude checkbox.
		/// </summary>
		public string ExcludeToolTip { get => this._excludeToolTip; set => this.SetProperty( ref this._excludeToolTip, value ); }
		private string _excludeToolTip;

		/// <summary>
		/// Description of the matching test being applied.
		/// </summary>
		public string TestDesc { get => _testDesc; set => this.SetProperty( ref this._testDesc, value ); }
		private string _testDesc;

		/// <summary>
		/// Message to display when the match condition is an exclude condition.
		/// </summary>
		public string ExcludeText { get => excludeText; set => this.SetProperty( ref this.excludeText, value ); }
		private string excludeText;

		/// <summary>
		/// Message to display when the match condition is an include condition. (Default.)
		/// </summary>
		public string IncludeText { get => includeText; set => this.SetProperty( ref this.includeText, value ); }
		private string includeText;

		#endregion DISPLAY TEXT

		/// <summary>
		/// The underlying condition being displayed.
		/// </summary>
		public IMatchCondition Condition {
			get { return condition; }
			set {

				if( this.SetProperty( ref this.condition, value ) ) {

					if( this.condition != null ) {

						this.condition.Exclude = this.Exclude;
						this.ConditionType = value.GetType();

					} else {
						this.ConditionType = null;
					}
					// update all property indexers on the condition.
					this.NotifyPropertyChanged( "Property[]" );

				}

			} //set

		} // MatchCondition
		protected IMatchCondition condition;

		/// <summary>
		/// Gives access to properties of the underlying condition.
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		[IndexerName( "Property")]
		public object this[string propName] {

			get {

				if( this._conditionType == null ) {
					return null;
				}

				TypeInfo info = this._conditionType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return null;
				}
				return prop.GetValue( this.condition );

			}
			set {

				if( this._conditionType == null ) {
					return;
				}

				TypeInfo info = this._conditionType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return;
				}

				// check if property has changed.
				object current = prop.GetValue( this.condition );
				if( !Object.Equals( current, value ) ) {

					prop.SetValue( this.condition, value );
					/// unfortunately refreshes ALL indexers on Property.
					this.NotifyPropertyChanged( "Property[]" );
				}

			} // set

		}

		/// <summary>
		/// Type of the condition being displayed.
		/// Use in xaml to change the display Template based on type.
		/// </summary>
		public Type ConditionType {
			get {
				return this._conditionType;
			}
			set {
				this.SetProperty( ref this._conditionType, value );
			}
		}
		private Type _conditionType;

		/// <summary>
		/// Whether the Matching operation includes or excludes a file which matches
		/// the test condition. Default is false ( file IS included if it matches. )
		/// </summary>
		public bool Exclude {

			get => this._exclude;
			set {

				if( this.SetProperty( ref this._exclude, value ) ) {
					if( condition != null ) {
						condition.Exclude = value;
					}
				}

			} // set

		} // Exclude

		/// <summary>
		/// This variable duplicates the exclude from the BaseCondition itself,
		/// but this allows the value to be saved when the BaseCondition is null,
		/// or is switched.
		/// </summary>
		private bool _exclude;

		#endregion

		public FileTestVM() { }

		public FileTestVM( IMatchCondition matchTest, bool exclude=false ) {
			this.Condition = matchTest;
			this._exclude = exclude;
		}

		public bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( this.condition == null ) {
				return false;
			}

			return condition.IsMatch( info, settings );

		} //

	} // class

} // namespace