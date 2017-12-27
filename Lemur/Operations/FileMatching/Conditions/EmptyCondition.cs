using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Matches directories which are empty.
	/// Files passed to this Condition are ignored. (False is returned
	/// regardless of Exclude variable.)
	/// </summary>
	[Serializable]
	[NameDesc( "Directory Empty", "Matches only empty directories." )]
	public class EmptyCondition : DirectoryCondition {

		public override bool IsMatch( FileSystemInfo info, FileMatchSettings settings ) {

			DirectoryInfo dInfo = info as DirectoryInfo;
			if( dInfo == null ) { return false; }

			FileSystemInfo[] infos = dInfo.GetFileSystemInfos();
			if( infos.Length == 0 ) {
				return base.IsMatch( true );
			}

			return base.IsMatch(false);

		}

	} // class

} // namespace