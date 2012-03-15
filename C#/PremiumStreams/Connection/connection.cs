using System;
using System.IO;
using System.Net;



namespace Connection
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string str;
			MainClass m = new MainClass();
			str = m.get();
			System.Console.WriteLine (str);
		}
        private WebRequest connect(string server, string method)
        {
            HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(server);
            request.Method = method;
 
            NetworkCredential nc = new NetworkCredential("username", "password");
            request.Credentials = nc;
            request.PreAuthenticate = true;
 
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            request.Headers.Add("Accept-Encoding", "gzip");
 
            // Need Accept and ContentType Headers set to JSON
            request.Accept = "application/json";
            request.ContentType = "application/json";
 
           request.Timeout = 5000; // in milliseconds, so max timeout after 5 seconds.
 
//            string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("mbusch@gnipcentral.com", "nexus7"));
//            request.Headers.Add("Authorization", "\"Basic \"" + credentials);
            return (WebRequest)request;
        }
 
        private string get()
        {
            return get_response(connect("your-rules-url","GET"));
        }
 
        private string get_response(WebRequest request)
        {
 
            String serverResponse = string.Empty;
            try
            {
                WebResponse response = request.GetResponse();
				StreamReader responseStream = new StreamReader(response.GetResponseStream());

				serverResponse = responseStream.ReadLine();
				return serverResponse;
			}
			catch (Exception e)
			{
				System.Console.WriteLine("Exception  => " + e.Message);
				return e.Message;
			}
		}
			
	}
}
