# steam game updater for windows
### update games & servers

This is a simple automation of steamcmd.  
Basically it will run app_status, check wether an update for the app is available and update it if required.

#### steam_game_updater.exe.config
```...
        <steam_game_updater.My.MySettings>
		
			<!-- enter path to steam library aka the directory containing the steamapps folder -->
            <setting name="steamappsPath" serializeAs="String">
                <value>E:\steam</value>
            </setting>
			
			<!-- your steam username -->
            <setting name="username" serializeAs="String">
                <value>username</value>
            </setting>
			
			<!-- your steam password -->
            <setting name="password" serializeAs="String">
                <value>password</value>
            </setting>
			
			<!-- id of the game/server -->
            <setting name="appId" serializeAs="String">
                <value>730</value>
            </setting>
			
			<!-- enable to be asked for confirmation before updating & closing the program -->
            <setting name="debugMode" serializeAs="String">
                <value>False</value>
            </setting>
			
			<!-- use validation with app_update. you may want to skip this when operating in small time frames -->
            <setting name="validate" serializeAs="String">
                <value>True</value>
            </setting>
...
```