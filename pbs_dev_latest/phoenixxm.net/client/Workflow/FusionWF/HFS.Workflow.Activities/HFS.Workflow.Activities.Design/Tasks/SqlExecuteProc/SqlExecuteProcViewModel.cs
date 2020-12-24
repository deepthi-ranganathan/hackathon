#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2013  Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: SqlExecuteProcViewModel.cs
// NameSpace: HFS.Workflow.Activities.Design
//-------------------------------------------------------------------------------
//Date			Ver Initial		Change
//-------------------------------------------------------------------------------
//9/13/13		1   mramalin	Created
//11/04/2015    2   sbabcock	CR-38606 - License Employee Cert
//05/06/2016    3   Vipin       Task-43992 - If same column was added in the Select query it will have some error, need to change the name
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Activities;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//
using HFS.Windows.Controls;
using HFS.Windows.Forms;
using HFS.Workflow.Activities.Design.ViewModels;
using HFS.Workflow.BusObj.Misc;
using HFS.Workflow.Shared;
//
using Phoenix.FrameWork.BusFrame;
using Phoenix.FrameWork.Core;

namespace HFS.Workflow.Activities.Design
{
	/// <summary>
	/// SqlExecuteProcViewModel used for the SqlExecuteProcEditor
	/// </summary>
	public class SqlExecuteProcViewModel : ViewModelBase<SqlExecuteProc>
	{

		#region Private variables
		private ModelItemCollection _argumentsCollection;
		private ModelItemCollection _outVariablesCollection;

		private ParamMapViewModel _sqlArgViewModel;
		private ParamMapViewModel _outVariablesViewModel;
		private string _originalSQL = null;
		private SharedWfHelper _wfHelperBO;
		// CR-38606
		private bool _isEnabled = false;

		#endregion

		#region constructor
		public SqlExecuteProcViewModel(HWorkflowEditor editor, ModelItem ownerActivity)
			: base(ownerActivity)
		{
			_argumentsCollection = ModelItem.Properties[SqlExecuteProc.SqlArgumentProperty].Collection;
			_outVariablesCollection = ModelItem.Properties[SqlExecuteProc.OutVariablesProperty].Collection;

			OutVariables = new ParamMapViewModel(ownerActivity, _outVariablesCollection, editor);
			SqlArguments = new ParamMapViewModel(ownerActivity, _argumentsCollection, editor);

			// CR-38606
			// Max Rows Returned & Max Query Response Time values can only be edited from Professional Session.
			if (LicenseInfo.IsProfessionalSession)
				_isEnabled = true;

			_originalSQL = SqlStatement;

			editor.OnOk = this.OnOk;
			
			UpdateArguments();
		}
		#endregion

		// CR-38606
		public  bool IsEnabled
		{
			get { return _isEnabled; }
		}

		#region Public Bindable Elements
		/// <summary>
		/// Bindable string for the Designer
		/// </summary>
		public string SqlStatement
		{
			get { return Model.SqlStatement; }
			set
			{
				NotifyIfChanged(Model.SqlStatement, value, SqlExecuteProc.SqlStatementProperty, () =>
				{
					Model.SqlStatement = value;
					UpdateArguments();
					return true;
				});
			}
		}

        // CR-38606
		public int MaxRuntimeQuery
		{
			get 
			{
				if (Model.MaxRuntimeQuery <= 0)
					Model.MaxRuntimeQuery = SqlExecuteProc.MaxRuntimeQueryValue;
				
				return Model.MaxRuntimeQuery; 
			}
			set
			{
				NotifyIfChanged(Model.MaxRuntimeQuery, value, SqlExecuteProc.MaxRuntimeQueryProperty, () =>
				{
					if (value <= 0)
						value = SqlExecuteProc.MaxRuntimeQueryValue;

					Model.MaxRuntimeQuery = value;
					return true;
				});
			}
		}

