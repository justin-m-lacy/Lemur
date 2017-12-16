using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Threading {

	/// <summary>
	/// Interface for providing a lock object that can be locked
	/// to synchronize access to shared variables.
	/// NOTE: no longer recommended.
	/// </summary>
	public interface ISyncRoot {

		object SyncRoot {
			get;
		}

    } // interface

} // namespace