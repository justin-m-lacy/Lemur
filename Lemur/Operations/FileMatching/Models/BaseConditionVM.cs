using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	[Obsolete]
	public class BaseConditionVM : ViewModelBase {

		protected BaseCondition condition;
		public BaseCondition MatchCondition {
			get { return condition; }
			set {

				this.SetProperty( ref this.condition, value );
				if( this.condition != null ) {
					this.condition.Exclude = this.Exclude;
				}

			}

		}

		private bool _exclude;
		public bool Exclude {
			get {
				return this._exclude;
			}
			set {

				if( this.SetProperty( ref this._exclude, value ) ) {
					if ( condition != null ) {
						condition.Exclude = value;
					}
				}
				
			} // set

		} // Exclude

    } // class

} // namespace