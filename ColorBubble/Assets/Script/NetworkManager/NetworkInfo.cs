using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;

public class NetworkInfo : MonoBehaviour
{
    public string GetLocalIPAddress()
    {
        string localIPAddress = "localhost";
        IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

        foreach (IPAddress ip in localIPs)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                localIPAddress = ip.ToString();
                break;
            }
        }
        return localIPAddress;
    }
    private string ExtractIpAddress(string url)
    {
        string pattern = @"\b(?:\d{1,3}\.){3}\d{1,3}\b";

        MatchCollection matches = Regex.Matches(url, pattern);

        if (matches.Count > 0)
        {
            return matches[0].Value;
        }
        else
        {
            return "No IP address found";
        }
    }
    public string GetIPAddressFromUrl(string url)
    {
        return ExtractIpAddress(url);
    }
}
