using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            var suggestWordsFromSearchHistories = context.SearchHistories.Where(x => x.Keyword.Contains(prefix)&& x.IsExist==true)
                .OrderByDescending(x => x.Counter)
                .Select(x => x.Keyword).Take(8);

            IQueryable<string> suggestWordsFromEntries;

            // Count words in suggest Search History
            int count = suggestWordsFromSearchHistories.Count();
           

            // Check count
            if (count >= 8)
            {
                return suggestWordsFromSearchHistories.ToList();
            }
            else
            {
                // Get suggest from Entries
                suggestWordsFromEntries = context.WordIndexes.Where(x => x.HeadWord.Contains(prefix))
              .Select(x => x.HeadWord).Take(8 - count);

                // Add to list
                listSuggest.AddRange(suggestWordsFromSearchHistories);
                listSuggest.AddRange(suggestWordsFromEntries);

                return listSuggest.Distinct().ToList();
            }            
        }
    }
}