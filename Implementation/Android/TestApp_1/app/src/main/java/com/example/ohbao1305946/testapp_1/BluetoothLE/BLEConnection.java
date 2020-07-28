package com.example.ohbao1305946.testapp_1.BluetoothLE;

import android.bluetooth.BluetoothAdapter;
import android.bluetooth.BluetoothDevice;
import android.bluetooth.BluetoothGatt;
import android.bluetooth.BluetoothGattCallback;
import android.bluetooth.BluetoothGattCharacteristic;
import android.bluetooth.BluetoothGattDescriptor;
import android.bluetooth.BluetoothGattService;
import android.bluetooth.BluetoothProfile;
import android.content.Context;
import android.content.Intent;
import android.os.Build;
import android.os.Bundle;
import android.util.Log;
import android.widget.ListView;

import com.example.ohbao1305946.testapp_1.MainActivity;
import com.example.ohbao1305946.testapp_1.R;
import com.example.ohbao1305946.testapp_1.RFID.AdapterRFID;
import com.example.ohbao1305946.testapp_1.RFID.DeviceRFID;

import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;

public class BLEConnection {

    //Declare control variables
    private BluetoothGatt bluetoothGatt;
    private BluetoothDevice device;
    private BluetoothGattService service;
    private BluetoothAdapter mBluetoothAdapter;
    private BluetoothGattCharacteristic characteristic;

    private static UUID SERVICE_UUID = UUID.fromString("0003CDD0-0000-1000-8000-00805F9B0131");
    private static final UUID CHARACTERISTIC_UUID_READ = UUID.fromString("0003CDD1-0000-1000-8000-00805F9B0131");
    private static final UUID CHARACTERISTIC_UUID_WRITE = UUID.fromString("0003CDD2-0000-1000-8000-00805F9B0131");

    private String MAC_BLE;
    private Context sThis;
    private static final String TAG = "SHOW";
    private boolean StatusConnect;
    private ListView lv;
    private AdapterRFID adapterRFID;
    private List<DeviceRFID> rfidListDevice = new ArrayList<>();

    public BLEConnection(String macBLE, Context sThis, BluetoothAdapter mBluetoothAdapter, ListView lvDevice) {
        this.MAC_BLE  = macBLE;
        this.sThis = sThis;
        this.mBluetoothAdapter = mBluetoothAdapter;
        this.lv = lvDevice;
        adapterRFID = new AdapterRFID(sThis, R.layout.rfid_layout, rfidListDevice);
        lv.setAdapter(adapterRFID);
    }

