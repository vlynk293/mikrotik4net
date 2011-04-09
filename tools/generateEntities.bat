REM System
Mikrotik.ApiGenerator.exe ".\defs\System.SystemResource.xml" "..\tik4net\Objects\System\SystemResources"
Mikrotik.ApiGenerator.exe ".\defs\Log.xml" "..\tik4net\Objects\Log"
REM Queue
Mikrotik.ApiGenerator.exe ".\defs\Queue.QueueSimple.xml" "..\tik4net\Objects\Queue\QueueSimple"
Mikrotik.ApiGenerator.exe ".\defs\Queue.QueueTree.xml" "..\tik4net\Objects\Queue\QueueTree"
Mikrotik.ApiGenerator.exe ".\defs\Queue.QueueType.xml" "..\tik4net\Objects\Queue\QueueType"
REM firewal
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallFilter.xml" "..\tik4net\Objects\Ip\Firewall\FirewallFilter"
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallNat.xml" "..\tik4net\Objects\Ip\Firewall\FirewallNat"
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallMangle.xml" "..\tik4net\Objects\Ip\Firewall\FirewallMangle"
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallAddressList.xml" "..\tik4net\Objects\Ip\Firewall\FirewallAddressList"
REM interface
Mikrotik.ApiGenerator.exe ".\defs\Interface.xml" "..\tik4net\Objects\Interface"
Mikrotik.ApiGenerator.exe ".\defs\Interface.InterfaceEthernet.xml" "..\tik4net\Objects\Interfaces\InterfaceEthernet"
Mikrotik.ApiGenerator.exe ".\defs\Interface.InterfaceWireless.xml" "..\tik4net\Objects\Interfaces\InterfaceWireless"