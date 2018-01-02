using Lemur.Types;
using Lemur.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[NameDesc( "Recycle", "Recycles the target file or directory." )]
	[Serializable]
	public class RecycleAction : FileActionBase {


		override public bool Run( FileSystemInfo info ) {

			return RecycleBinDeleter.Delete( info.Name );

		}

	} // class

} // namespace
