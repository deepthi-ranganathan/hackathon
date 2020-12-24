#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2013  Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: SqlExecuteProcMetadata.cs
// NameSpace:  HFS.Workflow.Activities.Design.MetadataProvider
//-------------------------------------------------------------------------------
//Date			Ver Initial		Change
//-------------------------------------------------------------------------------
//9/13/13		1   mramalin    Created
//08/22/2014    2   randes      CR-27880 - WorkflowSpec Specific Properties
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Activities.Presentation.Model;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//
using HFS.Workflow.Shared;

namespace HFS.Workflow.Activities.Design.MetadataProvider
{
	/// <summary>
	/// Sql ExecuteQuery Metadata provider
	/// </summary>
	public class SqlExecuteProcMetadata : MetadataProviderBase
	{
		/// <summary>
		/// Public constructor
		/// </summary>
		public SqlExecuteProcMetadata()
		{
			ActivityType = typeof(SqlExecuteProc);
			DesignerType = typeof(SqlExecuteProcDesigner);
			BitmapName = "images/sql-execute16.png";
			//Not a Real value, this will not be needed for the task center
			TaskInvokerAssembly = "HFS.Workflow.Activities.Runtime";
			TaskInvokerClass = "HFS.Workflow.Activities.Runtime";
        }

        public override WorkflowSpecProperties GetSpecificActivityProperties(ModelItem modelItem)
        {
            // Properties:
            WorkflowSpecProperties specificProperties = base.GetSpecificActivityProperties(modelItem);
            if (specificProperties != null && specificProperties.Count > 0)
            {
                // View Model:
                SqlExecuteProcEditor activityEditor = new SqlExecuteProcEditor(modelItem);

                // Loop through Properties:
                foreach (WorkflowSpecProperty activityProp in specificProperties)
                {
                    // Value Adjustments:
                    if (activityProp.Name == SqlExecuteProc.SqlArgumentProperty)
                    {
                        // Create DataTable to store check list:
                        DataTable dt = new DataTable();
                        dt.TableName = "WhereClauseInfo";
                        dt.Columns.Add(new DataColumn("Parameter Name"));
                        dt.Columns.Add(new DataColumn("Data Type"));
                        dt.Columns.Add(new DataColumn("Value"));
                        // Extended Properties Must be string to serialize:
                        dt.Columns[0].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "30");
                        dt.Columns[1].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "25");
                        dt.Columns[2].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "45");

                        // Populate DataTable from View Model:
                        foreach (ArgumentInfoWrapper argInfo in activityEditor.ViewModel.SqlArguments.Items)
                        {
                            dt.Rows.Add(argInfo.Name, argInfo.DataType.ToString(), argInfo.Value.GetModelItemExpressionText());
                        }

                        // Serialize into Value:
                        activityProp.SerializeDataTableToValue(dt);
                    }
                    else if (activityProp.Name == SqlExecuteProc.OutVariablesProperty)
                    {
                        // Create DataTable to store check list:
                        DataTable dt = new DataTable();
                        dt.TableName = "SelectValuesInfo";
                        dt.Columns.Add(new DataColumn("Column Name"));
                        dt.Columns.Add(new DataColumn("Data Type"));
                        dt.Columns.Add(new DataColumn("Value"));
                        // Extended Properties Must be string to serialize:
                        dt.Columns[0].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "30");
                        dt.Columns[1].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "25");
                        dt.Columns[2].ExtendedProperties.Add(WorkflowSpecProperty.SpecPropViewWidthPct, "45");

                        // Populate DataTable from View Model:
                        foreach (ArgumentInfoWrapper argInfo in activityEditor.ViewModel.OutVariables.Items)
                        {
                            dt.Rows.Add(argInfo.Name, argInfo.DataType.ToString(), argInfo.Value.GetModelItemExpressionText());
                        }

                        // Serialize into Value:
                        activityProp.SerializeDataTableToValue(dt);
                    }

				}
            }

            return specificProperties;
        }

        public override Dictionary<string, string> GetSpecificActivityPropertiesFilter(ModelItem modelItem)
        {
			return new Dictionary<string, string>()
                {
                    { SqlExecuteProc.SqlStatementProperty, "SQL To Execute" },
                    { SqlExecuteProc.SqlArgumentProperty, "Where Clause" },
                    { SqlExecuteProc.OutVariablesProperty, "Select Values" }
                };
        }
	}
}
