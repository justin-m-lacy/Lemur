using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// A minimal ViewModel. Currently only supports PropertyChange notifications.
	/// Change name to PropertyNotifier?
	/// </summary>
	public class ViewModelLite : INotifyPropertyChanged {

		public ViewModelLite() {}

		#region PROPERTY CHANGES

		/// <summary> 
		/// Notify ViewModel property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Updates a property and dispatches a changed event if the new value is
		/// not the same as the current value.
		/// </summary>
		/// <param name="propertyName"></param>
		protected bool SetProperty<T>( ref T original, T newVal,
			[CallerMemberName] string propertyName = "" ) {

			if( EqualityComparer<T>.Default.Equals( original, newVal ) ) {
				return false;
			}

			this.VerifyPropertyName( propertyName );

			original = newVal;

			this.PropertyChanged?.Invoke( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );

			return true;

		} //

		protected void NotifyPropertyChanged( [CallerMemberName] string propertyName = "" ) {

			this.VerifyPropertyName( propertyName );
			this.PropertyChanged?.Invoke( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );

		} //

		[Conditional( "DEBUG" )]
		public void VerifyPropertyName( string propertyName ) {

			if( TypeDescriptor.GetProperties( this )[propertyName] == null ) {

				string msg = "Invalid property name: " + propertyName;
				throw new Exception( msg );

			}

		}

		#endregion


	} // class

} // namespace
