package DataBase;

import com.google.api.client.auth.oauth2.Credential;
import com.google.api.client.extensions.java6.auth.oauth2.AuthorizationCodeInstalledApp;
import com.google.api.client.extensions.jetty.auth.oauth2.LocalServerReceiver;
import com.google.api.client.googleapis.auth.oauth2.GoogleAuthorizationCodeFlow;
import com.google.api.client.googleapis.auth.oauth2.GoogleClientSecrets;
import com.google.api.client.googleapis.javanet.GoogleNetHttpTransport;
import com.google.api.client.http.javanet.NetHttpTransport;
import com.google.api.client.json.JsonFactory;
import com.google.api.client.json.jackson2.JacksonFactory;
import com.google.api.client.util.store.FileDataStoreFactory;
import com.google.api.services.sheets.v4.Sheets;
import com.google.api.services.sheets.v4.SheetsScopes;
import com.google.api.services.sheets.v4.model.AppendValuesResponse;
import com.google.api.services.sheets.v4.model.UpdateValuesResponse;
import com.google.api.services.sheets.v4.model.ValueRange;
import java.io.FileInputStream;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;

public class SheetsQuickstart {

    private static final String APPLICATION_NAME = "Google Sheets API Java Quickstart";
    private static final JsonFactory JSON_FACTORY = JacksonFactory.getDefaultInstance();
    private static final String TOKENS_DIRECTORY_PATH = "tokens";

    private static final List<String> SCOPES = Collections.singletonList(SheetsScopes.SPREADSHEETS);
    private static final String CREDENTIALS_FILE_PATH = "credentials.json";

    //device 
    private static Sheets service;
    private static NetHttpTransport HTTP_TRANSPORT;
    private static final String spreadsheetId = "1rdQB3aXvY7fEuqzjb0LQRnlQMyYH0HV0wn-z9c8Ho7c";

    // declare Column
    //private String colA;
    public SheetsQuickstart() throws GeneralSecurityException, IOException {
        HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
        service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                .setApplicationName(APPLICATION_NAME)
                .build();
    }

    private static Credential getCredentials(final NetHttpTransport HTTP_TRANSPORT) throws IOException {
        InputStream in = new FileInputStream(CREDENTIALS_FILE_PATH);
        if (in == null) {
            throw new FileNotFoundException("Resource not found: " + CREDENTIALS_FILE_PATH);
        }
        GoogleClientSecrets clientSecrets = GoogleClientSecrets.load(JSON_FACTORY, new InputStreamReader(in));
        GoogleAuthorizationCodeFlow flow = new GoogleAuthorizationCodeFlow.Builder(
                HTTP_TRANSPORT, JSON_FACTORY, clientSecrets, SCOPES)
                .setDataStoreFactory(new FileDataStoreFactory(new java.io.File(TOKENS_DIRECTORY_PATH)))
                .setAccessType("offline")
                .build();
        LocalServerReceiver receiver = new LocalServerReceiver.Builder().setPort(8080).build();
        return new AuthorizationCodeInstalledApp(flow, receiver).authorize("user");
    }

//    public static void main(String[] args) throws IOException, GeneralSecurityException {
//
//        HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
//        service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
//                .setApplicationName(APPLICATION_NAME)
//                .build();
//        
//        
////        int index = getPosMaThietBi("ABCDEF5");
////
////        updateInformationOnSheet(index + 15,"", "", "", "", "Mango", "Mango", "Mango", "Mango");
////
//        String range = "account!A1:A10";
//        ValueRange response = service.spreadsheets().values()
//                .get(spreadsheetId, range)
//                .execute();
//
//        List<List<Object>> values = response.getValues();
//        if (values == null || values.isEmpty()) {
//            System.out.println("No data found.");
//        } else {
//            System.out.println(" Data");
//            for (List row : values) {
//                System.out.println(row.get(0));
//            }
//        }
//    }
    
    
    public List<String> getListAccount(){
        List<String> arrList = new ArrayList<>();
        try {
            String range = "account!A1:A10";
            ValueRange response = service.spreadsheets().values()
                    .get(spreadsheetId, range)
                    .execute();
            
            List<List<Object>> values = response.getValues();
            if (values == null || values.isEmpty()) {
                System.out.println("No data found.");
            } else {
                System.out.println(" Data");
                for (List row : values) {
                    String ac = row.get(0).toString();
                    System.out.println(ac);
                    arrList.add(ac);
                }
            }
        } catch (IOException ex) { 
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        }
        return arrList;
   }
    

    public static void updateInformationOnSheet(int row,
            String C, String D, String E, String F,
            String G, String H, String I, String J
    ) throws IOException {
        updateInforCell("C"+String.valueOf(row), "Mango");
        updateInforCell("D"+String.valueOf(row), "Mango");
        updateInforCell("E"+String.valueOf(row), "Mango");
        updateInforCell("F"+String.valueOf(row), "Mango");
        updateInforCell("G"+String.valueOf(row), "Mango");
        updateInforCell("H"+String.valueOf(row), "Mango");
        updateInforCell("I"+String.valueOf(row), "Mango");
        updateInforCell("J"+String.valueOf(row), "Mango");
    }

    private static void updateInforCell(String pos, String value) throws IOException {
        List<List<Object>> values = Arrays.asList(
                Arrays.asList(value)
        );

        ValueRange body = new ValueRange()
                .setValues(values);

        UpdateValuesResponse result
                = service.spreadsheets().values().update(spreadsheetId, pos, body)
                .setValueInputOption("RAW")
                .execute();

    }

    public void createNewItemOnSheet(String sCode) throws IOException {
        int index = getPosMaThietBi(sCode);
        if(index != -1){
            
        }
    }
    
    private static int getPosMaThietBi(String sCode) throws IOException{
        String range = "data!B1:B100";
        ValueRange response = service.spreadsheets().values()
                .get(spreadsheetId, range)
                .execute();

        List<List<Object>> values = response.getValues();
        if (values == null || values.isEmpty()) {
            System.out.println("No data found.");
        } else {
            System.out.println(" Data");
            int i = 0;
            for (List row : values) {
                i++;                
                if(row.get(0).toString().contains(sCode)) {
                    System.out.println("data: " + row.get(0) + "  row: " + i);
                    return i;
                }               
            }
        }
        return -1;
    }
}
