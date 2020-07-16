/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package TestFunction;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.net.ssl.HttpsURLConnection;

/**
 *
 * @author bao13
 */
public class testHTTP {

    private static String GET_URL = "https://script.google.com/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?"
            + "maThietBi=ABCDzxcEFsGHsCDE123&properties_name=ohbao&properties_sx=1234&trangthai=Tot&dvMuon=phong1&ngayTra=12-12-2020&lsdc=A1&hientai=Kho";
    
    private static String GET_URL_FIRST = "https://script.google.com/macros/s/AKfycbyv6koMALyPSHIlVG8pp0ysDgmIaA-csMx4bB05utsQZXlJKN0/exec?";
   
//    public static void main(String[] args) throws IOException {
//        executeGET(GET_URL);
//    }

    public static void executeGET(String httpsURL) throws MalformedURLException {       
        try {
            String https_url = getURL("1","2","3","4","5","6","7","8","9");
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
    
    private static String getURL(String maThietbi, String properties_name, String properties_sx,
                          String trangthai, String dvMuon, String ngayMuon, String ngayTra,
                          String lsdc, String hientai){
        String url = GET_URL_FIRST 
                + "maThietBi=" + maThietbi + "&"
                + "properties_name=" + properties_name + "&"
                + "properties_name=" + properties_name + "&"
                + "properties_sx=" + properties_sx + "&"
                + "trangthai=" + trangthai + "&"
                + "dvMuon=" + dvMuon + "&"
                + "ngayTra=" + ngayTra + "&"
                + "lsdc=" + lsdc + "&"
                + "hientai=" + hientai;
        return url;
    }   

}
