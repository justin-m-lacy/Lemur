using Lemur.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// A File Size Condition for file matching.
	/// </summary>
	public class SizeConditionVM : BaseConditionVM {

		public string _badFormatMessage = "Invalid Data Size";

		/// <summary>
		/// Message to display when the size format is incorrect.
		/// </summary>
		public string BadFormatMessage {
			get { return this._badFormatMessage; }
			set {
				if( this._badFormatMessage != value ) {
					this._badFormatMessage = value;
					this.NotifyPropertyChanged();
				}
			}
		}

		/// <summary>
		/// Minimum size to match.
		/// </summary>
		public string MinSize {
			get { return ( (SizeCondition)this.condition ).MinSize.ToString(); }
			set {

				DataSize newSize;
				if( DataSize.TryParse( value, out newSize ) ) {

					( (SizeCondition)this.condition ).MinSize = newSize;
					NotifyPropertyChanged();

				} else {
					throw new ValidationException( _badFormatMessage );
				}

			}
		}

		/// <summary>
		/// Maximum size to match.
		/// </summary>
		public string MaxSize {
			get { return ( (SizeCondition)this.condition ).MaxSize.ToString(); }
			set {


				DataSize newSize;
				if( DataSize.TryParse( value, out newSize ) ) {

					( (SizeCondition)this.condition ).MaxSize = newSize;
					NotifyPropertyChanged();

				} else {
					throw new ValidationException( _badFormatMessage );
				}

			}
		}

		public SizeConditionVM() {

			this.condition = new SizeCondition();

		}

	} // class

} // namespace
