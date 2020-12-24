#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2013  Harland Financial Solutions
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: SqlExecuteQuery.cs
// NameSpace: HFS.Workflow.Activities
//-------------------------------------------------------------------------------
//Date			Ver Initial		Change
//-------------------------------------------------------------------------------
//09/13/2013	1   mramalin	Created
//11/15/2013	2	mramalin	Wi-25774 Fixed the naming discrepancies	
//12/11/2013	3	mramalin	WI-26047 Fixed a threading issue when the SQLExecute completes
//12/11/2014	4	mramalin	WI-33343 - Added logic to create ErrorInfo to Db
//11/04/2015    5   sbabcock    CR-38606 - License Employee Cert
//-------------------------------------------------------------------------------
#endregion

using System;
using System.Activities;
using System.Activities.Tracking;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
//
using HFS.Workflow.Shared;
//
using Phoenix.FrameWork.Core;

namespace HFS.Workflow.Activities
{
	/// <summary>
	/// SqlExecute Query Task
	/// </summary>
	[HFSTaskAttributes(TaskId = TaskGuids.TASK_GUID_SqlExecuteProc,
	ProductId = ProductGuids.PROD_GUID_EnterpriseWorkflow, Name = "Sql Immediate Task",
	Description = "This task is used to execute SQL on the databaes server.")]
	public class SqlExecuteProc : TaskBase
	{
		#region Private Variables
		private Variable<DataTable> _dataTable;
		private Variable<int> _currentIndex;

		#endregion

		#region Constants
		// CR-38606
		public const string MaxRuntimeQueryProperty = "MaxRuntimeQuery";
		public const string MaxRowsReturnProperty = "MaxRowsReturn";
		public const int MaxRuntimeQueryValue = 20;
		public const int MaxRowsReturnValue = 500;

		#endregion
		
		#region String properties to match the name of the Prperties
		public static readonly string SqlStatementProperty = "SqlStatement";
		public static readonly string SqlArgumentProperty = "SqlArguments";
		public static readonly string OutVariablesProperty = "OutVariables";
		public static readonly string MultipleRowsProperty = "MultipleRows";
		

		#endregion

		#region public Properties

		/// <summary>
		/// Sql Text
		/// </summary>
		[Browsable(false)]
		public string SqlStatement { get; set; }

		/// <summary>
		/// Max Runtime for Query
		/// </summary>
		[Browsable(false)]
		public int MaxRuntimeQuery { get; set; }

		/// <summary>
		/// Max Rows to Return
		/// </summary>
		[Browsable(false)]
		public int MaxRowsReturn { get; set; }

		/// <summary>
		/// Arguments to workflow
		/// </summary>
		[Browsable(false)]
		public ArgumentInfoCollection SqlArguments { get; set; }

		/// <summary>
		/// Arguments to workflow
		/// </summary>
		[Browsable(false)]
		public ArgumentInfoCollection OutVariables { get; set; }

		/// <summary>
		/// Multiple Rows Expected
		/// </summary>
		[Browsable(false)]
		public bool MultipleRows { get; set; }

		/// <summary>
		/// Record Handler Actvity
		/// </summary>
		[Browsable(false)]
		public Activity RecordHandler
		{
			get;
			set;
		}

		[Browsable(false)]
		public Activity<bool> Condition
		{
			get;
			set;
		}


		#endregion

		#region Constructor
		public SqlExecuteProc()
		{
			UIInvocation = false;
			HFSTaskAttributes taskAttributes = new HFSTaskAttributes()
			{
				TaskId = TaskGuids.TASK_GUID_SqlExecuteProc,
				ProductId = ProductGuids.PROD_GUID_EnterpriseWorkflow,
				Name = "Sql Execute Task",
				Description = "This task is used to run Sql Statement and get values."
			};
			SetTaskAttributes(taskAttributes);

			OutVariables = new ArgumentInfoCollection();
			SqlArguments = new ArgumentInfoCollection();

			_dataTable = new Variable<DataTable>();
			_currentIndex = new Variable<int>();
		}
		#endregion

