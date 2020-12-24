#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2013  Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: SqlExecuteProcDesigner.xaml.cs
// NameSpace: HFS.Workflow.Activities.Design
//-------------------------------------------------------------------------------
//Date			Ver Initial		Change
//-------------------------------------------------------------------------------
//9/13/2013		1   mramalin	Created
//11/26/2013	2	mramalin	CR-25422 - Modification due to change in ActivityDesignerBase
//05/01/2014	3	stran		CR-26112 - License level
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HFS.Workflow.Activities.Design.MetadataProvider;
using System.Activities.Presentation.Model;
using Phoenix.FrameWork.Core;
using Phoenix.FrameWork.BusFrame;
using HFS.Workflow.Shared;

namespace HFS.Workflow.Activities.Design
{
	/// <summary>
	/// Interaction logic for SqlExecuteProcDesigner.xaml
	/// </summary>
	public partial class SqlExecuteProcDesigner : ActivityDesignerBase
	{
		#region Constructor
		public SqlExecuteProcDesigner()
		{
			MetadataProvider = new SqlExecuteProcMetadata();
			InitializeComponent();

			// CR-38606
			if (WfGlobalVars.ProductLicenseLevelSession == HFS.Workflow.Shared.StringConstants.ProductLicenseLevelPro)
				this.TaskPropertiesCommand.CommandVisible = System.Windows.Visibility.Visible;
			else if (WfGlobalVars.ProductLicenseLevel == HFS.Workflow.Shared.StringConstants.ProductLicenseLevelPro && WfGlobalVars.ProductLicenseLevelEmplCert == true)
				this.TaskPropertiesCommand.CommandVisible = System.Windows.Visibility.Visible;
			else
				this.TaskPropertiesCommand.CommandVisible = System.Windows.Visibility.Hidden;

			////CR-26112 
			//if (!(WfGlobalVars.ProductLicenseLevelSession == HFS.Workflow.Shared.StringConstants.ProductLicenseLevelPro
			//|| WfGlobalVars.ProductLicenseLevel == HFS.Workflow.Shared.StringConstants.ProductLicenseLevelPro))
			//    this.TaskPropertiesCommand.CommandVisible = System.Windows.Visibility.Hidden;

		}
		#endregion


		/// <summary>
		/// TaskProperties override to handle the click
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override bool ShowTaskSpecificProperties(object sender, System.Windows.RoutedEventArgs e)
		{

			return ShowTaskPropertiesDialog(this.ModelItem);
		}

		/// <summary>
		/// Method implementation to display the task properties dialog
		/// </summary>
		/// <param name="item"></param>
		public static bool ShowTaskPropertiesDialog(ModelItem item)
		{
			bool isModified = false;
			if (item == null)
			{
				CoreService.LogPublisher.LogError("ModelItem passed is null. Unable to contiune.");
				return isModified;
			}
			// 
			string displayName = item.GetDisplayName(); // String.Empty;
			using (ModelEditingScope scope = item.BeginEdit())
			{
				SqlExecuteProcEditor frm = new SqlExecuteProcEditor(item);
			
				if (frm.ShowOkCancel())
				{
					scope.Complete();
					isModified = true;
				}
				else
					scope.Revert();
			}
			return isModified;
		}
	}
}
