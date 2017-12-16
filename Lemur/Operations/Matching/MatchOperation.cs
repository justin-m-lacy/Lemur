using Lemur.Tasks;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.Matching {

	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T">The type of objects being tested for matches.</typeparam>
	/// <typeparam name="S">The class or enum which provides settings when running
	/// the Match operation.</typeparam>
	public class MatchOperation<T,S> : ProgressOperation {

		#region PROPERTIES

		/// <summary>
		/// Conditions that files or directories must match in order
		/// to be included in a match operation.
		/// </summary>
		IEnumerable< IMatchTest<T,S> > _conditions;

		public IEnumerable<IMatchTest<T, S> > Conditions {
			get { return this._conditions; }

		}

		/// <summary>
		/// List of Exceptions encountered during the search.
		/// </summary>
		private readonly List<Exception> errorList = new List<Exception>();

		/// <summary>
		/// List of Exceptions encountered during the search.
		/// Exceptions are cleared at the start of the Run() method.
		/// </summary>
		public Exception[] ErrorList {
			get { return this.errorList.ToArray(); }
		}

		/// <summary>
		/// Item matches found in the Current operation.
		/// The list is cleared after every run.
		/// </summary>
		private readonly List<T> matches = new List<T>();

		/// <summary>
		/// List of items which match the search criteria.
		/// The List is cleared at the start of the Run() method.
		/// </summary>
		public List<T> Matches { get { return this.matches; } }

		/// <summary>
		/// Settings used for the operation.
		/// </summary>
		private S _settings;
		public S Settings => _settings;

		/// <summary>
		/// Provides the items being tested by the operation.
		/// </summary>
		private IEnumerable<T> _itemProvider;
		public IEnumerable<T> ItemProvider {
			get {
				return this._itemProvider;
			}
			set {
				this._itemProvider = value;
			}
		}

		#endregion

		public MatchOperation( string basePath, S settings = default(S),
			IEnumerable<IMatchTest<T, S>> conditions = null ) {

			this._settings = settings;
			this._conditions = conditions;

		}

		public override void Run() {

			this.matches.Clear();
			this.errorList.Clear();

		}

		/// <summary>
		/// Returns true if file at path is a valid match,
		/// false otherwise.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		private bool TestItem( T item ) {

			foreach( IMatchTest<T, S> cond in this.Conditions ) {

				if( !cond.IsMatch( item, _settings ) ) {
					return false;
				}

			} // foreach

			return true;

		} //

	} // class

} // namespace
