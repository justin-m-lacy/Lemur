using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lemur.Windows.MVVM {

	public class WindowView : Window, IView {

		/*private IView _owner;
		public IView OwnerView {

			get {
				return this._owner;
			}

			set {

				Window win = value as Window;
				if( win != null ) {
					this.Owner = win;
				}
				this._owner = value;

			}
		}*/

		public void SetModel( ViewModelBase model ) {
			throw new NotImplementedException();
		}

		/*public void Show( IView Owner ) {

			this.OwnerView = Owner;
			this.Show();

		}*/

	} // class

}
