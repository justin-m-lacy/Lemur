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
	/// </summary>
	public class FileTestVM : DataObjectVM {

		#region PROPERTIES

		#region DISPLAY TEXT

		/// <summary>
		/// Tooltip to display when hovering over the exclude checkbox.
		/// </summary>
		public string ExcludeToolTip { get => this._excludeToolTip; set => this.SetProperty( ref this._excludeToolTip, value ); }
		private string _excludeToolTip;

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
		/// Whether the Matching operation includes or excludes a file which matches
		/// the test condition. Default is false ( File is INCLUDED on a match. )
		/// </summary>
		public bool Exclude {

			get => ((IMatchCondition)Data).Exclude;
			set {

				IMatchCondition cond = (IMatchCondition)Data;
				if( cond.Exclude != value ) {
					cond.Exclude = value;
					this.NotifyPropertyChanged();
				}

			} // set

		} // Exclude

		#endregion

		public FileTestVM() { }

		public FileTestVM( IMatchCondition matchTest ) {

			this.Data = matchTest;

		}

		public bool IsMatch( FileSystemInfo info ) {

			if( this.Data == null ) {
				return false;
			}

			return ((IMatchCondition)Data).IsMatch( info );

		} //

	} // class

} // namespace