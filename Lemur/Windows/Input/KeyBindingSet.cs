using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Lemur.Windows.Input {

	public class KeyBindingSet {

		ObservableCollection<KeyBinding> bindings;
		public ObservableCollection<KeyBinding> Bindings {
			get { return this.bindings; }
		}

		public KeyBindingSet() {

			this.bindings = new ObservableCollection<KeyBinding>();

		}

		public void AddBinding( KeyBinding binding ) {
			this.bindings.Add( binding );
		}

    } // class

} // namespace