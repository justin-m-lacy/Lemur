using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Settings {

	public class EnumSetting : Setting {

		/// <summary>
		/// Name,Description pairs for possible values of the setting.
		/// </summary>
		readonly Dictionary<Enum, ValueDesc> _valueDisplays = new Dictionary<Enum, ValueDesc>();

		public int Count => this._valueDisplays.Count;

		public bool IsReadOnly => ( (ICollection<KeyValuePair<Enum, ValueDesc>>)_valueDisplays ).IsReadOnly;

		public EnumSetting() {}

		public void SetDesc( Enum e, ValueDesc d ) {
			this._valueDisplays[e] = d;
		}

		public bool TryGetDesc( Enum e, out ValueDesc d ) {
			
			if( this._valueDisplays.TryGetValue( e, out d ) ) {
				return true;
			}
			return false;

		}

		public ValueDesc GetDesc( Enum e ) {

			ValueDesc d;
			if( this._valueDisplays.TryGetValue( e, out d ) ) {
				return d;
			}
			return new ValueDesc( e.ToString(), string.Empty );

		}

		public IEnumerator< KeyValuePair<Enum, ValueDesc>> GetEnumerator() {
			return this._valueDisplays.GetEnumerator();
		}

		public void Add( KeyValuePair<Enum, ValueDesc> item ) {
			( (ICollection<KeyValuePair<Enum, ValueDesc>>)_valueDisplays ).Add( item );
		}

		public void Clear() {
			_valueDisplays.Clear();
		}

		public bool Contains( KeyValuePair<Enum, ValueDesc> item ) {
			return ( (ICollection<KeyValuePair<Enum, ValueDesc>>)_valueDisplays ).Contains( item );
		}

		public void CopyTo( KeyValuePair<Enum, ValueDesc>[] array, int arrayIndex ) {
			( (ICollection<KeyValuePair<Enum, ValueDesc>>)_valueDisplays ).CopyTo( array, arrayIndex );
		}

		public bool Remove( KeyValuePair<Enum, ValueDesc> item ) {
			return ( (ICollection<KeyValuePair<Enum, ValueDesc>>)_valueDisplays ).Remove( item );
		}
	} // class

} // namespace
