using System.IO;
using UnityEditor;

using static SheetsImporter.DataImportHandlers;

public static class EditorUtils
{
    [MenuItem("Data/Character")]
    public static void PullCharacterData()
    {
        bool succeeded = CreateScriptableObjects<CharacterData>("CharacterData", Path.Combine("Assets", "WorkingTitle", "Resources", "CharacterData", "{0}.asset"));
        SuccessMessage(succeeded);
    }

    [MenuItem("Data/Equipable")]
    public static void PullEquipableData()
    {
        bool succeeded = CreateScriptableObjects<EquipableData>("EquipableData", Path.Combine("Assets", "WorkingTitle", "Resources", "EquipableData", "{0}.asset"));
        SuccessMessage(succeeded);
    }

    public static void SuccessMessage(bool succeeded)
    {
        if (succeeded)
        {
            bool message = EditorUtility.DisplayDialog("Data pull success", "Successfully pulled data", "OK");
        }
        else
        {
            bool message = EditorUtility.DisplayDialog("Data pull failure", "Failed to pull data", "OK");
        }
    }
}
