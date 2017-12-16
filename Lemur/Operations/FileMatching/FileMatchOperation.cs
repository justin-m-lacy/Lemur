﻿using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Operation that matches files or directories by some criteria,
	/// and reports the match results.
	/// </summary>
	public class FileMatchOperation : ProgressOperation {

		#region PROPERTIES

		/// <summary>
		/// Conditions that files or directories must match in order
		/// to be included in a match operation.
		/// </summary>
		IEnumerable<IMatchCondition> _conditions;

		public IEnumerable<IMatchCondition> Conditions {
			get { return this._conditions; }

		}

		/// <summary>
		/// Condition to determine whether a subdirectory should
		/// be recursively followed. This does not not affect whether
		/// or not the directory is added as a match itself.
		/// </summary>
		private IEnumerable<IMatchCondition> _recursionConditions = new List<IMatchCondition>();

		/// <summary>
		/// Conditions a directory must meet in order to continue the search into
		/// a subdirectory. Directories meeting the recursion criteria will not be included
		/// in the search results unless they also meet the MatchConditions.
		/// </summary>
		public IEnumerable<IMatchCondition> RecursionConditions {
			get { return this._recursionConditions; }
		}

		private string folderPath;
		public string FolderPath => this.folderPath;

		/// <summary>
		/// List of Exceptions encountered during the search.
		/// </summary>
		private readonly List<Exception> errorList = new List<Exception>();
		public Exception[] ErrorList {
			get { return this.errorList.ToArray(); }
		}

		private readonly List<FileSystemInfo> matches = new List<FileSystemInfo>();

		/// <summary>
		/// List of files or directories that match the search conditions.
		/// </summary>
		public List<FileSystemInfo> Matches { get { return this.matches; } }

		/// <summary>
		/// Settings used for the operation.
		/// </summary>
		private FileMatchSettings _settings;
		public FileMatchSettings Settings => _settings;

		#endregion

		public FileMatchOperation( string basePath, FileMatchSettings settings=null, IEnumerable<IMatchCondition> conditions = null ) {

			this.folderPath = basePath;
			this._settings = settings;
			this._conditions = conditions;

		}

		public override void Run() {

			this.matches.Clear();
			this.errorList.Clear();

			if( string.IsNullOrEmpty( this.folderPath ) ) {
			} else {
				this.VisitDirectory( this.folderPath );
			}

		}

		/// <summary>
		/// Visit a directory, attempt to search any subdirectories, and add
		/// any file system matches.
		/// </summary>
		/// <param name="dir"></param>
		private void VisitDirectory( string dir ) {

			if( !Directory.Exists( dir ) ) {
				this.OperationComplete();
				return;
			}

			try {

				if( this._settings.Recursive ) {
					this.VisitSubDirs( dir );
				}

			} catch( Exception ) {
			}

			this.VisitFiles( dir );

		} // VisitDirectory()

		/// <summary>
		/// Visit the files in a directory and add any matches.
		/// </summary>
		/// <param name="parentDir"></param>
		private void VisitFiles( string parentDir ) {

			MatchType type = this._settings.Types;

			if ( type.HasFlag( MatchType.Directories ) ) {

				foreach( string dir in Directory.EnumerateDirectories( parentDir ) ) {
					
					try {

						DirectoryInfo info = new DirectoryInfo( dir );
						if( this.TestFile( info ) ) {
							this.matches.Add( info );
						}

					} catch( Exception e ) {
						this.errorList.Add( e );
					}

				} // foreach.

			}

			if( type.HasFlag( MatchType.Files ) ) {

				foreach( string file in Directory.EnumerateFiles( parentDir ) ) {

					try {

						FileInfo info = new FileInfo( file );
						if( this.TestFile( info ) ) {
							this.matches.Add( info );
						}

					} catch( Exception e ) {
						this.errorList.Add( e );
					}

				} // foreach.

			}

		}

		/// <summary>
		/// Returns true if file at path is a valid match,
		/// false otherwise.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private bool TestFile( FileSystemInfo fileInfo ) {

			foreach( IMatchCondition cond in this.Conditions ) {

				if( !cond.IsMatch( fileInfo, _settings ) ) {
					return false;
				}

			} // foreach

			return true;

		} //

		/// <summary>
		/// Visit all available subdirectories of the current directory.
		/// </summary>
		/// <param name="parentDir"></param>
		private void VisitSubDirs( string parentDir ) {

			/// Not using FileUtils since the recursive option is not yet applied?
			string[] dirs = Directory.GetDirectories( parentDir );

			if( this._recursionConditions == null ) {

				/// No Conditions for visiting subdirectories:
				foreach( string subdir in dirs ) {
					this.VisitDirectory( subdir );
				}
				return;
			} //

			/// Conditions for visiting subdirectories.
			foreach( string subdir in dirs ) {

				bool follow = true;

				DirectoryInfo info = new DirectoryInfo( subdir );

				foreach( IMatchCondition cond in this._recursionConditions ) {

					if( !cond.IsMatch( info, this._settings ) ) {
						follow = false;
						break;
					}

				}
				if( follow ) {
					this.VisitDirectory( subdir );
				}

			} //

		} // VisitSubDirs()

	} // class

} // namespace