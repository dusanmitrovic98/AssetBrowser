using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AssetBrowserWindow : EditorWindow
{
    private string searchQuery = "";
    private bool showPreview = true;
    private Vector2 scrollPosition;
    private int assetsPerPage = 50;
    private int currentPage = 0;
    private List<string> filteredAssetPaths = new List<string>();

    [MenuItem("Window/Asset Browser")]
    public static void OpenWindow()
    {
        AssetBrowserWindow window = GetWindow<AssetBrowserWindow>();
        window.titleContent = new GUIContent("Asset Browser");
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Search Assets", EditorStyles.boldLabel);
        searchQuery = EditorGUILayout.TextField("Search:", searchQuery);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
        showPreview = EditorGUILayout.Toggle("Show Preview", showPreview);

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Results", EditorStyles.boldLabel);

        // Update filtered asset paths if search query has changed
        if (GUI.changed)
        {
            currentPage = 0;
            filteredAssetPaths.Clear();
            filteredAssetPaths.AddRange(AssetDatabase.FindAssets(searchQuery));
        }

        // Calculate pagination range
        int startIndex = currentPage * assetsPerPage;
        int endIndex = Mathf.Min(startIndex + assetsPerPage, filteredAssetPaths.Count);

        // Display assets in the current page
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        for (int i = startIndex; i < endIndex; i++)
        {
            string guid = filteredAssetPaths[i];
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            string assetName = Path.GetFileNameWithoutExtension(assetPath);

            EditorGUILayout.BeginHorizontal();

            // Display asset preview
            if (showPreview)
            {
                Texture2D previewTexture = AssetDatabase.GetCachedIcon(assetPath) as Texture2D;
                EditorGUILayout.ObjectField(previewTexture, typeof(Texture2D), false, GUILayout.Width(64), GUILayout.Height(64));
            }

            // Display asset name and import button
            EditorGUILayout.LabelField(assetName, GUILayout.Width(200));
            
            if (GUILayout.Button("Import", GUILayout.Width(80)))
            {
                // Import the asset into the scene or project
                Object asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
                
                if (asset != null)
                {
                    if (Selection.activeObject is GameObject)
                    {
                        GameObject parent = Selection.activeObject as GameObject;
                        GameObject.Instantiate(asset, parent.transform);
                    }
                    else
                    {
                        AssetDatabase.CreateAsset(asset, "Assets/NewAsset.asset");
                    }
                }
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        
        // Display pagination buttons
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = currentPage > 0;
        
        if (GUILayout.Button("Previous"))
        {
            currentPage--;
            scrollPosition = Vector2.zero;
        }
        
        GUI.enabled = endIndex < filteredAssetPaths.Count;
        
        if (GUILayout.Button("Next"))
        {
            currentPage++;
            scrollPosition = Vector2.zero;
        }
        
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
    }
}
       
