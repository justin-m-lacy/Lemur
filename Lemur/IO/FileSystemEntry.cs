using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.IO {

	/// <summary>
	/// Wraps a FileSystemEntry to provide more convenience properties.
	/// </summary>
	public class FileEntryVM {

		/// <summary>
		/// Underlying file information being displayed.
		/// </summary>
		public FileSystemInfo FileInfo {
			get => _fileInfo;
		}
		private FileSystemInfo _fileInfo;

		public bool IsFile {
			get { return this._fileInfo is FileInfo; }
		}
		public bool IsDirectory {
			get { return this._fileInfo is DirectoryInfo; }
		}
		
		public long Size {
			get {
				if( _fileInfo is FileInfo ) {
					return ( (FileInfo)_fileInfo ).Length;
				}
				return 0;
			}
		}
		private long _size;

		public FileEntryVM( FileSystemInfo info ) {
			this._fileInfo = info;
		}

	} // class

} // namespace
