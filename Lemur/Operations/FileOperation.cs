using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace TorboFile.Operations {

	public enum FileOperationType {
		Move,
		Delete
	}

	/// <summary>
	/// Represents a Move or Delete File Operation.
	/// TODO: Expand to allow for more options; multiple files?
	/// </summary>
	public class FileOperation {

		private FileOperationType type;

		private string sourcePath;
		private string destPath;

		/// <summary>
		/// Maximum time in milliseconds to attempt a file operation
		/// before giving up.
		/// </summary>
		private long maxWaitTime = long.MaxValue;

		/// <summary>
		/// If this version of the constructor is used for a Delete opration, the dest path is ignored.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="dest"></param>
		/// <param name="max_time"></param>
		/// <param name="opType"></param>
		public FileOperation( string source, string dest, long max_time = 0, FileOperationType opType=FileOperationType.Move ) {

			if( max_time > 0 ) {
				this.maxWaitTime = max_time;
			}

			this.type = opType;

			this.sourcePath = source;
			this.destPath = dest;

		}

		public FileOperation( string source, FileOperationType opType, long max_time = 0 ) {

			if( opType == FileOperationType.Move ) {
				throw new ArgumentException( "File Move Operation requires a destination path." );
			}
			this.type = opType;

			if( max_time > 0 ) {
				this.maxWaitTime = max_time;
			}
			this.sourcePath = source;

		}

		public async Task<bool> RunAsync() {

			if( this.type == FileOperationType.Delete ) {
				return await RunDeleteAsync();
			} else if( this.type == FileOperationType.Move ) {
				return await RunMoveAsync();
			}
			return false;

		}

		/// <summary>
		/// Runs the operation asynchronously.
		/// </summary>
		/// <returns></returns>
		public async Task<bool> RunMoveAsync() {

			/// milliseconds since startup.
			long opStartTime = System.Environment.TickCount;

			if( string.IsNullOrEmpty( this.sourcePath ) || string.IsNullOrEmpty(this.destPath) ) {
				return false;
			}
			long maxWait = this.maxWaitTime;

			bool success = await Task<bool>.Run( async () => {

				if( !File.Exists( this.sourcePath ) ) {
					return false;
				}

				while( maxWait > ( System.Environment.TickCount - opStartTime ) ) {

					try {
						File.Move( this.sourcePath, this.destPath );
						return true;
					} catch( Exception ) {
						await Task.Delay( 100 );
					}
				}
				return false;

			} );

			Console.WriteLine( "success: " + success );

			return success;

		} //

		  /// <summary>
		  /// Runs the operation asynchronously.
		  /// </summary>
		  /// <returns></returns>
		public async Task<bool> RunDeleteAsync() {

			/// milliseconds since startup.
			long opStartTime = System.Environment.TickCount;

			if( string.IsNullOrEmpty( this.sourcePath ) ) {
				return false;
			}
			long maxWait = this.maxWaitTime;

			bool success = await Task<bool>.Run( async () => {

				if( !File.Exists( this.sourcePath ) ) {
					return false;
				}

				while( maxWait > ( System.Environment.TickCount - opStartTime ) ) {

					try {
						File.Delete( this.sourcePath );
						return true;
					} catch( Exception ) {
						await Task.Delay( 100 );
					}
				}
				return false;

			});

			Console.WriteLine( "success: " + success );

			return success;

		} //

	} // class

} // namespace