		// CR-38606
		public int MaxRowsReturn
		{
			get
			{
				if (Model.MaxRowsReturn <= 0)
					Model.MaxRowsReturn = SqlExecuteProc.MaxRowsReturnValue;

				return Model.MaxRowsReturn;
			}
			set
			{
				NotifyIfChanged(Model.MaxRowsReturn, value, SqlExecuteProc.MaxRowsReturnProperty, () =>
				{
					if (value <= 0)
						value = SqlExecuteProc.MaxRowsReturnValue;

					Model.MaxRowsReturn = value;
					return true;
				});
			}
		}

		/// <summary>
		/// Window Parameters
		/// </summary>
		public ParamMapViewModel SqlArguments
		{
			get
			{
				return _sqlArgViewModel;
			}
			set
			{
				if (_sqlArgViewModel != value)
				{
					_sqlArgViewModel = value;
					OnPropertyChanged("SqlArguments");
				}
			}
		}

		/// <summary>
		/// Extract Parameters
		/// </summary>
		public ParamMapViewModel OutVariables
		{
			get
			{
				return _outVariablesViewModel;
			}
			set
			{
				if (_outVariablesViewModel != value)
				{
					_outVariablesViewModel = value;
					OnPropertyChanged("OutVariables");
				}
			}
		}

		/// <summary>
        /// Bindable string for the Designer
        /// </summary>      

		public EnumValueCollection FieldTypes
		{
			get
			{
				return ExtractInfo.FieldTypes;
			}
		}

		#endregion

		#region Private properties
		private ObservableCollection<ArgumentInfoWrapper> ArgumentItems
		{
			get { return SqlArguments.Items; }
		}

		private ObservableCollection<ArgumentInfoWrapper> OutVariablesItems
		{
			get { return _outVariablesViewModel.Items; }
		}

		#endregion

		#region Private methods
		private void UpdateArguments()
		{
			if (_originalSQL == SqlStatement)
			{
				ArgumentItems.Populate(_argumentsCollection);
				OutVariablesItems.Populate(_outVariablesCollection);
			}
			else
			{
				string regexPatternOut = @"@r";
				MatchCollection matchOut = Regex.Matches(SqlStatement, regexPatternOut);
				bool bNormalVar = true;

				string regExPattern = @"@\w*\b";
				MatchCollection matchex = Regex.Matches(SqlStatement, regExPattern);
				List<string> paramNames = new List<string>();


				List<ArgumentInfo> paramList = new List<ArgumentInfo>();
				foreach (var myString in SqlStatement.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					if (myString.ToLower().Contains("output"))
					{
					
					}
					else if (myString.ToLower().Contains("exec"))
					{

					}
					else
					{
						if(myString.Contains("@"))
						paramNames.Add(myString.Replace(",",""));
					}

				}
				//foreach (Match match in matchex)
				//{
				//	if (paramNames.Contains(match.Value))
				//		continue;
				//	foreach(Match match1 in matchOut)
    //                {
				//		if(paramNames.Contains(match1.Value))
    //                    {
				//			bNormalVar = false;
				//			break;
				//		}
    //                }
				//	if(bNormalVar)
				//	paramNames.Add(match.Value);
				//}

				foreach (string paramName in paramNames)
				{
					bool found = false;
					foreach (ArgumentInfoWrapper wrapper in ArgumentItems)
					{
						if (wrapper.Name == paramName )
						{
							paramList.Add(wrapper);
							found = true;
							break;
						}
					}
					if( found == false )
						paramList.Add(new ArgumentInfo() { Name = paramName, Direction = ArgumentDirection.In });
				}
				ArgumentItems.UpdateActivityParameters(paramList);

				//TODO: Add BO Call

				UpdateIntoVariables();
				
			}
			
		}
			
