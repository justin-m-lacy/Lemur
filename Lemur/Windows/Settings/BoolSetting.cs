using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Settings {

	public class BoolSetting : Setting {

		/// <summary>
		/// If true, the bool setting should display the inverse option
		/// as well as the true option. (Typically for a radio button
		/// alternative.)
		/// </summary>
		private bool _showInverseOption;
		public bool ShowInverseOption {
			get { return this._showInverseOption; }
			set { this._showInverseOption = value; }
		}

		public string _inverseName;
		/// <summary>
		/// Name/Label of the inverse setting.
		/// </summary>
		public string InverseName {
			get { return this._inverseName; }
			set { this._inverseName = value; }
		}

		private string _inverseDesc;

		/// <summary>
		/// Description for using the inverse (false) value of a bool.
		/// </summary>
		public string InverseDesc {
			get { return this._inverseDesc; }
			set { this._inverseDesc = value; }
		}

    } // class

} // namespace