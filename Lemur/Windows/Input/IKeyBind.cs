using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Lemur.Windows.Input {

	/// <summary>
	/// Defines an object that provides a keybinding.
	/// </summary>
	public interface IKeyBind {

		KeyBinding GetKeyBinding();
		void SetKeyBinding( KeyBinding binding );

    } // class

} // namespace
