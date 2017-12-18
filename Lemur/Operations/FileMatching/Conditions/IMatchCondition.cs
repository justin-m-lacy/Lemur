using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	public interface IMatchCondition {

		bool IsMatch( FileSystemInfo info, FileMatchSettings settings );

    } // class

} // namespace