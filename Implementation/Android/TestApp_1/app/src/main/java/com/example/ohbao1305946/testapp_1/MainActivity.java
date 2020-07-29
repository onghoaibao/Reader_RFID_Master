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
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.IntentFilter;
import android.content.pm.PackageManager;
import android.graphics.Color;
import android.os.Build;
import android.os.Bundle;
import android.os.Handler;
import android.os.ParcelUuid;
import android.util.Log;
import android.view.Menu;
import android.view.View;
import android.widget.Button;
import android.widget.ListView;
import android.widget.TextView;
import android.widget.Toast;

import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEConnection;
import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEDevice;
import com.example.ohbao1305946.testapp_1.RFID.AdapterRFID;
import com.example.ohbao1305946.testapp_1.RFID.DeviceRFID;

import java.io.UnsupportedEncodingException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.List;
import java.util.UUID;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

public class MainActivity extends AppCompatActivity {

    private static final String TAG = "SHOW";
    private static final int REQUEST_ENABLE_BT = 1;
    private BluetoothManager mBluetoothManager;

    //Declare Object BLEDevice
    private BLEDevice bleDevice;
    private ListView lvDevice;
    private List<DeviceRFID> rfidList = new ArrayList<DeviceRFID>();;
    private AdapterRFID adapterRFID;

    // Declare mapping component
    private Button BTClear;
    private Button BTscan;
    private Button BTconnect;

    // Declare control variable
    boolean isScan = true;
    boolean isConnection = true;
    private BluetoothGatt bluetoothGatt;
    private BluetoothDevice device;
    private BluetoothGattService service;
    private BluetoothAdapter mBluetoothAdapter;
    private BluetoothGattCharacteristic characteristic;

    private static UUID SERVICE_UUID = UUID.fromString("0003CDD0-0000-1000-8000-00805F9B0131");
    private static final UUID CHARACTERISTIC_UUID_READ = UUID.fromString("0003CDD1-0000-1000-8000-00805F9B0131");
    private static final UUID CHARACTERISTIC_UUID_WRITE = UUID.fromString("0003CDD2-0000-1000-8000-00805F9B0131");

    private final String ACTION = "com.example.ohbao1305946.testapp_1.ACTION";
    private final String KEY = "BLE";


    private final String MAC_BLE = "9C:A5:25:98:F8:69";
    private boolean StatusConnect;
    private BroadcastReceiver receiver;
    private TextView tvStatus;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        initMappingComponent();
        rfidList.clear();
        adapterRFID = new AdapterRFID(MainActivity.this, R.layout.rfid_layout, rfidList);
        lvDevice.setAdapter(adapterRFID);


        initBluetoothLE();

        // Event Button Scan
        ButtonScanDeviceBLE();

        // Event Button Connect
        ButtonConnectToDevice();

        // Clear Data
        ButtonEventClearData();

        // checking function
        addDataToListView();

