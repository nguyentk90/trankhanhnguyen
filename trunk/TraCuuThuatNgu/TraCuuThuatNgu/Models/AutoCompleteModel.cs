using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace TraCuuThuatNgu.Models
{
    public class AutoCompleteModel : CommonModel
    {
        //get suggest words by prefix
        public List<string> GetByPrefix(string prefix)
        {
            // List suggest words
            List<string> listSuggest = new List<string>();

            //Get suggest from Search History Order by Counter DESC
            var suggestWordsFromSearchHistories = AutoCompelte_SearchHistory(prefix);


            // Count words in suggest Search History
            int count = suggestWordsFromSearchHistories.Count();


            // Check count
            if (count >= 10)
            {
                return suggestWordsFromSearchHistories.ToList();
            }
            else
            {
                // Get suggest from Entries except from Search History list
                var suggestWordsFromEntries = AutoCompelte_(prefix, 10 - count);

                // Add to list
                listSuggest.AddRange(suggestWordsFromSearchHistories);
                listSuggest.AddRange(suggestWordsFromEntries);

                return listSuggest.Distinct().ToList();
            }
        }

        // Suggest [fts_AutoCompelte_SearchHistory]
        public IEnumerable<string> AutoCompelte_SearchHistory(string keyword)
        {          
            var result = context.Database.SqlQuery<AutoComplete>(
              "SELECT * FROM fts_AutoCompelte_SearchHistory({0})", keyword).Select(x => x.Keyword);

            return result;
        }


        // Suggest [fts_AutoCompelte_SearchHistory]
        public IEnumerable<string> AutoCompelte_(string keyword, int top)
        {            
            var result = context.Database.SqlQuery<AutoComplete>(
                "SELECT * FROM fts_AutoCompelte({0},{1})", keyword, top).Select(x => x.Keyword);

            return result;
        }
    }
}