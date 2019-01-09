using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace mrRemoteForKodi_Update_1.Helpers
{
    class ConnectionHelper
    {
        /// <summary>
        /// contact the kodi/xbmc server with given data
        /// </summary>
        /// <param name="host">host</param>
        /// <param name="port">port</param>
        /// <param name="user">username</param>
        /// <param name="pass">password</param>
        /// <param name="requestData">data json command for kodi</param>
        /// <returns>response from the server, statusError or connectionError</returns>
        public static async Task<string> ExecuteRequest(string host, string port, string user, string pass, string requestData)
        {
            var httpClient = new HttpClient();

            // set the timeout to 5 seconds
            httpClient.Timeout = new TimeSpan(0, 0, 5);

            var uriString = "http://" + host + ":" + port + "/jsonrpc";
            var authString = user + ":" + pass;

            try
            {
                // create the requesto to send
                var request = new HttpRequestMessage(HttpMethod.Post, uriString);

                request.Content = new StringContent(requestData);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(authString)));

                // send the message
                var response = await httpClient.SendAsync(request);

                // check if the response has the correct status else return null
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return "statusError";
                }
            }
            catch
            {
                return "connectionError";
            }
        }

        public static async Task<Stream> ExecuteRequestImage(string host, string port, string user, string pass, string requestData)
        {
            var httpClient = new HttpClient();
            Stream stream = null;

            // set the timeout to 5 seconds
            httpClient.Timeout = new TimeSpan(0, 0, 5);

            var urlString = "http://" + host + ":" + port + "/image/";
            var authString = user + ":" + pass;

            try
            {
                var encodedRequestUrl = WebUtility.UrlEncode(requestData);
                var uriString = urlString + encodedRequestUrl;

                // create the requesto to send
                var request = new HttpRequestMessage(HttpMethod.Post, uriString);

                // authentication
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(authString)));

                // send the message
                var response = await httpClient.SendAsync(request);

                // check if the response has the correct status else return null
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    stream = await response.Content.ReadAsStreamAsync();

                    if (stream != null)
                    {
                        return stream;
                    }
                }

                return stream;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<HttpRandomAccessStream> ExecuteRequestVideo(string host, string port, string user, string pass, string requestData)
        {
            var httpClient = new Windows.Web.Http.HttpClient();

            var urlString = "http://" + host + ":" + port + "/" + requestData;
            var authString = user + ":" + pass;

            try
            {
                var uriString = new Uri(urlString);

                var encoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString));

                httpClient.DefaultRequestHeaders.Add("Authorization", "Basic " + encoded);

                var stream = await HttpRandomAccessStream.CreateAsync(httpClient, uriString);

                return stream;
            }
            catch
            {
                return null;
            }
        }
    }
}