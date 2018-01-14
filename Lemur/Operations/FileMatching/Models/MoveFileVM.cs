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

		public string Destination { get => destination;

			set {


				if( SetProperty( ref this.destination, value ) ) {

					/// SetProperty before directory test, because invalid text still gets displayed.
					/// If the incorrect displayed value isn't saved here, then changing the text back to
					/// the correct current value won't trigger a PropertyChangedEvent, and the error won't
					/// remove.
					if( !Directory.Exists( value ) ) {
						throw new ValidationException( "Directory does not exist." );
					}
					( (MoveFileAction)this.Data ).Destination = value;


				}

			}

		}
		private string destination;

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

				string dest = dialog.PickFolder( "Choose a destination folder..." );
				if( !string.IsNullOrEmpty( dest ) ) {
					this.Destination = dest;
				}

			}

		}

	} // class

} // namespace
