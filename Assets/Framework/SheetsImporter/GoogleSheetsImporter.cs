////////////////////////////////////////////////////////////
/////   GoogleSheetsImporter.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace SheetsImporter
{
    public struct SpreadsheetInformation
    {
        public string m_spreadsheetId;
        public string m_sheetName;
    }

    public static class GoogleSheetsImporter
    {
        private static string[] Scopes = { SheetsService.Scope.SpreadsheetsReadonly };
        private static readonly string k_credentialPath = Path.Combine("Assets", "Framework", "SheetsImporter", "Credentials");
        private const char k_ignoreRowOrColumnCharacter = '!';

        public static GoogleSheet GrabSheetData(SpreadsheetInformation sheetInfo, int headingRow = 0)
        {
            IList<IList<object>> values = GrabRawSheetData(sheetInfo);
            return GetFilteredData(values, headingRow);
        }

        public static IList<IList<object>> GrabRawSheetData(SpreadsheetInformation sheetInfo)
        {
            UserCredential credential = GetUserCredentials();

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Application.productName,
            });
            
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(sheetInfo.m_spreadsheetId, sheetInfo.m_sheetName);
            
            ValueRange response = request.Execute();
            return response.Values;
        }

        private static UserCredential GetUserCredentials()
        {
            using (var stream =
                new FileStream(Path.Combine(k_credentialPath, "credentials.json"), FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = Path.Combine(k_credentialPath, "token.json");
                return GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }
        }

        private static GoogleSheet GetFilteredData(IList<IList<object>> values, int headingRow)
        {
            //Cull data above the heading row
            for (int row = 0; row < headingRow; row++)
            {
                values.RemoveAt(row);
            }

            //Ignore any rows that are flagged to bypass or are empty
            for(int row = values.Count - 1; row > 0; row--)
            {
                if(values[row].Count == 0 || values[row][0].ToString().StartsWith(k_ignoreRowOrColumnCharacter.ToString()))
                {
                    values.RemoveAt(row);
                }
            }

            //Remove any headings that are flagged to be ignored
            for(int column = values[0].Count - 1; column >= 0; column--)
            {
                if(values[0][column].ToString().StartsWith(k_ignoreRowOrColumnCharacter.ToString()))
                {
                    for(int row = 0; row < values.Count; row++)
                    {
                        if(values[row].Count > column)
                        {
                            values[row].RemoveAt(column);
                        }
                    }
                }
            }

            string[] headings = new string[values[0].Count];
            for(int i = 0; i < headings.Length; i++)
            {
                headings[i] = values[0][i].ToString();
            }
            values.RemoveAt(0);

            return new GoogleSheet { m_headings = headings, m_rows = values };
        }
    }
}