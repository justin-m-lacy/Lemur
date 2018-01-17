using Lemur.Operations.FileMatching.Actions;
using Lemur.Windows;
using Lemur.Windows.MVVM;
using Lemur.Windows.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// ViewModel seems all but required since the View needs to be able to trigger a service to select
	/// a folder from a dialog. Also no good way to bind from the DataTemplate to the
	/// overall ViewModel displaying the whole thing?
	/// </summary>
	public class MoveFileVM : DataObjectVM {

		public bool IsRelative {
			get {

				MoveFileAction action = this.Data as MoveFileAction;
				if( action == null || string.IsNullOrEmpty( action.Destination ) ) {
					return false;
				}
				return action.IsRelative;

			}
		}

		public string Destination {

			get {
				MoveFileAction action = this.Data as MoveFileAction;
				if( action == null ) {
					return string.Empty;
				}
				return action.Destination;
			}
			set {

				// The value has to be passed on to the underlying Action, or else
				// replacing 'Data' won't update the property without additional work.
				MoveFileAction action = this.Data as MoveFileAction;
				if( action != null && value != action.Destination ) {

					action.Destination = value;
					this.NotifyPropertyChanged();
					this.NotifyPropertyChanged( "IsRelative" );

				}

			}

		} //

		public RelayCommand CmdPickFolder {
			get {
				return _cmdPickFolder ?? ( _cmdPickFolder = new RelayCommand( PickFolder ) );
			}
		}
		private RelayCommand _cmdPickFolder;

		public MoveFileVM() {
		}

		private void PickFolder() {

			IFileDialogService dialog = this.GetService<IFileDialogService>();
			if( dialog != null ) {

				string dest = dialog.PickFolder( @"Choose a destination folder..." );
				if( !string.IsNullOrEmpty( dest ) ) {
					this.Destination = dest;
				}

			}

		}

	} // class

} // namespace
