using MyEvernote.Business;
using MyEvernote.Entities;
using System.Collections.Generic;
using System.Web.Helpers;

namespace MyEvernote.Web.Models
{
    public class CacheHelper
    {
        public static List<Category> GetCategoriesFromCache()
        {
            var categoryList = WebCache.Get("category-cache");

            if (categoryList == null)
            {
                CategoryManager cm = new CategoryManager();
                categoryList = cm.List();
                WebCache.Set("category-cache", categoryList, 20, true);
            }
            return categoryList;
        }

        public static void RemoveCategoriesFromCache()
        {
            Remove("category-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}