using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	public interface IFileAction {

		/// <summary>
		/// Returns boolean indicating success.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		bool Run( FileSystemInfo info );

	} // class

} // namespace
