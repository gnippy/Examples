import sun.misc.BASE64Encoder;

import java.io.*;
import java.net.HttpURLConnection;
import java.net.URL;
import java.util.zip.GZIPInputStream;

class DeleteRule {
  public static void main(String[] args) throws IOException  {

        // ENTER REAL INFORMATION HERE

        String account = "ACCOUNT_NAME";
        String username = "USER_NAME";
        String password = "PASSWORD";
        String publisher = "PUBLISHER";
        String streamType = "STREAM_TYPE";
        String label = "STREAM_LABEL"; // Should be "rules" to list the rules

// EX:
//          String account = "mike";
//          String username = "mbusch@example.com";
//          String password = "mypassword";
//          String publisher = "twitter";
//          String streamType = "track";
//          String label = "rules"; // Should be "rules" to list the rules
        
        String charset = "UTF-8";
        String param1 = "testrule"; //our rule to delete
        String query = String.format("{\"rules\":[{\"value\":\"%s\"}]}", 
             java.net.URLEncoder.encode(param1, charset));

        String dataCollectorURL = getStreamUrl(account, publisher, streamType, label);

        HttpURLConnection connection = null;
        InputStream inputStream = null;
        try {
            connection = getConnection(dataCollectorURL, username, password);

            connection.setDoOutput(true);
            connection.setRequestProperty("Accept-Charset", charset);
            connection.setRequestProperty("Content-Type", "text/json");
            OutputStream output = null;
            try {
                 output = connection.getOutputStream();
                 output.write(query.getBytes(charset));
            } finally {
                 if (output != null) try { output.close(); } catch (IOException logOrIgnore) {}
            }

            inputStream = connection.getInputStream();
            int responseCode = connection.getResponseCode();

            if (responseCode >= 200 && responseCode <= 299) {

                // Just print the first line of the response.
                BufferedReader reader = new BufferedReader(new InputStreamReader(new GZIPInputStream(inputStream)));
                String line = reader.readLine();
                while(line != null){
                    System.out.println(line);
                    line = reader.readLine();
                }
            } else {
                handleNonSuccessResponse(connection);
            }
        }
    catch (Exception e) {
            e.printStackTrace();
            if (connection != null) {
                handleNonSuccessResponse(connection);
            }
        } finally {
            if (inputStream != null) {
                inputStream.close();
            }
        }
    
  }
      private static String getStreamUrl(String account, String publisher, String streamType, String label) {
      return	"https://api.gnip.com:443/accounts/" + account + "/publishers/" + publisher +"/streams/"+ streamType +"/track/"+ label +".json";
      }

      private static void handleNonSuccessResponse(HttpURLConnection connection) throws IOException {
          int responseCode = connection.getResponseCode();
          String responseMessage = connection.getResponseMessage();
          System.out.println("Non-success response: " + responseCode + " -- " + responseMessage);
      }

      private static HttpURLConnection getConnection(String urlString, String username, String password) throws IOException {
          URL url = new URL(urlString);

          HttpURLConnection connection = (HttpURLConnection) url.openConnection();
          connection.setRequestMethod("DELETE");
          connection.setReadTimeout(1000 * 60 * 60);
          connection.setConnectTimeout(1000 * 10);

          connection.setRequestProperty("Authorization", createAuthHeader(username, password));
          connection.setRequestProperty("Accept-Encoding", "gzip");

           return connection;
      }

      private static String createAuthHeader(String username, String password) throws UnsupportedEncodingException {
          BASE64Encoder encoder = new BASE64Encoder();
          String authToken = username + ":" + password;
          return "Basic " + encoder.encode(authToken.getBytes());
      }
  
}
