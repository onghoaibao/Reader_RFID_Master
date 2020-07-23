/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package DataBase;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;
import javax.net.ssl.HttpsURLConnection;

/**
 *
 * @author bao13
 */
public class RequestGETDataBase {
    private static String GET_URL_FIRST = "https://script.google.com/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?";

    public void executeGET(String maThietbi, String properties_name, String properties_sx,
                          String trangthai) throws MalformedURLException {       
        try {
            String https_url = getURL(maThietbi, properties_name, properties_sx,
                            trangthai);
            URL myUrl = new URL(https_url);
            HttpsURLConnection conn = (HttpsURLConnection) myUrl.openConnection();
            InputStream is = conn.getInputStream();
            InputStreamReader isr = new InputStreamReader(is);
            BufferedReader br = new BufferedReader(isr);          
            String inputLine;          
            while ((inputLine = br.readLine()) != null) {
                System.out.println(inputLine);
            }           
            br.close();
        } catch (IOException ex) {
            
        }
    }
    
    private String getURL(String maThietbi, String properties_name, String properties_sx,
                          String trangthai){
        String url = GET_URL_FIRST 
                + "maThietBi=" + maThietbi + "&"
                + "properties_name=" + properties_name + "&"
                + "properties_name=" + properties_name + "&"
                + "properties_sx=" + properties_sx + "&"
                + "trangthai=" + trangthai + "&"
                + "dvMuon=Khong&" 
                + "ngayTra=Khong&" 
                + "lsdc=Khonng&"
                + "hientai=Kho";
        System.out.println("URL: " + url);
        return url;
    }   

}