    public void connectToBLE(){
        device = mBluetoothAdapter.getRemoteDevice(MAC_BLE);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            bluetoothGatt = device.connectGatt(sThis, false, gattCallback, BluetoothDevice.TRANSPORT_AUTO);
            Log.i(TAG, "UUID TRANSPORT_AUTO: " + bluetoothGatt.getServices().toString());

        } else {
            bluetoothGatt = device.connectGatt(sThis, false, gattCallback);
            Log.i(TAG, "UUID: " + bluetoothGatt.getServices().toString());
        }
    }

    public void disconnect() {
        if (bluetoothGatt == null) {
            return;
        }
        bluetoothGatt.disconnect();
        bluetoothGatt = null;
    }

    private final BluetoothGattCallback gattCallback = new BluetoothGattCallback() {
        @Override
        public void onConnectionStateChange(BluetoothGatt gatt, int status, int newState) {
            Log.i(TAG, "onConnectionStateChange: " + String.valueOf(newState));
            if (newState == BluetoothProfile.STATE_CONNECTED) {
                gatt.discoverServices();
                StatusConnect = true;
                Log.i(TAG, "Conneted: " + String.valueOf(StatusConnect));

            } else if (newState == BluetoothProfile.STATE_DISCONNECTED) {
                StatusConnect = false;
                Log.i(TAG, "Disconnected ");
                gatt.close();
            }
        }

        @Override
        public void onServicesDiscovered(BluetoothGatt gatt, int status) {
            // super.onServicesDiscovered(gatt, status);
            if (status == BluetoothGatt.GATT_SUCCESS) {
                service = gatt.getService(SERVICE_UUID);
                Log.i(TAG, "service received: " + service.getUuid().toString());

                if (service != null) {
                    // do something
                    characteristic = service.getCharacteristic(CHARACTERISTIC_UUID_READ);
                    Log.i(TAG, "characteristic received: " + characteristic.getUuid().toString());
                    gatt.setCharacteristicNotification(characteristic, true);

                    if (gatt.setCharacteristicNotification(characteristic, true) == true) {
                        Log.i(TAG, "setCharacteristicNotification SUCCESS !");
                    }else {
                        Log.d(TAG, "setCharacteristicNotification FAILURE!");
                    }
                    BluetoothGattDescriptor descriptor = characteristic.getDescriptors().get(0);
                    if (0 != (characteristic.getProperties() & BluetoothGattCharacteristic.PROPERTY_INDICATE)) {
                        // It's an indicate characteristic
                        Log.d(TAG, "Characteristic (" + characteristic.getUuid() + ") is INDICATE");
                        if (descriptor != null) {
                            descriptor.setValue(BluetoothGattDescriptor.ENABLE_INDICATION_VALUE);
                            gatt.writeDescriptor(descriptor);
                        }
                    } else {
                        // It's a notify characteristic
                        Log.d(TAG, "Characteristic (" + characteristic.getUuid() + ") is NOTIFY");
                        if (descriptor != null) {
                            descriptor.setValue(BluetoothGattDescriptor.ENABLE_NOTIFICATION_VALUE);
                            gatt.writeDescriptor(descriptor);
                        }
                    }

                    gatt.readCharacteristic(characteristic);
                    Log.i(TAG, "Succesfully descriptor: " + descriptor.getValue().toString());
                    Log.i(TAG, "Succesfully received: " + status);
                }
            } else {
                Log.i(TAG, "Failed received: " + status);
            }
        }

        @Override
        public void onCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            //super.onCharacteristicRead(gatt, characteristic, status);
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.i(TAG, "Succesfully read characteristic: " + characteristic.toString());
                bluetoothGatt.setCharacteristicNotification(characteristic, true);

            } else {
                Log.i(TAG, "Characteristic read not successful");
            }
        }

        @Override
        public void onCharacteristicWrite(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, int status) {
            if (status == BluetoothGatt.GATT_SUCCESS) {
                Log.i(TAG, "Succesfully Write characteristic: " + characteristic);
            } else {
                Log.i(TAG, "Characteristic Write not successful");
            }
        }

        @Override
        public void onCharacteristicChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic) {
            final byte[] data = characteristic.getValue();
            try {
                String dataReadBLE =  new String(data, "UTF-8");
                rfidListDevice.add(new DeviceRFID(dataReadBLE, "Đã tìm thấy", "May Sieu Am 2"));
                adapterRFID.notifyDataSetChanged();
                Log.i(TAG, "Value Decimal: " + dataReadBLE);
            }  catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
            bluetoothGatt.setCharacteristicNotification(characteristic, true);
        }
    };

    public void writeCharacteristic(String data) {
        if (mBluetoothAdapter == null || bluetoothGatt == null) {
            Log.w(TAG, "BluetoothAdapter not initialized");
            return;
        }
        BluetoothGattService mCustomService = bluetoothGatt.getService(SERVICE_UUID);
        BluetoothGattCharacteristic mWriteCharacteristic = mCustomService.getCharacteristic(CHARACTERISTIC_UUID_WRITE);

        byte[] bdata = data.getBytes();
        mWriteCharacteristic.setValue(bdata);
        // TODO
        boolean st = bluetoothGatt.writeCharacteristic(mWriteCharacteristic);
    }

}
