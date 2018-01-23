using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Lemur.Utils {

	static public class FileUtils {

		/// <summary>
		/// Returns a list of all files and directories within a directory
		/// and all its subdirectories. Unlike Directory.GetFileSystemEntries(),
		/// the function does not return on an Exception, but continues to
		/// collect and return as many entries as possible.
		/// </summary>
		/// <param name="directory"></param>
		/// <returns></returns>
		static public string[] GetFileSystemEntries( string directory, string searchPattern = null ) {

			List<string> files = new List<string>();

			Queue<string> search_dirs = new Queue<string>();
			search_dirs.Enqueue( directory );

			while( search_dirs.Count > 0 ) {

				directory = search_dirs.Dequeue();

				try {

					search_dirs.Enqueue( Directory.EnumerateDirectories( directory ) );
					files.AddRange( Directory.GetFileSystemEntries( directory, searchPattern ) );

				} catch( Exception ) {
				}

			} // while-loop.

			return files.ToArray();

		} // GetFiles()

		  /// <summary>
		  /// Returns all directories in a directory and all its subdirectories.
		  /// Unlike Directory.GetDirectories(), the function does not return
		  /// on an Exception, but continues to collect and return as many
		  /// directories as possible.
		  /// </summary>
		  /// <param name="directory"></param>
		  /// <returns></returns>
		static public string[] GetDirectories( string directory, string searchPattern = null ) {

			List<string> files = new List<string>();

			Queue<string> search_dirs = new Queue<string>();
			search_dirs.Enqueue( directory );

			while( search_dirs.Count > 0 ) {

				directory = search_dirs.Dequeue();

				try {

					search_dirs.Enqueue( Directory.EnumerateDirectories( directory ) );
					files.AddRange( Directory.GetDirectories( directory, searchPattern ) );

				} catch( Exception ) {
				}

			} // while-loop.

			return files.ToArray();

		} // GetFiles()

		  /// <summary>
		  /// Returns all files in a directory and all of its subdirectories.
		  /// Unlike Directory.GetFiles(), the function does not return
		  /// on an Exception, but continues to collect and return as many files as possible.
		  /// </summary>
		  /// <param name="directory"></param>
		  /// <returns></returns>
		static public string[] GetFiles( string directory, string searchPattern=null ) {

			List<string> files = new List<string>();

			Queue<string> search_dirs = new Queue<string>();
			search_dirs.Enqueue( directory );

			while( search_dirs.Count > 0 ) {

				directory = search_dirs.Dequeue();

				try {

					search_dirs.Enqueue( Directory.EnumerateDirectories( directory ) );
					files.AddRange( Directory.GetFiles( directory, searchPattern ) );

				} catch( Exception ) {
				}

			} // while-loop.

			return files.ToArray();

		} // GetFiles()
		
		/// <summary>
		/// Serializes an object to a stream as binary data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="inst"></param>
		static public void WriteBinary<T>( FileStream stream, T inst ) {

			var formatter = new BinaryFormatter();
			formatter.Serialize( stream, inst );

		}

		/// <summary>
		/// Deserializes an object from a stream as binary data.
		/// The stream is closed after the read.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <returns>The object read from the stream or default(T) on error.</returns>
		static public T ReadBinary<T>( FileStream stream ) {

			if( stream == null || stream.Length == 0) {
				return default( T );
			}

			try {

				var formatter = new BinaryFormatter();
				return (T)formatter.Deserialize( stream );

			} catch( Exception e ) {
				Console.WriteLine( e.ToString() );
				return default( T );
			}

		}

		/// <summary>
		/// Serializes an object to file as binary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="path"></param>
		/// <param name="inst"></param>
		static public void WriteBinary<T>( string path, T inst ) {

			using( FileStream stream = File.OpenWrite( path ) ) {

				var formatter = new BinaryFormatter();
				formatter.Serialize( stream, inst );
			}


		}

		static public T ReadBinary<T>( string path ) {

			using( FileStream stream = File.OpenRead( path ) ) {

				var formatter = new BinaryFormatter();
				return (T)formatter.Deserialize( stream );
			}
	
		}


		/// <summary>
		/// Method checks if the path is a valid name for a directory path.
		/// It does NOT check if the directory actually exists.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		static public bool ValidDirChars( string path ) {

			char dirSeparator = Path.DirectorySeparatorChar;
			char altSeparator = Path.AltDirectorySeparatorChar;

			char[] illegals = Path.GetInvalidPathChars();
			for( int i = path.Length - 1; i >= 0; i-- ) {

				char c = path[i];
				if( illegals.Contains(c) ) {
					return false;
				}
				if( c == dirSeparator || c == altSeparator && (i > 0) ) {

					// check that the next character is not also a separator char.
					char prevchar = path[i - 1];
					if( prevchar == dirSeparator || prevchar == altSeparator ) {
						return false;
					}

				} //

			} // for-loop.

			return true;


		}

		/// <summary>
		/// Checks if the given name would be a valid name for a directory.
		/// Does not check if the directory actually exists.
		/// </summary>
		/// <param name="dirName"></param>
		/// <returns></returns>
		static public bool IsValidName( string dirName ) {

			char[] illegals = Path.GetInvalidFileNameChars();
			for( int i = illegals.Length - 1; i >= 0; i-- ) {

				char c = illegals[i];
				if( dirName.IndexOf( c ) >= 0 ) {
					return false;
				}

			} // for-loop.

			return true;

		}

		/// <summary>
		/// NOTE: Directory.CreateDirectory() already creates the full path.
		/// Creates all directories on the given directory path.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		/*static public bool CreateAllDirectories( string path ) {

			string[] dirs = path.Split( new char[]{ Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar } );

			int len = dirs.Length;
			for( int i = 0; i < len; i++ ) {

				Directory.CreateDirectory( dirs[i] );
	
			} // for-loop.

			return true;
		}*/

	} // class

} // namespace