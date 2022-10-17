using System;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;

[Serializable]
public class PrefabImporterTool : EditorWindow
{
    const string toolName = "PrefabImporter";

    ////////////////////////////////////////////
    //////////// VARIABLES GENERALES ///////////
    ////////////////////////////////////////////

    [MenuItem("Tools/Prefab Importer")]
    public static PrefabImporterTool OpenPrefabImporter() => GetWindow<PrefabImporterTool>("Prefab Importer");
    public static PrefabImporterTool Instance;

    ////////////////////////////////////////////
    //////// VARIABLES POUR L'UTILISATEUR //////
    ////////////////////////////////////////////

    [SerializeField, Tooltip("Prefab to import")]
    private GameObject prefabImport;
    [SerializeField, Tooltip("Prefab reference")]
    private GameObject prefabReference;
    [SerializeField, Tooltip("Folder where to save prefabs")]
    private string savePrefabsFolder = "Prefabs";
    [SerializeField, Tooltip("Should Renderers be used")]
    private bool useRenderer = true;
    [SerializeField, Tooltip("Should Colliders be used")]
    private bool useCollider = true;
    [SerializeField, Tooltip("Should the prefab be resized")]
    private bool useResize = false;

    ////////////////////////////////////////////
    ////////// UTILS POUR L'UTILISATEUR ////////
    ////////////////////////////////////////////

    private Vector2 scrollPosition;

    ////////////////////////////////////////////
    //////////// VARIABLES SERIALISE ///////////
    ////////////////////////////////////////////

    SerializedObject so;
    SerializedProperty propPrefabImport;
    SerializedProperty propPrefabReference;
    SerializedProperty propSavePrefabsFolder;
    SerializedProperty propUseRenderer;
    SerializedProperty propUseCollider;
    SerializedProperty propUseResize;

    ////////////////////////////////////////////
    //////////////// ON ENABLE /////////////////
    ////////////////////////////////////////////

    void OnEnable()
    {
        Instance = this;
        so = new SerializedObject(this);
        propPrefabImport = so.FindProperty("prefabImport");
        propPrefabReference = so.FindProperty("prefabReference");
        propSavePrefabsFolder = so.FindProperty("savePrefabsFolder");
        propUseRenderer = so.FindProperty("useRenderer");
        propUseCollider = so.FindProperty("useCollider");
        propUseResize = so.FindProperty("useResize");
    }

    ////////////////////////////////////////////
    ///////////////// ON GUI ///////////////////
    ////////////////////////////////////////////

    void OnGUI()
    {
        so.Update();
        GUILayoutOption maxWidth = GUILayout.MaxWidth(500f);
        using (var scrollView = new EditorGUILayout.ScrollViewScope(scrollPosition)) {
            scrollPosition = scrollView.scrollPosition;
            using (new EditorGUILayout.VerticalScope()) {
                GUILayout.Space(2);
                
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox)) {
                        EditorGUIUtility.labelWidth = 80f;
                        EditorGUILayout.PropertyField(propPrefabImport, GUILayout.MinWidth(100f), GUILayout.MaxWidth(250f));
                        EditorGUIUtility.labelWidth = 100f;
                        EditorGUILayout.PropertyField(propPrefabReference, GUILayout.MinWidth(100f), GUILayout.MaxWidth(250f));
                    }
                    GUILayout.FlexibleSpace();
                }
                
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox, maxWidth)) {
                        using (new EditorGUILayout.HorizontalScope()) {
                            GUILayout.FlexibleSpace();
                            EditorGUIUtility.labelWidth = 120f;
                            EditorGUILayout.PropertyField(propSavePrefabsFolder, GUILayout.MinWidth(100f), maxWidth);
                            GUILayout.FlexibleSpace();
                        }

                        using (new EditorGUILayout.HorizontalScope()) {
                            GUILayout.FlexibleSpace();
                            EditorGUIUtility.labelWidth = 85f;
                            EditorGUILayout.PropertyField(propUseRenderer);
                            GUILayout.Space(10f);
                            EditorGUIUtility.labelWidth = 75f;
                            EditorGUILayout.PropertyField(propUseCollider);
                            GUILayout.Space(10f);
                            EditorGUIUtility.labelWidth = 70f;
                            EditorGUILayout.PropertyField(propUseResize);
                            GUILayout.FlexibleSpace();
                        }
                    }
                    GUILayout.FlexibleSpace();
                }
                using (new EditorGUILayout.HorizontalScope()) {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Import Prefab", GUILayout.MinWidth(280f))) ImportPrefab();
                    GUILayout.FlexibleSpace();
                }
            }
        }
        so.ApplyModifiedProperties();

        if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
            GUI.FocusControl(null);
            Repaint();
        }
    }

    ////////////////////////////////////////////
    //////////// IMPORT PREFAB /////////////////
    ////////////////////////////////////////////
    
    private void ImportPrefab()
    {
        if (prefabImport == null) {
            Debug.LogError($"[{toolName}] PrefabImport must not be null");
            return;
        }
        if (!useRenderer && !useCollider) {
            Debug.LogError($"[{toolName}] UseRenderer or UseCollider must be enabled");
            return;
        }
        if (useResize && prefabReference == null) {
            Debug.LogError($"[{toolName}] PrefabReference must not be null if using Resize");
            return;
        }

        GameObject go = Instantiate(prefabImport, Vector3.zero, Quaternion.identity);
        GameObject parent = new GameObject(go.name);
        Bounds bounds = GetBounds(go);

        if (useResize) {
            float scale = GetReferenceResize(bounds, prefabReference);
            go.transform.localScale = new Vector3(scale, scale, scale);
        }

        go.transform.position = -bounds.center;
        go.transform.SetParent(parent.transform);
        go.name = "Graphics";

        string folder = $"Assets/{Regex.Replace(savePrefabsFolder, @"\s+/\s+", "/")}";
        if (!AssetDatabase.IsValidFolder(folder)) {
            Debug.LogError($"[{toolName}] SavePrefabFolder is not Valid");
            return;
        }

        string path = $"{folder}/{parent.name}.prefab";
        path = AssetDatabase.GenerateUniqueAssetPath(path);

        PrefabUtility.SaveAsPrefabAsset(parent, path, out bool success);
        if (success) Debug.Log($"[{toolName}] Prefab Imported");
        else Debug.LogError($"[{toolName}] Failed To Import");
        DestroyImmediate(parent);
    }

    private float GetReferenceResize(Bounds bounds, GameObject reference)
    {
        Vector3 size = bounds.size;
        float factor = (size.x + size.y + size.z) / 3;
        GameObject referenceGo = Instantiate(reference, Vector3.zero, Quaternion.identity);
        Vector3 referenceSize = GetBounds(referenceGo).size;
        float referenceFactor = (referenceSize.x + referenceSize.y + referenceSize.z) / 3;
        DestroyImmediate(referenceGo);
        return referenceFactor / factor;
    }

    private Bounds GetBounds(GameObject go)
    {
        Bounds bounds = new Bounds();
        if (useRenderer) {
            foreach (Renderer renderer in go.GetComponentsInChildren<Renderer>()) {
                bounds.Encapsulate(renderer.bounds);
            }
        }
        if (useCollider) {
            foreach (Collider collider in go.GetComponentsInChildren<Collider>()) {
                bounds.Encapsulate(collider.bounds);
            }
        }
        return bounds;
    }
}