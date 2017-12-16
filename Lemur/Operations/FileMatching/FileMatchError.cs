using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	public class FileMatchError {

		/// <summary>
		/// File or directory path where the error occured.
		/// </summary>
		public string sourcePath;

		/// <summary>
		/// Optional object associated with the error.
		/// </summary>
		public object errorObj;

		/// <summary>
		/// Optional error message.
		/// </summary>
		public string errMessage;

	} // class

} // namespace
