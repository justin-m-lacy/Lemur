using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace Lemur.Windows.Behaviors {

	/// <summary>
	/// Bubbles a mousewheel event to a parent control.
	/// This is useful for example, when a parent control has its own ScrollViewer
	/// but the mousewheel event is being taken by a descendent ListView.
	/// 
	/// NOTE: A better idea is to create a ListView control template without a ScrollViewer.
	/// 
	/// Basic idea comes from StackOverflow with credit to "Josh Einstein Blog"
	/// Link no longer works.
	/// </summary>
	public class BubbleMouseWheel : Behavior<UIElement> {

		protected override void OnAttached() {

			base.OnAttached();
			AssociatedObject.PreviewMouseWheel += PreviewMouseWheel;

		}

		protected override void OnDetaching() {

			AssociatedObject.PreviewMouseWheel -= PreviewMouseWheel;
			base.OnDetaching();

		}

		void PreviewMouseWheel( object sender, MouseWheelEventArgs e ) {

			e.Handled = true;
			var newArgs = new MouseWheelEventArgs( e.MouseDevice, e.Timestamp, e.Delta );
			newArgs.RoutedEvent = UIElement.MouseWheelEvent;
			AssociatedObject.RaiseEvent( newArgs );

		}

	} // class

} // namespace
