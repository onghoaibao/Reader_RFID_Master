package com.example.ohbao1305946.testapp_1.RFID;

public class DeviceRFID {

    private String nameDevice;
    private String maDevice;
    private String statusDevice;

    public DeviceRFID(String maDevice, String statusDevice, String nameDevice) {
        this.maDevice = maDevice;
        this.nameDevice = nameDevice;
        this.statusDevice = statusDevice;
    }


    public String getMaDevice() {
        return maDevice;
    }

    public void setMaDevice(String maDevice) {
        this.maDevice = maDevice;
    }

    public String getNameDevice() {
        return nameDevice;
    }

    public void setNameDevice(String nameDevice) {
        this.nameDevice = nameDevice;
    }

    public String getStatusDevice() {
        return statusDevice;
    }

    public void setStatusDevice(String statusDevice) {
        this.statusDevice = statusDevice;
    }
}
