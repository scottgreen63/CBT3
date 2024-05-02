using System.Security.Principal;

namespace CBT3_Shared;

public class UserDetails
{
    private readonly IHttpContextAccessor _contextaccessor;

    //public UserDetails(IHttpContextAccessor contextaccessor)
    //{
    //    _contextaccessor = contextaccessor;


    //}
    public UserDetails()
    {



    }

    

    public string GetRemoteUserName()
    {
        // Accessing HttpContext, User, Identity, Name, and splitting by '\' to get the user name.
        // Then converting to uppercase. If any member along the chain is null, it returns null.
        string? user = _contextaccessor?.HttpContext?.User?.Identity?.Name?.Split('\\').Last()?.ToUpper();

        // If 'user' is null, return "Unknown", otherwise return the user name.
        return user ?? "Unknown";
    }
    public string GetRemoteIpIP()
    {
        // Accessing HttpContext, Connection, RemoteIpAddress, converting to IPv4, and then to string.
        // If any member along the chain is null, it returns null.
        string? userIP = _contextaccessor?.HttpContext?.Connection?.RemoteIpAddress?.MapToIPv4()?.ToString();

        // If 'userIP' is null, return "Unknown", otherwise return the user's IP address.
        return userIP ?? "Unknown";
    }
    private static string GetMachineNameFromIPAddress(string ipAddress)
    {
        string machineName;
        try
        {
            IPHostEntry hostEntry = Dns.GetHostEntry(ipAddress);

            machineName = hostEntry.HostName;

        }
        catch (Exception)
        {
            return "unknown";
        }

        return machineName;
    }
    public string GetLocalUserName()
    {
        // Using Environment to get the current user name
        return Environment.UserName ?? "Unknown";
    }
    public string GetLocalMachineName()
    {
        // Using Environment to get the machine name
        return Environment.MachineName ?? "Unknown";
    }
    public string GetLocalWindowsUserName()
    {
        // Using WindowsIdentity to get the current user name in a Windows environment
        return WindowsIdentity.GetCurrent()?.Name?.Split('\\').Last() ?? "Unknown";
    }
    public string GetLocalIpIP()
    {
        // Getting local IP address
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);
        foreach (IPAddress address in addresses)
        {
            if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return address.ToString();
            }
        }
        return "Unknown";
    }
}
