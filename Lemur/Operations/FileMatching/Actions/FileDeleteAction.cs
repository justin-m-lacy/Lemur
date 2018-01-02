using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	/// <summary>
	/// Performs a hard delete on the files or directories supplies.
	/// Files and directories are not moved to recycle bin.
	/// </summary>
	[NameDesc( "Delete Files", "Deletes the target files or directory without moving to the recycle bin." )]
	[Serializable]
	public class FileDeleteAction : FileActionBase {

		override public bool Run( FileSystemInfo info ) {

			info.Delete();

			return true;

		}

	} // class

} // namespace