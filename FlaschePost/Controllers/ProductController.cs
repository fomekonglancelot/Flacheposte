using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using FlaschePost.Models;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace FlaschePost.Controllers
{
    public class ProductController : ApiController
    {

    
        // List Product

        public List<Product> GetListProduct()
        {
            List<Product> productList = new List<Product>();

            using (var webClient = new WebClient())
            {
                String jsonString = webClient.DownloadString("https://flapotest.blob.core.windows.net/test/ProductData.json");
                var product = Product.FromJson(jsonString);


                productList = product;


            }

            return productList;
        }


        // GET: api/Product/5
        public Product Get(long id)
        {
            
            Product result = GetListProduct().Find(x => x.Id== id);


            return result;
        }


        //suche nach MAX und Mix
        [Route("api/Product/article/{seach}")]
        public Product Get(string seach)
        {
            var dictionary = new Dictionary<long, double>();

            Product p2 = new Product();

            if (seach == "max")
            {
                foreach (Product p in GetListProduct())
                {
                    dictionary.Add(p.Id, p.Articles.Max(x => Convert.ToDouble(ExecuteRegEx(x.PricePerUnitText))));

                }

                var maxprod = dictionary.Max(x => x.Value);

                foreach (var da in dictionary)
                {
                    if (da.Value == maxprod)
                    {
                        p2 = Get(da.Key);

                    }


                }
            }
           else if (seach == "min")
            {
                foreach (Product p in GetListProduct())
                {
                    dictionary.Add(p.Id, p.Articles.Min(x => Convert.ToDouble(ExecuteRegEx(x.PricePerUnitText))));

                }

                var minprod = dictionary.Min(x => x.Value);

                foreach (var da in dictionary)
                {
                    if (da.Value == minprod)
                    {
                        p2 = Get(da.Key);

                    }


                }
            }
            else if (seach == "mostBottles")
            {

                foreach (Product p in GetListProduct())
                {
                    dictionary.Add(p.Id, p.Articles.Max(x => Convert.ToDouble(convertor(x.ShortDescription))));

                }

                var minprod = dictionary.Max(x => x.Value);

                foreach (var da in dictionary)
                {
                    if (da.Value == minprod)
                    {
                        p2 = Get(da.Key);

                    }


                }
            }
            else
            {
                return null;
            }



            return p2;
        }


        //suche nach price
        //bei surche, muss man so in browser api/Product/price/17.99/ eingeben
        [Route("api/Product/price/{price}")]
        public ICollection<Product> GetPrice(Double price){
              
            List<Product> listproduct = new List<Product>();
            foreach (Product p in GetListProduct())
                {

                   var ar = p.Articles.Exists(x => x.Price == price);

                if (ar)
                {
                    listproduct.Add(p);
                }

                }

            return listproduct;
        }


        //suche  nummer prix in shortunite
        public static string convertor(string inputString)
        {

            string[] words = inputString.Split('x');

            return words[0]; ;
        }


        public static string ExecuteRegEx(string inputString)
        {
            string pattern = "[0-9]";
            Regex r = new Regex(pattern);
            MatchCollection mc = r.Matches(inputString);
            string retVal = string.Empty;
            for (int i = 0; i < mc.Count; i++)
            { retVal += mc[i].Value; }
            return retVal;
        }
    }
}
