#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: lnhelper.cs
// NameSpace: Phoenix.BusObj.Loan.Helper
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change
//-------------------------------------------------------------------------------
//Apr-08-2008	1.0		rpoddar		Created.
//------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.Data;
using System.Text;
using System.Collections;
using System.Globalization;
using Phoenix.FrameWork.BusFrame;


namespace Phoenix.BusObj.Loan
{
	/// <summary>
	/// Summary description for Helper.
	/// </summary>
	public class LnHelper : Phoenix.FrameWork.BusFrame.BusObjectBase
	{
		#region Private Fields
		#endregion Private fields

		#region Constructor
		public LnHelper():
				base() {
			InitializeMap();
		}
		#endregion

		#region Public Properties
		#endregion Public Properties

		#region Initialize method
		protected override void InitializeMap()
		{
			#region Table Mapping
			this.DbObjects.Add( "LN_HELPER", "X_LN_HELPER");
			#endregion Table Mapping

			#region  Column Mapping
			#endregion

			#region  Keys
			#endregion

			#region  Enumerable Values
			#endregion

			#region  Supported Actions
			this.SupportedAction |= XmActionType.Update | XmActionType.New | XmActionType.Delete | XmActionType.Custom;
            this.IsOfflineSupported = false;
			#endregion
			base.InitializeMap();
		}
		#endregion

	}

}
