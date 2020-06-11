////////////////////////////////////////////////////////////
/////   GoogleSheet.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;

namespace SheetsImporter
{
    public struct GoogleSheet
    {
        public string[] m_headings;
        public IList<IList<object>> m_rows;

        public string[] ToJsonRowArrays()
        {
            string[] rowJson = new string[m_rows.Count];
            for(int i = 0; i < m_rows.Count; i++)
            {
                rowJson[i] = "{";
                for(int j = 0; j < m_headings.Length; j++)
                {
                    if (j > 0) rowJson[i] += ",";
                    rowJson[i] += $"\"{m_headings[j]}\":";
                    if(float.TryParse(m_rows[i][j].ToString(), out float result) || m_rows[i][j].ToString().StartsWith("{"))
                    {
                        rowJson[i] += m_rows[i][j].ToString();
                    }
                    else
                    {
                        rowJson[i] += $"\"{m_rows[i][j]}\"";
                    }
                }
                rowJson[i] += "}";
            }
            return rowJson;
        }
    }
}
