using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Tasks {

	/// <summary>
	/// Progress information containing an additional item object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class ProgressInformation<T> : ProgressInformation {

		/// <summary>
		/// Current item being processed by the operation.
		/// </summary>
		public T CurrentItem {
			get; set;
		}

		/*private T info;
		public T Info {
			get { return this.info; }
			set { this.info = value; }
		}*/

	}

	/// <summary>
	/// Provides information about a task in progress that can be
	/// supplied to a Progress.ProgressChanged() Event.
	/// </summary>
	public class ProgressInformation {

		private long _progress;
		public long CurProgress {
			get { return this._progress; }
			set { this._progress = value; }
		}

		private long _maxProgress;
		public long MaxProgress {
			get { return this._maxProgress; }
			set { this._maxProgress = value; }
		}


		/// <summary>
		/// Name of the task in progress that can be displayed to the user.
		/// </summary>
		public string TaskName {
			get;
			set;
		}

		/// <summary>
		/// Information about the current item/operation being processed.
		/// </summary>
		public string Message {
			get;
			set;
		}

		private bool _complete = false;
		/// <summary>
		/// Set to true when operation is complete.
		/// </summary>
		public bool IsComplete {
			get {
				return this._complete;
			}
			set {
				this._complete = value;
			}

		}

		/// <summary>
		/// Returns current progress as a percent.
		/// </summary>
		/// <returns></returns>
		public double GetProgress() { return (double)CurProgress / MaxProgress; }

		public ProgressInformation( string name, long maxValue ) {

			this.TaskName = name;
			this.MaxProgress = maxValue;
			this.CurProgress = 0;

		}

		public ProgressInformation() {
			this.TaskName = "";
			this.MaxProgress = 0;
			this.CurProgress = 0;
		}

	} // class

} // namespace
