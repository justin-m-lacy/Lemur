using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Only Files", "Directories not included in search results." )]
	public class IsFile : BaseCondition {

		override public bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( info is FileInfo ) {
				return true;
			}

			return false;

		}

	} // class

} // namespace
