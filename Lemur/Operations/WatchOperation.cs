using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Lemur.Operations {

	public class WatchOperation<T> : IProgress<T>, IDisposable {

		private bool _disposed;
		IProgress<T> _progress;
		CancellationTokenSource _tokenSource;

		public CancellationToken Token {
			get => this._tokenSource.Token;
		}

		public WatchOperation( IProgress<T> progress, CancellationTokenSource source=null ) {

			if( source == null ) {
				this._tokenSource = new CancellationTokenSource();
			} else {
				this._tokenSource = source;
			}

		} // WatchOperation()

		public void Report( T value ) {
			_progress.Report( value );
		}

		public void Dispose() {

			if( this._disposed ) {
				return;
			}

			this._disposed = true;

			if( this._tokenSource != null ) {
				this._tokenSource.Dispose();
				this._tokenSource = null;
			}

		}

	} // class

} // namespace
