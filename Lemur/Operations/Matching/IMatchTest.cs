using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.Matching {

	/// <summary>
	/// TODO: Possible future interface to replace individual Match testing operations
	/// (RegistryMatchTesting, FileMatchTesting, etc... with a single base class. )
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="S"></typeparam>
	public interface IMatchTest<T,S> {

		bool IsMatch( T item, S settings );

    } // class

} // namespace