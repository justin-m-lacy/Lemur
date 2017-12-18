using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// Interface for testing a FileSystem entry against a given condition.
	/// </summary>
	public interface IMatchCondition {

		/// <summary>
		/// Whether an item that matches the test condition is excluded from the results,
		/// rather than included ( default ).
		/// </summary>
		bool Exclude { get; set; }

		/// <summary>
		/// Method to determine if a given FileSystem object matches the test condition.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		bool IsMatch( FileSystemInfo info, FileMatchSettings settings );

    } // class

} // namespace