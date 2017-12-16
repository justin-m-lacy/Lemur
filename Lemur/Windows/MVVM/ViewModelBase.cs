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

		/// <summary> 
		/// Notify ViewModel property changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		public const string ServiceProviderString = "ServiceProvider";

		/// <summary>
		/// Provides any services needed by the ViewModel.
		/// </summary>
		private IServiceProvider serviceProvider;
		public IServiceProvider ServiceProvider {
			get {
				return this.serviceProvider;
			}
			set {
				if( this.serviceProvider != value ) {
					this.serviceProvider = value;
					this.NotifyPropertyChanged( ServiceProviderString );
				}
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
				if( _view != value ) {
					this._view = value;
					this.NotifyPropertyChanged( "ViewElement" );
				}
			}

		}

		public T GetService<T>() {

			if( this.serviceProvider == null ) {
				Console.WriteLine( "ViewModelBase.GetService<T>(): ServiceProvider is null" );
				return default( T );
			}
			return (T)this.serviceProvider.GetService( typeof(T) );

		}

		/// <summary>
		/// Updates property and notifies a change if the new value is not the same as the previous value.
		/// </summary>
		/// <param name="propertyName"></param>
		protected virtual void TryPropertyChanged( ref object oldValue, object newVal,
			[CallerMemberName] string propertyName = "" ) {

			if( oldValue.Equals( newVal ) ) {
				return;
			}

			this.VerifyPropertyName( propertyName );
			this.PropertyChanged?.Invoke( this, new System.ComponentModel.PropertyChangedEventArgs( propertyName ) );

		} //

		protected virtual void NotifyPropertyChanged( [CallerMemberName] string propertyName="" ) {

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

	} // class

} // namespace
