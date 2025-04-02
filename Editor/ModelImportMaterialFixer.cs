using UnityEditor;
using UnityEngine;

public class ModelImportMaterialFixer : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        if (!assetPath.StartsWith("Assets/")) return; // ✅ Only process mutable assets

        ModelImporter modelImporter = (ModelImporter)assetImporter;

        // Set material import mode to External (Legacy-compatible)
        modelImporter.materialImportMode = ModelImporterMaterialImportMode.ImportStandard;
        modelImporter.materialLocation = ModelImporterMaterialLocation.External;
        modelImporter.materialName = ModelImporterMaterialName.BasedOnMaterialName;
        modelImporter.materialSearch = ModelImporterMaterialSearch.Everywhere;
    }

    void OnPostprocessModel(GameObject g)
    {
        if (!assetPath.StartsWith("Assets/")) return;

        Debug.Log($"✔ Auto-material setup applied to model: {assetPath}");
    }
}