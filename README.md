# PopSign

PopSign is a bubble shooter style game that teaches basic American Sign Language vocabulary. It is developed and maintained by the [Center for Accessible Technology in Sign](http://cats.gatech.edu/) at the Georgia Institute of Technology.

## Requirements

This game can be built and run using the latest release of [Unity 2017](https://unity3d.com/get-unity/download/archive). Newer versions of Unity may not be compatible with PopSign.  

## Source Code File Structure

Note: this isn't every file and directory in the project, just the ones that I felt were relevant

.
├── Assets: the main directory where content is stored in Unity games
| ├── FramesToVideo: all videos for the game, plus images for each frame in the video
| ├── PopSignMain: what it sounds like
  | ├── Animation: files related to in game animation effects
  | ├── Audio: files related to in game audio
  | ├── Fonts: files related to the font used in the game
  | ├── Materials: Unity meta files (purpose is unclear)
  | ├── Plugins: files related to plugins we have added (e.g. Android and iOS)
  | ├── Prefabs: Unity files for prefab objects in the game
  | └── Resources: assets that are loaded in to the game programmatically (levels, word banks, icons, etc.)
    | ├── Levels: one file per level, defining the level type and what the ball structure should look like for that level
    | ├── Particles: Unity meta files (purpose is unclear)
    | ├── Prefabs: Unity files for prefab objects that are loaded programmatically in to the game
    | ├── VideoCaption: banners that caption each video if the user presses the "answer" button in game
    | ├── VideoConnection: one file per level, defines words for each level and the video location for each word
    | ├── WordBanks: word banks for the various Macarthur-Bates categories we use
    | └── WordIcons: one png per word, drawn on top of each ball in the game
  | ├── Scenes: contains the actual scene files that you can modify and run using the Unity Editor
    | ├── game: the actual gameplay scene
    | ├── map: the "home screen" scene that allows you to select a level or navigate to wordlist/settings
    | ├── opening: the "Welcome To PopSign!" scene that shows when you open the game
    | ├── practice: the scene where users practice signs before playing the game
    | ├── settings: the scene where users can adjust settings or learn how to play the game
    | ├── tutorial: shows the user how to play the game (shown before the first attempt at level 1)
    | └── wordlist: allows the user to review all the words they have learned so far in the game
  ├── Scripts: what it sounds like- all the C# scripts that control the game (plus some Python scripts I wrote)
    | ├── Bubbles: gameplay specific code lives in this directory
    | ├── Core: code that is core to the flow of the game lives in this directory
    | ├── Editor: the code that allows us to directly edit levels in Unity lives in this directory
    | ├── GUI: code that is specific to a particular UI element (e.g. answer button) goes here
    | └── Python: various scripts used to edit level data and video files (not used in the actual game)
    └── Textures: static assets (such as images) that are used to populate scenes and are not loaded programmatically
├── Library: Unity specific lib files
├── PlayerPrefsEditor: files for the plugin that allows us to edit PlayerPrefs in the Unity editor
├── ProjectSettings: settings for our Unity project
└── UnityPackageManager: contains our manifest.json (needed for Android build)
