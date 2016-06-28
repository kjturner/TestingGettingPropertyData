using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.Entity.SqlServer;
using Newtonsoft.Json;

namespace TestingGettingPropertyData
{
    class Program
    {

        static void UpdateTable(string acct, string stat, string res)
        {
            int i = Int32.Parse(acct);
            var context = new StagingEntities();
            var result = context.PropertyDatas.Single(x => x.Account_Number == i);
            result.results = res;
            result.Status = stat;
            context.SaveChanges();
        }


        static List<string> getAccountIDs(int ct)
        {
            var context = new StagingEntities();
            var i = context.PropertyDatas
                    .Where(x => x.Status.Equals(null))
                    .OrderBy(x => Guid.NewGuid())
                    .Take(ct);

            return i.Select(x => x.AcctNumStr)
                .ToList();

        }

        private static async Task MainAsync()
        {

            HttpClient client =
                new HttpClient() { MaxResponseContentBufferSize = 10000000 };

            TimeSpan interval = new TimeSpan(0, 0, 0, 3, 0);
            client.Timeout = interval;
            int loops = 5;
            List<string> acts = getAccountIDs(loops);

            List<Task<string>> downloads = new List<Task<string>>();
            foreach (var item in acts)
            {
                downloads.Add(ProcessURLAsync("http://api.phila.gov/opa/v1.1/account/" + item + "?format=json", client));
            }

            List<string> results = new List<string>();
            foreach (var item in downloads)
            {
                results.Add(await item);
            }

            for (int i = 0; i < results.Count; i++)
            {
                if (!String.IsNullOrEmpty(results[i]))
                {
                    UpdateTable(acts[i], "success", results[i]);
                    Console.WriteLine("Success: " + acts[i]);
                }
                else
                {
                    //UpdateTable(acts[i], "failed", null);
                    Console.WriteLine("Fail: " + acts[i]);
                }
            }
        }

        static async Task<string> ProcessURLAsync(string url, HttpClient client)
        {
            try
            {
                var byteArray = await client.GetByteArrayAsync(url);
                return System.Text.Encoding.Default.GetString(byteArray);
            }
            catch (Exception e)
            {
                //Console.WriteLine("error " + url);
                return null;
            }

            // DisplayResults(url, byteArray);


        }


        static void nonAsync()
        {
            string sURL;
            WebRequest wrGETURL;
            Stream objStream;
            string results;
            for (int i = 0; i < 100000; i++)
            {
                Console.WriteLine(i + ") ");




                var context = new StagingEntities();
                var a = context.PropertyDatas
                        .Where(x => x.Status.Equals("failed"))
                        .OrderBy(x => Guid.NewGuid())
                        .Take(1);

                string acct = a.First().AcctNumStr;

                try
                {
                    sURL = "http://api.phila.gov/opa/v1.1/account/" + acct + "?format=json";
                    wrGETURL = WebRequest.Create(sURL);
                    wrGETURL.Timeout = 10000;
                    objStream = wrGETURL.GetResponse().GetResponseStream();
                    StreamReader objReader = new StreamReader(objStream);
                    results = objReader.ReadLine();
                    Console.WriteLine("Success: " + sURL);
                    UpdateTable(acct, "success", results);
                    results = null;
                }
                catch (WebException e)
                {
                    using (WebResponse response = e.Response)
                    {
                        // Console.WriteLine("Failed Response: " + response.ResponseUri.AbsoluteUri);
                        Console.WriteLine("Failed Response: ");

                        UpdateTable(acct, "failed", null);
                    }
                }
            }
        }
        static void Main(string[] args)
        {

            var context = new StagingEntities();
            PropertyData prop = context.PropertyDatas
                .Where(x => x.Status.Equals("success"))
                .FirstOrDefault();

            RootObject result = JsonConvert.DeserializeObject<RootObject>(prop.results);
        }

     
    }
}






