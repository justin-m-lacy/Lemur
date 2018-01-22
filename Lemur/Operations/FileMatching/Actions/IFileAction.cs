using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	public interface IFileAction {

		/// <summary>
		/// Indicates the action should only be run once, even when applied to multiple files.
		/// An example of this is the EmptyRecycleBin action, which doesn't benefit from being
		/// applied multiple times.
		/// </summary>
		bool RunOnce {
			get;
			set;
		}

		/// <summary>
		/// Returns boolean indicating success.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		FileActionResult Run( FileSystemInfo info );

		/// <summary>
		/// Run the action on an enumeration of files.
		/// If RunOnce is true, the action should be run on the first
		/// file of the enumeration, or on the root directory,
		/// if the enumeration is empty.
		/// </summary>
		/// <param name="fileList"></param>
		/// <returns></returns>
		FileActionResult[] Run( IEnumerable<FileSystemInfo> fileList );

	} // class

} // namespace