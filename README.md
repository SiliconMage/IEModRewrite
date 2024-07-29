# IEModRewrite
The popular mod for Pillars of Eternity returns, written in the Unity Mod Manager framework! 

Please see the [Nexus Mods](https://www.nexusmods.com/pillarsofeternity/mods/347) page for a list of available features and how to use them.


# How to compile this project
You can get started by downloading the source code and extracting it to its own directory.  The most important step you'll want to complete is modifying IEMod.csproj.

You'll find a property called "<PoEDir>".  Make sure you set that to the installation directory of where Pillars of Eternity is installed.  This will make your references to the various DLLs that Pillars uses valid references in the project.

This also allows Aze.Publicize to find your Assembly-CSharp for publicization.  
