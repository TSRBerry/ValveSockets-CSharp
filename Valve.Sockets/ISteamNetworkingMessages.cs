// ----------------------------------------------------------------------------
// <auto-generated>
// This is autogenerated code by CppSharp.
// Do not edit this file or all your changes will be lost after re-generation.
// </auto-generated>
// ----------------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using __CallingConvention = global::System.Runtime.InteropServices.CallingConvention;
using __IntPtr = global::System.IntPtr;


namespace Valve.Sockets
{
    /// <summary>
    /// <para>The non-connection-oriented interface to send and receive messages</para>
    /// <para>(whether they be "clients" or "servers").</para>
    /// </summary>
    /// <remarks>
    /// <para>ISteamNetworkingSockets is connection-oriented (like TCP), meaning you</para>
    /// <para>need to listen and connect, and then you send messages using a connection</para>
    /// <para>handle.  ISteamNetworkingMessages is more like UDP, in that you can just send</para>
    /// <para>messages to arbitrary peers at any time.  The underlying connections are</para>
    /// <para>established implicitly.</para>
    /// <para>Under the hood ISteamNetworkingMessages works on top of the ISteamNetworkingSockets</para>
    /// <para>code, so you get the same routing and messaging efficiency.  The difference is</para>
    /// <para>mainly in your responsibility to explicitly establish a connection and</para>
    /// <para>the type of feedback you get about the state of the connection.  Both</para>
    /// <para>interfaces can do "P2P" communications, and both support both unreliable</para>
    /// <para>and reliable messages, fragmentation and reassembly.</para>
    /// <para>The primary purpose of this interface is to be "like UDP", so that UDP-based code</para>
    /// <para>can be ported easily to take advantage of relayed connections.  If you find</para>
    /// <para>yourself needing more low level information or control, or to be able to better</para>
    /// <para>handle failure, then you probably need to use ISteamNetworkingSockets directly.</para>
    /// <para>Also, note that if your main goal is to obtain a connection between two peers</para>
    /// <para>without concerning yourself with assigning roles of "client" and "server",</para>
    /// <para>you may find the symmetric connection mode of ISteamNetworkingSockets useful.</para>
    /// <para>(See k_ESteamNetworkingConfig_SymmetricConnect.)</para>
    /// </remarks>
    public partial interface ISteamNetworkingMessages : IDisposable
    {
        /// <summary>
        /// <para>Sends a message to the specified host.  If we don't already have a session with that user,</para>
        /// <para>a session is implicitly created.  There might be some handshaking that needs to happen</para>
        /// <para>before we can actually begin sending message data.  If this handshaking fails and we can't</para>
        /// <para>get through, an error will be posted via the callback SteamNetworkingMessagesSessionFailed.</para>
        /// <para>There is no notification when the operation succeeds.  (You should have the peer send a reply</para>
        /// <para>for this purpose.)</para>
        /// </summary>
        /// <remarks>
        /// <para>Sending a message to a host will also implicitly accept any incoming connection from that host.</para>
        /// <para>nSendFlags is a bitmask of k_nSteamNetworkingSend_xxx options</para>
        /// <para>nRemoteChannel is a routing number you can use to help route message to different systems.</para>
        /// <para>You'll have to call ReceiveMessagesOnChannel() with the same channel number in order to retrieve</para>
        /// <para>the data on the other end.</para>
        /// <para>Using different channels to talk to the same user will still use the same underlying</para>
        /// <para>connection, saving on resources.  If you don't need this feature, use 0.</para>
        /// <para>Otherwise, small integers are the most efficient.</para>
        /// <para>It is guaranteed that reliable messages to the same host on the same channel</para>
        /// <para>will be be received by the remote host (if they are received at all) exactly once,</para>
        /// <para>and in the same order that they were sent.</para>
        /// <para>NO other order guarantees exist!  In particular, unreliable messages may be dropped,</para>
        /// <para>received out of order with respect to each other and with respect to reliable data,</para>
        /// <para>or may be received multiple times.  Messages on different channels are *not* guaranteed</para>
        /// <para>to be received in the order they were sent.</para>
        /// <para>A note for those familiar with TCP/IP ports, or converting an existing codebase that</para>
        /// <para>opened multiple sockets:  You might notice that there is only one channel, and with</para>
        /// <para>TCP/IP each endpoint has a port number.  You can think of the channel number as the</para>
        /// <para>*destination* port.  If you need each message to also include a "source port" (so the</para>
        /// <para>recipient can route the reply), then just put that in your message.  That is essentially</para>
        /// <para>how UDP works!</para>
        /// <para>Returns:</para>
        /// <para>- k_EREsultOK on success.</para>
        /// <para>- k_EResultNoConnection, if the session has failed or was closed by the peer and</para>
        /// <para>k_nSteamNetworkingSend_AutoRestartBrokenSession was not specified.  (You can</para>
        /// <para>use GetSessionConnectionInfo to get the details.)  In order to acknowledge the</para>
        /// <para>broken session and start a new one, you must call CloseSessionWithUser, or you may</para>
        /// <para>repeat the call with k_nSteamNetworkingSend_AutoRestartBrokenSession.  See</para>
        /// <para>k_nSteamNetworkingSend_AutoRestartBrokenSession for more details.</para>
        /// <para>- See ISteamNetworkingSockets::SendMessageToConnection for more possible return values</para>
        /// </remarks>
        global::Valve.Sockets.EResult SendMessageToUser(global::Valve.Sockets.SteamNetworkingIdentity identityRemote, __IntPtr pubData, uint cubData, int nSendFlags, int nRemoteChannel);

