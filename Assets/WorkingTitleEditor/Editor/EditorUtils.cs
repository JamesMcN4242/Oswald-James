using System.IO;
using UnityEditor;

using static SheetsImporter.DataImportHandlers;

public static class EditorUtils
{
#region Editor Toolbar

    [MenuItem("Data/All Data")]
    private static void PullAllData()
    {
        bool success = PullCharacterData();
        success &= PullEquipableData();
        success &= PullTeamData();
        SuccessMessage(success);
    }

    [MenuItem("Data/Character")]
    public static void PullCharacterDataEditor()
    {
        bool succeeded = PullCharacterData();
        SuccessMessage(succeeded);
    }

    [MenuItem("Data/Equipable")]
    public static void PullEquipableDataEditor()
    {
        bool succeeded = PullEquipableData();
        SuccessMessage(succeeded);
    }

    [MenuItem("Data/Team Data")]
    public static void PullTeamDataEditor()
    {
        bool succeeded = PullTeamData();
        SuccessMessage(succeeded);
    }

#endregion

    private static bool PullCharacterData()
    {
        return CreateScriptableObjects<CharacterData>("CharacterData", Path.Combine("Assets", "WorkingTitle", "Resources", "CharacterData", "{0}.asset"));
    }
    
    private static bool PullEquipableData()
    {
        return CreateScriptableObjects<EquipableData>("EquipableData", Path.Combine("Assets", "WorkingTitle", "Resources", "EquipableData", "{0}.asset"));
    }

    private static bool PullTeamData()
    {
        return CreateScriptableObjects<TeamSettings>("TeamSettings", Path.Combine("Assets", "WorkingTitle", "Resources", "Settings", "{0}.asset"));
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
