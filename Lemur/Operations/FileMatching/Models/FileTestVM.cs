using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// A view model for a File Match Condition.
	/// </summary>
	public class FileTestVM : ViewModelLite {

		#region PROPERTIES

		private string testName;
		/// <summary>
		/// Name of Test condition to display to the user.
		/// </summary>
		public string TestName { get => testName; set => this.SetProperty( ref this.testName, value ); }

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

		protected BaseCondition condition;
		public BaseCondition MatchCondition {
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


	} // class

} // namespace