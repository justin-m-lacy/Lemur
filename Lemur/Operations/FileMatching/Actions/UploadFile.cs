using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Lemur.Operations.FileMatching.Actions {

	[Serializable]
	public class UploadFile : FileActionBase {

		private static WebClient _Client;
		private static WebClient Client {
			get {
				return _Client ?? ( _Client = new WebClient() );
			}
		}

		private Uri _uri;
		public Uri Uri {
			get => this._uri;
			set => this._uri = value;
		}

		private string method;
		public string Method {
			get => this.method;
			set => this.method = value;
		}

		public override FileActionResult Run( FileSystemInfo info ) {

			WebClient web = UploadFile.Client;

			if( !string.IsNullOrEmpty( method ) ) {
				web.UploadFile( this._uri, method, info.FullName );
			} else {
				web.UploadFile( this._uri, info.FullName );

			}

			return new FileActionResult( true );

		}

	} // class

} // namespace
