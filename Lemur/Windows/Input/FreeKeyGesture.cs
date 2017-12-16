using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Input;

namespace Lemur.Windows.Input {

	/// <summary>
	/// KeyGesture without forced modifier keys.
	/// </summary>
	[Serializable]
	public class FreeKeyGesture : InputGesture, ISerializable {

		private const char SEPARATOR_CHAR = '+';

		ModifierKeys modifiers;
		Key key;

		public FreeKeyGesture( Key key ) {

			this.key = key;
			this.modifiers = ModifierKeys.None;

		}

		public FreeKeyGesture( Key key, ModifierKeys modifiers ) {

			this.key = key;
			this.modifiers = modifiers;

		}

		public override bool Matches( object targetElement, InputEventArgs inputEventArgs ) {

			KeyEventArgs keyEvent = inputEventArgs as KeyEventArgs;
			if( keyEvent == null ) {
				return false;
			}


			if( (keyEvent.Key == this.key) && (Keyboard.Modifiers & this.modifiers) == this.modifiers ) {
				return true;
			}
			return false;

		}

		protected FreeKeyGesture( SerializationInfo info, StreamingContext context ) {

			this.key = (Key)info.GetValue( "key", typeof( Key ) );
			this.modifiers = (ModifierKeys)info.GetValue( "modifiers", typeof( ModifierKeys ) );

			//Console.WriteLine( "DESERIALIZING KEY: " + this.key );

		}

		public virtual void GetObjectData( SerializationInfo info, StreamingContext context ) {

			info.AddValue( "key", this.key );
			info.AddValue( "modifiers", this.modifiers );

			Console.WriteLine( "SERIALIZING: " + this.key );

		} //

		/// <summary>
		/// Override Equals() to identify logically equal gestures.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals( object obj ) {

			if( obj == null || obj.GetType() != this.GetType() ) {
				return false;
			}
			FreeKeyGesture other = (FreeKeyGesture)obj;
			return other.key == this.key && other.modifiers == this.modifiers;

		}

		public override int GetHashCode() {
			return this.key.GetHashCode() ^ this.modifiers.GetHashCode();
		}

		public override string ToString() {

			if( modifiers == ModifierKeys.None ) {
				return key.ToString();
			} else {
				return key.ToString() + SEPARATOR_CHAR + modifiers.ToString();
			}

		}

	} // class

} // namespace
