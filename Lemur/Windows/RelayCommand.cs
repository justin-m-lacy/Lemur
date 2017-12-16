using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Lemur.Windows {

	/// <summary>
	/// A command whose sole purpose is to relay its functionality 
	/// to other objects by invoking delegates. 
	/// The default return value for the CanExecute method is 'true'.
	/// <see cref="RaiseCanExecuteChanged"/> needs to be called whenever
	/// <see cref="CanExecute"/> is expected to return a different value.
	/// </summary>
	public class RelayCommand : ICommand {
		#region Private members
		/// <summary>
		/// Creates a new command that can always execute.
		/// </summary>
		private readonly Action execute;

		/// <summary>
		/// True if command is executing, false otherwise
		/// </summary>
		private readonly Func<bool> canExecute;
		#endregion

		/// <summary>
		/// Initializes a new instance of <see cref="RelayCommand"/> that can always execute.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		public RelayCommand( Action execute ) : this( execute, canExecute: null ) {
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RelayCommand"/>.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand( Action execute, Func<bool> canExecute ) {

			this.execute = execute;
			this.canExecute = canExecute;
		}

		/// <summary>
		/// Raised when RaiseCanExecuteChanged is called.
		/// </summary>
		public event EventHandler CanExecuteChanged;

		/// <summary>
		/// Determines whether this <see cref="RelayCommand"/> can execute in its current state.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed, this object can be set to null.
		/// </param>
		/// <returns>True if this command can be executed; otherwise, false.</returns>
		public bool CanExecute( object parameter ) {
			return this.canExecute == null ? true : this.canExecute();
		}

		/// <summary>
		/// Executes the <see cref="RelayCommand"/> on the current command target.
		/// </summary>
		/// <param name="parameter">
		/// Data used by the command. If the command does not require data to be passed, this object can be set to null.
		/// </param>
		public void Execute( object parameter ) {
			if( this.execute != null ) {
				this.execute();
			}
		}

		/// <summary>
		/// Method used to raise the <see cref="CanExecuteChanged"/> event
		/// to indicate that the return value of the <see cref="CanExecute"/>
		/// method has changed.
		/// </summary>
		public void RaiseCanExecuteChanged() {
			var handler = this.CanExecuteChanged;
			if( handler != null ) {
				handler( this, EventArgs.Empty );
			}
		}
	}

	public class RelayCommand<T> : ICommand {

		#region Fields

		readonly Action<T> _execute = null;
		readonly Predicate<T> _canExecute = null;

		#endregion // Fields

		#region Constructors

		public RelayCommand( Action<T> execute )
			: this( execute, null ) {
		}

		/// <summary>
		/// Creates a new command.
		/// </summary>
		/// <param name="execute">The execution logic.</param>
		/// <param name="canExecute">The execution status logic.</param>
		public RelayCommand( Action<T> execute, Predicate<T> canExecute ) {
			_execute = execute;
			_canExecute = canExecute;
		}

		#endregion // Constructors

		#region ICommand Members

		public bool CanExecute( object parameter ) {
			return _canExecute == null ? true : _canExecute( (T)parameter );
		}

		public event EventHandler CanExecuteChanged {
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		public void Execute( object parameter ) {
			if( this._execute != null ) {
				this._execute( (T)parameter );
			}
		}

		#endregion // ICommand Members
	}

}
