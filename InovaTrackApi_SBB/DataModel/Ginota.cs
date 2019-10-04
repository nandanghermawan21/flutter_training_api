using InovaTrackApi_SBB.Helper;
using Microsoft.IdentityModel.Protocols;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace InovaTrackApi_SBB.DataModel
{
    public class Ginota
    {
        public string apiKey { get; set; }
        public string apiSecreet { get; set; }
        public Message message { get; set; }
        public Response response { get; set; }

        public Ginota(string apiKey, string apiSecreet)
        {
            this.apiKey = apiKey ?? "";
            this.apiSecreet = apiSecreet ?? "";
        }

        public class Message
        {
            public string senderName { get; set; }
            public string phoneNumber { get; set; }
            public string content { get; set; }
            public bool flash { get; set; }
        }

        public class Response
        {
            public string status;
            public string parts;
            public string messageId;
        }

        public async Task<Ginota> Send(Message message)
        {
            this.message = message;
            //We will make a GET request to a really cool website...

            string baseUrl = this.GetUrl();
            //The 'using' will help to prevent memory leaks.
            //Create a new instance of HttpClient
            using (HttpClient client = new HttpClient())

                //Setting up the response...         

                try
                {
                    using (HttpResponseMessage res = await client.GetAsync(baseUrl))
                    using (HttpContent content = res.Content)
                    {
                        string data = await content.ReadAsStringAsync();
                        if (data != null)
                        {
                            this.response = JsonConvert.DeserializeObject<Response>(data);
                            return this;
                        }
                        else
                        {
                            return this;
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
        }

        public string GetUrl()
        {
            return $"https://www.ginota.com/gemp/sms/json?apiKey={this.apiKey}&apiSecret={this.apiSecreet}&srcAddr={this.message.senderName}&dstAddr={this.message.phoneNumber}&flash={this.message.flash}&content={this.message.content}";
        }
    }
}
