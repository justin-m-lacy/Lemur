using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Utils {

	static class CollectionUtils {

		static public void Enqueue<T>( this Queue<T> queue, IEnumerable<T> items ) {

			foreach( T item in items ) {
				queue.Enqueue( item );
			}

		} // Enqueue()

		static public string Join( string[] a ) {

			StringBuilder b = new StringBuilder();
			int len = a.Length;
			for ( int i = 0; i < len; i++ ) {
				b.Append( a[i] );
			}

			return b.ToString();

		} // Join

		///NOTE: don't need. Use Array<>.Distinct()
		/*static char[] RemoveDuplicates( this char[] chars ) {
		}*/

		/*static public int IndexOf<T>( this T[] a, T elm ) {

		int len = a.Length;
				for ( int i = 0; i < len; i-- ) {

			if ( a[i].Equals(elm) ) {
				return i;
				}
			}

			return -1;

		}*/

	} // class

} // namespace