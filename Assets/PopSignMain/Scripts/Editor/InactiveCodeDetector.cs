/*
This tool iterates through all the files in your project and checks for scripts.
It then goes through all the GameObjects in all the scenes in the project, and 
checks for scripts that are not present on any GameObjects.
Note that this does not mean that the script is not used, just that it is possible that it isn't. 
The script could still be used in many other ways. This tool is just to narrow the search for 
unused code.
The script is tested on MacOS, but should work just as fine on Windows. 
////// DISCLAIMER /////
Permission is hereby granted, free of charge, to any person obtaining a copy 
of this software and associated documentation files (the "Software"), to deal 
in the Software without restriction, including without limitation the rights 
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
of the Software, and to permit persons to whom the Software is furnished to do so, 
subject to the following conditions: The above copyright notice and this permission 
notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
(MIT License)
*/


#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Linq;

public class InactiveCodeDetector : EditorWindow
{
    private bool isDone = false;
    private List<string> paths = new List<string>();
    private List<Scene> allScenes = new List<Scene>();

    private Scene currentlyOpenScene;

    Vector2 scrollPosition;


    private List<string> allUsedComponents = new List<string>();

    private List<String> unusedAssets = new List<string>();

    [MenuItem("Tools/Clean up scripts")]
    public static void ShowWindow(){
        EditorWindow.GetWindow<InactiveCodeDetector>("Clean up scripts");
    }

    private void OnGUI()
    {
        GUILayout.Label("Find possibly unused scripts.");
        if(GUILayout.Button("Search")){
            isDone = false;
            unusedAssets.Clear();
            currentlyOpenScene = EditorSceneManager.GetActiveScene();  
            FindAll();
            CheckAllScenes();
            Compare();
            CloseScenes();
            CleanUp();
            isDone = true;
        }

        if(isDone){
            PromtUser();
        }

    }

    private void PromtUser(){

        string content = "The following scripts are not found on any GameObject in this project.";
        string warning = "NOTE - This does NOT mean it is safe to delete. The script could still be accessed via code or on a prefab";

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        GUILayout.Label(content);
        GUILayout.Label(warning, EditorStyles.boldLabel);
        foreach(string s in unusedAssets){
            GUILayout.Label(s);
            if(GUILayout.Button("Show in finder/explorer")){
                EditorUtility.RevealInFinder(s);
            }
            GUILayout.Space(10);
        }
        EditorGUILayout.EndScrollView();
    }

    private void CleanUp(){
        paths.Clear();
        allScenes.Clear();
        allUsedComponents.Clear();
        
    }

    private void Compare(){
        foreach(string filename in paths){
            string formated = filename.Split('/').Last();
            if (formated != "InactiveCodeDetector.cs")
            {
                string isUsed = (from string s in allUsedComponents where s.ToLower() == formated.ToLower() select s).FirstOrDefault();

                if (isUsed == null)
                {
                    unusedAssets.Add(filename);
                    //Debug.Log("Script " + filename + " is not attached to any game object in this project");
                }
            }
        }
    }

    private void CheckAllScenes(){
        Scene[] scenes = new Scene[EditorSceneManager.sceneCount];
        for (int i = 0; i < scenes.Length; i++)
        {
            scenes[i] = EditorSceneManager.GetSceneAt(i);


            GameObject[] rootObjectsInScene = scenes[i].GetRootGameObjects();
            foreach (GameObject g in rootObjectsInScene)
            {
                foreach(Component c in g.GetComponents<Component>()){
                    string name = ObjectNames.GetInspectorTitle(c);
                    if (name.EndsWith("(Script)"))
                    {
                        string formated = name.Replace("(Script)", String.Empty).Replace(" ", String.Empty) + ".cs";
                        allUsedComponents.Add(formated.ToLower());
                    }

                }
            }
        }
    }

    private void CloseScenes(){

        foreach(Scene s in allScenes){
            if(s != currentlyOpenScene){
                EditorSceneManager.CloseScene(s, true);
            }
        }

    }

    private void FindAll(){
        string p = Application.dataPath;
        ProcessDirectory(p);
    }

    private void ProcessDirectory(string path){
        string[] fileEntries = Directory.GetFiles(path);
        foreach(string filename in fileEntries){
            if (filename.EndsWith(".cs"))
            {
                paths.Add(filename);
            }
            if(filename.EndsWith(".unity")){

                Scene scene = EditorSceneManager.OpenScene(filename, OpenSceneMode.Additive);
                allScenes.Add(scene);
            }
        }
        string[] subdirectories = Directory.GetDirectories(path);
        foreach(string sub in subdirectories){
            ProcessDirectory(sub);
        }
    }


}


#endif