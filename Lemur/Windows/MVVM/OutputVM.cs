using Lemur.Windows.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Class for basic color-coded output.
	/// </summary>
	public class OutputVM : ViewModelBase {
		
		private readonly ObservableCollection<TextString> strings = new ObservableCollection<TextString>();
		public ObservableCollection<TextString> Strings {
			get => this.strings;
		}

		public OutputVM() {
		}

		public static OutputVM operator +( OutputVM output, string s ) {
			output.Add( s );
			return output;
		}

		public static OutputVM operator +( OutputVM output, TextString s ) {
			output.Add( s );
			return output;
		}

		public void Clear() {
			this.strings.Clear();
		}

		public void AddError( string s ) {
			this.strings.Add( new TextString( s, TextString.Error ) );
		}

		public void Add( string s ) {
			this.strings.Add( new TextString( s ) );
		}

		public void Add( TextString text ) {
			this.strings.Add( text );
		}


    } // class

} // namespace
