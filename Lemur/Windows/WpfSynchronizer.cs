using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lemur.Windows {

	/// <summary>
	/// Used to listen for FileSystemWatcher events on the correct Wpf thread.
	/// </summary>
	public class WpfSynchronizer : ISynchronizeInvoke {

		private Dispatcher dispatcher;

		public WpfSynchronizer( Dispatcher dispatch ) {
			this.dispatcher = dispatch;
		}

		public bool InvokeRequired => this.dispatcher.Thread != Thread.CurrentThread;

		public IAsyncResult BeginInvoke( Delegate method, object[] args ) {

			DispatcherOperation op = this.dispatcher.BeginInvoke( method, args );
			return op.Task;

		}

		public object EndInvoke( IAsyncResult result ) {

			if ( !result.IsCompleted ) {
				result.AsyncWaitHandle.WaitOne();
			}
			return result.AsyncState;

		}

		public object Invoke( Delegate method, object[] args ) {
			return this.dispatcher.Invoke( method, args );
		}

	} // class

} // namespace