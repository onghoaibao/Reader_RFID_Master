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

import com.android.volley.Request;
import com.android.volley.RequestQueue;
import com.android.volley.Response;
import com.android.volley.VolleyError;
import com.android.volley.toolbox.StringRequest;
import com.android.volley.toolbox.Volley;
import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEConnection;
import com.example.ohbao1305946.testapp_1.BluetoothLE.BLEDevice;
import com.example.ohbao1305946.testapp_1.CSVModule.CSVFileModule;
import com.example.ohbao1305946.testapp_1.GoogleSheet.GoogleSheetModule;
import com.example.ohbao1305946.testapp_1.RFID.AdapterRFID;
import com.example.ohbao1305946.testapp_1.RFID.DeviceRFID;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.UUID;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;

public class MainActivity extends AppCompatActivity {
    private boolean enableConnect = false;
    private List<String> revDataList = new ArrayList<>();

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
    private TextView TVCounter;

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
    private final String ACTION_1 = "com.example.ohbao1305946.testapp_1.CONNECTED";
    private final String KEY = "BLE";
    private final String KEY1 = "CON";
    private List<String> ListOutdDataCode = new ArrayList<>();
    private Map<String, String> mapOutData = new HashMap();

    private final String MAC_BLE = "9C:A5:25:98:F4:BB";
    private boolean StatusConnect;
    private BroadcastReceiver receiver;
    private TextView tvStatus;

    private CSVFileModule csvFileModule;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        readDataBase();

