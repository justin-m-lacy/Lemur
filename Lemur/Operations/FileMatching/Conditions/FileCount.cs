using Lemur.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	[NameDesc( "Subfile Count", "Limits directories to those containing the given number of subfiles." )]
	[Serializable]
	public class FileCount : BaseCondition {

		private bool countSubDirs;

		/// <summary>
		/// If true, sub directories are counted against the maximum file count.
		/// If false, only files are counted.
		/// </summary>
		public bool CountSubDirs {
			get { return this.countSubDirs; }
			set { this.countSubDirs = value; }
		}

		private bool allowFiles;

		/// <summary>
		/// Match all files and only apply the condition to directory objects.
		/// </summary>
		public bool AllowFiles {
			get { return this.allowFiles; }
			set { this.allowFiles = value; }
		}

		private int minFiles;
		private int maxFiles;
		public int MinFiles { get => minFiles; set => minFiles = value; }
		public int MaxFiles { get => maxFiles; set => maxFiles = value; }

		override public bool IsMatch( FileSystemInfo info ) {

			if( info is FileInfo ) {
				return this.allowFiles;
			}

			DirectoryInfo dir = (DirectoryInfo)info;
			int fileCount = dir.GetFiles().Length;

			if( fileCount >= minFiles && fileCount <= maxFiles ) {
				return base.IsMatch( true );
			}

			return base.IsMatch( false );

		}

	} // class

} // namespace
