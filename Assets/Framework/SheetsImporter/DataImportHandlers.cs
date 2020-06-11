////////////////////////////////////////////////////////////
/////   GoogleSheetsImporter.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using UnityEditor;

using static SheetsImporter.GoogleSheetsImporter;
using static UnityEngine.Debug;

namespace SheetsImporter
{
    public static class DataImportHandlers
    {
        private const string k_exampleSheetId = "1NAnYOoaBWMdGt5-jVgZ-1b-5fiauMd6_eMHQmm8fSn4";

        public static bool CreateScriptableObjects<T>(string sheetName, string assetNameFormat)
            where T : ScriptableObject
        {
            bool success = true;
            try
            {
                SpreadsheetInformation sheetInfo = new SpreadsheetInformation
                {
                    m_sheetName = sheetName,
                    m_spreadsheetId = k_exampleSheetId
                };

                GoogleSheet values = GrabSheetData(sheetInfo);
                string[] jsonArray = values.ToJsonRowArrays(GoogleSheet.HeadingAttributes.MEMBER_VARIABLE | GoogleSheet.HeadingAttributes.CAMEL_CASE);
                for (int i = 0; i < jsonArray.Length; i++)
                {
                    T scriptableExample = ScriptableObject.CreateInstance<T>();
                    JsonUtility.FromJsonOverwrite(jsonArray[i], scriptableExample);
                    AssetDatabase.CreateAsset(scriptableExample, string.Format(assetNameFormat, values.m_rows[i][0]));
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
            catch (Exception e)
            {
                LogError(e.Message);
                success = false;
            }
            return success;
        }
    }
}
