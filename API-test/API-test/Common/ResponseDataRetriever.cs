using API_test.ApiResponseModels;
using API_test.RequestSpecs;
using System;
using System.Collections.Generic;

namespace API_test.Helpers
{
    public class ResponseDataRetriever
    {
        public int GetExpectedlNumberOfItems(RestHelper restHelper)
        {
            var response = restHelper.GetQuery(RequestSpec.Query);
            return response.Content.ToObject<TotalDto>().Total;
        }

        //я полагаю, что факт того, что каждая предыдущая страница заканчивается на неком item, 
        //а следующая страница начинается с него же - ожидаемое поведение. Потому, я удаляю эти дубли.
        //В реальной ситуации я бы уточнила это у разработчика, если не указано в документации
        public List<ItemDto> GetAllUniqueItems(RestHelper restHelper)
        {
            var response = restHelper.GetQuery(RequestSpec.Query);
            List<ItemDto> items = response.Content.SelectToken("items").ToObject<List<ItemDto>>();

            List<ItemDto> nextPageItems;
            int page = 2;
            while (true)
            {
                response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.PageNumberParameter, page.ToString());
                nextPageItems = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
                if (nextPageItems.Count != 0)
                {
                    nextPageItems.RemoveAt(0);
                    items.AddRange(nextPageItems);
                    page++;
                }
                else
                    break;
            }
            return items;
        }

        public List<ItemDto> GetFilteredItemsByCountryCode(RestHelper restHelper, string countryCode)
        {
            var response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.CountryCodeParameter, countryCode);
            List<ItemDto> items = response.Content.SelectToken("items").ToObject<List<ItemDto>>();

            List<ItemDto> nextPageItems;
            int page = 2;
            while (true)
            {
                response = restHelper.GetQuery(RequestSpec.Query, RequestSpec.CountryCodePageNumberParam(countryCode, page.ToString()));
                nextPageItems = response.Content.SelectToken("items").ToObject<List<ItemDto>>();
                if (nextPageItems.Count != 0)
                {
                    nextPageItems.RemoveAt(0);
                    items.AddRange(nextPageItems);
                    page++;
                }
                else
                    break;
            }
            return items;
        }
    }
}
