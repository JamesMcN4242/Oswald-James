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

        public enum HeadingAttributes
        {            
            MEMBER_VARIABLE = 1,
            CAMEL_CASE = 1 << 1,
            NO_SPACES = 1 << 2,
            NONE = 1 << 3
        }

        public string[] ToJsonRowArrays(HeadingAttributes headingAttributes)
        {
            string[] headings = GetAttributedHeadings(headingAttributes);
            string[] rowJson = new string[m_rows.Count];
            for(int i = 0; i < m_rows.Count; i++)
            {
                rowJson[i] = "{";
                for(int j = 0; j < headings.Length; j++)
                {
                    if (j > 0) rowJson[i] += ",";
                    rowJson[i] += $"\"{headings[j]}\":";
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

        private string[] GetAttributedHeadings(HeadingAttributes headingAttributes)
        {
            if ((headingAttributes & HeadingAttributes.NONE) > 0) return m_headings;

            string[] headings = new string[m_headings.Length];
            for(int i = 0; i < headings.Length; i++)
            {
                if((headingAttributes & HeadingAttributes.MEMBER_VARIABLE) > 0)
                {
                    headings[i] = "m_";
                }

                if ((headingAttributes & HeadingAttributes.CAMEL_CASE) > 0)
                {
                    bool afterSpace = false;
                    for(int j = 0; j < m_headings[i].Length; j++)
                    {
                        if(m_headings[i][j] == ' ')
                        {
                            afterSpace = true;
                            continue;
                        }

                        if(afterSpace)
                        {
                            headings[i] += char.ToUpper(m_headings[i][j]);
                            afterSpace = false;
                        }
                        else
                        {
                            headings[i] += char.ToLower(m_headings[i][j]);
                        }
                    }
                }
                else
                {
                    headings[i] += m_headings[i];
                    if((headingAttributes & HeadingAttributes.NO_SPACES) > 0)
                    {
                        headings[i].Replace(" ", string.Empty);
                    }
                }
            }
            return headings;
        }
    }
}
