using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Lemur.IO {

	static class AsyncIO {

		static async Task<string[]> GetDirectories( string path ) {

			return await Task.Run<string[]>( () => { return Directory.GetDirectories( path ); } );
			
		}

		static async Task<string[]> GetFiles( string path ) {

			return await Task.Run<string[]>( () => { return Directory.GetFiles( path ); } );

		}

	} // class

}
