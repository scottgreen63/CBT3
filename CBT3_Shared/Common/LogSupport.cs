namespace CBT3_Shared.Common;

public class LogSupport
{
    private static string? _currentuser;
    private static string? _currentuserIp;
    public static string LogEntryHeader(UserDetails userdetails)
    {
        _currentuser = userdetails.GetLocalUserName();
        _currentuserIp = userdetails.GetLocalIpIP();
        return $"{_currentuser} {_currentuserIp} =>";
    }

    public static string LogPipelineEntryHeader(UserDetails userdetails)
    {
        _currentuser = userdetails.GetLocalUserName();
        _currentuserIp = userdetails.GetLocalIpIP();
        return $"{_currentuser} {_currentuserIp} =>";
    }
}
