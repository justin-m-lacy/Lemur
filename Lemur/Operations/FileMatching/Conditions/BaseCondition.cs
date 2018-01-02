using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	[Serializable]
	public abstract class BaseCondition : IMatchCondition {

		/// <summary>
		/// Whether to exclude matches, instead of including them.
		/// (Inverts the inclusion.)
		/// </summary>
		private bool _exclude;
		public bool Exclude {
			get { return this._exclude; }
			set { this._exclude = value; }
		}

		/*virtual public void GetObjectData( SerializationInfo info, StreamingContext context ) {
			throw new NotImplementedException();
		}*/

		/// <summary>
		/// Returns the correct match behavior based on the current Exclusion value
		/// and the match result of a derived class.
		/// Derived classes should call this function to return their final result,
		/// to factor in the presence of the Base-class Exclude variable.
		/// </summary>
		/// <param name="derivedResult"></param>
		/// <returns></returns>
		public bool IsMatch( bool derivedResult ) {
			return derivedResult ^ this._exclude;
		}

		/// <summary>
		/// If exclude is false, matches all entries.
		/// If exclude is true, rejects all entries.
		/// </summary>
		/// <param name="info"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		virtual public bool IsMatch( FileSystemInfo info ) {
			return !this._exclude;
		}

	} // class

} // namespace