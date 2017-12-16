using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Images {

	/// <summary>
	/// Expands the default System.Windows.Media.Stretch with more options.
	/// </summary>
	public enum Stretch {

		/// <summary>
		/// Matches System.Windows.Media.Stretch.None
		/// </summary>
		None = 0,

		/// <summary>
		/// Matches System.Windows.Media.Stretch.Fill
		/// </summary>
		Fill = 1,

		/// <summary>
		/// Matches System.Windows.Media.Stretch.Uniform
		/// </summary>
		Uniform = 2,

		/// <summary>
		/// Matches System.Windows.Media.Stretch.UniformToFill
		/// </summary>
		UniformToFill = 3,

		/// <summary>
		/// Stretch until the width of the item fills the available space.
		/// </summary>
		FillWidth=4,

		/// <summary>
		/// Stretch until the height of the item fills the available space.
		/// </summary>
		FillHeight=5

    } // class

} // namespace
