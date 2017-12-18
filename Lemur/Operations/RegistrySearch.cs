using Lemur.Tasks;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations {

	/// <summary>
	/// Searches registry for keys while providing real-time updates
	/// to the keys found. (Since the search could take some time.)
	/// </summary>
	public class RegistrySearch : ProgressOperation {

		private RegistryKey[] _baseKeys;

		/// <summary>
		/// String to match within the Registry.
		/// </summary>
		private string matchString;

		/// <summary>
		/// Whether to search recursively. True by default.
		/// </summary>
		private bool recursive;

		private List<Exception> errors;

		/// <summary>
		/// Returns an array of Exceptions accumulated during the operation.
		/// </summary>
		public Exception[] Errors {
			get { return this.errors.ToArray(); }
		}

		private ObservableCollection<string> matches;

		/// <summary>
		/// Initializes a Search from the given start key.
		/// </summary>
		/// <param name="searchStart"></param>
		/// <param name="match"></param>
		/// <param name="recursive"></param>
		public RegistrySearch( RegistryKey searchStart, string match, bool recursive=true ) {

			this._baseKeys = new RegistryKey[]{ searchStart };

			this.matchString = match;
			this.recursive = recursive;

		}

		/// <summary>
		/// Initialize a RegistrySearch with an array of starting keys.
		/// </summary>
		/// <param name="startKeys"></param>
		/// <param name="match"></param>
		/// <param name="recursive"></param>
		public RegistrySearch( RegistryKey[] startKeys, string match, bool recursive = true ) {

			this._baseKeys = startKeys;

			this.matchString = match;
			this.recursive = recursive;

		}

		override public void Run() {

			this.matches = new ObservableCollection<string>();
			this.errors = new List<Exception>();

			if( this._baseKeys == null ) {
				throw new NullReferenceException( "No RegistryKeys included in the search" );
			}
			if( this.matchString == null ) {
				throw new NullReferenceException( "Match string is null" );
			}

			foreach( RegistryKey key in this._baseKeys ) {

				this.SearchKey( key );

			}

			this.OperationComplete();

		} // Run()

		/// <summary>
		/// Performs the recursive search.
		/// </summary>
		/// <param name="parentKey"></param>
		private void SearchKey( RegistryKey parentKey ) {

			try {

				string[] sub_names = parentKey.GetSubKeyNames();
				this.AdvanceMaxProgress( sub_names.Length );

				foreach( string keyName in sub_names ) {

					if( keyName.Contains( this.matchString ) ) {
						this.matches.Add( keyName );
					}

					try {

						using( RegistryKey childKey = parentKey.OpenSubKey( keyName ) ) {

							this.SearchKey( childKey );

						}

						if( this.CancelRequested() ) {
							return;
						}

						/// advance progress by a search of a single key.
						this.AdvanceProgress();

					} catch( Exception e ) {
						this.errors.Add( e );
					}

	
				} // for-loop.

			} catch( Exception e ) {
				this.errors.Add( e );
			}

		}

		protected override void DisposeUnmanaged() {

			if( this._baseKeys != null ) {

				foreach( RegistryKey key in this._baseKeys ) {
					key.Dispose();
				}
				this._baseKeys = null;

			}

		}

	} // class

} // namespace