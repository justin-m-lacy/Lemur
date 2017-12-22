using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Types {

	/// <summary>
	/// Attribute to provide an object with a name and description of its usage
	/// that can be presented to the user.
	/// </summary>
	[AttributeUsage( AttributeTargets.All )]
	public class NameDescAttribute : Attribute {

		private string _name;
		/// <summary>
		/// Display name of the type.
		/// </summary>
		public string Name {
			get { return this._name; }
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Description of the type.
		/// </summary>
		public string Desc { get => desc; set => desc = value; }
		private string desc;

		public NameDescAttribute( string name = null, string desc = null ) {

			this._name = name;
			this.desc = desc;

		}

	} // class

} // namespace
