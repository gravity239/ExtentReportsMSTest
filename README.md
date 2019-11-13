# KiewitTeamBinder Automation Test


## InternetExplorer - Required Configuration

In order for the TeamBinder tests to run well on the InternetExplorer 11 browser, you will need to configure the browser on the target machine (where you desire to run automation):

**Protected Mode Settings**
You must set the Protected Mode settings same value for all the Zones. To set the Protected Mode settings, choose "Internet Options..." from the Tools menu, and click on the Security tab. For each zone, there will be a check box at the bottom of the tab labeled "Enable Protected Mode".
Additionally, _"Enhanced Protected Mode"_ must be disabled for IE 10 and higher. This option is found in the Advanced tab of the Internet Options dialog.

**Browser Zoom Level**
The browser zoom level must be set to 100% so that the native mouse events can be set to the correct coordinates.

**Pop-up Blocker**
Pop-up Blocker need to turn off. This setting is found in the Privacy tab of the Internet Options dialog.

**Setting Registry Key / Entry**
For IE 11 only, you will need to set a registry entry on the target computer so that the driver can maintain a connection to the instance of Internet Explorer it creates. 
For 32-bit Windows installations, the key you must examine in the registry editor is "HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BFCACHE". 
For 64-bit Windows installations, the key is "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BFCACHE". 
Please note that the FEATURE_BFCACHE subkey may or may not be present, and should be created if it is not present. 
**Important:** Inside this key, create a DWORD value named **iexplore.exe** with the value of 0.


