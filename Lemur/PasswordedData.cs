using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Security {

	public class PasswordedData<T> where T : class {

		private T data;
		public T Data {
			get { return this.data; }
			set { this.data = value; }
		}

		private string _passHash;
		public string PasswordHash {
			get { return this._passHash; }
			set { if( this._passHash != value ) { this._passHash = value; } }
		}

		private string _saveFile;
		public string SaveFile {
			get { return this._saveFile; }
			set { this._saveFile = value; }
		}

		public PasswordedData( T data ) {

			this.data = data;

		} //

	} // class

} // namespace
