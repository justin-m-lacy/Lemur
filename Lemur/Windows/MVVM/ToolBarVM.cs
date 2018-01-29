using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;

namespace Lemur.Windows.MVVM {

	public class CommandItem {

		public ICommand Command {
			get => command;
			set => command = value;
		}
	
		public object Item {
			get => item;
			set => item = value;
		}

		public object Group {
			get => this.group;
			set => this.group = value;
		}

		private object item;
		private object group;
		private ICommand command;

		public CommandItem( object item, ICommand command ) {
			this.item = item;
			this.command = command;
		}

		public CommandItem( object item, ICommand command, object grp ) {
			this.item = item;
			this.command = command;
			this.group = grp;
		}

	}

	public class ToolBarVM :ViewModelBase {

		/// <summary>
		/// Allows commands to be removed in groups.
		/// </summary>
		private readonly Dictionary<object, List<CommandItem>> groupedCommands = new Dictionary<object, List<CommandItem>>();

		/// <summary>
		/// Items that can be removed from toolbar.
		/// </summary>
		private readonly ObservableCollection<CommandItem> items = new ObservableCollection<CommandItem>();
		public ObservableCollection<CommandItem> Items {
			get => items;
		}

		/// <summary>
		/// Add an object and associated command, with an optional command group.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="cmd"></param>
		/// <param name="group"></param>
		public void Add( object item, ICommand cmd, object group=null ) {

			CommandItem cmdItem = new CommandItem( item, cmd, group );
			this.items.Add( cmdItem );
			if( group != null ) {
				this.AddToGroup( group, cmdItem );
			} //
	
		}

		/// <summary>
		/// Remove all commands associated with a group.
		/// </summary>
		/// <param name="group"></param>
		public void RemoveGroup( object group ) {

			List<CommandItem> cmds;
			if( groupedCommands.TryGetValue( group, out cmds ) ) {

				groupedCommands.Remove( group );
				this.RemoveItems( cmds );

			}

		} //


		public void Remove( object item ) {

			for( int i = this.items.Count - 1; i >= 0; i-- ) {

				CommandItem cmdItem = items[i];
				if( cmdItem.Item == item ) {
					this.items.RemoveAt( i );
					return;
				}

			} // for-loop.

		}

		public void RemoveItems( IEnumerable<object> items ) {

			CommandItem cmdItem;
			for( int i = this.items.Count - 1; i >= 0; i-- ) {

				cmdItem = this.items[i];
			}

		}

		public void AddRange( IEnumerable<CommandItem> new_items ) {

			foreach( CommandItem item in new_items ) {

				this.items.Add( item );
				if( item.Group != null ) {
					AddToGroup( item.Group, item );
				}


			}

		}

		private void AddToGroup( object group, CommandItem item ) {

			List<CommandItem> grpItems;
			if( !this.groupedCommands.TryGetValue( group, out grpItems ) ) {
				this.groupedCommands[group] = grpItems = new List<CommandItem>();
			}
			grpItems.Add( item );

		}

		public ToolBarVM() {
		} // ToolBarVM()

    } // class

} // namespace
