using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Lemur.Windows;

namespace Lemur.Operations.FileMatching.Models {

	/// <summary>
	/// Placeholder before a specific condition is selected. A list of all available
	/// FileMatchConditions must be provided.
	/// 
	/// These controls are made a separate viewmodel so they can apppear in a list
	/// of existing Match Conditions as an item to be modified and added.
	/// </summary>
	public class PlaceholderVM : ViewModelBase {

		/// <summary>
		/// All available matching conditions.
		/// </summary>
		private static List<Type> conditions;
		protected static List<Type> Conditions {
			get {
				return PlaceholderVM.conditions ?? ( PlaceholderVM.conditions = FindConditionTypes() );
			}
		}

		private RelayCommand<Type> _cmdPickCondition;
		/// <summary>
		/// Called to select a condition to create.
		/// </summary>
		public RelayCommand<Type> CmdPickCondition {

			get {
				return this._cmdPickCondition ??
					( this._cmdPickCondition = new RelayCommand<Type>(
						this.DispatchConditionSelected )
					);
			}

		}

		/// <summary>
		/// Event triggers when a condition has been selected.
		/// </summary>
		public event Action<Type> OnConditionSelected;
		protected void DispatchConditionSelected( Type t ) {
			this.OnConditionSelected?.Invoke( t );
		}


		public List<Type> AvailableConditions {
			get { return PlaceholderVM.Conditions; }
		}

		public PlaceholderVM() {
		}

		static public List<Type> FindConditionTypes() {

			Type[] available = Assembly.GetCallingAssembly().GetTypes();

			Type conditionType = typeof( BaseCondition );

			List<Type> results = new List<Type>();

			foreach( Type t in available ) {

				if( conditionType.IsAssignableFrom(t) ) {
					results.Add( t );
				}

			} // foreach

			return results;

		}

    } // class

} // namespace
