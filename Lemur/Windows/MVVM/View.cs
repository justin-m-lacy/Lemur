using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lemur.Windows.MVVM {

	public class View : IView {

		private Window window = null;

		public IView OwnerView {
			get {
				if( this.window != null ) {
					return this.window as IView;
				}
				return null;
			}
			set {

				Window win = value as Window;
				if( win != null ) {

					if( this.window != null ) {
						this.window.Owner = win;
					}

				}

			}

		} // OwnerView()

		public void SetModel( ViewModelBase model ) {
			throw new NotImplementedException();
		}

		public void Show() {
			if( this.window != null ) {
				this.window.Show();
			}
		}

		public void Show( IView Owner ) {

			if( this.window != null ) {

				this.OwnerView = Owner;
				this.window.Show();

			}

		}

	} // class

}