        /// <summary>
        /// <para>Reads the next message that has been sent from another user via SendMessageToUser() on the given channel.</para>
        /// <para>Returns number of messages returned into your list.  (0 if no message are available on that channel.)</para>
        /// </summary>
        /// <remarks>When you're done with the message object(s), make sure and call SteamNetworkingMessage::Release()!</remarks>
        int ReceiveMessagesOnChannel(int nLocalChannel, global::Valve.Sockets.SteamNetworkingMessage ppOutMessages, int nMaxMessages);

        /// <summary>
        /// <para>Call this in response to a SteamNetworkingMessagesSessionRequest callback.</para>
        /// <para>SteamNetworkingMessagesSessionRequest are posted when a user tries to send you a message,</para>
        /// <para>and you haven't tried to talk to them first.  If you don't want to talk to them, just ignore</para>
        /// <para>the request.  If the user continues to send you messages, SteamNetworkingMessagesSessionRequest</para>
        /// <para>callbacks will continue to be posted periodically.</para>
        /// </summary>
        /// <remarks>
        /// <para>Returns false if there is no session with the user pending or otherwise.  If there is an</para>
        /// <para>existing active session, this function will return true, even if it is not pending.</para>
        /// <para>Calling SendMessageToUser() will implicitly accepts any pending session request to that user.</para>
        /// </remarks>
        bool AcceptSessionWithUser(global::Valve.Sockets.SteamNetworkingIdentity identityRemote);

        /// <summary>
        /// <para>Call this when you're done talking to a user to immediately free up resources under-the-hood.</para>
        /// <para>If the remote user tries to send data to you again, another SteamNetworkingMessagesSessionRequest</para>
        /// <para>callback will be posted.</para>
        /// </summary>
        /// <remarks>Note that sessions that go unused for a few minutes are automatically timed out.</remarks>
        bool CloseSessionWithUser(global::Valve.Sockets.SteamNetworkingIdentity identityRemote);

        /// <summary>
        /// <para>Call this  when you're done talking to a user on a specific channel.  Once all</para>
        /// <para>open channels to a user have been closed, the open session to the user will be</para>
        /// <para>closed, and any new data from this user will trigger a</para>
        /// <para>SteamSteamNetworkingMessagesSessionRequest callback</para>
        /// </summary>
        bool CloseChannelWithUser(global::Valve.Sockets.SteamNetworkingIdentity identityRemote, int nLocalChannel);

        /// <summary>
        /// <para>Returns information about the latest state of a connection, if any, with the given peer.</para>
        /// <para>Primarily intended for debugging purposes, but can also be used to get more detailed</para>
        /// <para>failure information.  (See SendMessageToUser and k_nSteamNetworkingSend_AutoRestartBrokenSession.)</para>
        /// </summary>
        /// <remarks>
        /// <para>Returns the value of SteamNetConnectionInfo::m_eState, or k_ESteamNetworkingConnectionState_None</para>
        /// <para>if no connection exists with specified peer.  You may pass nullptr for either parameter if</para>
        /// <para>you do not need the corresponding details.  Note that sessions time out after a while,</para>
        /// <para>so if a connection fails, or SendMessageToUser returns k_EResultNoConnection, you cannot wait</para>
        /// <para>indefinitely to obtain the reason for failure.</para>
        /// </remarks>
        global::Valve.Sockets.ESteamNetworkingConnectionState GetSessionConnectionInfo(global::Valve.Sockets.SteamNetworkingIdentity identityRemote, global::Valve.Sockets.SteamNetConnectionInfo pConnectionInfo, global::Valve.Sockets.SteamNetConnectionRealTimeStatus pQuickStatus);
    }
}