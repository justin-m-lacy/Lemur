using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Validation rule with a customizable validation message.
	/// </summary>
	public abstract class CustomValidationRule : ValidationRule {

		//private RuleProperties properties;
		//public RuleProperties Properties { get => properties; set => properties = value; }

		/// <summary>
		/// Returns the default error message if it is not not and not empty, or the alternative string
		/// if it does not.
		/// </summary>
		/// <returns></returns>
		protected string ErrorMessage( string altMessage=null ) {
			return string.IsNullOrEmpty(this._defaultMessage) ? altMessage : this._defaultMessage;
		}

		private string _defaultMessage;
		public string DefaultErrorMessage {
			get {
				return this._defaultMessage;
			}
			set {
				this._defaultMessage = value;
			}
		}

    } // class

} // namespace