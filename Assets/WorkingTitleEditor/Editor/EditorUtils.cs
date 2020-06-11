using System.IO;
using UnityEditor;

using static SheetsImporter.DataImportHandlers;

public static class EditorUtils
{
    [MenuItem("Data/Character")]
    public static void PullCharacterData()
    {
        bool succeeded = CreateScriptableObjects<CharacterData>("CharacterData", Path.Combine("Assets", "WorkingTitle", "Resources", "CharacterData", "{0}.asset"));
    }
}
