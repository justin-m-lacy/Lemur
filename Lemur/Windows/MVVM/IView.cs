using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Windows.MVVM {

	public interface IView {

		/*void Show();

		/// <summary>
		/// Show the View using the given Owner.
		/// </summary>
		/// <param name="Owner"></param>
		void Show( IView Owner );

		IView OwnerView {
			get;
			set;
		}*/

		void SetModel( ViewModelBase model );

	} // interface

} // namespace