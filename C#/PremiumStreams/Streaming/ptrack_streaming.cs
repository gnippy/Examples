using System;
using System.IO;
using System.Net;
using System.Text;


namespace Connection
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create("your-stream-url-here");
      request.Method = "GET";
       
      //Setup Credentials
      NetworkCredential nc = new NetworkCredential("username", "password");
      request.Credentials = nc;
      
      // If you receive a 401 error, try commenting the two lines above and using the two below
//      string credentials = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes("username"+ ":" + "password"));
//      request.Headers.Add("Authorization", "Basic " + credentials);
      request.PreAuthenticate = true;
      // Need Accept and ContentType Headers set to XML
      // Need the compression settings
      request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
      request.Headers.Add("Accept-Encoding", "gzip");

      request.Accept = "application/json";
      request.ContentType = "application/json";
          
      Stream objStream;
      objStream = request.GetResponse().GetResponseStream();

      StreamReader objReader = new StreamReader(objStream);

      string sLine = "";
//      int i = 0;
       
      while (!objReader.EndOfStream)
      {
//        i++;
        sLine = objReader.ReadLine();
        if (sLine!=null)
          Console.WriteLine(sLine);
      }
      Console.ReadLine();      
    }
  }
}


