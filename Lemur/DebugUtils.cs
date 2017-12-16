using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Debug {

	public static class DebugUtils {

		[Conditional("DEBUG")]
		static public void Log( string s ) {
			Console.WriteLine( s );
		}

	} //

} //