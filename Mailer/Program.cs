using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net;

namespace Mailer
{
	class Program
	{
		static Config m_Config = new Config();

		static void Main( string[] args )
		{
			Console.WriteLine( "### Mailer ###\n" );

			Console.ReadKey();
		
			if( args.Length < 1 )
			{
				Console.WriteLine( "Error, no Data" );
				return;
			}

			m_Config.LoadXml( args[ 0 ] );

			Console.WriteLine( "\nConfiguration:\n" );
			Console.WriteLine( m_Config.ToString() );

			Console.WriteLine( "\nPress [Return] to continue..." );
			Console.ReadKey();

			for( int nIdx = 0; nIdx < m_Config.Count; nIdx++ )
			{
				try
				{
					// Email Anlegen
					MailMessage Email = new MailMessage();

					// Die Email kommt von
					Email.From = new MailAddress( m_Config.Absender );

					// Die Email geht an
					foreach( String sMail in m_Config.Targets )
					{
						Email.To.Add( sMail );
					}

					// Email als Wichtig markieren
					Email.Priority = MailPriority.High;

					// mit dem Betreff
					Email.Subject = "Importand!";

					// mit dem Inhalt
					Email.Body = "Some Body";

					// der SMTPClient
					SmtpClient MailClient = new SmtpClient( m_Config.ServerAdress, m_Config.Port );

					// SSL (de)aktivieren, je nach Konfiguration
					MailClient.EnableSsl = m_Config.SSL;

					// Anmeldungsinformationen hinzufügen
					NetworkCredential MyCredentials = new NetworkCredential( m_Config.UserName, m_Config.Password );

					// Die Anmeldungsinformationen dem MailClient zufügen
					MailClient.Credentials = MyCredentials;

					// Und die E-Mail senden
					MailClient.Send( Email );
				}

				// wenn was schief läuft will ich das wissen
				catch( Exception ex )
				{
					Console.WriteLine( "Error while sending message", ex );
				}
			}
		}
	}
}
