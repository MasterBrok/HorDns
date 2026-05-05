using Application.Exceptions;
using Application.Models;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Principal;

namespace Application.Services;

public class DnsService
{

    public bool Set(Ip ip)
    {
        try
        {
            
            if (!IsRunAsAdmin())
                throw new AccessDeniedException();

            var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
            if (CurrentInterface == null) return false;

            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Caption"].ToString().Contains(CurrentInterface.Description))
                    {
                        ManagementBaseObject objdns = objMO.GetMethodParameters("SetDNSServerSearchOrder");
                        if (objdns != null)
                        {
                            var dnsArray = string.IsNullOrEmpty(ip.Alternate)
                                 ? new[] { ip.Preferred }
                                 : new[] { ip.Preferred, ip.Alternate };
                            if (ip.IsEpmty)
                            {
                                dnsArray = null;
                            }
                            
                            objdns["DNSServerSearchOrder"] = dnsArray;
                            ManagementBaseObject outParams = objMO.InvokeMethod("SetDNSServerSearchOrder", objdns, null);
                            return ((uint)(outParams["ReturnValue"])) == 0;
                        }
                    }
                }
            }
            return false;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    private NetworkInterface GetActiveEthernetOrWifiNetworkInterface()
    {
        var Nic = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(
            a => a.OperationalStatus == OperationalStatus.Up &&
            (a.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || a.NetworkInterfaceType == NetworkInterfaceType.Ethernet) &&
            a.GetIPProperties().GatewayAddresses.Any(g => g.Address.AddressFamily.ToString() == "InterNetwork"));

        return Nic;
    }
    public Ip GetCurrentDns()
    {
        try
        {
            var CurrentInterface = GetActiveEthernetOrWifiNetworkInterface();
            if (CurrentInterface == null) return null;

            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection objMOC = objMC.GetInstances();

            foreach (ManagementObject objMO in objMOC)
            {
                if ((bool)objMO["IPEnabled"])
                {
                    if (objMO["Caption"].ToString().Contains(CurrentInterface.Description))
                    {
                        string[] dnsServers = (string[])objMO["DNSServerSearchOrder"];
                        if (dnsServers != null && dnsServers.Length > 0)
                        {
                            return new Ip
                            {
                                Preferred = dnsServers[0],
                                Alternate = dnsServers.Length > 1 ? dnsServers[1] : null
                            };
                        }
                    }
                }
            }
            return null;
        }
        catch (Exception ex)
        {
            return Ip.Epmty;
        }
    }
    public string[] GetNetworkAdapters()
    {

        var adapters = new List<string>();
        try
        {
            var scope = new ManagementScope("root\\CIMV2");
            scope.Connect();

            var query = new ObjectQuery(
                "SELECT Name, NetConnectionID, InterfaceIndex " +
                "FROM Win32_NetworkAdapter " +
                "WHERE NetEnabled = true OR PhysicalAdapter = true");

            using (var searcher = new ManagementObjectSearcher(scope, query))
            {
                foreach (ManagementObject adapter in searcher.Get())
                {
                    string name = adapter["Name"]?.ToString() ?? "";
                    string id = adapter["NetConnectionID"]?.ToString() ?? "";
                    string index = adapter["InterfaceIndex"]?.ToString() ?? "";

                    if (!string.IsNullOrEmpty(name))
                        adapters.Add($"[{index}] {name} ({id})");
                }
            }
            return adapters.ToArray();
        }
        catch (Exception ex)
        {
            return new[] { "Wi-Fi", "Ethernet", "Local Area Connection" };
        }
    }

    public bool IsRunAsAdmin()
    {
        try
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            return new WindowsPrincipal(identity).IsInRole(WindowsBuiltInRole.Administrator);
        }
        catch (Exception)
        {
            return false;
        }
    }
}
