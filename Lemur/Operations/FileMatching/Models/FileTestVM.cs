using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
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

		#region PROPERTIES

		private string _displayName;
		/// <summary>
		/// Name of Test condition to display to the user.
		/// </summary>
		public string DisplayName { get => _displayName; set => this.SetProperty( ref this._displayName, value ); }

		private string _testDesc;
		/// <summary>
		/// Description of the matching test being applied.
		/// </summary>
		public string TestDesc { get => _testDesc; set => this.SetProperty( ref this._testDesc, value ); }

		private string excludeText;
		/// <summary>
		/// Message to display when the match condition is an exclude condition.
		/// </summary>
		public string ExcludeText { get => excludeText; set => this.SetProperty( ref this.excludeText, value ); }

		private string includeText;
		/// <summary>
		/// Message to display when the match condition is an exclude condition.
		/// </summary>
		public string IncludeText { get => includeText; set => this.SetProperty( ref this.includeText, value ); }

		protected IMatchCondition condition;

		/// <summary>
		/// The underlying condition being displayed.
		/// </summary>
		public IMatchCondition MatchCondition {
			get { return condition; }
			set {

				this.SetProperty( ref this.condition, value );
				if( this.condition != null ) {
					this.condition.Exclude = this.Exclude;
				}

			}

		}

		/// <summary>
		/// This variable duplicates the exclude from the BaseCondition itself,
		/// but this allows the value to be saved when the BaseCondition is null,
		/// or is switched.
		/// </summary>
		private bool _exclude;

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

		#endregion

		public FileTestVM() { }

		public FileTestVM( IMatchCondition matchTest, bool exclude=false ) {
			this.condition = matchTest;
			this._exclude = exclude;
		}

		public bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {
			return condition.IsMatch( info, settings );
		}

	} // class

} // namespace