		#region Override Functions
		protected override void CacheMetadata(NativeActivityMetadata metadata)
		{
			base.CacheMetadata(metadata);
			//
			foreach (ArgumentInfo arg in SqlArguments)
			{
				Argument argument = arg.Argument;
				RuntimeArgument runtimeArgument = new RuntimeArgument(arg.Name, argument.ArgumentType, argument.Direction);
				metadata.Bind(argument, runtimeArgument);
				metadata.AddArgument(runtimeArgument);
			}
			//
			foreach (ArgumentInfo arg in OutVariables)
			{
				Argument argument = arg.Argument;
				RuntimeArgument runtimeArgument = new RuntimeArgument(arg.Name, argument.ArgumentType, argument.Direction);
				metadata.Bind(argument, runtimeArgument);
				metadata.AddArgument(runtimeArgument);
			}
			//
			metadata.AddImplementationVariable(_dataTable);
			metadata.AddImplementationVariable(_currentIndex);
		}

		public override NonUIExecuteStatus OnNonUIExecute(NativeActivityContext context, TaskBaseProperties properties, WFTaskInfo taskInfo)
		{
			SqlConnection conn = null;
			SqlDataReader reader = null;
			SqlTransaction transaction = null;
			try
			{
				IDbHelper dbHelper = CoreService.DbHelper;
				conn = dbHelper.NewConnection(true) as SqlConnection;
				// Set the database to primary database
				dbHelper.ExecuteNonQuery(conn, CommandType.Text, string.Format("use {0}", dbHelper.PhoenixDbName));
				//
				transaction = conn.BeginTransaction();
				//
				CustomTrackingRecord ctr = CreateNewTrackingRecord(TraceLevel.Info, "Sql Execute");
				//string sqlCommandText = string.Empty;
				//if (SqlStatement.Contains("psp_rp_rwd_cash"))
				//{
				//	sqlCommandText = @"Declare @nRC int, @rnDiscountAmt decimal(14, 2),	@rnSQLError int
				//                                   EXEC	[dbo].[psp_rp_rwd_cash]";

				//}
				//using (SqlCommand procCmd = conn.CreateCommand())
				//{
				//	procCmd.CommandText = parameterStatement.getQuery();
				//	procCmd.CommandType = CommandType.StoredProcedure;
				//	procCmd.Parameters.AddWithValue("SeqName", "SeqNameValue");

				//	var returnParameter = procCmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
				//	returnParameter.Direction = ParameterDirection.ReturnValue;

				//	conn.Open();
				//	procCmd.ExecuteNonQuery();
				//	var result = returnParameter.Value;
				//}

				SqlCommand _procCommand = new SqlCommand("[dbo].[psp_rp_rwd_cash]", conn);
				_procCommand.CommandType = CommandType.StoredProcedure;
				SqlParameter sqlParam = null;
				foreach (ArgumentInfo argument in SqlArguments)
				{
					object argValue = argument.Argument.Get(context);
					sqlParam = new SqlParameter(argument.Name, argValue);
					//_procCommand.Parameters.Add(sqlParam);
					_procCommand.Parameters.AddWithValue(argument.Name.Trim().TrimStart().TrimEnd(), argValue);
				}
				var returnParameter = _procCommand.Parameters.Add("@rnDiscountAmt", SqlDbType.Decimal);
				var returnParam2 = _procCommand.Parameters.Add("@rnSQLError", SqlDbType.Int);
				returnParameter.Direction = ParameterDirection.Output;
				returnParam2.Direction = ParameterDirection.Output;
				_procCommand.Transaction = transaction;
				_procCommand.ExecuteNonQuery();
				var result = returnParameter.Value;

				//SqlCommand cmd = new SqlCommand(SqlStatement, conn);

				//// Set time query is allowed to execute before timeout error occurs
				//cmd.CommandTimeout = (MaxRuntimeQuery <= 0 ? MaxRuntimeQueryValue : MaxRuntimeQuery);

				//cmd.Transaction = transaction;

				//ctr.Data["SqlStmt"] = SqlStatement;

				//foreach (ArgumentInfo argument in SqlArguments)
				//{
				//	object argValue = argument.Argument.Get(context);
				//	SqlParameter sqlParam = new SqlParameter(argument.Name, argValue);
				//	ctr.Data[argument.Name] = argValue;
				//	//sb.AppendLine(String.Format("{0} - {1}", argument.Name, argValue));
				//	if (argument.Direction == ArgumentDirection.In)
				//		sqlParam.Direction = ParameterDirection.Input;
				//	else if (argument.Direction == ArgumentDirection.InOut)
				//		sqlParam.Direction = ParameterDirection.InputOutput;
				//	else if (argument.Direction == ArgumentDirection.Out)
				//		sqlParam.Direction = ParameterDirection.Output;

				//	cmd.Parameters.Add(sqlParam);

				//}

				//SqlDataAdapter da = new SqlDataAdapter(cmd);

				//// Log the information
				//this.Track(context, false, ctr);

				//DataSet ds = new DataSet();

				//// CR-38606
				//if (MaxRowsReturn <= 0)
				//	MaxRowsReturn = MaxRowsReturnValue;
				//da.Fill(ds, 0, (MaxRowsReturn + 1), "SqlExec");
				////da.Fill(ds);

				////context.SetValue<DataSet>(OutDataSet, ds);
				//if (ds.Tables.Count > 0)
				//{
				//	DataTable dataTable = ds.Tables[0];
				//	if (RecordHandler != null)
				//	{
				//		_currentIndex.Set(context, -1);
				//		_dataTable.Set(context, dataTable);
				//		RecordHandlerActivityComplete(context, null);
				//		return NonUIExecuteStatus.Working;  // We are not done yet
				//	}
				//	else
				//	{
				//		if (dataTable.Rows.Count > 0)
				//		{
				//			// CR-38606
				//			// if rows returned in datatable equals "(MaxRowsReturn + 1)", then throw new exception.
				//			if (dataTable.Rows.Count == (MaxRowsReturn + 1))
				//			{
				//				CoreService.LogPublisher.LogDebug("Sql Execution Task - Maximum rows returned exceeded the configured count.");
				//				throw new Exception("Sql Execution Task - Maximum rows returned exceeded the configured count.");
				//			}
				//			PopulateOutArguments(context, dataTable, dataTable.Rows[0]);
				//		}
				//		else
				//		{
				//			TrackInfo(context, "Now Rows Fetched");
				//			//CoreService.LogPublisher.LogDebug("Now Rows Fetched");
				//		}
				//	}
				//}
				//else
				//{
				//	TrackInfo(context, "No Table found");
				//	//CoreService.LogPublisher.LogDebug("No Table found");
				//}

				//
				foreach (ArgumentInfo ourVariable in OutVariables)
				{
					if(ourVariable.Name.Contains(returnParameter.ParameterName))
                    {
						if(returnParameter.Value == null || string.IsNullOrEmpty(Convert.ToString(returnParameter.Value)))
                        {
							ourVariable.Argument.Set(context, 10);
						}
						else
                        {
							ourVariable.Argument.Set(context, returnParameter.Value);
                        }
                    }
					
				}
				return base.OnNonUIExecute(context, properties, taskInfo);
			}
			catch (SqlException exSql)
			{
				TrackError(context, exSql);
				if (exSql.Number == -2)
				{
					CoreService.LogPublisher.LogDebug("Sql Execution Task - Maximum run time exceeded the configured duration.");
					throw new Exception("Sql Execution Task - Maximum run time exceeded the configured duration.");
				}
				CoreService.LogPublisher.LogDebug(exSql.Message);
				return NonUIExecuteStatus.Failed;
			}
			catch (Exception ex)
			{
				TrackError(context, ex);
				CoreService.LogPublisher.LogDebug(ex.ToString());
			}
			finally
			{
				if (transaction != null)
					transaction.Rollback();
				if (reader != null && reader.IsClosed == false)
					reader.Close();
				if (conn != null)
					conn.Close();
			}

			return NonUIExecuteStatus.Failed;
		}
		#endregion

