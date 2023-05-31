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
