using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.Services {

	public interface IFileDialogService {

		string PickLoadFile( string dialogTitle, string defaultPath = null, string defaultFileName = null );
		string PickSaveFile( string dialogTitle, string defaultPath = null, string defaultFileName = null );
		string PickFolder( string dialogTitle, string defaultPath = null );

	} // class

} // namespace
