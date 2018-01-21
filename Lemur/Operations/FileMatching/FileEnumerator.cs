using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// CURRENTLY UNUSED.
	/// Might delete. Idea is to provide files that can be processed while more files are found?
	/// </summary>
	public class FileLister : IEnumerable<FileSystemInfo> {

		/// <summary>
		/// Fringe of files to be visited.
		/// </summary>
		private readonly List<FileSystemInfo> _fringe = new List<FileSystemInfo>();

		private string baseDirectory;

		public FileLister( string baseDir ) {

			this.baseDirectory = baseDir;

		} //

		public IEnumerator<FileSystemInfo> GetEnumerator() {
			throw new NotImplementedException();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			throw new NotImplementedException();
		}

	} // class

	public class FileEnumerator : IEnumerator<FileSystemInfo> {

		/// <summary>
		/// Fringe of files to be visited.
		/// </summary>
		private readonly List<FileSystemInfo> _fringe = new List<FileSystemInfo>();

		private string baseDirectory;
		public FileEnumerator( string baseDir ) {

			this.baseDirectory = baseDir;

		}

		public FileSystemInfo Current => throw new NotImplementedException();

		object IEnumerator.Current => throw new NotImplementedException();

		public void Dispose() {
			throw new NotImplementedException();
		}

		public bool MoveNext() {
			throw new NotImplementedException();
		}

		public void Reset() {
			throw new NotImplementedException();
		}

	} // FileEnumerator

} // namespace