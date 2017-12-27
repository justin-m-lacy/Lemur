using Lemur.Operations.FileMatching.Actions;
using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Lemur.Operations.FileMatching.Models {

	public class FileActionVM : ViewModelLite {

		#region PROPERTIES

		#region DISPLAY TEXT

		/// <summary>
		/// Name of the action being displayed to the user.
		/// </summary>
		public string DisplayName { get => _displayName; set => this.SetProperty( ref this._displayName, value ); }
		private string _displayName;

		/// <summary>
		/// Description of the action being applied.
		/// </summary>
		public string Desc { get => _desc; set => this.SetProperty( ref this._desc, value ); }
		private string _desc;

		#endregion DISPLAY TEXT

		/// <summary>
		/// The underlying condition being displayed.
		/// </summary>
		public IFileAction Action {
			get { return _action; }
			set {

				if( this.SetProperty( ref this._action, value ) ) {

					if( this._action != null ) {

						this.ActionType = value.GetType();

					} else {
						this.ActionType = null;
					}
					// update all property indexers on the condition.
					this.NotifyPropertyChanged( "Property[]" );

				}

			} //set

		}
		protected IFileAction _action;

		/// <summary>
		/// Gives access to properties of the underlying condition.
		/// </summary>
		/// <param name="propName"></param>
		/// <returns></returns>
		[IndexerName( "Property" )]
		public object this[string propName] {

			get {

				if( this._actionType == null ) {
					return null;
				}

				TypeInfo info = this._actionType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return null;
				}
				return prop.GetValue( this._action );

			}
			set {

				if( this._actionType == null ) {
					return;
				}

				TypeInfo info = this._actionType.GetTypeInfo();
				PropertyInfo prop = info.GetProperty( propName, BindingFlags.IgnoreCase );
				if( prop == null ) {
					return;
				}

				// check if property has changed.
				object current = prop.GetValue( this._action );
				if( !Object.Equals( current, value ) ) {

					prop.SetValue( this._action, value );
					/// unfortunately refreshes ALL indexers on Property.
					this.NotifyPropertyChanged( "Property[]" );
				}

			} // set

		}

		/// <summary>
		/// Type of the action being displayed.
		/// Use in xaml to change the display Template based on type.
		/// </summary>
		public Type ActionType {
			get {
				return this._actionType;
			}
			set {
				this.SetProperty( ref this._actionType, value );
			}
		}
		private Type _actionType;

		#endregion

		public FileActionVM() { }

		public FileActionVM( IFileAction action ) {
			this.Action = action;
		}

	} // class

} // namespace