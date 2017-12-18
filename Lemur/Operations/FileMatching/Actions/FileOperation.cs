using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	public class FileOperation : ProgressOperation {

		#region PROPERTIES

		private IEnumerable<IFileAction> _actions;

		/// <summary>
		/// Conditions that files or directories must match in order
		/// to be included in a match operation.
		/// </summary>
		public IEnumerable<IFileAction> Actions {
			get { return this._actions; }

		}

		/// <summary>
		/// Condition to determine whether an action should be continued into a subdirectory?
		/// TODO: Implement this? Get rid of it?
		/// </summary>
		private IEnumerable<IMatchCondition> _recursionConditions = new List<IMatchCondition>();

		/// <summary>
		/// Conditions a directory must meet in order to continue the search into
		/// a subdirectory. Directories meeting the recursion criteria will not be included
		/// in the search results unless they also meet the MatchConditions.
		/// </summary>
		public IEnumerable<IMatchCondition> RecursionConditions {
			get { return this._recursionConditions; }
		}

		private string folderPath;
		public string FolderPath => this.folderPath;

		/// <summary>
		/// List of Exceptions encountered during the search.
		/// </summary>
		private readonly List<Exception> errorList = new List<Exception>();
		public Exception[] ErrorList {
			get { return this.errorList.ToArray(); }
		}

		private readonly List<FileSystemInfo> matches = new List<FileSystemInfo>();

		/// <summary>
		/// List of files or directories that match the search conditions.
		/// The list is cleared at the start of every call to Run()
		/// </summary>
		public List<FileSystemInfo> Matches { get { return this.matches; } }

		/// <summary>
		/// Settings used for the operation.
		/// </summary>
		private FileMatchSettings _settings;
		public FileMatchSettings Settings => _settings;

		#endregion

		public override void Run() {

			this.OperationComplete();

		}

	} // class

} // namespace
