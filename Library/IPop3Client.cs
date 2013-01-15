/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the Creative Commons Attribution License. 
 * To view a copy of this license, visit http://creativecommons.org/licenses/by/2.0
 * or send a letter to Creative Commons, 559 Nathan Abbott Way, Stanford, California 94305, USA.
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Collections.Generic;
#if NET45 || NETFX_CORE
using System.Threading.Tasks;
#endif

namespace Pop3
{
    internal interface IPop3Client : IDisposable
    {
        #region Properties 

        bool IsConnected
        {
            get;
        }

        #endregion

        #region Methods

#if !NETFX_CORE

        void Connect( string server, string userName, string password );
        
        void Connect( string server, string userName, string password, bool useSsl );
        
        void Connect( string server, string userName, string password, int port, bool useSsl );
        
        void Disconnect( );
        
        IEnumerable<Pop3Message> List( );
        
        void Retrieve( Pop3Message message );
        
        void Retrieve( IEnumerable<Pop3Message> messages );
        
        void RetrieveHeader( Pop3Message message );
        
        void RetrieveHeader( IEnumerable<Pop3Message> messages );
        
        IEnumerable<Pop3Message> ListAndRetrieve( );
        
        IEnumerable<Pop3Message> ListAndRetrieveHeader( );
        
        void Delete( Pop3Message message );
#endif

        #endregion

        #region Async Methods

#if NET45 || NETFX_CORE

        Task ConnectAsync( string server, string userName, string password );
        
        Task ConnectAsync( string server, string userName, string password, bool useSsl );
        
        Task ConnectAsync( string server, string userName, string password, int port, bool useSsl );
        
        Task DisconnectAsync( );
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        Task<IEnumerable<Pop3Message>> ListAsync( );
        
        Task RetrieveAsync( Pop3Message message );
        
        Task RetrieveAsync( IEnumerable<Pop3Message> messages );
        
        Task RetrieveHeaderAsync( Pop3Message message );
        
        Task RetrieveHeaderAsync( IEnumerable<Pop3Message> messages );
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        Task<IEnumerable<Pop3Message>> ListAndRetrieveAsync( );
        
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures" )]
        Task<IEnumerable<Pop3Message>> ListAndRetrieveHeaderAsync( );
        
        Task DeleteAsync( Pop3Message message );
#endif

        #endregion
    }
}
