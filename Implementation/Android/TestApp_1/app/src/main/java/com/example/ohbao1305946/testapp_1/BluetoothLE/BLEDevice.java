package com.example.ohbao1305946.testapp_1.BluetoothLE;


import android.app.Activity;
import android.app.ListActivity;
import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothManager;
import android.content.Context;
import android.content.ContextWrapper;
import android.content.Intent;
import android.util.Log;

import java.lang.reflect.Field;
import java.lang.reflect.InvocationTargetException;
import java.util.ArrayList;
import java.util.List;
import java.util.Map;

public class BLEDevice extends ListActivity {

    //  Declare variables
    private final String TAG = "SHOW";
    private BluetoothAdapter mBluetoothAdapter;
    private List<BluetoothDevice> listDevice;


    public BLEDevice(BluetoothAdapter mBluetoothAdapter) {
        this.mBluetoothAdapter = mBluetoothAdapter;
        this.listDevice = new ArrayList<>();
    }

    public void startScanDeviceBLE(){
        mBluetoothAdapter.startLeScan(leScanCallback);
    }

    public void stopScanDeviceBLE(){
        mBluetoothAdapter.stopLeScan(leScanCallback);
    }

    public List<BluetoothDevice> getListDevice(){
        return this.listDevice;
    }

    public BluetoothAdapter.LeScanCallback leScanCallback = new BluetoothAdapter.LeScanCallback() {
        @Override
        public void onLeScan(final BluetoothDevice bluetoothDevice, int i, byte[] bytes) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    String dt = " Name: " + bluetoothDevice.getName() +
                                " Address: " + bluetoothDevice.getAddress();

                    if(!listDevice.contains(bluetoothDevice)) {
                        listDevice.add(bluetoothDevice);
                        Log.i(TAG, "BLE Device: " + dt);
                    }
                }
            });
        }
    };

}
