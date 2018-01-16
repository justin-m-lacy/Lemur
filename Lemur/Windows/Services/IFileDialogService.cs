using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Services {

	public interface IFileDialogService {

		string PickOpenFile( string dialogTitle, string defaultPath = null, string defaultExtension = null );
		string PickSaveFile( string dialogTitle, string defaultPath = null, string defaultFileName = null, string defaultExtension=null );
		string PickFolder( string dialogTitle, string defaultPath = null );

	} // class

} // namespace