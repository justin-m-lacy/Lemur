using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.Matching {

	public class BaseTest<T, S> : IMatchTest<T, S> {

		/// <summary>
		/// Whether to exclude matches, instead of including them.
		/// (Inverts match inclusion.)
		/// </summary>
		private bool _exclude;
		public bool Exclude {
			get { return this._exclude; }
			set { this._exclude = value; }
		}

		/// <summary>
		/// Returns the correct match behavior based on the current Exclusion value
		/// and the match result of a derived class.
		/// Derived classes should call this function to return their final result,
		/// in order to factor in the presence of the Base-class Exclude variable.
		/// </summary>
		/// <param name="derivedResult"></param>
		/// <returns></returns>
		public bool IsMatch( bool derivedResult ) {
			return derivedResult ^ this._exclude;
		}

		/// <summary>
		/// If Exclude is false, matches all items.
		/// If Exclude is true, rejects all items.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		virtual public bool IsMatch( T item, S settings ) {
			return !this._exclude;
		}

	} // class

} // namespace
