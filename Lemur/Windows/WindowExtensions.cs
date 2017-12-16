using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lemur.Windows {

	public static class WindowExtensions {

		public static async Task<bool?> ShowDialogAsync( this Window self ) {

			if( self == null ) throw new ArgumentNullException( "self" );

			return await self.Dispatcher.InvokeAsync<bool?>( self.ShowDialog );

		}

    } // class

} // namespace
