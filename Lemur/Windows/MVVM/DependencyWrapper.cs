using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lemur.Windows.MVVM {

	public class DependencyWrapper<T> : DependencyObject {

		private T value;
		public T Value { get => this.value; set => this.value = value; }

		public DependencyWrapper() {}

		public DependencyWrapper( T startValue ) {
			this.value = startValue;
		}

    } // class

} // namespace