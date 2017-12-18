using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.Validation {

	/// <summary>
	/// Useful properties for binding ValidationRules.
	/// </summary>
	public class RuleProperties : DependencyObject {

		public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register( "DuplicateMessage",
				typeof( string ), typeof( RuleProperties ), new PropertyMetadata( "Invalid entry." ) );

		public string ErrorMessage {
			get { return (string)this.GetValue( ErrorMessageProperty ); }
			set { this.SetValue( ErrorMessageProperty, value ); }
		}


	} // class

} // namespace
