using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	public class BaseConditionVM : ViewModelBase {

		protected BaseCondition condition;
		public BaseCondition MatchCondition {
			get { return condition; }
		}

		private bool _exclude;
		public bool Exclude {
			get {
				return this._exclude;
			}
			set {

				if( value == this._exclude ) {
					return;
				}
	
				this._exclude = value;
				if( this.condition != null ) {
					condition.Exclude = value;
				}
				
			} // set

		} // Exclude

    } // class

} // namespace