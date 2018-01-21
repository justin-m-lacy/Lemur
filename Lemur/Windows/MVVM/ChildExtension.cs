using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Intention is to set an object reference by specifying a parent object
	/// and a child name or type. Used because future references aren't allowed.
	/// NOTE: CURRENTLY UNUSED. Might delete.
	/// </summary>
	public class ChildExtension : MarkupExtension {

		private FrameworkContentElement parentObject;
		public FrameworkContentElement Parent {

			get { return parentObject; }
			set {
				parentObject = value;
			}

		}

		private string childName;
		public string ChildName {
			get => this.childName;
			set => this.childName = value;
		}

		public ChildExtension( string child ) {
			this.childName = child;
		}

		public ChildExtension() { }

		public override object ProvideValue( IServiceProvider serviceProvider ) {

			if( parentObject == null ) {
				return null;
			}
			if( string.IsNullOrEmpty( childName ) ) {
				return null;
			}

			for( int i = VisualTreeHelper.GetChildrenCount( parentObject ) - 1; i >= 0; i-- ) {
	
				DependencyObject child = VisualTreeHelper.GetChild( parentObject, i );
				if( child is FrameworkElement ) {

					FrameworkElement elm = (FrameworkElement)child;
					if( elm.Name == childName ) {
						return elm;
					}

				}

			} // for-loop.
		
			return null;
		}

	} // class

} // namespace
