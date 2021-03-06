/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package reader_rfid;

import DataBase.Fields;
import GUI.MainJFrame;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import jdk.nashorn.internal.parser.JSONParser;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

/**
 *
 * @author bao13
 */
public class Reader_RFID {

    private static List<Fields> lFields = new ArrayList<>();
    
    //private final String 
    
    
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

    private static void sendGET(String field1,
            String field2,
            String field3,
            String field4,
            String field5,
            String field6,
            String field7,
            String field8
    ) throws IOException {
        String GET_URL = "https://api.thingspeak.com/update?api_key=A1YDKPEI7SQAWE50&field1=123&field2=0&field3=40&field4=0&field5=120&field6=0&field7=0&field8=120";
        GET_URL = (GET_URL
                + "&field1=" + field1
                + "&field2=" + field2
                + "&field3=" + field3
                + "&field4=" + field4
                + "&field5=" + field5
                + "&field6=" + field6
                + "&field7=" + field7
                + "&field8=" + field8);

        URL obj = new URL(GET_URL);
        HttpURLConnection con = (HttpURLConnection) obj.openConnection();
        con.setRequestMethod("GET");
        //con.setRequestProperty("User-Agent", USER_AGENT);
        int responseCode = con.getResponseCode();
        System.out.println("GET Response Code :: " + responseCode);
        if (responseCode == HttpURLConnection.HTTP_OK) { // success
            BufferedReader in = new BufferedReader(new InputStreamReader(
                    con.getInputStream()));
            String inputLine;
            StringBuffer response = new StringBuffer();

            while ((inputLine = in.readLine()) != null) {
                response.append(inputLine);
            }
            in.close();

            // print result
            System.out.println(response.toString());
        } else {
            System.out.println("GET request not worked");
        }
    }

    public void getContentJSON() {
        List<String> arrList = new ArrayList<>();
        try {
            JSONObject json = readJsonFromUrl("https://api.thingspeak.com/channels/1100376/feeds.json?api_key=UQNY68M62985KQC6&results");
            JSONArray results_arr = json.getJSONArray("feeds");
            int c = 0;
            for (Object res : results_arr) {
                c++;
                JSONObject obj = (JSONObject) res;
                String ac = obj.getString("field1");
                System.out.println("Account: " + ac);
                arrList.add(ac);
            }          
        } catch (IOException ex) {
            Logger.getLogger(Reader_RFID.class.getName()).log(Level.SEVERE, null, ex);
        } catch (JSONException ex) {
                        JOptionPane.showMessageDialog(null, "Thiết bị chưa kết nối mạng !!!",
            "Error",JOptionPane.ERROR_MESSAGE);
            Logger.getLogger(Reader_RFID.class.getName()).log(Level.SEVERE, null, ex);
        }
    }
    
    public int statusJson(){

        return 1;
    }
    
    public List<Fields> getListData(){
        getContentJSON();
        return lFields;
    }
}
