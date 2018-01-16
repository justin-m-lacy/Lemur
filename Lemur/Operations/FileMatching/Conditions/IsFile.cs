using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Only Files", "Directories not included in search results." )]
	[Serializable]
	public class IsFile : BaseCondition {

		override public bool IsMatch( FileSystemInfo info ) {

			if( info is FileInfo ) {
				return base.IsMatch( true );
			}

			return base.IsMatch( false );

		}

	} // class

} // namespace
