using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace FileDownload
{
    public class NovelConfigDictionaryValue
    {
        public string NovelHost = string.Empty;
        public string NovelName = string.Empty;
        public string NovelUrl = string.Empty;
        public string NovelMaxPages = string.Empty;
        public string NovelSaveTo = string.Empty;
        public string NovelUpdateMode = string.Empty;
    }

    public class NovelConfigDictionary : Dictionary<string, NovelConfigDictionaryValue> 
    {
        
    }
}
