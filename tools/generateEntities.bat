REM System
Mikrotik.ApiGenerator.exe ".\defs\System.SystemResource.xml" "..\tik4net\Objects\System\SystemResources"
Mikrotik.ApiGenerator.exe ".\defs\Log.xml" "..\tik4net\Objects\Log"
REM Queue
Mikrotik.ApiGenerator.exe ".\defs\Queue.QueueTree.xml" "..\tik4net\Objects\Queue\QueueTree"
REM firewal
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallMangle.xml" "..\tik4net\Objects\Ip\Firewall\FirewallMangle"
Mikrotik.ApiGenerator.exe ".\defs\Ip.Firewall.FirewallAddressList.xml" "..\tik4net\Objects\Ip\Firewall\FirewallAddressList"