        // Register broadcast
        BroadReceiverBLE103();
    }

    private void BroadReceiverBLE103(){
        IntentFilter filter = new IntentFilter();
        filter.addAction(ACTION);
        receiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                //do something based on the intent's action
                switch (intent.getAction()){
                    case ACTION:
                        String data = intent.getExtras().get(KEY).toString();
                        Log.i(TAG, " --------------------- onReceive ----------------------");
                        Log.i(TAG, "Data: " + data);
                        DeviceRFID d = new DeviceRFID(data,"True",
                                "May Sieu Am 1");
                        rfidList.add(d);
                        adapterRFID.notifyDataSetChanged();
                        break;
                }
            }

        };
        registerReceiver(receiver, filter);
    }

    @Override
    protected void onDestroy() {
        if (receiver != null) {
            unregisterReceiver(receiver);
            receiver = null;
        }
        super.onDestroy();
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
        BTClear = (Button) findViewById(R.id.BTClearData);
        BTscan = (Button) findViewById(R.id.BTScan);
        BTconnect = (Button) findViewById(R.id.BTconnectCPN);
        lvDevice = (ListView) findViewById(R.id.lvDeviceRFID);
        tvStatus = (TextView) findViewById(R.id.tvStatusOn_Off);
    }

    private void ButtonEventClearData(){
        BTClear.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(!rfidList.isEmpty()) {
                    rfidList.clear();
                    adapterRFID.notifyDataSetChanged();
                }
            }
        });
    }

    private void eventStartBLEScanning(){
        if (ContextCompat.checkSelfPermission(getApplicationContext(), Manifest.permission.ACCESS_FINE_LOCATION)
                != PackageManager.PERMISSION_GRANTED) {
            Toast.makeText(getApplicationContext(), "Xin vui lòng bật quyền vị trị", Toast.LENGTH_LONG).show();
            return;
        }
        if (isScan) {
            tvStatus.setBackgroundResource(R.drawable.staus_bluetooth_online_red);
            mBluetoothAdapter.startLeScan(leScanCallback);
            Log.i(TAG, "Start Scan");
            isScan = false;
            BTscan.setText("Scanning");
        } else {
            mBluetoothAdapter.stopLeScan(leScanCallback);
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
                if(isConnection) {
                    connectToBLE();
                    BTconnect.setText("Disconnect");
                    BTClear.setEnabled(false);
                    BTscan.setEnabled(false);
                    isConnection = false;
                }
                else{
                    disconnect();
                    BTconnect.setText("Connect");
                    BTClear.setEnabled(true);
                    BTscan.setEnabled(true);
                    isConnection = true;
                }
            }
        });
    }

    private void addDataToListView(){
//        DeviceRFID deviceRFID = new DeviceRFID("E20050248611009019304F75", "True", "May Sieu Am 1");
//        rfidList.add(new DeviceRFID("E20050248611009019304F75", "Đã tìm thấy", "May Sieu Am 2"));
//        rfidList.add(new DeviceRFID("E20050248611009019304F76", "Không tìm thấy", "May Sieu Am 3"));
//        rfidList.add(new DeviceRFID("E20050248611009019304F77", "Đã tìm thấy", "May Sieu Am 4"));
//        rfidList.add(new DeviceRFID("E20050248611009019304F78", "Không tìm thấy","May Sieu Am 5"));
//        rfidList.add(new DeviceRFID("E20050248611009019304F79", "Đã tìm thấy", "May Sieu Am 6"));
//        adapterRFID = new AdapterRFID(MainActivity.this, R.layout.rfid_layout, rfidList);
//        lvDevice.setAdapter(adapterRFID);
    }

    public void connectToBLE(){
        device = mBluetoothAdapter.getRemoteDevice(MAC_BLE);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
            bluetoothGatt = device.connectGatt(MainActivity.this, false, gattCallback, BluetoothDevice.TRANSPORT_AUTO);
            Log.i(TAG, "UUID TRANSPORT_AUTO: " + bluetoothGatt.getServices().toString());

        } else {
            bluetoothGatt = device.connectGatt(MainActivity.this, false, gattCallback);
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

    public BluetoothAdapter.LeScanCallback leScanCallback = new BluetoothAdapter.LeScanCallback() {
        @Override
        public void onLeScan(final BluetoothDevice bluetoothDevice, int i, byte[] bytes) {
            runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    String dt = " Name: " + bluetoothDevice.getName() +
                            " Address: " + bluetoothDevice.getAddress();
                    String mac = bluetoothDevice.getAddress();
                    if(mac.contains(MAC_BLE)){
                        tvStatus.setBackgroundResource(R.drawable.staus_bluetooth_online);
                    }
                }
            });
        }
    };

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
                sendBroadCast(dataReadBLE);
                Log.i(TAG, "Value Decimal: " + dataReadBLE);
            }  catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
            //bluetoothGatt.setCharacteristicNotification(characteristic, true);
        }
    };

    private void sendBroadCast(String s){
        Intent intent = new Intent();
        intent.setAction(ACTION);
        Bundle bundle = new Bundle();
        bundle.putString(KEY, s);
        intent.putExtras(bundle);
        sendBroadcast(intent);
    }

}