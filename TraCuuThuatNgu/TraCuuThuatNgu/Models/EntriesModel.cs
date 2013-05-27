using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using TraCuuThuatNgu.ViewModels;
using System.Data.SqlClient;
using System.Data;
using System.Data.OleDb;
using EntityFramework.Extensions;

namespace TraCuuThuatNgu.Models
{
    public class EntriesModel : CommonModel
    {
        // Get all terms paged x size
        public IPagedList<WordIndex> GetEntriesPaged(int page, int size)
        {
            return context.WordIndexes.OrderBy(x => x.HeadWord).ToPagedList(page, size);
        }

        // Get terms by startWith paged x size
        public IPagedList<WordIndex> GetEntriesByStartWithPaged(int page, int size, string startWith)
        {
            return context.WordIndexes.Where(x => x.HeadWord.StartsWith(startWith)).OrderBy(x => x.HeadWord).ToPagedList(page, size);
        }

        // Get terms contains keyword paged x size
        public IPagedList<WordIndex> GetEntriesContainsPaged(int page, int size, string keyword)
        {
            return context.WordIndexes.Where(x => x.HeadWord.Contains(keyword)).OrderBy(x => x.HeadWord).ToPagedList(page, size);
        }



        // Add new term or new synset
        public int AddNewTermOrSynset(AddTermViewModel term)
        {

            // Check isExist term
            WordIndex checkTerm = context.WordIndexes.Find(term.HeadWord);

            if (checkTerm == null)
            {
                // Entry
                checkTerm = new WordIndex();
                checkTerm.HeadWord = term.HeadWord;
                string[] word = term.HeadWord.Split(' ');

                if (word.Count() > 1)
                {
                    checkTerm.WordType = "c";
                }
                else
                {
                    checkTerm.WordType = "s";
                }
            }
            else
            {
                // Check duplicate content
                //var checkDup = checkTerm.Synsets.Contains(context.Synsets.Where(x => x.Def.Contains(term.Def)).FirstOrDefault());
                bool checkDup2 = false;
                foreach (var item in checkTerm.Synsets)
                {
                    if (item.Def.Equals(term.Def))
                    {
                        checkDup2 = true;
                        break;
                    }
                }
                if (checkDup2)
                    return 0;

            }

            // Synset
            Synset synset = new Synset();
            synset.Category = term.Catagory;
            synset.Def = term.Def;
            synset.Exa = term.Exa;

            // If synset has synonym(s)
            if (!String.IsNullOrWhiteSpace(term.Synonyms))
            {
                string[] synonyms = term.Synonyms.Split(',');

                foreach (string headWord in synonyms)
                {
                    // Check headWord exist in database
                    WordIndex wordIndex = context.WordIndexes.Find(headWord);
                    if (wordIndex != null)
                    {
                        //synset.WordIndexes.Add(wordIndex);
                    }
                    else
                    {
                        //entry
                        wordIndex = new WordIndex();
                        wordIndex.HeadWord = headWord;
                        string[] words = headWord.Split(' ');

                        if (words.Count() > 1)
                        {
                            wordIndex.WordType = "c";
                        }
                        else
                        {
                            wordIndex.WordType = "s";
                        }
                    }
                    synset.WordIndexes.Add(wordIndex);
                }
            }

            // 
            synset.WordIndexes.Add(checkTerm);
            context.Synsets.Add(synset);
            int result = context.SaveChanges();
            return result;
        }

        // Delete synset by synsetId
        public int DeleteSynsetBySynsetId(int synsetId, string headWord)
        {
            Synset synset = context.Synsets.Find(synsetId);

            WordIndex term = context.WordIndexes.Find(headWord);

            term.Synsets.Remove(synset);
            context.Synsets.Remove(synset);

            return context.SaveChanges();
        }


        // Edit synset by synsetId
        public int EditSynsetBySynsetId(AddTermViewModel editedSynset, int synsetId)
        {
            Synset synset = context.Synsets.Find(synsetId);
            synset.Category = editedSynset.Catagory;
            synset.Def = editedSynset.Def;
            synset.Exa = editedSynset.Exa;

            //delete all synonym relation 
            synset.WordIndexes.Clear();

            synset.WordIndexes.Add(context.WordIndexes.Find(editedSynset.HeadWord));

            // If synset has synonym(s)
            if (!String.IsNullOrWhiteSpace(editedSynset.Synonyms))
            {
                string[] synonyms = editedSynset.Synonyms.Split(',');

                foreach (string headWord in synonyms)
                {
                    // Check headWord exist in database
                    WordIndex wordIndex = context.WordIndexes.Find(headWord);
                    if (wordIndex != null)
                    {
                        //synset.WordIndexes.Add(wordIndex);
                    }
                    else
                    {
                        //entry
                        wordIndex = new WordIndex();
                        wordIndex.HeadWord = headWord;
                        string[] word = headWord.Split(' ');

                        if (word.Count() > 1)
                        {
                            wordIndex.WordType = "c";
                        }
                        else
                        {
                            wordIndex.WordType = "s";
                        }
                    }
                    synset.WordIndexes.Add(wordIndex);
                }
            }

            context.Entry(synset).State = EntityState.Modified;
            return context.SaveChanges();
        }


