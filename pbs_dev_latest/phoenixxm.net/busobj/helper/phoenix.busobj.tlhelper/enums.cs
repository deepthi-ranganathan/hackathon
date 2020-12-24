#region (C) Copyright
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: Enums.cs
// NameSpace: Phoenix.FrameWork.BusFrame
//-------------------------------------------------------------------------------
//Date        	Ver 	Init    	Change
//-------------------------------------------------------------------------------
//Jul-28-2004	1.0		Muthu		Created
//02/11/08      2       DGupta      #76052 - Added TranOdCcAction
//-------------------------------------------------------------------------------

#endregion

using System;

namespace Phoenix.BusObj.Teller
{
	/// <summary>
	/// Db Types
	/// </summary>

	public enum TranPostStatus
	{
		/// <summary>
		/// Posted Transaction(Set) is a success
		/// </summary>
		Success = 0,
		/// <summary>
		/// Posted Transaction(Set) is a failure
		/// </summary>
		Failure = 1,
		/// <summary>
		/// Posted Transaction(Set) has overrides
		/// </summary>
		HasOverrides = 2
	}

	public enum TranOfflineStatus
	{
		/// <summary>
		/// Post transcation only in offline
		/// </summary>
		OfflineOnly = 1,
		/// <summary>
		/// Post transcation both in offline/online
		/// </summary>
		OnlinePlus = 2,
		/// <summary>
		/// Post transcation only in online
		/// </summary>
		OnlineOnly = 3
	}
	public enum TlJournalTranStatus
	{	
		/// <summary>
		/// Transaction posted Normal OR Transaction posted in online only mode
		/// </summary>
		OnlinePosted = 0,

		/// <summary>
		/// Transaction posted in network offline only mode
		/// </summary>
		NetworkOfflineNotForwarded = 1,
		
		/// <summary>
		/// Transaction reposted in network offline mode
		/// </summary>
		NetworkOfflineForwarded = 2,

		/// <summary>
		/// Transaction filed during the network offline forwarding
		/// </summary>
		NetworkOfflineFailedForwarding = 3,

		/// <summary>
		/// Transaction posted to primary and network offline databases
		/// </summary>
		OnlinePlusPosted = 4,

		/// <summary>
		/// Transaction failed but force forwarded during the network offline forwarding
		/// </summary>
		NetworkOfflineForceForwarded = 5,

		/// <summary>
		/// Transaction memo posted
		/// </summary>
		MemoPosted = 8,
		/// <summary>
		/// Transaction real-time posted
		/// </summary>
		RealTimePosted = 9
	}
	public enum TlJournalBatchStatus
	{
		/// <summary>
		/// Transaction is not part of a batch
		/// </summary>
		NotPartOfTransactionBatch = 0,

		/// <summary>
		/// Transaction posted as part of a batch but not yet batched
		/// </summary>
		UnbatchedItem = 1,


		/// <summary>
		/// Tranaction posted as part of a batch and batch processed
		/// </summary>
		BatchedItem = 2
	}
    public enum TranOdCcAction
    {
        None = 0,
        Post = 1, // Post the OD & Nsf charge
        Waive = 2 // Waive the OD & NSF charge
   

    }
}
