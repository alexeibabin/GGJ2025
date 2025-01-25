using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class BatchSpriteSlicer : EditorWindow
{
    private Texture2D referenceSprite;
    private List<Texture2D> targetSprites = new List<Texture2D>();
    private Vector2 scrollPosition;

    [MenuItem("Tools/Batch Sprite Slicer")]
    public static void ShowWindow()
    {
        GetWindow<BatchSpriteSlicer>("Batch Sprite Slicer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Sprite Slicer", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Reference sprite selection
        EditorGUILayout.LabelField("Reference Sprite", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Select the sprite that has the slicing you want to copy.", MessageType.Info);
        
        referenceSprite = (Texture2D)EditorGUILayout.ObjectField(
            "Reference Sprite", 
            referenceSprite, 
            typeof(Texture2D), 
            false
        );

        EditorGUILayout.Space();

        // Target sprites selection
        EditorGUILayout.LabelField("Target Sprites", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Select the sprites you want to apply the slicing to.", MessageType.Info);

        if (GUILayout.Button("Get Selected Sprites as Targets"))
        {
            GetSelectedSprites();
        }

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        
        // Display target sprites
        for (int i = 0; i < targetSprites.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            
            targetSprites[i] = (Texture2D)EditorGUILayout.ObjectField(
                $"Target Sprite {i + 1}", 
                targetSprites[i], 
                typeof(Texture2D), 
                false
            );

            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                targetSprites.RemoveAt(i);
                i--;
            }
            
            EditorGUILayout.EndHorizontal();
        }
        
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        // Apply button
        GUI.enabled = referenceSprite != null && targetSprites.Count > 0;
        if (GUILayout.Button("Apply Slicing"))
        {
            ApplySlicing();
        }
        GUI.enabled = true;
    }

    private void GetSelectedSprites()
    {
        Object[] selection = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);
        
        foreach (Object obj in selection)
        {
            if (obj is Texture2D texture)
            {
                string path = AssetDatabase.GetAssetPath(texture);
                TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                
                if (importer != null && importer.textureType == TextureImporterType.Sprite)
                {
                    if (texture != referenceSprite && !targetSprites.Contains(texture))
                    {
                        targetSprites.Add(texture);
                    }
                }
            }
        }
    }

    private void ApplySlicing()
    {
        if (referenceSprite == null || targetSprites.Count == 0)
            return;

        // Get reference sprite settings
        string referencePath = AssetDatabase.GetAssetPath(referenceSprite);
        TextureImporter referenceImporter = AssetImporter.GetAtPath(referencePath) as TextureImporter;

        if (referenceImporter == null || referenceImporter.spriteImportMode != SpriteImportMode.Multiple)
        {
            EditorUtility.DisplayDialog("Error", "Reference sprite must be in Multiple sprite mode!", "OK");
            return;
        }

        // Store reference sprite settings
        SpriteMetaData[] referenceSpritesheetData = referenceImporter.spritesheet;
        TextureImporterSettings referenceSettings = new TextureImporterSettings();
        referenceImporter.ReadTextureSettings(referenceSettings);

        // Apply to all target sprites
        foreach (Texture2D targetTexture in targetSprites)
        {
            string targetPath = AssetDatabase.GetAssetPath(targetTexture);
            TextureImporter targetImporter = AssetImporter.GetAtPath(targetPath) as TextureImporter;

            if (targetImporter == null)
                continue;

            // Force Multiple sprite mode and settings
            targetImporter.spriteImportMode = SpriteImportMode.Multiple;
            
            TextureImporterSettings targetSettings = new TextureImporterSettings();
            targetImporter.ReadTextureSettings(targetSettings);
            referenceSettings.CopyTo(targetSettings);
            targetImporter.SetTextureSettings(targetSettings);

            // Create new sprite sheet data
            SpriteMetaData[] targetSpritesheetData = new SpriteMetaData[referenceSpritesheetData.Length];
            for (int j = 0; j < referenceSpritesheetData.Length; j++)
            {
                targetSpritesheetData[j] = new SpriteMetaData
                {
                    name = $"{Path.GetFileNameWithoutExtension(targetPath)}_{j + 1}",
                    alignment = referenceSpritesheetData[j].alignment,
                    border = referenceSpritesheetData[j].border,
                    pivot = referenceSpritesheetData[j].pivot,
                    rect = referenceSpritesheetData[j].rect
                };
            }
            targetImporter.spritesheet = targetSpritesheetData;

            EditorUtility.SetDirty(targetImporter);
            targetImporter.SaveAndReimport();
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Batch Sprite Slicer", "Slicing completed successfully!", "OK");
    }
} 