using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Contained In", "Only subfolders and files of the given directory will be searched." )]
	[Serializable]
	public class ContainedIn : BaseCondition {

		/// <summary>
		/// The directory in which valid matches are located. Files and directories outside this
		/// location are never matched.
		/// </summary>
		public string SearchDirectory { get => searchDirectory; set => searchDirectory = value; }
		private string searchDirectory;

		/// <summary>
		/// If true, only the immediate children of the directory should be included.
		/// </summary>
		public bool ChildrenOnly { get => childrenOnly; set => childrenOnly = value; }
		private bool childrenOnly;

		public ContainedIn() {
		}
		public ContainedIn( string basePath ) {
			this.searchDirectory = basePath;
		}

		public override bool IsMatch( FileSystemInfo info ) {

			//todo: account for multiple path formats.
			bool subfile = info.FullName.Contains( this.searchDirectory );
			return base.IsMatch( subfile );

		}

	} // class

} // namespace
