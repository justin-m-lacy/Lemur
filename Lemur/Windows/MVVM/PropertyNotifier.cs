using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Class with implementation of INotifyPropertyChanged
	/// </summary>
	[Serializable]
	public class PropertyNotifier : INotifyPropertyChanged {

		[field:NonSerialized]
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void NotifyPropertyChanged( [CallerMemberName] string propertyName = "" ) {

			this.VerifyPropertyName( propertyName );
			PropertyChangedEventHandler handler = this.PropertyChanged;
			if( handler != null ) {
				//Console.WriteLine( "Dispatching Prop Changed: " + propertyName );
				handler( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );
			}

		} //

		[Conditional( "DEBUG" )]
		public void VerifyPropertyName( string propertyName ) {

			if( TypeDescriptor.GetProperties( this )[propertyName] == null ) {

				string msg = "Invalid property name: " + propertyName;
				throw new Exception( msg );

			}

		}

	} // class

} // namespace