		private void UpdateIntoVariables()
		{
			List<ArgumentInfo> outValues = new List<ArgumentInfo>();
			//if (MultipleRows == false)
			//{
				if (_wfHelperBO == null)
				{
					_wfHelperBO = new SharedWfHelper();
				}
				try
				{
					//Visibility="{Binding Path=ViewModel.MultipleRows, Converter={x:Static hwc:GlobalResources.TrueFalseHiddenVisible}}"
					DataTable schemaTable = GetSchema(SqlStatement);
                    /*Begin #43992*/
                    schemaTable.DefaultView.Sort = "ColumnName";
                    schemaTable = schemaTable.DefaultView.ToTable();
                    /*End   #43992*/
					foreach (DataRow row in schemaTable.Rows)
					{
						string colName = (string)row["ColumnName"];
						
						bool found = false;
                        /*Begin #43992*/
                        int i = 1;
                        string stemp = colName;
                        /*End   #43992*/
						foreach (ArgumentInfoWrapper wrapper in OutVariablesItems)
						{
                            /*Begin #43992*/
                            if (outValues.Exists(x => x.Name == colName))
                            {

                                colName = stemp + Convert.ToString(i);
                                i = i + 1;
                            }
                            else
                            {
                                i = 1;                                
                            }
                            /*End   #43992*/
                            if (wrapper.Name == colName  && wrapper.Direction == ArgumentDirection.Out)
							{
								outValues.Add(wrapper);
								found = true;
								break;
							}
						}

                    if (found == false)
                    {
                        /*Begin #43992*/
                        if (outValues.Exists(x => x.Name == colName))
                        {
                            colName = stemp + Convert.ToString(i);
                            i = i + 1;
                        }
                        else
                        {
                            i = 1;
                        }
                        /*End   #43992*/
                        outValues.Add(new ArgumentInfo() { Name = colName, Direction = ArgumentDirection.Out, DataType = ParamFieldType.String });
						
                    }
					}
					//

				}
				catch (Exception ex)
				{
					CoreService.LogPublisher.LogDebug(ex.ToString());
					HMessageBox.Show(ex);
				}
			//}
			OutVariablesItems.UpdateActivityParameters(outValues);
		}

		public DataTable GetSchema(string sql)
        {
			DataTable rtDt = new DataTable();
			rtDt.Columns.Add("ColumnName", typeof(string));
			foreach (var myString in sql.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))	
            {
				if(myString.ToLower().Contains("output"))
                {
					rtDt.Rows.Add(myString.Replace("output",""));
				}
				else if(myString.ToLower().Contains("exec"))
                {

                }
				else
                {
					
                }

            }
			
			return rtDt;
        }
		#endregion

		#region OnOk Click Handler
		public bool OnOk()
		{
			SqlArguments.OnOk();
			return OutVariables.OnOk();
			
		}
		#endregion

	}
}


#region Junk YARD
//private ModelItemCollection _modelCollection;
//private ModelItem _ownerActivity;
//
//public ArgumentInfoCollection SqlArguments
//{
//    get { return Model.SqlArguments; }
//    set
//    {
//        NotifyIfChanged(Model.SqlArguments, value, SqlExecuteProc.SqlArgumentProperty, () =>
//        {
//            Model.SqlArguments = value;
//            return true;
//        });
//    }
//}


//public List<ArgumentInfo> OutVariables
//{
//    get { return Model.OutVariables; }
//    set
//    {
//        NotifyIfChanged(Model.OutVariables, value, SqlExecuteProc.OutVariablesProperty, () =>
//        {
//            Model.OutVariables = value;
//            return true;
//        });
//    }
//}


//public ModelItem OwnerActivity
//{
//    get
//    {
//        return this._ownerActivity;
//    }
//    set
//    {
//        if (this._ownerActivity != value)
//        {
//            this._ownerActivity = value;
//            base.OnPropertyChanged("OwnerActivity");
//        }
//    }
//}

//public Type OutputRowType
//{
//    get { return typeof(List<DataRow>); }
//}



///// <summary>
///// Field Required
///// </summary>
//public bool MultipleRows
//{
//    get { return Model.MultipleRows; }
//    set { NotifyIfChanged(Model.MultipleRows, value, SqlExecuteProc.MultipleRowsProperty, () => { Model.MultipleRows = value; return true; }); }
//}


#endregion