using System;
using System.Collections;
using System.Reflection;
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
		/// Property path in the source item to use as a Key into the Templates table.
		/// If no key path is set, the item itself is used as the key.
		/// </summary>
		public string KeyPath {
			get {
				if( keyPath == null || keyPath.Length == 0 ) {
					return string.Empty;
				}
				return string.Join( ".", this.keyPath );
			}
			set {
				if( string.IsNullOrEmpty( value ) ) {
					this.keyPath = null;
				} else {
					this.keyPath = value.Split( '.' );
				}
			}

		}
		private string[] keyPath;

		public override DataTemplate SelectTemplate( object item, DependencyObject container ) {

			FrameworkElement parent = container as FrameworkElement;
			if( parent == null ) {
				return null;
			}

			object templateObject;
			if( this.templateTable == null ) {

				templateObject = this.defaultTemplate;

			} else {

				// key into the hash to get the templateKey, or template object.
				object hashKey = this.ReadKeyPath( item, this.keyPath );

				Console.WriteLine( "FINDING TEMPLATE FOR KEY: " + hashKey );
				if( this.templateTable.ContainsKey( hashKey ) ) {

					Console.WriteLine( "KEY EXISTS" );
					templateObject = this.templateTable[hashKey] ?? this.defaultTemplate;

				} else {

					Console.WriteLine( "KEY DOES NOT EXIST" );
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
			Console.WriteLine( "EXPECTED TEMPLATE: " + templateObject );
			return ( string.IsNullOrEmpty( templateKey ) ) ? null : parent.FindResource( templateKey ) as DataTemplate;

		}

		/// <summary>
		/// </summary>
		/// <param name="item"></param>
		/// <param name="props"></param>
		/// <returns></returns>
		private object ReadKeyPath( object item, string[] props ) {

			if( props == null || props.Length == 0 ) {
				return item;
			}
			int len = props.Length;

			object curObject = item;
			for( int i = 0; i < len; i++ ) {

				if( curObject == null ) {
					return null;
				}

				string propName = props[i];
				PropertyInfo pInfo = curObject.GetType().GetProperty( propName );
				if( pInfo == null ) {
					throw new Exception( "Property: " + pInfo + " not found on object: " + curObject );
				}
				curObject = (object)pInfo.GetValue( curObject );

			}

			return curObject;

		}

	} // class

} // namespace