        initMappingComponent();

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
        filter.addAction(ACTION_1);
        receiver = new BroadcastReceiver() {
            @Override
            public void onReceive(Context context, Intent intent) {
                //do something based on the intent's action
                switch (intent.getAction()){
                    case ACTION:
                        //Log.i(TAG, " --------------------- onReceive ----------------------");
                        String sCode = intent.getExtras().get(KEY).toString();
                        if(!revDataList.contains(sCode) && ListOutdDataCode.contains(sCode)) {
                            revDataList.add(sCode);
                            tvStatus.setText(String.valueOf(revDataList.size()) +
                                            "/" + String.valueOf(ListOutdDataCode.size()));
                            Log.i(TAG, "Data: " + sCode);
                            DeviceRFID d = new DeviceRFID(sCode, "Có",
                                    mapOutData.get(sCode));
                            csvFileModule.appendDataToCSV(sCode, mapOutData.get(sCode), "Da Tim Thay");
                            rfidList.add(d);
                            adapterRFID.notifyDataSetChanged();
                        }
                        break;

                    case ACTION_1:
                        String data1 = intent.getExtras().get(KEY1).toString();
                        Log.i(TAG, "Device connected !!!");
                        if(data1.contains("PASS")) {
                            Toast.makeText(MainActivity.this, "Device connected !!!", Toast.LENGTH_LONG).show();
                        }
                        else if(data1.contains("DIS")){
                            Toast.makeText(MainActivity.this, "File is Saved In 0MedicalCSV Folder", Toast.LENGTH_LONG).show();
                        }
                        else{
                            Toast.makeText(MainActivity.this, "Device Can Not Connect !!!", Toast.LENGTH_LONG).show();
                        }
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

    private void sendBroadCast(String s, String action, String key){
        Intent intent = new Intent();
        intent.setAction(action);
        Bundle bundle = new Bundle();
        bundle.putString(key, s);
        intent.putExtras(bundle);
        sendBroadcast(intent);
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
        tvStatus = (TextView) findViewById(R.id.tvCouter);
        rfidList.clear();
        adapterRFID = new AdapterRFID(MainActivity.this, R.layout.rfid_layout, rfidList);
        lvDevice.setAdapter(adapterRFID);
    }

    private void ButtonEventClearData(){
        BTClear.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                if(!rfidList.isEmpty()) {
                    revDataList.clear();
                    rfidList.clear();
                    adapterRFID.notifyDataSetChanged();
                    tvStatus.setText("0/" + String.valueOf(ListOutdDataCode.size()));
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
            BTscan.setText("Scan");
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
                if(!isScan){
                    Toast.makeText(MainActivity.this, "Please stop scanning !!!",
                            Toast.LENGTH_SHORT).show();
                }
                else if(!enableConnect){
                    Toast.makeText(MainActivity.this, "Could not found the device !!!",
                            Toast.LENGTH_SHORT).show();
                }
                else{
                    if(isConnection) {
                        csvFileModule = new CSVFileModule();
                        csvFileModule.WriteCSVFile();
                        connectToBLE();
                        BTconnect.setText("Disconnect");
                        BTClear.setEnabled(false);
                        BTscan.setEnabled(false);
                        isConnection = false;
                    }
                    else{
                        disconnect();

                        for(String sCode : ListOutdDataCode){
                           if(!revDataList.contains(sCode)){
                               csvFileModule.appendDataToCSV(sCode, mapOutData.get(sCode), "Khong Tim Thay");
                           }
                        }
                        csvFileModule.updateDataCSV();
                        csvFileModule.closeCSV();
                        BTconnect.setText("Connect");
                        BTClear.setEnabled(true);
                        BTscan.setEnabled(true);
                        isConnection = true;
                    }
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
                    Log.i(TAG, "Thong tin: " + dt);
                    String mac = bluetoothDevice.getAddress();
                    if(mac.contains(MAC_BLE)){
                        tvStatus.setBackgroundResource(R.drawable.staus_bluetooth_online);
                        enableConnect = true;
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
                sendBroadCast("DIS", ACTION_1, KEY1);
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
                        sendBroadCast("PASS", ACTION_1, KEY1);
                    }else {
                        Log.d(TAG, "setCharacteristicNotification FAILURE!");
                        sendBroadCast("FAIL", ACTION_1, KEY1);
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
                if(dataReadBLE.length() == 4) {
                    sendBroadCast(dataReadBLE, ACTION, KEY);
                }
                Log.i(TAG, "Value Decimal: " + dataReadBLE);
            }  catch (UnsupportedEncodingException e) {
                e.printStackTrace();
            }
            //bluetoothGatt.setCharacteristicNotification(characteristic, true);
        }
    };



    private void readDataBase(){

        RequestQueue queue = Volley.newRequestQueue(this);
        String url ="https://script.google.com/macros/s/AKfycbw4HUZNWeNL7D15dxpteRobaDhXIFe0uNnH3abATzkXAl_aZz0/exec";

// Request a string response from the provided URL.
        StringRequest stringRequest = new StringRequest(Request.Method.POST, url,
                new Response.Listener<String>() {
                    @Override
                    public void onResponse(String response) {
                        // Display the first 500 characters of the response string.
                        Log.i("SHOW","Response is: "+ response);
                        try {
                            JSONObject obj = new JSONObject(response);
                            JSONArray jsonArrayCode = obj.getJSONArray("DataCode");
                            JSONArray jsonArrayName = obj.getJSONArray("DataName");

                            for(int i=0; i<jsonArrayName.length(); i++) {
                                Log.i("SHOW", "Code: " + jsonArrayCode.getString(i) +
                                                        " - Name: " + jsonArrayName.getString(i));
                                ListOutdDataCode.add(jsonArrayCode.getString(i));
                                mapOutData.put(jsonArrayCode.getString(i), jsonArrayName.getString(i));
                            }
                            tvStatus.setText("0/" + String.valueOf(jsonArrayCode.length()));
                        } catch (JSONException e) {
                            e.printStackTrace();
                        }

                    }
                }, new Response.ErrorListener() {
            @Override
            public void onErrorResponse(VolleyError error) {
                Log.i("SHOW","cannot work !!");
            }
        });
        queue.add(stringRequest);
    }

}