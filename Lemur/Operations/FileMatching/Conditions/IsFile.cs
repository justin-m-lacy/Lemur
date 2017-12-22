using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Only Files", "Only files (not directories) are included in the search." )]
	public class IsFile : BaseCondition {

		override public bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			if( info is FileInfo ) {
				return true;
			}

			return false;

		}

	} // class

} // namespace
