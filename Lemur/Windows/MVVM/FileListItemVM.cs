using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// A checkable/selectable file list item.
	/// </summary>
	public class FileListItemVM : ListItemVM<FileSystemInfo> {

		#region PROPERTIES

		public string Name {
			get => this.info.Name;
		}

		public string Path {
			get => this.info.FullName;
		}

		public long Size {
			get {
				if( this._attrs.HasFlag( FileAttributes.Directory ) ) {
					return 0;
				}
				return ( this.Item as FileInfo ).Length;
			}

		}

		public bool IsDirectory {
			get {
				return this._attrs.HasFlag( FileAttributes.Directory );
			}
		}

		public bool IsFile {
			get {
				return !this._attrs.HasFlag( FileAttributes.Directory );
			}
		}

		public string Extension {
			get => this.info.Extension;
		}

		/// <summary>
		/// Returns the parent directory of the file.
		/// </summary>
		public string Directory {
			get {
				string path = System.IO.Path.GetDirectoryName( this.info.FullName );
				return path;
			}
		}

		public FileAttributes Attributes {
			get => this._attrs;
		}

		public DateTime CreateTime {
			get => this.info.CreationTime;
		}
		public DateTime ModifiedTime {
			get => this.info.LastWriteTime;
		}
		public DateTime AccessedTime {
			get => this.info.LastWriteTime;
		}

		#endregion

		/// <summary>
		/// Cached attributes.
		/// </summary>
		private FileAttributes _attrs;
		private FileSystemInfo info;

		/// <summary>
		/// Update the file path in the case of a file being moved.
		/// </summary>
		/// <param name="path"></param>
		public void UpdatePath( string newPath ) {
		}

		public void UpdateName( string newName ) {
		}


		public FileListItemVM( FileSystemInfo file ) : base(file) {

			this.info = file;
			this._attrs = file.Attributes;

		}

    } // class

} // namespace