		#region Private Methods
		private void PopulateOutArguments(NativeActivityContext context, DataTable dataTable, DataRow dataRow)
		{
			if (dataTable == null || dataRow == null)
			{
				return;
			}
			foreach (ArgumentInfo ourVariable in OutVariables)
			{
				try
				{
					int colIndex = dataTable.Columns.IndexOf(ourVariable.Name);
					if (colIndex < 0)
					{
						TrackWarning(context, "Column Name {0} doesn't exist", ourVariable.Name);
						//CoreService.LogPublisher.LogDebug("Column Name " + ourVariable.Name + " doesn't exist");
						continue;
					}
					object value = null;
					if (dataRow.IsNull(colIndex))
					{
						TrackVerbose(context, "Column Name {0} is null", ourVariable.Name);
						//CoreService.LogPublisher.LogDebug("Column Name " + ourVariable.Name + " is null");
						if (dataTable.Columns[colIndex].DataType.IsValueType)
						{
							value = Activator.CreateInstance(dataTable.Columns[colIndex].DataType);
						}
					}
					else
						value = dataRow[colIndex];
					ourVariable.Argument.Set(context, value);
				}
				catch (Exception ex)
				{
					TrackError(context, ex.ToString());
					//CoreService.LogPublisher.LogDebug(ex.ToString());
				}
			}
		}

