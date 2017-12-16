using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemur.Windows {

    public class UserSetting {

		public UserSetting( string name, string title, Type type, string description = "", Type frameworkElement = null ) {

			this.settingName = name;
			this.settingTitle = title;
			this.settingType = type;
			this.desc = description;
			this.uiElement = frameworkElement;

		}

		/// <summary>
		/// internal name for the setting.
		/// </summary>
		private string settingName;
		public string Name {
			get { return this.settingName; }
			set { this.settingName = value; }
		}

		/// <summary>
		/// Text displayed for the setting.
		/// </summary>
		private string displayText;
		public string DisplayText {
			get { return this.displayText; }
			set { this.displayText = value; }
		}

		/// <summary>
		/// Internal type used to represent the setting.
		/// </summary>
		private Type settingType;
		public Type Type {
			get { return this.settingType; }
			set { this.settingType = value; }
		}

		/// <summary>
		/// The frameworkUIElement used to display the setting.
		/// If no element is specified, a default is used.
		/// </summary>
		private Type uiElement;
		public Type DisplayType {
			get { return this.uiElement; }
			set { this.uiElement = value; }
		}

		/// <summary>
		/// Description of the setting.
		/// </summary>
		private string desc;
		public string Desc {
			get { return this.desc; }
			set { this.desc = value; }
		}

		/// <summary>
		/// Title to display for the setting.
		/// If no title is specified, none will be displayed.
		/// </summary>
		private string settingTitle;
		public string Title {
			get { return this.settingTitle; }
			set { this.settingTitle = value; }
		}

    } // class

} // namespace