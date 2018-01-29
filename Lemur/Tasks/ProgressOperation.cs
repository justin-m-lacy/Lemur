using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Lemur.Tasks {

	  /// <summary>
	  /// Combines Progress<> with cancellation capability.
	  /// WARNING: A ProgressOperation must be created on the SynchronizationContext
	  /// from which it will be cancelled. This is typically the UI-thread.
	  /// </summary>
	public abstract class ProgressOperation : Progress<ProgressInformation>, IDisposable {

		#region PROPERTIES

		/// <summary>
		/// Context for dispatching events.
		/// </summary>
		private Dispatcher dispatcher;

		private ProgressInformation progressInfo;
		/// <summary>
		/// Information about the current progress of the operation.
		/// </summary>
		public ProgressInformation ProgressInformation {
			get {
				return this.progressInfo;
			}
		}

		/// <summary>
		/// Indicates whether the operation is currently in progress.
		/// </summary>
		public bool IsRunning { get { return !this.progressInfo.IsComplete; } }

		private bool _disposed = false;
		public bool IsDisposed { get { return this._disposed; } }

		/// <summary>
		/// Maximum value for the operation's progress.
		/// </summary>
		protected long MaxProgress {
			get {
				return this.progressInfo.MaxProgress;
			}
			set {
				this.progressInfo.MaxProgress = value;
				this.OnReport( this.progressInfo );
			}
		}

		/// <summary>
		/// Current progress of the operation.
		/// </summary>
		protected long CurProgress {
			get {
				return this.progressInfo.CurProgress;
			}
		}

		#endregion

		/// <summary>
		/// Allows for overriding of ProgressInformation type in subclass.
		/// </summary>
		/// <returns></returns>
		protected ProgressInformation CreateInformation() {
			return new ProgressInformation();
		}

		/// <summary>
		/// Creates a new ProgressOperation.
		/// If specified, the Dispatcher can be used by subclasses to make changes
		/// to UI-related data, or marshal events.
		/// </summary>
		/// <param name="dispatch"></param>
		public ProgressOperation( Dispatcher dispatch=null) {

			this.cancelSource = new CancellationTokenSource();
			this.token = cancelSource.Token;

			this.progressInfo = this.CreateInformation();

			if( dispatch != null ) {
				this.dispatcher = dispatch;
			} else {
				this.dispatcher = Dispatcher.CurrentDispatcher;
			}

		} // ProgressOperation()

		#region PROGRESS UPDATES

		protected void SetMaxProgress( long amount ) {

			this.progressInfo.MaxProgress = amount;
			this.OnReport( this.progressInfo );

		}

		/// <summary>
		/// Adds a specified amount to the maximum progress of the operation.
		/// This is useful when the amount of work to be done is calculated
		/// and adjusted as the operation progresses ( such as finding new files
		/// to process. )
		/// </summary>
		/// <param name="amount"></param>
		protected void AdvanceMaxProgress( long amount = 1 ) {
			this.progressInfo.MaxProgress += amount;
			this.OnReport( this.progressInfo );
		}

		/// <summary>
		/// Advances the amount of progress completed and Reports the changes
		/// to any Progress listeners.
		/// </summary>
		/// <param name="amount"></param>
		protected void AdvanceProgress( long amount = 1 ) {

			this.progressInfo.CurProgress += amount;
			//Console.WriteLine( "newProgress: " + this.progressInfo.curProgress );
			this.OnReport( this.progressInfo );

		}

		/// <summary>
		/// Subclasses should call to signal that an Operation is complete.
		/// Sets the current progress to the maximum progress, marks the operation
		/// as complete, and reports the change to Progress listeners.
		/// </summary>
		protected void OperationComplete() {

			this.progressInfo.IsComplete = true;
			this.progressInfo.CurProgress = this.progressInfo.MaxProgress;
			this.OnReport( this.progressInfo );

		}

		/// <summary>
		/// Resets the progress to 0 and sets complete to false.
		/// </summary>
		protected void ResetProgress() {

			this.progressInfo.CurProgress = 0;
			this.progressInfo.IsComplete = false;
			this.OnReport( this.progressInfo );
		}

		#endregion

		/// <summary>
		/// Dispatch to the UI thread.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="action"></param>
		/// <param name="data"></param>
		protected void Dispatch<T>( Action<T> action, T data ) {

			this.dispatcher.Invoke( action, data );

		} // Send()

		protected void Dispatch( Action action ) {
			this.dispatcher.Invoke( action );
		}

		/// <summary>
		/// Override to run the operation.
		/// </summary>
		abstract public void Run();

		/// <summary>
		/// Method will not usually need to be overridden in a subclass because it simply
		/// wraps the Run() method in an awaitable Task.
		/// </summary>
		/// <returns></returns>
		virtual public async Task RunAsync() {

			await Task.Run( (Action)this.Run );

		}

		#region CANCELLATION

		/// <summary>
		/// A source that can be used to cancel the Operation.
		/// </summary>
		private CancellationTokenSource cancelSource;
		private CancellationToken token;

		/// <summary>
		/// Returns a CancellationToken that can be used to cancel other
		/// Tasks when this task is ended.
		/// </summary>
		/// <returns></returns>
		public CancellationToken GetToken() {
			return this.token;
		}


		/// <summary>
		/// Wraps CancelletionToken IsCanellationRequested
		/// </summary>
		/// <returns></returns>
		protected bool CancelRequested() {
			return this.token.IsCancellationRequested;
		}

		/// <summary>
		/// Cancels an Operation in progress.
		/// This technically only requests the operation to cancel. The operation must
		/// return from any running methods.
		/// </summary>
		public void Cancel() {

			if ( !this._disposed ) {
				this.cancelSource.Cancel();
			}

		}

		#endregion

		/// <summary>
		/// Attempts to dispose the Operation. If the operation
		/// is still running, it will first be cancelled.
		/// </summary>
		public void Dispose() {
			this.Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <summary>
		/// Subclasses override to dispose their own resources.
		/// </summary>
		virtual protected void DisposeUnmanaged() { }

		protected void Dispose( bool not_finalizer ) {

			if( this._disposed ) {
				return;
			}

			this.DisposeUnmanaged();

			if( this.cancelSource != null ) {

				if( this.IsRunning ) {
					this.Cancel();
				}
				this.cancelSource.Dispose();
				this.cancelSource = null;
			}

			if( not_finalizer ) {
				this.dispatcher = null;
				this.progressInfo = null;
			}

		} //

		~ProgressOperation() {
			this.Dispose( false );
		}

	} // class

	

} // namespace