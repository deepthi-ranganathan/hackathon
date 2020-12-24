#region (C) CopyRight
//-------------------------------------------------------------------------------
// Copyright (C) 2003-2004 London Bridge Phoenix Software
// All rights reserved.
//-------------------------------------------------------------------------------
#endregion

#region Comments
//-------------------------------------------------------------------------------
// File Name: PhoenixApplication.cs
// NameSpace: PhoenixApp
//-------------------------------------------------------------------------------
//Date			Ver 	Init    	Change              
//-------------------------------------------------------------------------------
//07/20/2008    1       TBH         #74577 - .Net Infrastructure Changes
//-------------------------------------------------------------------------------

#endregion

using System;
using System.Reflection;
using System.Xml;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
//
using Phoenix.FrameWork.StartUp.RemotingStub;




namespace Phoenix.Client
{
	/// <summary>
	/// Summary description for PhoenixApplication.
	/// </summary>
	public class PhoenixApplication
	{
		
		#region Private Variables
		string _assemblyName = "Phoenix.Client.LogonMain";
		string _launcherName = "Phoenix.Client.LogonMain.AppLauncher";
		bool _supportOffline;
		string _mdiFullTypeName;
		string _privateRuntime;
		string _appName;
		string _configUrl;
		string _releaseGroup;
		private RemotingStub _remotingStub	= null;
		#endregion

		#region Constructors
		public PhoenixApplication( )
		{
				ParseCommandLine();		
		}

		public PhoenixApplication( string appName )
		{
			_appName = appName;
		}


		#endregion

		#region Public Properties
		public string MdiFullTypeName
		{
			get{ return _mdiFullTypeName;}
			set{ _mdiFullTypeName = value;}
		}

		public bool IsOfflineSupported
		{
			get{ return _supportOffline;}
			set{ _supportOffline = value;}
		}

		public string StartAssemblyName
		{
			get{ return _assemblyName;}
			set{ _assemblyName = value;}
		}
		
		public string StartClassName
		{
			get{ return _launcherName;}
			set{ _launcherName = value;}
		}
		
		public string ApplicationName
		{
			get{ return _appName;}
			set{ _appName = value;}
		}

		public string ConfigWsvcsUrl
		{
			get{ return _configUrl;}
			set{ _configUrl = value;}
		}
		public string ReleaseGroup
		{
			get{ return _releaseGroup;}
			set{ _releaseGroup = value;}
		}
		#endregion


		#region Public Methods
	
		public void LaunchApplication()
		{
			string errorMessage = null;
			#region STEP - 1 - Initialise
			try
			{
				if( !Initialize())
				{
					errorMessage = "Initialization Failed";
				}
				
			}
			catch ( Exception e )
			{
				errorMessage = e.Message ;
				
			}
			if( errorMessage != null )
			{
				ShowMessage( errorMessage);
				return;
			}
			#endregion

			#region STEP - 2 - Load Assembly
			Assembly assemblyInstance	= null;
			Type typeInstance			= null;
			MethodInfo initFunction		= null;
			object launcher				= null;
			object[] parameters			= new object[3];
			//Type[] typeArray =new Type[6];
			

			// string configUrl, 
			//string relGroup, 
			// string appName, bool supportsOffline, string mdiAssembly, Bitmap logonBitMapstring configUrl, string relGroup, string appName, bool supportsOffline, string mdiAssembly, Bitmap logonBitMap
			parameters[0] = _configUrl; 
			//typeArray.SetValue(typeof(string),0);
			parameters[1] = _releaseGroup;
			//typeArray.SetValue(typeof(string),1);
			parameters[2] = _appName;
			//typeArray.SetValue(typeof(string),2);
//			parameters[3] = SupportOffline;
//			typeArray.SetValue(typeof(bool),3);
//			parameters[4] = MdiFullTypeName;
//			typeArray.SetValue(typeof(string),4);
//			parameters[5] = GetLogonBitmap();
//			typeArray.SetValue(typeof(Bitmap),5);
			//
			try
			{
				assemblyInstance = Assembly.Load( _assemblyName );
				//
				if( assemblyInstance != null  )
				{
					//  use type name to get type from asm; note we WANT case specificity 
					typeInstance = assemblyInstance.GetType( _launcherName, true, false );
				}

				if( typeInstance != null )
				{
			
					initFunction = typeInstance.GetMethod( "Launch"); //, typeArray );
				}
				
				if( initFunction != null )
				{
					launcher =  Activator.CreateInstance( typeInstance );
					if( MdiFullTypeName != null )
						SetProperty(launcher, "MdiFullTypeName", MdiFullTypeName );
					if( IsOfflineSupported )
						SetProperty(launcher, "IsOfflineSupported", IsOfflineSupported );
					Bitmap logonBmp = GetLogonBitmap();
					if( logonBmp != null )
						SetProperty( launcher, "LogonBitmap", logonBmp );
				}
				else
					ShowMessage( "Unable to get the entry function." );
			}
			catch( Exception e )
			{
				ShowMessage( "Unable to Start the application.\n\nDetails:\n" + e.Message  );
				return;
			}
			
			if( initFunction != null && launcher != null )
				initFunction.Invoke( launcher, parameters );
			
			
			#endregion
		

		}

