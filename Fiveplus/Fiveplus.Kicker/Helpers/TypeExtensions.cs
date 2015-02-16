using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Fiveplus.Kicker.Helpers
{
    public static  class TypeExtensions
    {

        /// <summary>
        /// IQueryable Helper to Sort object by a field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="sortField"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> query,
                                                string sortField,
                                                SortDirection direction)
        {
            if (direction == SortDirection.Ascending)
                return query.OrderBy(s => s.GetType()
                                           .GetProperty(sortField));
            return query.OrderByDescending(s => s.GetType()
                                                 .GetProperty(sortField));

        }


        public static string ConvertToJson<T>(this IEnumerable<T> list, string key)
        {

            if (list.Count() == 0)
                return "";

            //IEnumerable<string> list = new List<string>() { "X", "Y", "Z" };
            string data = string.Empty;
            foreach (var item in list)
            {
                if (String.IsNullOrEmpty(data))
                    data = "\"" + item + "\"";
                else
                    data = data + ",\"" + item + "\"";
            }

            string result = String.Format("{{ {0} : [{1}] }}", "\"Errors\"", data);
            return result;
        }


    }
}