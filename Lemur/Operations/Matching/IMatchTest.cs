using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.Matching {

	public interface IMatchTest<T,S> {

		bool IsMatch( T item, S settings );

    } // class

} // namespace