using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	/// <summary>
	/// MatchCondition consisting of a list of MatchConditions.
	/// </summary>
	[Serializable]
	public class ConditionList : BaseCondition, IList<IMatchCondition> {

		private IList<IMatchCondition> conditions;
		public IList<IMatchCondition> Conditions {
			get { return this.conditions; }
			set { this.conditions = value; }
		}

		/// <summary>
		/// Tests if a FileSystem entry matches all listed conditions.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		public override bool IsMatch( FileSystemInfo file, FileMatchSettings settings ) {

			int len = this.conditions.Count;
			IList<IMatchCondition> conds = this.conditions;

			for( int i = 0; i < len; i++ ) {

				IMatchCondition c = conds[i];
				if( c == null ) {
					continue;
				}
				if( !c.IsMatch( file, settings ) ) {
					return base.IsMatch( false );
				}

			} // for

			return base.IsMatch( true );

		} // IsMatch()

		#region IList methods

		public IMatchCondition this[int index] { get => conditions[index]; set => conditions[index] = value; }

		public int Count => conditions.Count;

		public bool IsReadOnly => conditions.IsReadOnly;

		public void Add( IMatchCondition item ) {
			conditions.Add( item );
		}

		public void Clear() {
			conditions.Clear();
		}

		public bool Contains( IMatchCondition item ) {
			return conditions.Contains( item );
		}

		public void CopyTo( IMatchCondition[] array, int arrayIndex ) {
			conditions.CopyTo( array, arrayIndex );
		}

		public IEnumerator<IMatchCondition> GetEnumerator() {
			return conditions.GetEnumerator();
		}

		public int IndexOf( IMatchCondition item ) {
			return conditions.IndexOf( item );
		}

		public void Insert( int index, IMatchCondition item ) {
			conditions.Insert( index, item );
		}

		public bool Remove( IMatchCondition item ) {
			return conditions.Remove( item );
		}

		public void RemoveAt( int index ) {
			conditions.RemoveAt( index );
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return conditions.GetEnumerator();
		}

		#endregion

	} // class

	public class ConditionEnumeration : BaseCondition, IEnumerable<IMatchCondition> {

		private IEnumerable<IMatchCondition> conditions;
		public IEnumerable<IMatchCondition> Conditions {
			get { return this.conditions; }
			set { this.conditions = value; }
		}

		public override bool IsMatch( FileSystemInfo file, FileMatchSettings settings ) {

			foreach( IMatchCondition c in this.conditions ) {

				if( c == null ) {
					continue;
				}
				if( !c.IsMatch( file, settings  ) ) {
					return base.IsMatch( false );
				}

			} // for

			return base.IsMatch( true );

		} // IsMatch()

		public IEnumerator<IMatchCondition> GetEnumerator() {
			return conditions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return conditions.GetEnumerator();
		}

	} // class

} // namespace