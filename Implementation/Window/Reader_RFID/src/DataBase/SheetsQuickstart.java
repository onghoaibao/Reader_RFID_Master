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
import com.google.api.services.sheets.v4.model.BatchUpdateSpreadsheetRequest;
import com.google.api.services.sheets.v4.model.DeleteDimensionRequest;
import com.google.api.services.sheets.v4.model.DimensionRange;
import com.google.api.services.sheets.v4.model.Request;
import com.google.api.services.sheets.v4.model.UpdateValuesResponse;
import com.google.api.services.sheets.v4.model.ValueRange;
import java.io.FileInputStream;

import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.security.GeneralSecurityException;
import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.Collections;
import java.util.Date;
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
    private static final String spreadsheetId = "1td6-VRHm92Tattq2xQC_3PHYRyDZIoMO8XH_dmayQB0";

    private static final String spreadsheetIdAcount = "1rdQB3aXvY7fEuqzjb0LQRnlQMyYH0HV0wn-z9c8Ho7c";

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
//        
//        getDeviceInformation();
//        HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
//        service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
//                .setApplicationName(APPLICATION_NAME)
//                .build();
//        int index = getPosMaThietBi("ABCDEF5");
//
//        updateInformationOnSheet(index + 15,"", "", "", "", "Mango", "Mango", "Mango", "Mango");
//
//        String range = "data!A2:j100";
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
//                for (int i = 0; i < row.size(); i++) {
//                    if (row.get(i) != null) {
//                        System.out.print(row.get(i) + "  ");
//                    }
//                }
//                System.out.println();
//            }
//        }
//    }
    private int getRowInSheet() {
        int row = 0;
        try {
//            HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
//            service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
//                    .setApplicationName(APPLICATION_NAME)
//                    .build();

            String range = "data!A2:J100";
            ValueRange response = service.spreadsheets().values()
                    .get(spreadsheetId, range)
                    .execute();
            List<List<Object>> values = response.getValues();
            row = values.size();
        } catch (IOException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        }
        return row;
    }

    public List<Fields> getDeviceInformation() {
        List<Fields> lFields = new ArrayList<>();
        try {
            HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
            service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                    .setApplicationName(APPLICATION_NAME)
                    .build();

            String range = "data!A2:J100";
            ValueRange response = service.spreadsheets().values()
                    .get(spreadsheetId, range)
                    .execute();

            List<List<Object>> values = response.getValues();
            if (values == null || values.isEmpty()) {
                System.out.println("No data found.");
            } else {
                System.out.println(" Data " + "  ROW=" +values.size());
                for (List row : values) {
                    Fields fields = new Fields();
                    fields.setField1(row.get(0).toString());
                    fields.setField2(row.get(3).toString());
                    fields.setField3(row.get(4).toString());
                    fields.setField4(row.get(5).toString());
                    fields.setField5(row.get(6).toString());
                    fields.setField6(row.get(7).toString());
                    fields.setField7(row.get(8).toString());
                    fields.setField8(row.get(9).toString());
                    fields.setMTB(row.get(1).toString());
                    System.out.println();
                    lFields.add(fields);
                }

//                for (Fields f : lFields) {
//                    System.out.println("f1: " + f.getField1());
//                    System.out.println("f2: " + f.getField2());
//                    System.out.println("f3: " + f.getField3());
//                    System.out.println("f4: " + f.getField4());
//                    System.out.println("f5: " + f.getField5());
//                    System.out.println("f6: " + f.getField6());
//                    System.out.println("f7: " + f.getField7());
//                    System.out.println("f8: " + f.getField8());
//                    System.out.println("fMTB: " + f.getMTB());
//                    System.out.println("------------------");
//                }
            }

        } catch (IOException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        } catch (GeneralSecurityException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        }
        return lFields;
    }

    public List<String> getListAccount() {
        List<String> arrList = new ArrayList<>();
        try {
            HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
            service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                    .setApplicationName(APPLICATION_NAME)
                    .build();

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
                    //System.out.println(ac);
                    arrList.add(ac);
                }
            }
        } catch (IOException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        } catch (GeneralSecurityException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
        }
        return arrList;
    }

    public void updateInformationOnSheet(int rowMTB,
            String MTB, String TTTB, String NSX, String TTHD,
            String BR, String RT, String LSDC, String POS, String isKHO
    ) throws IOException {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("dd/MM/yyyy");
        LocalDateTime now = LocalDateTime.now();
        updateInforCell("A" + String.valueOf(rowMTB), String.valueOf(rowMTB - 1));
        updateInforCell("B" + String.valueOf(rowMTB), MTB);
        updateInforCell("C" + String.valueOf(rowMTB), String.valueOf(dtf.format(now)));
        updateInforCell("D" + String.valueOf(rowMTB), TTTB);
        updateInforCell("E" + String.valueOf(rowMTB), NSX);
        updateInforCell("F" + String.valueOf(rowMTB), TTHD);
        updateInforCell("G" + String.valueOf(rowMTB), BR);
        updateInforCell("H" + String.valueOf(rowMTB), RT);
        updateInforCell("I" + String.valueOf(rowMTB), LSDC);
        updateInforCell("J" + String.valueOf(rowMTB), POS);
        updateInforCell("K" + String.valueOf(rowMTB), isKHO);
    }

    private void updateNumberID(int row){
        int tr = getRowInSheet() + 1;
        for(int i = row; i < tr + 1; i++){
            try {
                System.out.println("delete row: " + i);
                updateInforCell("A" + String.valueOf(i), String.valueOf(i - 1));
            } catch (IOException ex) {
                Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
            }
        }       
    }
    
    public boolean editInformationOnSheet(
            String MTB, String TTTB, String NSX, String TTHD,
            String BR, String RT, String LSDC, String POS, String isKHO
    ) throws IOException {
        DateTimeFormatter dtf = DateTimeFormatter.ofPattern("dd/MM/yyyy");
        LocalDateTime now = LocalDateTime.now();

        int rowMTB = getPosMaThietBi(MTB) + 1;
        System.out.println("rowMTB: " + rowMTB);
        if (rowMTB != -1) {
            updateInforCell("A" + String.valueOf(rowMTB), String.valueOf(rowMTB - 1));
            updateInforCell("B" + String.valueOf(rowMTB), MTB);
            updateInforCell("C" + String.valueOf(rowMTB), String.valueOf(dtf.format(now)));
            updateInforCell("D" + String.valueOf(rowMTB), TTTB);
            updateInforCell("E" + String.valueOf(rowMTB), NSX);
            updateInforCell("F" + String.valueOf(rowMTB), TTHD);
            updateInforCell("G" + String.valueOf(rowMTB), BR);
            updateInforCell("H" + String.valueOf(rowMTB), RT);
            updateInforCell("I" + String.valueOf(rowMTB), LSDC);
            updateInforCell("J" + String.valueOf(rowMTB), POS);
            //updateInforCell("K" + String.valueOf(rowMTB), isKHO);
            return true;
        } else {
            return false;
        }
    }

    public boolean deleteRowInSheet(String mtb) {
        try {
            int row = getPosMaThietBi(mtb);
            System.out.println("Delete row: " + row);
            Request request = new Request()
                    .setDeleteDimension(new DeleteDimensionRequest()
                            .setRange(new DimensionRange()
                                    .setSheetId(0)
                                    .setDimension("ROWS")
                                    .setStartIndex(row)
                                    .setEndIndex(row+ 1)
                            )
                    );

            List<Request> requests = new ArrayList<>();
            requests.add(request);

            BatchUpdateSpreadsheetRequest content = new BatchUpdateSpreadsheetRequest();
            content.setRequests(requests);
            service.spreadsheets().batchUpdate(spreadsheetId, content).execute();
            
            updateNumberID(row);

        } catch (IOException ex) {
            Logger.getLogger(SheetsQuickstart.class.getName()).log(Level.SEVERE, null, ex);
            return false;
        }
        return true;
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

    public void createNewItemOnSheet(String MTB, String TTTB, String NSX,
            String TTHD)
            throws IOException {
        int row = getRowInSheet() + 2;
        updateInformationOnSheet(row, MTB, TTTB, NSX, TTHD,
                "Khong", "Khong", "Khong", "Kho", "True");

    }

    private static int getPosMaThietBi(String sCode) throws IOException {
        String range = "data!B2:B100";
        ValueRange response = service.spreadsheets().values()
                .get(spreadsheetId, range)
                .execute();

        List<List<Object>> values = response.getValues();
        if (values == null || values.isEmpty()) {
            System.out.println("No data found.");
        } else {
            try {
                System.out.println(" Data");
                int i = 0;
                for (List row : values) {
                    i++;
                    if (row.get(0).toString().contains(sCode)
                            || sCode.contains(row.get(0).toString())) {
                        System.out.println("data: " + row.get(0) + "  row: " + i);
                        return i;
                    }
                }
            } catch (IndexOutOfBoundsException e) {
                return -1;
            }
        }
        return -1;
    }
}
