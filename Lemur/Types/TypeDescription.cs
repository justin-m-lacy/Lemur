using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Types {

	/// <summary>
	/// Name and description of a type that can be presented to the user.
	/// NOTE: using this instead of NameDescAttribute allows for loading from xaml
	/// and language localization.
	/// </summary>
	public class TypeDescription {

		private Type _objType;
		/// <summary>
		/// Type being described.
		/// </summary>
		public Type Type {
			get { return this._objType; }
			set { this._objType = value; }
		}

		private string _name;
		/// <summary>
		/// Display name of the type.
		/// </summary>
		public string Name {
			get { return this._name ?? ( _objType != null ? _objType.Name : string.Empty ); }
			set {
				this._name = value;
			}
		}

		/// <summary>
		/// Description of the type.
		/// </summary>
		public string Desc { get => desc; set => desc = value; }
		private string desc;

		public TypeDescription( Type instanceType, string name = null, string desc = null ) {

			this._objType = instanceType;
			this._name = name;
			this.desc = desc;
		}

	} // class

} // namespace
