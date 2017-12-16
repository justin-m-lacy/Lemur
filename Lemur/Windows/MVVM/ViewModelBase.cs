using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lemur.Windows.MVVM {

	public class ViewModelBase : INotifyPropertyChanged {

		/// <summary>
		/// Set delegate to a handler that can close a view/viewmodel
		/// when the delegate is called by a view model.
		/// Returns a bool that indicates whether the close action succeeded.
		/// </summary>
		static public Func<ViewModelBase, bool> CloseView;

		#region PROPERTIES

		/// <summary>
		/// Provides any services needed by the ViewModel.
		/// </summary>
		private IServiceProvider serviceProvider;
		public IServiceProvider ServiceProvider {
			get {
				return this.serviceProvider;
			}
			set {
				this.SetProperty( ref this.serviceProvider, value );
			}

		}
		/// <summary>
		/// ViewElement bound to the ViewMode.
		/// NOTE: This could be a problem for VM's bound to multiple Views...
		/// </summary>
		private FrameworkElement _view;
		public FrameworkElement ViewElement {
			get { return this._view; }
			set {
				this.SetProperty( ref this._view, value );
			}

		}

		#endregion

		public ViewModelBase() {
		}

		public ViewModelBase( IServiceProvider services ) {
			this.serviceProvider = services;
		}

		/// <summary>
		/// Attempts to close the calling ViewModel/View using
		/// the static CloseView delegate.
		/// </summary>
		/// <returns></returns>
		protected bool TryClose() {

			Func<ViewModelBase, bool> tryClose = ViewModelBase.CloseView;

			if ( tryClose == null ) {
				return false;
			}
			return tryClose( this );


		} // TryClose()

		public T GetService<T>() {

			if( this.serviceProvider == null ) {
				Console.WriteLine( "ViewModelBase.GetService<T>(): ServiceProvider is null" );
				return default( T );
			}
			return (T)this.serviceProvider.GetService( typeof(T) );

		}

		#region PROPERTY CHANGES

		/// <summary> 
		/// Notify ViewModel property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Updates a property and dispatches a changed event if the new value is
		/// not the same as the current value.
		/// </summary>
		/// <param name="propertyName"></param>
		protected bool SetProperty<T>( ref T original, T newVal,
			[CallerMemberName] string propertyName = "" ) {

			if( EqualityComparer<T>.Default.Equals( original, newVal ) ) {
				return false;
			}

			this.VerifyPropertyName( propertyName );

			original = newVal;

			this.PropertyChanged?.Invoke( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );

			return true;

		} //

		protected void NotifyPropertyChanged( [CallerMemberName] string propertyName="" ) {

			this.VerifyPropertyName( propertyName );
			this.PropertyChanged?.Invoke( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );

		} //

		[Conditional("DEBUG")]
		public void VerifyPropertyName( string propertyName ) {

			if( TypeDescriptor.GetProperties( this )[propertyName] == null ) {

				string msg = "Invalid property name: " + propertyName;
				throw new Exception( msg );

			}

		}

		#endregion

	} // class

} // namespace
