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
