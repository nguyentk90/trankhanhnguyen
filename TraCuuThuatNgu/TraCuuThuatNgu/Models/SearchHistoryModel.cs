using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using PagedList;

namespace TraCuuThuatNgu.Models
{
    //summary search history model
    public class SummarySearchHistoryModel : CommonModel
    {
        public SummarySearchHistoryModel()
        {
            GetSearchHistoryCountKeyword = context.SearchHistories.AsQueryable().Count();
            GetSearchHistoryCountTime = context.SearchHistories.Sum(x => x.Counter);
            GetSearchHistoryIsExist = context.SearchHistories.Where(x => x.IsExist == true).Count();
            GetSearchHistoryIsNotExist = GetSearchHistoryCountKeyword - GetSearchHistoryIsExist;
        }

        public int GetSearchHistoryCountTime { get; set; }
        public int GetSearchHistoryCountKeyword { get; set; }
        public int GetSearchHistoryIsExist { get; set; }
        public int GetSearchHistoryIsNotExist { get; set; }
    }

    //search history model
    public class SearchHistoryModel
    {

        public static int ORDERBYDATE = 0;
        public static int ORDERBYSTATUS = 1;
        public static int ORDERBYCOUNT = 2;


        TraCuuThuatNguEntities context = null;

        public SearchHistoryModel()
        {
            context = new TraCuuThuatNguEntities();
        }

        //get all history lastest paged x size
        public IPagedList<SearchHistory> GetAllSearchHistory(int page,int size,int orderby)
        {
            if (orderby == ORDERBYDATE)
            {
                return context.SearchHistories.OrderByDescending(x => x.DateModify).ToPagedList(page, size);
            }
            else if (orderby == ORDERBYSTATUS)
            {
                return context.SearchHistories.OrderBy(x => x.IsExist).ThenByDescending(x=>x.Counter).ToPagedList(page, size);
            }
            else
            {
                return context.SearchHistories.OrderByDescending(x => x.Counter).ToPagedList(page, size);
            }            
        }

        //get top % history lastest
        public IQueryable<SearchHistory> GetTopLastest(int top)
        {
            return context.SearchHistories.OrderByDescending(x => x.DateModify).Take(top);
        }


        //add searching history 
        public int AddSearchHistory(string keyword, bool isExist)
        {
            try
            {
                SearchHistory searchHistory = context.SearchHistories.Find(keyword);
                if (searchHistory == null)
                {

                    searchHistory = new SearchHistory();
                    searchHistory.Keyword = keyword;
                    searchHistory.IsExist = isExist;
                    searchHistory.Counter = 1;
                    searchHistory.DateModify = DateTime.Now;
                    //add new
                    context.SearchHistories.Add(searchHistory);
                }
                else
                {
                    searchHistory.IsExist = isExist;
                    searchHistory.Counter++;
                    searchHistory.DateModify = DateTime.Now;
                    //increment count
                    context.Entry(searchHistory).State = EntityState.Modified;
                }

                //change context
                return context.SaveChanges();
            }
            catch
            {
                return 0;
            }

        }
    }
}