namespace Valve.Sockets
{
    public enum EDurationControlNotification : uint
    {
        k_EDurationControlNotification_None = 0,
        k_EDurationControlNotification_1Hour = 1,
        k_EDurationControlNotification_3Hours = 2,
        k_EDurationControlNotification_HalfProgress = 3,
        k_EDurationControlNotification_NoProgress = 4,
        k_EDurationControlNotification_ExitSoon_3h = 5,
        k_EDurationControlNotification_ExitSoon_5h = 6,
        k_EDurationControlNotification_ExitSoon_Night = 7,
    }
}