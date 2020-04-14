/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package reader_rfid;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.logging.Level;
import java.util.logging.Logger;
import jdk.nashorn.internal.parser.JSONParser;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 *
 * @author bao13
 */
public class Reader_RFID {

    /**
     * @param args the command line arguments
     */
    private static String readAll(Reader rd) throws IOException {
        StringBuilder sb = new StringBuilder();
        int cp;
        while ((cp = rd.read()) != -1) {
            sb.append((char) cp);
        }
        return sb.toString();
    }

    public static JSONObject readJsonFromUrl(String url) throws IOException, JSONException {
        try (InputStream is = new URL(url).openStream()) {
            BufferedReader rd = new BufferedReader(new InputStreamReader(is, Charset.forName("UTF-8")));
            String jsonText = readAll(rd);
            JSONObject json = new JSONObject(jsonText);
            return json;
        }
    }

    public static void main(String[] args) {
        try {
            // TODO code application logic here
            JSONObject json = readJsonFromUrl("https://api.thingspeak.com/channels/1034449/feeds.json?api_key=ZEINJQJL6OIAO9JP&results=10");
            JSONArray results_arr = json.getJSONArray("feeds");
            int c = 0;
            for(Object res : results_arr){
                c++;
                JSONObject obj = (JSONObject)res;
                System.out.println(c +   " field1:" + obj.get("field1")
                                    + "  field2:" + obj.get("field2")
                                    + "  field3:" +obj.get("field3")
                                    + "  field4:" +obj.get("field4")
                                    + "  field5:" +obj.get("field5")
                                    + "  field6:" +obj.get("field6")
                                    + "  field7:" +obj.get("field7")
                                    + "  field8:" +obj.get("field8"));
            }
        } catch (IOException ex) {
            Logger.getLogger(Reader_RFID.class.getName()).log(Level.SEVERE, null, ex);
        } catch (JSONException ex) {
            Logger.getLogger(Reader_RFID.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

}
