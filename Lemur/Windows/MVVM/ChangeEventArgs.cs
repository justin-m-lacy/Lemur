using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Lemur.Windows.MVVM {

	public class ChangeEventArgs<T> : PropertyChangedEventArgs {

		public T NewValue;

		public ChangeEventArgs( string propertyName, T data ) : base( propertyName ) {
			this.NewValue = data;
		}

    } // class

} // namespace