        // View edit synset
        public AddTermViewModel ViewEditSynset(int synsetId, string headWord)
        {
            AddTermViewModel editSynset = new AddTermViewModel();

            Synset synset;
            if (synsetId == -1)
            {
                synset = context.WordIndexes.Find(headWord).Synsets.FirstOrDefault();
            }
            else
            {
                synset = context.Synsets.Find(synsetId);
            }


            editSynset.Catagory = synset.Category;
            editSynset.Def = synset.Def;
            editSynset.Exa = synset.Exa;
            editSynset.HeadWord = headWord;

            return editSynset;
        }

        //get synonyms
        public IEnumerable<string> GetSynonyms(int synsetId, string headWord)
        {
            if (synsetId == -1)
                return context.WordIndexes.Find(headWord)
                    .Synsets
                    .FirstOrDefault()
                    .WordIndexes
                    .Where(x => x.HeadWord != headWord)
                    .Select(x => x.HeadWord)
                    .ToList();

            return context.Synsets.Find(synsetId)
                .WordIndexes.Where(x => x.HeadWord != headWord)
                .Select(x => x.HeadWord).ToList();
        }


        // Get another synset of term
        public IEnumerable<Synset> GetAnotherSynsetOfTerm(string headWord, int synsetId)
        {
            if (synsetId == -1)
                synsetId = context.WordIndexes.Find(headWord).Synsets.FirstOrDefault().SynsetId;

            var list = context.WordIndexes.Find(headWord).Synsets
                .Where(x => x.SynsetId != synsetId).ToList();
            return list;
        }


        // Suggest freetext
        public IEnumerable<WordIndex> SuggestTerm(string keyword)
        {
            return context.Database.SqlQuery<WordIndex>(
                "SELECT * FROM fts_termSearch(@param)", new SqlParameter("param", keyword));
        }


        // --------------------------------------------------------
        // Read Excel file
        public DataTable ReadExcelContents(string fileName)
        {
            try
            {
                OleDbConnection connection = new OleDbConnection();

                connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Extended Properties=Excel 8.0;Data Source=" + fileName); //Excel 97-2003, .xls //string excelQuery = @"Select [Day],[Outlook],[temp],[Humidity],[Wind], [PlaySport] // FROM [Sheet1$]";

                string excelQuery = @"Select [Thuật ngữ],[Loại từ],[Giải thích],[Ví dụ],[Từ đồng nghĩa] FROM [Sheet1$]";
                connection.Open();
                OleDbCommand cmd = new OleDbCommand(excelQuery, connection);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = cmd;
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                DataTable dt = ds.Tables[0];

                // dataGridView1.DataSource = dt.DefaultView;
                connection.Close();
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }



        // Import data
        public int ImportData(DataTable source)
        {
            int result = 0;
            if (source == null)
            {
                //file sai cau truc
                return -1;
            }
            else
            {
                for (int i = 0; i < source.Rows.Count; i++)
                {
                    AddTermViewModel termOrSynset = new AddTermViewModel();

                    var term = source.Rows[i]["Thuật ngữ"].ToString();
                    var def = source.Rows[i]["Giải thích"].ToString();

                    if (!String.IsNullOrWhiteSpace(term) && !String.IsNullOrWhiteSpace(def))
                    {
                        termOrSynset.HeadWord = term;
                        termOrSynset.Catagory = source.Rows[i]["Loại từ"].ToString();
                        termOrSynset.Def = def;
                        termOrSynset.Exa = source.Rows[i]["Ví dụ"].ToString();
                        termOrSynset.Synonyms = source.Rows[i]["Từ đồng nghĩa"].ToString();
                        if (AddNewTermOrSynset(termOrSynset) > 0)
                        { result++; }
                    }
                }

            }

            return result;

        }


        // Delete synset of term
        public int DeleteSynsetOfTerm(string headWord, int synsetId)
        {
            // Check term isExist
            WordIndex index = context.WordIndexes.Find(headWord);
            Synset synset = context.Synsets.Find(synsetId);
            if (index != null && synset != null)
            {
                int countSynsets = index.Synsets.Count;
                index.Synsets.Remove(synset);
                if (countSynsets <= 1)
                {
                    context.WordIndexes.Remove(index);
                }
                if (synset.WordIndexes.Count == 0)
                {
                    context.Synsets.Remove(synset);
                }
            }
            else
            {
                return -1;
            }


            // Remove PK
            context.Comments.Delete(x => x.HeadWord == headWord);
            context.Favorites.Delete(x => x.HeadWord == headWord);

            // Save changes
            return context.SaveChanges();
        }

    }
}