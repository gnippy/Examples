using System;
using System.Net;
using System.IO;
using System.Text;

namespace BasicOps
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string choice = "";
			
			MainClass op = new MainClass();
			
			while (!choice.Equals("q"))
			{
				Console.WriteLine ("This program demonstrates the basic operations of");
				Console.WriteLine ("(a)dding, (d)eleting, and (l)isting rules.  As a bonus, you can also (q)uit.");
				Console.WriteLine ();
				
				Console.Write ("Operation => ");
				
				choice = Console.ReadLine();
				
				choice = choice.ToLower();
				
				if (!(choice.Equals("a") || choice.Equals("d") || choice.Equals("l")))
				{
					break;
				}
				//op.ruleAction(choice);
				switch (choice) 
				{
					case "a":
					case "d":
						op.ruleAction(choice);
						break;
					case "l":
						op.listRules();
						break;
					default:
						break;
				}
			
			}
		}
		
		public HttpWebRequest makeRequest()
		{
			// Open the Power Track stream
			string urlString = "your-url-here";			
			                    
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlString);
			
			string username = "username";
			string password = "password";
			
		    NetworkCredential nc = new NetworkCredential(username, password);
		    request.Credentials = nc;
			request.PreAuthenticate = true;

			
			// Need the compression settings - EKE
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			request.Headers.Add("Accept-Encoding", "gzip");
			
			return request;			
		}
		
		
		public void listRules()
		{
			HttpWebRequest request = makeRequest();
            request.Method = "GET";
			HttpWebResponse response = (HttpWebResponse) request.GetResponse();
            // Display the status.
            Console.WriteLine (((HttpWebResponse)response).StatusDescription);
            // Read the content.
			StreamReader reader = new StreamReader(response.GetResponseStream());

            string responseFromServer = reader.ReadToEnd ();
            // Display the content.
            Console.WriteLine (responseFromServer);
			Console.WriteLine();
            // Clean up the streams.
            reader.Close ();
            response.Close ();
		}
		
		public void ruleAction(string action)
		{
			HttpWebRequest request = makeRequest();
			// Set the Method property of the request to POST.
			string postData = "";
			string rule = "";
			

			Console.WriteLine("Enter below the rule to add.");
			rule = Console.ReadLine();
			
			
			switch (action) 
			{
			case "a":
				request.Method = "POST";
				postData = "{\"rules\":[{\"tag\":\"tests\",\"value\":\"" + rule + "\"}]}";
				break;
			case "d":
				request.Method = "DELETE";
				postData = "{\"rules\":[{\"value\":\"" + rule + "\"}]}";
				break;
			default:
				break;
			}
   

			
			byte[] byteArray = Encoding.UTF8.GetBytes (postData);
            // Set the ContentType property of the WebRequest.
            request.ContentType = "application/x-www-form-urlencoded";
            // Set the ContentLength property of the WebRequest.
            request.ContentLength = byteArray.Length;
            // Get the request stream.
            Stream dataStream = request.GetRequestStream ();			
            // Write the data to the request stream.
            dataStream.Write (byteArray, 0, byteArray.Length);
            // Close the Stream object.
            dataStream.Close ();

            // Get the response.
            WebResponse response = request.GetResponse ();
            // Display the status.
            Console.WriteLine (((HttpWebResponse)response).StatusDescription);
            // Get the stream containing content returned by the server.
            dataStream = response.GetResponseStream ();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader (dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd ();
            // Display the content.
            Console.WriteLine (responseFromServer);
			Console.WriteLine();
            // Clean up the streams.
            reader.Close ();
            dataStream.Close ();
            response.Close ();
		}		
	}
}

