﻿using System;
using System.Net;
using System.IO;
using System.Collections.Specialized;
using System.Globalization;

namespace SPXDataFetcher
{
    class DataFetcher
    {
        static void Main(string[] args)
        {
            List<string> companiesSymbols = new List<string>();
            DateTime dateTimeNow = DateTime.Now, dateTimePast20years = DateTime.Now.AddYears(-20);

            using (WebClient client = new WebClient())
            {
                string url = "https://datahub.io/core/s-and-p-500-companies/r/constituents.csv";
                string[] data = client.DownloadString(url).Split('\n');
                for (int i = 1; i < data.Length; ++i)
                {
                    string[] lineData = data[i].Split(',');
                    companiesSymbols.Add(lineData[0]);
                }
            }

            for (int i = 0; i < companiesSymbols.Count; i++) // TODO: Download conventions for SPX companies, extract the symbols, and put them into a list
            {
                using (WebClient client = new WebClient())
                {
                    string url = $"https://www.wsj.com/market-data/quotes/{companiesSymbols[i]}/historical-prices/download?MOD_VIEW=page&num_rows=5100&range_days=7395&startDate={dateTimePast20years.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}&endDate={dateTimeNow.Date.ToString("MM/dd/yyyy", CultureInfo.InvariantCulture)}";
                    string data = client.DownloadString(url);
                    File.WriteAllText($"{companiesSymbols[i]}.csv", data);
                }

                Console.WriteLine($"Downloaded {companiesSymbols[i]}.csv");
            }
        }
    }
}