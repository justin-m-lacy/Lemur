using System;
using System.Collections;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// An extremely generic DataTemplateSelector.
	/// </summary>
	public class TemplateSelector : DataTemplateSelector {

		/// <summary>
		/// Maps Data Types to templates.
		/// </summary>
		public Hashtable Templates { get => templateTable; set => templateTable = value; }
		private Hashtable templateTable;

		/// <summary>
		/// Default template to use if no better template found.
		/// This can be either be a DataTemplate instance, or a key into a ResourceDictionary.
		/// </summary>
		public object DefaultTemplate { get => defaultTemplate; set => defaultTemplate = value; }
		private object defaultTemplate;

		/// <summary>
		/// Property of the source item to use as a Key into the Templates table.
		/// If no property is set, the item itself is used as the key.
		/// </summary>
		public string KeyProperty { get => keyProperty; set => keyProperty = value; }
		private string keyProperty;

		public override DataTemplate SelectTemplate( object item, DependencyObject container ) {

			if( item == null ) {
				return null;
			}

			FrameworkElement parent = container as FrameworkElement;
			if( parent == null ) {
				return null;
			}

			object templateObject;
			if( this.templateTable == null ) {

				templateObject = this.defaultTemplate;

			} else {

				object hashKey;		// key into the hash to get the templateKey, or template object.

				if( !string.IsNullOrEmpty( this.keyProperty ) ) {

					/// Find the type of the given property.
					hashKey = item.GetType().GetProperty( this.keyProperty ).GetValue( item );

				} else {
					hashKey = item;
				}

				Console.WriteLine( "ATTEMPTING TO FIND TEMPLATE: " + hashKey );
				if( this.templateTable.ContainsKey( hashKey ) ) {

					templateObject = this.templateTable[hashKey] ?? this.defaultTemplate;

				} else {

					templateObject = this.defaultTemplate;
				}

			}

			if( templateObject == null ) {
				return null;
			}

			/// The templateObject might be a referenced DataTemplate. Check this first.
			DataTemplate template = templateObject as DataTemplate;
			if( template != null ) {
				return template;
			}

			// The template object should be a key into a ResourceDictionary. Find the actual DataTemplate.
			string templateKey = templateObject as string;
			return ( string.IsNullOrEmpty( templateKey ) ) ? null : parent.FindResource( templateKey ) as DataTemplate;

		}

		public TemplateSelector() {
		}

	} // class

} // namespace
