using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Mailer
{
	class Config
	{
		private List<String> m_lMailAddr = new List<String>();
		private int m_nCount;
		private String m_sServer;
		private int m_nPort;	
		private String m_sUserName;
		private String m_sPassword;
		private String m_sAbsender;
		private bool m_bSSL;

		public List<String> Targets
		{
			get
			{
				return m_lMailAddr;
			}
		}

		public int Count
		{
			get
			{
				return m_nCount;
			}
		}

		public String ServerAdress
		{
			get
			{
				return m_sServer;
			}
		}
		
		public int Port
		{
			get
			{
				return m_nPort;
			}
		}
		
		public String UserName
		{
			get
			{
				return m_sUserName;
			}
		}
		
		public String Password
		{
			get
			{
				return m_sPassword;
			}
		}
		
		public String Absender
		{
			get
			{
				return m_sAbsender;
			}
		}

		public bool SSL
		{
			get
			{
				return m_bSSL;
			}
		}

		public void LoadXml( String sXmlFile )
		{
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreWhitespace = true;

			using( XmlReader xmlReader = XmlReader.Create( sXmlFile, xmlReaderSettings ) )
			{
				int nMailConfigCnt = 0;

				while( xmlReader.Read() )
				{
					if( xmlReader.NodeType == XmlNodeType.Element )
					{
						switch( xmlReader.Name )
						{
							case "Count":
								{
									m_nCount = Convert.ToInt32( xmlReader.ReadString() );
								}
								break;

							case "Targets":
								{
									switch( xmlReader.Value )
									{
										case "count":
											{
												xmlReader.MoveToElement();

											}
											break;
									}

									while( xmlReader.Read() )
									{
										if( xmlReader.NodeType == XmlNodeType.Element )
										{
											switch( xmlReader.Name )
											{
												case "Mail":
													{
														m_lMailAddr.Add( xmlReader.ReadString() );
													}
													break;
											}
										}

										if( xmlReader.NodeType == XmlNodeType.EndElement )
										{
											if( xmlReader.Name == "Targets" )
											{
												break;
											}
										}
									}
								}
								break;

							case "MailConfig":
								{
									if( nMailConfigCnt == 0 )
									{
										nMailConfigCnt++;

										while( xmlReader.Read() )
										{
											if( xmlReader.NodeType == XmlNodeType.Element )
											{
												switch( xmlReader.Name )
												{
													case "Server":
														{
															m_sServer = xmlReader.ReadString();
														}
														break;

													case "Port":
														{
															m_nPort = Convert.ToInt32( xmlReader.ReadString() );
														}
														break;

													case "Username":
														{
															m_sUserName = xmlReader.ReadString();
														}
														break;

													case "Password":
														{
															m_sPassword = xmlReader.ReadString();
														}
														break;

													case "Absender":
														{
															m_sAbsender = xmlReader.ReadString();
														}
														break;

													case "SSL":
														{
															try
															{
																m_bSSL = Convert.ToBoolean( xmlReader.ReadString() );
															}
															catch( FormatException )
															{
																StringBuilder sMsg = new StringBuilder();
																sMsg.AppendFormat( "MailConfig.SSL - Ungültiger Wert '{0}'!\nNur 0 und 1 ist gültig", xmlReader.ReadString() );
																throw new NotSupportedException( sMsg.ToString() );
															}
														}
														break;
												}
											}

											if( xmlReader.NodeType == XmlNodeType.EndElement )
											{
												if( xmlReader.Name == "MailConfig" )
												{
													break;
												}
											}
										}
									}
								}
								break;
						}
					}
				}
			}
		}

		public override String ToString()
		{
			StringBuilder sb = new StringBuilder();

			sb.AppendFormat( "Server: {0}\n" +
							  "Port: {1}\n" +
							  "User: {2}\n" +
							  "Password: {3}\n" +
							  "Absender: {4}\n" +
							  "SSL: {5}\n"
							, m_sServer
							, m_nPort
							, m_sUserName
							, m_sPassword
							, m_sAbsender
							, m_bSSL
							);

			foreach( String sTarget in m_lMailAddr )
			{
				sb.AppendFormat( "Target: {0}\n", sTarget );
			}

			sb.AppendFormat( "Count: {0}\n", m_nCount );

			return sb.ToString();
		}
	}
}
