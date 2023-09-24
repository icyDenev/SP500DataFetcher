using System;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Globalization;

namespace SP500DataFetcher
{
    class DataFetcher
    {
        static void Main(string[] args)
        {
            List<string> companiesSymbols = new List<string>();
            Int32 unixTimestampMinus20Years = (int)DateTime.UtcNow.AddYears(-20).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Int32 unixTimestampNow = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            string data;

            // Download S&P500 companies symbols
            using (WebClient client = new WebClient())
            {
                string url = "https://raw.githubusercontent.com/datasets/s-and-p-500-companies/main/data/constituents.csv";
                string[] dataSymbols = client.DownloadString(url).Split("\n");

                // Extract the symbols from the data and put them into a list
                for (int i = 1; i < dataSymbols.Length; ++i)
                {
                    string[] lineData = dataSymbols[i].Split(',');
                    if (!lineData[0].Equals(""))
                    {
                        companiesSymbols.Add(lineData[0]);
                    }
                }

                // Download S&P500 companies historical data
                for (int i = 0; i < companiesSymbols.Count; i++)
                { // TODO: Download conventions for S&P500 companies, extract the symbols, and put them into a list
                    url = $"https://query1.finance.yahoo.com/v7/finance/download/{companiesSymbols[i]}?period1={unixTimestampMinus20Years}&period2={unixTimestampNow}&interval=1d&events=history&includeAdjustedClose=true";
                    data = client.DownloadString(url);
                    File.WriteAllText($"{companiesSymbols[i]}.csv", data);

                    Console.WriteLine($"Downloaded {companiesSymbols[i]}.csv");

                    Thread.Sleep(200); // Sleep for 1 second to avoid Yahoo! Finance API rate limit
                }
            }
        }
    }
}