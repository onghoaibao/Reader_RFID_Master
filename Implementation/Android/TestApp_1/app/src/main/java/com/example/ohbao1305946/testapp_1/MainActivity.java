package com.example.ohbao1305946.testapp_1;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.Manifest;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattServer;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothManager;
import android.bluetooth.BluetoothProfile;
import android.bluetooth.le.BluetoothLeScanner;
import android.bluetooth.le.ScanCallback;
import android.bluetooth.le.ScanFilter;
import android.bluetooth.le.ScanResult;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.os.ParcelUuid;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEConnection;
import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEDevice;

import java.io.UnsupportedEncodingException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.UUID;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "SHOW";
    private static final int REQUEST_ENABLE_BT = 1;
    private BluetoothAdapter mBluetoothAdapter;
    private BluetoothManager mBluetoothManager;

    //Declare Object BLEDevice
    private BLEDevice bleDevice;

    // Declare mapping component
    private Button BTenable;
    private Button BTscan;
    private Button BTconnect;

    // Declare control variable
    boolean isScan = true;
    boolean isConnection = true;
    BLEConnection bleConnection;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        initMappingComponent();
        initBluetoothLE();

        // Event Button Scan
        ButtonScanDeviceBLE();

        // Event Button Connect
        ButtonConnectToDevice();

    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return super.onCreateOptionsMenu(menu);
    }

    private void initBluetoothLE(){
        mBluetoothAdapter = BluetoothAdapter.getDefaultAdapter();
        if (!mBluetoothAdapter.isEnabled()) {
            Intent enableBtIntent = new Intent(BluetoothAdapter.ACTION_REQUEST_ENABLE);
            startActivityForResult(enableBtIntent, REQUEST_ENABLE_BT);
        }
        mBluetoothManager = (BluetoothManager)
                getSystemService(Context.BLUETOOTH_SERVICE);
        mBluetoothAdapter = mBluetoothManager.getAdapter();
        bleDevice = new BLEDevice(mBluetoothAdapter);
    }

    private void initMappingComponent(){
        BTenable = (Button) findViewById(R.id.BTenableCPN);
        BTscan = (Button) findViewById(R.id.BTstartCPN);
        BTconnect = (Button) findViewById(R.id.BTconnectCPN);
    }

    private void eventStartBLEScanning(){
        if (ContextCompat.checkSelfPermission(getApplicationContext(), Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {
            Toast.makeText(getApplicationContext(), "Xin vui lòng bật quyền vị trị", Toast.LENGTH_LONG).show();
            return;
        }
        if (isScan) {
            bleDevice.startScanDeviceBLE();
            Log.i(TAG, "Start Scan");
            isScan = false;
            BTscan.setText("Scanning");
        } else {
            bleDevice.stopScanDeviceBLE();
            Log.i(TAG, "Stop Scan");
            isScan = true;
            BTscan.setText("Start");
        }
    }

    private void ButtonScanDeviceBLE(){
        BTscan.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                eventStartBLEScanning();
            }
        });
    }

    private void ButtonConnectToDevice(){
        BTconnect.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                final String addr = "C3:90:A1:3E:AA:B6";
                if(isConnection) {
                    bleConnection = new BLEConnection(addr, MainActivity.this, mBluetoothAdapter);
                    bleConnection.connectToBLE();
                    isConnection = false;
                }
                else{
                    bleConnection.disconnect();
                    isConnection = true;
                }
            }
        });
    }

}