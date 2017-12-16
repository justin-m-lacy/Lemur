using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.Matching {

	/// <summary>
	/// Matches items which pass every match-test in the list.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="S"></typeparam>
	public class TestList<T,S> : BaseTest<T,S> {

		private IList< IMatchTest<T,S> > conditions;
		public IList<IMatchTest<T, S>> Conditions {
			get { return this.conditions; }
			set { this.conditions = value; }
		}

		/// <summary>
		/// Tests if an item matches all Tests in the list.
		/// </summary>
		/// <param name="file"></param>
		/// <param name="settings"></param>
		/// <returns></returns>
		public override bool IsMatch( T file, S settings ) {

			int len = this.conditions.Count;
			IList<IMatchTest<T, S>> conds = this.conditions;

			for( int i = 0; i < len; i++ ) {

				IMatchTest<T, S> c = conds[i];
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

		public IMatchTest<T, S> this[int index] { get => conditions[index]; set => conditions[index] = value; }

		public int Count => conditions.Count;

		public bool IsReadOnly => conditions.IsReadOnly;

		public void Add( IMatchTest<T, S> item ) {
			conditions.Add( item );
		}

		public void Clear() {
			conditions.Clear();
		}

		public bool Contains( IMatchTest<T, S> item ) {
			return conditions.Contains( item );
		}

		public void CopyTo( IMatchTest<T, S>[] array, int arrayIndex ) {
			conditions.CopyTo( array, arrayIndex );
		}

		public IEnumerator<IMatchTest<T, S>> GetEnumerator() {
			return conditions.GetEnumerator();
		}

		public int IndexOf( IMatchTest<T, S> item ) {
			return conditions.IndexOf( item );
		}

		public void Insert( int index, IMatchTest<T, S> item ) {
			conditions.Insert( index, item );
		}

		public bool Remove( IMatchTest<T, S> item ) {
			return conditions.Remove( item );
		}

		public void RemoveAt( int index ) {
			conditions.RemoveAt( index );
		}


		#endregion

	} // class

	/// <summary>
	/// Matches items which pass every match-test in the Enumeration.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <typeparam name="S"></typeparam>
	public class TestEnumeration<T,S> : BaseTest<T,S>, IEnumerable< IMatchTest<T,S> > {

		private IEnumerable<IMatchTest<T, S>> conditions;
		public IEnumerable<IMatchTest<T, S>> Conditions {
			get { return this.conditions; }
			set { this.conditions = value; }
		}

		public override bool IsMatch( T file, S settings ) {

			foreach( IMatchTest<T, S> c in this.conditions ) {

				if( c == null ) {
					continue;
				}
				if( !c.IsMatch( file, settings ) ) {
					return base.IsMatch( false );
				}

			} // for

			return base.IsMatch( true );

		} // IsMatch()

		public IEnumerator<IMatchTest<T, S>> GetEnumerator() {
			return conditions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator() {
			return conditions.GetEnumerator();
		}

	} // class

} // namespace