		private DataRow GetNextRow(NativeActivityContext context)
		{
			DataRow dataRow = null;
			//
			DataTable dataTable = _dataTable.Get(context);
			if (dataTable == null)
				return null;
			int curIndex = _currentIndex.Get(context);
			curIndex++; // Increase the counter
			if (curIndex < dataTable.Rows.Count)
			{
				_currentIndex.Set(context, curIndex);
				dataRow = dataTable.Rows[curIndex];
				//
				PopulateOutArguments(context, dataTable, dataRow);
				//
			}
			//
			return dataRow;
		}

		
		//private void ChildActivityComplete(NativeActivityContext context, ActivityInstance completedInstance)
		//{
		//    RecordHandlerActivityComplete(context, completedInstance);
		//}

	
		private CompletionCallback onRecordHandlerComplete;
		private CompletionCallback<bool> onConditionComplete;


		private void ScheduleCondition(NativeActivityContext context)
		{
			if (Condition != null)
			{
				CoreService.LogPublisher.LogDebug("ScheduleCondition");
				if (this.onConditionComplete == null)
				{
					this.onConditionComplete = new CompletionCallback<bool>(this.OnConditionComplete);
				}
				context.ScheduleActivity<bool>(this.Condition, this.onConditionComplete, null);
			}
		}

		private void OnConditionComplete(NativeActivityContext context, ActivityInstance completedInstance, bool result)
		{
			if (result)
			{
				if (this.RecordHandler != null)
				{
					ScheduleRecardHandlerActivity(context);
					return;
				}
				this.ScheduleCondition(context);
			}
			else
			{
				CoreService.LogPublisher.LogDebug("Exiting since the condition is not met.");
				UpdateTaskInfo(context);
			}
		}

		private void ScheduleRecardHandlerActivity(NativeActivityContext context)
		{
			if (this.RecordHandler != null)
			{
				if (onRecordHandlerComplete == null)
					onRecordHandlerComplete = new CompletionCallback(RecordHandlerActivityComplete);
				context.ScheduleActivity(this.RecordHandler, onRecordHandlerComplete);
				return;
			}
		}

		private void RecordHandlerActivityComplete(NativeActivityContext context, ActivityInstance completedInstance)
		{
			if (completedInstance != null && (completedInstance.State == ActivityInstanceState.Canceled
				|| (context.IsCancellationRequested && completedInstance.State == ActivityInstanceState.Faulted)))
			{
				context.MarkCanceled();
				CoreService.LogPublisher.LogDebug("Marking Cancelled, since the state is wrong");
				return;
			}

			if (context.IsCancellationRequested)
			{
				CoreService.LogPublisher.LogDebug("Marking Cancelled, since IsCancellationRequested");
				context.MarkCanceled();
				return;
			}

			DataRow row = GetNextRow(context);
			if (row == null)
			{
				CoreService.LogPublisher.LogDebug("We are exiting since no more row");
				UpdateTaskInfo(context);
				
				//base.OnNonUICompleteExecute(context, null);

			}
			else
			{
				CoreService.LogPublisher.LogDebug("Scheduling the next actions ");
				if (Condition != null)
					ScheduleCondition(context);
				else
					ScheduleRecardHandlerActivity(context);

				
				//context.ScheduleActivity(this.RecordHandler, this.ChildActivityComplete);
			}

		}

		private void UpdateTaskInfo(NativeActivityContext context)
		{
			
			// This will be completed in a separate thread so make sure the context 
			ValidateContext(context);
			//
			using (HThreadContext threadContext = base.GetThreadContext(context))
			{
				base.OnNonUICompleteExecute(context, null);
			}
			
		}

		#endregion
	}
}


#region JUNK YARD
///// <summary>
///// 
///// </summary>
//[Browsable(false)]
//public OutArgument<DataSet> OutDataSet { get; set; }


//[Browsable(false)]
//public OutArgument<List<DataRow>> OutputRows { get;set;}


//public static readonly string OutDataSetProperty = "OutDataSet";
//public static readonly string OutputRowsProperty = "OutputRows";
#endregion