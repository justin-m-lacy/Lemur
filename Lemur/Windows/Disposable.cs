using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows {

	public abstract class Disposable : IDisposable {

		private bool _isDisposed;
		public bool IsDisposed {
			get { return this._isDisposed; }
		}
	
		/// <summary>
		/// Frees and releases unmanaged resources, apparently.
		/// </summary>
		public void Dispose() {
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		public void Dispose( bool disposeCalled ) {

			if( this._isDisposed ) {
				return;
			}

			if( disposeCalled ) {
				this.DisposeManaged();
			}
			this.DisposeUnmanaged();
			this._isDisposed = true;
	
		} //

		protected virtual void DisposeManaged() {
		}

		protected virtual void DisposeUnmanaged() {
		}

		~Disposable() {
			this.Dispose( false );
		} // ~Dispose()

	} // class

} // namespace
