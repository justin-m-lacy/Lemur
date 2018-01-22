using Lemur.Types;
using Lemur.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Move To Recycle", "Recycles the target file or directory." )]
	[Serializable]
	public class RecycleAction : FileActionBase {

		override public FileActionResult Run( FileSystemInfo info ) {

			bool success = RecycleBinDeleter.Delete( info.FullName );
			return new FileActionResult( success );

		}

	} // class

} // namespace
