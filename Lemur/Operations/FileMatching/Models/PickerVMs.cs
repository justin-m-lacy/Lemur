using Lemur.Windows.MVVM;
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using Lemur.Windows;
using Lemur.Types;
using System.Linq;
using Lemur.Operations.FileMatching.Actions;

namespace Lemur.Operations.FileMatching.Models {

	public class ConditionPickerVM : TypePickerVM<IMatchCondition> { }
	public class ActionPickerVM : TypePickerVM<IFileAction> { }

} // namespace