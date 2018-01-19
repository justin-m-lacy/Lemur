using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lemur.Windows.MVVM {

	public class TemplateDictionary : Dictionary<Type, string> { }

	/// <summary>
	/// Selects a DataTemplate based on a source data type.
	/// </summary>
	public class TypeTemplateSelector : DataTemplateSelector {

		/// <summary>
		/// Maps Data Types to templates.
		/// </summary>
		public TemplateDictionary Templates { get => templates; set => templates = value; }
		private TemplateDictionary templates;

		/// <summary>
		/// Name of a template to use if other templates fail.
		/// </summary>
		public string DefaultTemplate { get => defaultTemplate; set => defaultTemplate = value; }
		private string defaultTemplate;

		/// <summary>
		/// Property of the source item to get the type of.
		/// If null or empty, the object iteself is used.
		/// </summary>
		public string TestProperty { get => testProperty; set => testProperty = value; }
		private string testProperty;

		public override DataTemplate SelectTemplate( object item, DependencyObject container ) {

			if( item == null ) {
				return null;
			}

			FrameworkElement parent = container as FrameworkElement;
			if( parent == null ) {
				return null;
			}

			string templateName;
			if( this.templates == null ) {
				Console.WriteLine( "TEMPLATE DICTIONARY IS NULL" );
				templateName = this.defaultTemplate;

			} else {

				Type testingType;
				if( !string.IsNullOrEmpty( this.testProperty ) ) {

					/// Find the type of the given property.
					testingType = item.GetType().GetProperty( this.testProperty ).GetValue( item ).GetType();

				} else {
					testingType = item.GetType();
				}

				//Console.WriteLine( "ATTEMPTING TO FIND TEMPLATE: " + testingType.Name );
				if( !this.templates.TryGetValue( testingType, out templateName ) ) {
					templateName = this.defaultTemplate;
				}

			}
			//Console.WriteLine( "USING TEMPLATE: " + templateName );
			return ( string.IsNullOrEmpty( templateName ) ) ? null : parent.FindResource( templateName ) as DataTemplate;

		}

	} // class

} // namespace