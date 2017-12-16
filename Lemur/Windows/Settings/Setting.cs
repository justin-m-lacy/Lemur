using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Settings {

	/// <summary>
	/// Describes a Setting Name, Setting Description combination
	/// for a given setting value.
	/// </summary>
	public class ValueDesc : Tuple<string, string> {

		public string Name => this.Item1;
		public string Desc => this.Item2;

		public ValueDesc( string name, string desc ) : base( name, desc ) {
		}

	}

	/// <summary>
	/// Defines information about a setting that can be displayed and
	/// edited by the user.
	/// </summary>
	public class Setting {

		private Type _settingType;
		public Type Type {
			get { return this._settingType; }
			set { this._settingType = value; }
		}

		private string _displayName;
		/// <summary>
		/// Name of setting displayed to the user.
		/// </summary>
		public string Name {
			get {
				return this._displayName;
			}
			set { this._displayName = value; }
		}

		/// <summary>
		/// Variable name that stores the setting.
		/// </summary>
		private string _variableName;
		public string VariableName {
			get { return _variableName; }
			set { this._variableName = value; }
		}

		public string _desc;
		/// <summary>
		/// Description of the setting.
		/// </summary>
		public string Desc {
			get { return this._desc; }
			set { this._desc = value; }
		}

		public string _toolTip;
		/// <summary>
		/// Tooltip displayed for the setting.
		/// </summary>
		public string ToolTip {
			get { return this._toolTip; }
			set { this._toolTip = value; }
		}

    } // class

} // namespace
