# Pixel Gun 4.7.1 Decompilation (Work In Progress)

## Contents
1. [Setup](#setup)
2. [Building](#building)
   - [Preparing for building](#preparing-for-building)
   - [Windows/Standalone](#windows--standalone)
   - [Android](#android)
3. [Wiki](#wiki)
4. [Contributing](#contributing)
5. [Credits](#credits)

## Setup
1. Get Unity 2017.4.40f1 [(link)](https://unity.com/releases/editor/whats-new/2017.4.40#installs)
   - Make sure you have [Unity Hub](https://unity.com/download) installed
   - Optional: Use [UniHacker](https://github.com/tylearymf/UniHacker) to unlock dark theme and get rid of license related stuff.
2. Also install Visual Studio to edit scripts, there *should* be option in instalation window.
3. Clone this repo on your computer.
4. Open it with Unity Hub.
   - Make sure that you have proper unity version installed first.
5. Open it and you're done.
   - The *proper* way of playing it, is opening scene called __AppCenter__ and then running it.
  
## Building
#### Preparing for building
If you are on windows and want to build for it, nothing required, same goes to Linux, i dont know much about Mac OS.

If you want to build for Other Standalone platform than OS installed on your computer:
1. Open __Unity Hub__.
2. Move to __Instalations__.
3. Scroll down until you see *Your Unity Version* and click gear icon on it.
4. Choose __Add Modules__ option.
5. Select Windows/Linux/Mac checkbox, depends on your needs.
6. Click __Continue__ and wait until it installs everything you need and move to [Windows / Standalone](#windows--standalone).

If you want to build for Android:
1. Open __Unity Hub__.
2. Move to __Instalations__.
3. Scroll down until you see *Your Unity Version* and click gear icon on it.
4. Choose __Add Modules__ option.
5. Select __Android Build Support__ and click __Continue__.
6. Move to [Android](#android).

#### Windows / Standalone
1. Move to __File > Build Settings__
   - If platform is not Standalone by default, click Standalone option on left list and click __Switch Platform__
     - If __Switch Platform__ button is disabled, it means you're already using it.
   - Its on Left Up corner by the way.
2. In __Target Platform__ option, use platform you want to build to (Windows/Linux/Mac)
  - If you want to build for other platforms that your computer (like linux), you should install proper module for it. (See Modules Part)
3. All other options all optional, leave them like they are if you dont need anything special like debug logs and etc.
4. Click __Build__ button and select any **Empty** folder on you computer.
5. Done! Now you can enjoy your build!

#### Android 
- Work In Progress.

## Wiki
Work In Progress.

## Contributing
Bugs that weren't in original/unmodded version of game are allowed to be fixed.
Also Shader/Material/Other errors that weren't in original version of the game are allowed.

It is also allowed to change stuff to make PC gameplay better, and not change the og game at the same time, for example
Player could unpause game while being in shop, which leads to dying, we had to fix that.
Yes, it does change core of the game a little bit, but doesnt really change the game itself.

If you want to add guns/maps/other stuff into the game, this is not allowed, please make fork/copy of repo and then do whatever you want.

For other stuff, if you are not sure, just make a request, ill accept it or explain why i wont.
## Credits
Everything is currently done by overmet15.
