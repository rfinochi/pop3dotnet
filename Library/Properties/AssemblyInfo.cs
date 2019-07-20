/*
 * Author: Rodolfo Finochietti
 * Email: rfinochi@shockbyte.net
 * Web: http://shockbyte.net
 *
 * This work is licensed under the MIT License. 
 * 
 * No warranties expressed or implied, use at your own risk.
 */
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Pop3;

[assembly: AssemblyTitle( "Pop3.NET" )]
[assembly: AssemblyProduct( "Pop3.NET" )]
[assembly: AssemblyDescription( "Small and simple library for retrieving messages from Post Office Protocol version 3 (POP3) servers." )]

[assembly: CLSCompliant( true )]

[assembly: Guid( "431ce83b-0fa5-41a5-ad9d-69ae3032462f" )]

[assembly: InternalsVisibleTo( "Pop3.Tests, PublicKey=" + Constants.PublicKey )]
[assembly: InternalsVisibleTo( "DynamicProxyGenAssembly2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100c547cac37abd99c8db225ef2f6c8a3602f3b3606cc9891605d02baa56104f4cfc0734aa39b93bf7852f7d9266654753cc297e7d2edfe0bac1cdcf9f717241550e0a7b191195b7667bb4f64bcb8e2121380fd1d9d46ad2d92d2d15605093924cceaf74c4861eff62abf69b9291ed0a340e113be11e6a7d3113e92484cf7045cc7" )]
