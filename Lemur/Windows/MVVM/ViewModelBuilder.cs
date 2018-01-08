using System;
using System.Collections.Generic;
using System.Text;

namespace Lemur.Windows.MVVM {

	/// <summary>
	/// Links data types to the ViewModel types that should be instantiated to represent them.
	/// </summary>
	public class ViewModelBuilder {

		/// <summary>
		/// Given an object, delegate must return a ViewModel that represents the
		/// object.
		/// </summary>
		/// <param name="data"></param>
		/// <returns></returns>
		public delegate object ViewModelCreator( object data, object view=null );

		/// <summary>
		/// Creator to use if no other is found for a given data Type.
		/// </summary>
		private ViewModelCreator fallbackCreator;
		public ViewModelCreator FallbackCreator {
			get => fallbackCreator;
			set => fallbackCreator = value;
		}

		/// <summary>
		/// Matches Data Types with creators that can instantiate ViewModels
		/// for that DataType.
		/// </summary>
		private Dictionary<Type, ViewModelCreator> creators;

		private Dictionary<Type, Type> viewModels;

		public ViewModelBuilder() {

			this.creators = new Dictionary<Type, ViewModelCreator>();
			this.viewModels = new Dictionary<Type, Type>();

		} //

		public void SetCreator<T>( ViewModelCreator creator ) {
			this.creators[typeof( T )] = creator;
		} //

		public object CreateViewModel( object data, object view = null, bool tryBaseClasses=true ) {

			ViewModelCreator creator = GetCreator( data, tryBaseClasses );
			if( creator == null ) {

				creator = fallbackCreator;
				if( creator == null ) {
					return null;
				}

			}

			// Use the Creator delegate to return a new ViewModel for the given data.
			return creator( data, view );

		}

		public object CreateViewModel<T>( T data, object view=null ) {

			ViewModelCreator creator = GetCreator<T>();
			if( creator == null ) {

				creator = fallbackCreator;
				if( creator == null ) {
					return null;
				}

			}

			// Use the Creator delegate to return a new ViewModel for the given data.
			return creator( data, view );

		}

		public ViewModelCreator GetCreator( object data, bool checkBaseClasses=true ) {

			ViewModelCreator creator;

			Type testType = data.GetType();

			if( this.creators.TryGetValue( testType, out creator ) ) {
				return creator;
			}

			if( checkBaseClasses ) {

				testType = testType.BaseType;
				while( testType != null ) {

					if( this.creators.TryGetValue( testType, out creator ) ) {
						return creator;
					}

					testType = testType.BaseType;

				} //while

			}

			return null;

		}

		public ViewModelCreator GetCreator<T>() {

			ViewModelCreator creator;
			if( this.creators.TryGetValue( typeof(T), out creator ) ) {
				return creator;
			}
			return null;

		}

		public void SetViewModel( Type dataType, Type viewModelType ) {

			this.viewModels[dataType] = viewModelType;

		}

		/// <summary>
		/// Instantiates the appropriate ViewModel for the given type
		/// of data.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="data"></param>
		/// <returns></returns>
		public object GetViewModel<T>( T data ) {

			Type modelType = this.GetViewModelType<T>();
			if( modelType == null ) {
				return null;
			}

			return null;

		} //

		public Type GetViewModelType<T>() {

			Type modelType;
			if( this.viewModels.TryGetValue( typeof( T ), out modelType ) ) {
				return modelType;
			}
			return null;
		}

    } // class

} // namespace