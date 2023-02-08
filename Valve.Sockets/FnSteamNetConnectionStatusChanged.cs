using System.Runtime.InteropServices;

namespace Valve.Sockets
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public unsafe delegate void FnSteamNetConnectionStatusChanged(SteamNetConnectionStatusChangedCallback_t* param0);
}
