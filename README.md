# PopSign

PopSign is a bubble shooter style game that teaches basic American Sign Language vocabulary. It is developed and maintained by the [Center for Accessible Technology in Sign](http://cats.gatech.edu/) at the Georgia Institute of Technology.

## Requirements

This game can be built and run on Android/iOS devices using the latest version of [Unity 2021 LTS](https://unity3d.com/get-unity/download/archive). Newer versions of Unity may not be compatible with PopSign.  

## Source Code File Structure

Note: this isn't every file and directory in the project, just the ones that I felt were relevant

├── Assets: the main directory where content is stored in Unity games  
| ├── FramesToVideo: all videos for the game, plus images for each frame in the video  
| ├── PopSignMain: what it sounds like  
&nbsp;&nbsp;&nbsp;| ├── Animation: files related to in game animation effects  
&nbsp;&nbsp;&nbsp;| ├── Audio: files related to in game audio  
&nbsp;&nbsp;&nbsp;| ├── Fonts: files related to the font used in the game  
&nbsp;&nbsp;&nbsp;| ├── Materials: Unity meta files (purpose is unclear)  
&nbsp;&nbsp;&nbsp;| ├── Plugins: files related to plugins we have added (e.g. Android and iOS)  
&nbsp;&nbsp;&nbsp;| ├── Prefabs: Unity files for prefab objects in the game  
&nbsp;&nbsp;&nbsp;| └── Resources: assets that are loaded in to the game programmatically (levels, word banks, icons, etc.)  
&nbsp;&nbsp;&nbsp;| ├── Levels: one file per level, defining the level type and what the ball structure should look like for that level  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── Particles: Unity meta files (purpose is unclear)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── Prefabs: Unity files for prefab objects that are loaded programmatically in to the game  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── VideoCaption: banners that caption each video if the user presses the "answer" button in game  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── VideoConnection: one file per level, defines words for each level and the video location for each word  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── WordBanks: word banks for the various Macarthur-Bates categories we use  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| └── WordIcons: one png per word, drawn on top of each ball in the game  
&nbsp;&nbsp;&nbsp;| ├── Scenes: contains the actual scene files that you can modify and run using the Unity Editor  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── game: the actual gameplay scene  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── map: the "home screen" scene that allows you to select a level or navigate to wordlist/settings  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── opening: the "Welcome To PopSign!" scene that shows when you open the game  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── practice: the scene where users practice signs before playing the game  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── settings: the scene where users can adjust settings or learn how to play the game  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── tutorial: shows the user how to play the game (shown before the first attempt at level 1)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| └── wordlist: allows the user to review all the words they have learned so far in the game  
&nbsp;&nbsp;&nbsp;├── Scripts: what it sounds like- all the C# scripts that control the game (plus some Python scripts I wrote)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── Bubbles: gameplay specific code lives in this directory  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── Core: code that is core to the flow of the game lives in this directory  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── Editor: the code that allows us to directly edit levels in Unity lives in this directory  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| ├── GUI: code that is specific to a particular UI element (e.g. answer button) goes here  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;| └── Python: various scripts used to edit level data and video files (not used in the actual game)  
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;└── Textures: static assets (such as images) that are used to populate scenes and are not loaded programmatically  
├── Library: Unity specific lib files  
├── PlayerPrefsEditor: files for the plugin that allows us to edit PlayerPrefs in the Unity editor  
├── ProjectSettings: settings for our Unity project  
└── UnityPackageManager: contains our manifest.json (needed for Android build)  
