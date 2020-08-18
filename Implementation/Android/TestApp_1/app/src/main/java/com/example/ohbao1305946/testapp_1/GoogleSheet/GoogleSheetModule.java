package com.example.ohbao1305946.testapp_1.GoogleSheet;

import android.util.Log;

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
import com.google.api.services.sheets.v4.model.ValueRange;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class GoogleSheetModule {
    private static final String APPLICATION_NAME = "Google Sheets API Java Quickstart";
    private static final JsonFactory JSON_FACTORY = JacksonFactory.getDefaultInstance();
    private static final String TOKENS_DIRECTORY_PATH = "/sdcard/0MedicalCSV/tokens";

    private static final List<String> SCOPES = Collections.singletonList(SheetsScopes.SPREADSHEETS);
    private static final String CREDENTIALS_FILE_PATH = "/sdcard/0MedicalCSV/credentials.json";

    //device
    private static Sheets service;
    private static NetHttpTransport HTTP_TRANSPORT;
    private static final String spreadsheetId = "1td6-VRHm92Tattq2xQC_3PHYRyDZIoMO8XH_dmayQB0";
    private static final String spreadsheetIdAcount = "1rdQB3aXvY7fEuqzjb0LQRnlQMyYH0HV0wn-z9c8Ho7c";

    public GoogleSheetModule(){
        try {
            HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
        } catch (GeneralSecurityException e) {
            e.printStackTrace();
        } catch (IOException e) {
            e.printStackTrace();
        }
        try {
            service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                    .setApplicationName(APPLICATION_NAME)
                    .build();
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private static Credential getCredentials(final NetHttpTransport HTTP_TRANSPORT) throws IOException {
        InputStream in = null;
        try {
            in = new FileInputStream(CREDENTIALS_FILE_PATH);
        } catch (FileNotFoundException e) {
            e.printStackTrace();
        }
        if (in == null) {
            try {
                throw new FileNotFoundException("Resource not found: " + CREDENTIALS_FILE_PATH);
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            }
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

    private int getRowInSheet() {
        int row = 0;
        try {
            String range = "data!B2:B100";
            ValueRange response = service.spreadsheets().values()
                    .get(spreadsheetId, range)
                    .execute();
            List<List<Object>> values = response.getValues();
            row = values.size();
        } catch (IOException ex) {

        }
        return row;
    }

    public List<String> getAllCodeDevice() {
        List<String> lFields = new ArrayList<>();
        try {
            HTTP_TRANSPORT = GoogleNetHttpTransport.newTrustedTransport();
            service = new Sheets.Builder(HTTP_TRANSPORT, JSON_FACTORY, getCredentials(HTTP_TRANSPORT))
                    .setApplicationName(APPLICATION_NAME)
                    .build();

            String range = "data!B2:B100";
            ValueRange response = service.spreadsheets().values()
                    .get(spreadsheetId, range)
                    .execute();

            List<List<Object>> values = response.getValues();
            if (values == null || values.isEmpty()) {
                Log.i("SHOW","No data found.");
            } else {
                Log.i("SHOW"," Data " + "  ROW=" +values.size());
                for(List<Object> obj : values){
                    Log.i("SHOW","Data sheet: " + obj.get(1));
                }
            }

        } catch (IOException ex) {
        } catch (GeneralSecurityException ex) {
        }
        return lFields;
    }
}
