using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur {

	/// <summary>
	/// Visits items from a list as the 'Current' item.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IListVisitor<T> {

		T Current { get; }
		int CurrentIndex { get; set; }

		/// <summary>
		/// Indicates if the visitor should loop when the end of a
		/// list is reached ( moving forward or backwards. )
		/// </summary>
		bool Looping { get; set; }

		void Prev();
		void Next();

	} // interface

} // namespace