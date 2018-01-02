using Lemur.Types;
using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Operations.FileMatching {

	[TypeConverter(typeof(EnumNameDescConverter))]
	public enum MatchType {

		[Description("Match Files" )]
		Files=1,

		[Description( "Match Directories" )]
		Directories=2,

		[Description("Match Both" )]
		Both = Files | Directories

	}

	/// <summary>
	/// Settings which can be applied to a Match attempt.
	/// Settings were made a class to make it Bindable in MVVM.
	/// </summary>
	public class FileMatchSettings : INotifyPropertyChanged {

		public event PropertyChangedEventHandler PropertyChanged;

		private void NotifyPropertyChanged( [CallerMemberName] string propName=null ) {
			this.PropertyChanged?.Invoke( this, new PropertyChangedEventArgs(propName) );
		}

		public FileMatchSettings() {}

		/// <summary>
		/// Constructor for copying an existing set of settings.
		/// </summary>
		/// <param name="settings"></param>
		public FileMatchSettings( FileMatchSettings settings ) {

			this.recursive = settings.recursive;
			this.types = settings.types;
			this.sizeRange = settings.SizeRange;
			this.useSizeRange = settings.useSizeRange;
			this.deleteEmptyFiles = settings.deleteEmptyFiles;
			this.moveToTrash = settings.moveToTrash;

		}

		/// <summary>
		/// Whether the operation is applied recursively.
		/// </summary>
		private bool recursive = false;
		public bool Recursive {
			get { return this.recursive; }
			set {
				if( this.recursive != value ) {
					this.recursive = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		private MatchType types = MatchType.Both;
		public MatchType Types {
			get { return this.types; }
			set {
				if( this.types != value ) {
					this.types = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Whether to attempt to delete all files in a given size range.
		/// false by default.
		/// </summary>
		private bool useSizeRange = false;
		public bool UseSizeRange {
			get { return this.useSizeRange; }
			set {
				if( this.useSizeRange != value ) {
					this.useSizeRange = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Size range of files to delete, if a size range is specified.
		/// NOTE: A size range could be a condition by itself, though this setting
		/// could apply winnowing before the condition is even applied?
		/// Might be a bad setting...
		/// </summary>
		private DataRange sizeRange;
		public DataRange SizeRange {
			get { return this.sizeRange; }
			set {
				if( this.sizeRange != value ) {
					this.sizeRange = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Whether to delete any files of size 0. This is in addition
		/// to the option to use a more general size range.
		/// TODO: unnecessary. this would be a condition.
		/// </summary>
		private bool deleteEmptyFiles = false;
		public bool DeleteEmptyFiles {
			get { return this.deleteEmptyFiles; }
			set {
				if( this.deleteEmptyFiles != value ) {
					this.deleteEmptyFiles = value;
					this.NotifyPropertyChanged();
				}
			}

		}

		/// <summary>
		/// Whether files should be moved to trash on delete.
		/// </summary>
		private bool moveToTrash = true;
		public bool MoveToTrash {
			get { return this.moveToTrash; }
			set {
				if( this.moveToTrash != value ) {
					this.moveToTrash = value;
					this.NotifyPropertyChanged();
				}
			}
		}

	} // class

} // namespace
