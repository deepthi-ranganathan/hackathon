#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2013  Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: SqlExecuteProcEditor.xaml.cs.cs
// NameSpace: HFS.Workflow.Activities.Design
//-------------------------------------------------------------------------------
//Date			Ver Initial		Change
//-------------------------------------------------------------------------------
//9/23/13		1   mramalin  Created
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
using HFS.Windows.Controls;
using System.Activities.Presentation.Model;

namespace HFS.Workflow.Activities.Design
{
	/// <summary>
	/// Interaction logic for SqlExecuteProcEditor.xaml
	/// </summary>
	public partial class SqlExecuteProcEditor : HWorkflowEditor
	{
	
		private SqlExecuteProcViewModel _viewModel;

		public SqlExecuteProcViewModel ViewModel
		{
			get { return _viewModel; }
		}

		#region Constructor
		public SqlExecuteProcEditor()
		{
			InitializeComponent();
		}

		public SqlExecuteProcEditor(ModelItem item)
		{
			base.ModelItem = item;
			_viewModel = new SqlExecuteProcViewModel(this, item); //, "WorkflowArguments");
			InitializeComponent();
		}
		#endregion

	}
}


#region JUNK YARD

//private void tabItemSelectValues_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
//{
//    if (tabItemSelectValues.IsEnabled == false)
//    {
//        //tabItemSelectValues.IsSelected = false;
//        tabItemWhereClause.IsSelected = true;
//    }
//}
#endregion