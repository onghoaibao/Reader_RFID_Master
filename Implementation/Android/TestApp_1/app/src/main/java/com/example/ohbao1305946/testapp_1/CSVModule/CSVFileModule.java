package com.example.ohbao1305946.testapp_1.CSVModule;

import android.os.Environment;
import android.util.Log;

import com.opencsv.CSVWriter;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Locale;

public class CSVFileModule {
    private CSVWriter writer = null;
    private String path = "/sdcard/0MedicalCSV/";
    private String csv;
    private SimpleDateFormat sdf = new SimpleDateFormat("dd_MM_yyyy_HH_mm", Locale.getDefault());
    private String currentDateandTime = sdf.format(new Date());
    private List<String[]> data = new ArrayList<>();

    public CSVFileModule(){
        csv = path + "0MedicalData_" + currentDateandTime + ".csv";
        Log.i("SHOW", "PATH: " + csv);
    }

    public void WriteCSVFile(){
        File themesDirectory = new File(path);
        if(!themesDirectory.exists()){
            themesDirectory.mkdirs();
        }
        Log.i("SHOW", "Path: " + csv);
        try {
            writer = new CSVWriter(new FileWriter(csv));
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    public void appendDataToCSV(String sData){
        if(writer != null){
            data.add(new String[]{sData});
        }
    }

    public void updateDataCSV(){
        if(writer != null){
            writer.writeAll(data);
            data.clear();
        }
    }

    public void closeCSV(){
        try {
            writer.close();
        } catch (IOException ex) {
            ex.printStackTrace();
        }
    }

}