		private void SetProperty( object launcher, string propertyName, object propertyValue )
		{
			PropertyInfo prop = launcher.GetType().GetProperty( propertyName );
			if( prop != null )
			{
				prop.SetValue( launcher, propertyValue, null );
			}

		}

		private Bitmap GetLogonBitmap()
		{
			Bitmap bitmap  =  null;
			Stream strm  = null;
			try
			{
				
				string resName = "logon.bmp";	
				string[] resNames = this.GetType().Assembly.GetManifestResourceNames();
				
				//Now get the resource out of the assembly.
				if( resNames != null && resNames.Length > 0 )
				{
					resName = resName.ToLower();
					for( int index = 0; index < resNames.Length; index++ )
					{
						if( resNames[index].ToLower().LastIndexOf(resName) == resNames[index].Length - resName.Length )
						{
							strm = this.GetType().Assembly.GetManifestResourceStream(resNames[index]);
							break;
						}
					}						
					
					if( strm != null )
					{						
						bitmap  = new System.Drawing.Bitmap(strm);
						strm.Close();
					}					
						
				}						
				
			}
			
			catch( Exception e )
			{
				//Form errorForm = new dlgException(e);
				//errorForm.ShowDialog();	
				ShowMessage( e.Message );
				if( strm != null )
					strm.Close();
			}
			return bitmap;	
		}

		#endregion 

		#region Private Methods
        //private bool AttachToDM()
        //{
        //    string[] commandLine =	 Environment.GetCommandLineArgs();
        //    if( _releaseGroup == null )
        //        _releaseGroup = string.Empty;
        //    // Create stub Instance
        //    _remotingStub	=	 new RemotingStub(_configUrl, _releaseGroup, commandLine[0],true,null);
        //    //Add AssemblyResolve
        //    AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        //    return true;
        //}

		private bool Initialize()
		{
			//
			ValidateParams();
			//
			if( (!(_configUrl != null && _configUrl.Length > 0 )) || ( !(_appName != null && _appName.Length > 0 )))
			{
				ShowUsage();
				return false;
			}
            return true;
			//
			//return AttachToDM();

		}

		private void ParseCommandLine()
		{
			string[] args = System.Environment.GetCommandLineArgs();
			for( int i = 1; i < args.Length; i++)
			{
				if( i == 1 )
					_configUrl = args[i];
				else if ( i== 2 )
					_appName = args[i];
				else if ( i == 3 )
					_releaseGroup = args[i];

			}

			
		}
		private void ValidateParams()
		{
			if( !(_appName != null && _appName.Length > 0 ))
				_appName = MakeAppName();
			
			
			Uri file = new Uri( Path.Combine (System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "Phoenix.Client.Config" ));
			//
			XmlDocument xmlDoc = new XmlDocument();
			bool isFile = file.Scheme == "file";
			if (isFile) 
			{
				xmlDoc.Load( file.LocalPath);
			}
			else 
			{
				XmlTextReader temp = new XmlTextReader( file.AbsoluteUri );
				xmlDoc.Load( temp );
		
			}
			
			if( xmlDoc != null )
			{
				XmlElement rootNode = xmlDoc.DocumentElement;
				foreach( XmlElement temp in rootNode.ChildNodes )
				{
					if( temp.Name == "APP_CONFIG" )
					{
						if( !(_configUrl != null && _configUrl.Length > 0) )
						{
							_configUrl = GetXmlValue( temp, "ConfigWsvcs" );
						}
						_privateRuntime = GetXmlValue( temp, "AssemblyDir" );
						break;
					}
				}

			}
						
			if( _privateRuntime != null && _privateRuntime.Length > 0 )
			{
				AppDomain.CurrentDomain.AppendPrivatePath( _privateRuntime );
			}

		}

		private string GetXmlValue( XmlElement parentElement, string childName )
		{
			XmlNode configNode = parentElement.SelectSingleNode( childName );
			if( configNode != null )
			{
				return configNode.InnerText;
			}
			return null;

		}
		private string MakeAppName()
		{
			string[] args =  System.Environment.GetCommandLineArgs();
			FileInfo temp = new FileInfo(  args[0] );
			string caller = temp.Name;
			

			string[] split = args[0].Split('.');
			string appName;
			if( split.Length > 1 )
				appName = split[split.Length - 2];
			else
				appName = split[0];
			return appName.ToUpper();
		}
		#endregion

		private System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			//Get the AssemblyName
			string assemblyName	= args.Name.Split(',')[0];
			//	
			//Get DM to Download the Assembly
			bool ret	=	_remotingStub.RequestAssemblyFromDM(assemblyName);
			return Assembly.LoadFrom(assemblyName);		
		}

		internal void ShowMessage( string message )
		{
			System.Windows.Forms.MessageBox.Show( message, "Application Error" );
		}

		
		private void ShowUsage( )
		{	
			string[] args =  System.Environment.GetCommandLineArgs();
			ShowMessage( "Unable to get enough information to start the application. \n\nUsage:\n\t" + args[0] + " <configWsvcs> <appName> <relGroupName>" );
		}
	}
}
