﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LNLamaScrape.Tools
{
    public class WebClient : IWebClient
    {
        protected static HttpClient Client { get; }

        static WebClient()
        {
            Client = new HttpClient(new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            });
            Client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:61.0) Gecko/20100101 Firefox/61.0");
            Client.DefaultRequestHeaders.Add("Accept", "text/html, application/xhtml+xml, application/json, text/javascript, */*");
            Client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        }

        public async Task<string> GetStringAsync(Uri uri, Uri referrer, CancellationToken token)
        {
            var output = default(string);
            Client.DefaultRequestHeaders.Referrer = referrer;
            try
            {
                var res = await Client.GetAsync(uri, token);
                if (res.IsSuccessStatusCode && !token.IsCancellationRequested)
                {
                    output = await res.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return null;
            }

            return output;
        }

        public async Task<byte[]> GetByteArrayAsync(Uri uri, Uri referrer, CancellationToken token)
        {
            var output = default(byte[]);
            Client.DefaultRequestHeaders.Referrer = referrer;
            try
            {
                var res = await Client.GetAsync(uri, token);
                if (res.IsSuccessStatusCode && !token.IsCancellationRequested)
                {
                    output = await res.Content.ReadAsByteArrayAsync();
                }
            }
            catch
            {
                return null;
            }

            return output;
        }

        public Task<HttpResponseMessage> PostAsync(HttpContent content, Uri uri, Uri referrer, CancellationToken token)
        {
            Client.DefaultRequestHeaders.Referrer = referrer;
            return Client.PostAsync(uri, content, token);
        }
    }
}
