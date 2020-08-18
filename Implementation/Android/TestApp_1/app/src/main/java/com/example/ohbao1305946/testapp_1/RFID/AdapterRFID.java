package com.example.ohbao1305946.testapp_1.RFID;

import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;
import android.widget.TextView;

import com.example.ohbao1305946.testapp_1.R;

import java.util.List;

public class AdapterRFID extends BaseAdapter {

    private Context myContext;
    private int myLayout;
    List<DeviceRFID> myDeviceRFIDList;

    public AdapterRFID(Context context, int layout, List<DeviceRFID> deviceRFIDList){
        this.myContext = context;
        this.myLayout  = layout;
        this.myDeviceRFIDList = deviceRFIDList;
    }

    @Override
    public int getCount() {
        return myDeviceRFIDList.size();
    }

    @Override
    public Object getItem(int i) {
        return null;
    }

    @Override
    public long getItemId(int i) {
        return 0;
    }

    @Override
    public View getView(int position, View view, ViewGroup viewGroup) {
        ViewHolder viewHolder;
        if(view == null){
            LayoutInflater inflater = (LayoutInflater) myContext.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
            view = inflater.inflate(R.layout.rfid_layout, null);

            viewHolder = new ViewHolder();
            viewHolder.matv = (TextView) view.findViewById(R.id.textView_MaThietBi);
            viewHolder.statustv = (TextView) view.findViewById(R.id.textView_status);
            viewHolder.nametv = (TextView) view.findViewById(R.id.textView_name);
            view.setTag(viewHolder);
        }
        else{
            viewHolder = (ViewHolder)view.getTag();
        }


        String sMTB = myDeviceRFIDList.get(position).getMaDevice();
        viewHolder.matv.setText("Mã thiết bị: " + sMTB);
        viewHolder.nametv.setText("Tên thiết bị: " + myDeviceRFIDList.get(position).getNameDevice());
        viewHolder.statustv.setText("Trong Kho: " + myDeviceRFIDList.get(position).getStatusDevice());
        // Mapping component
//        TextView matv = (TextView) view.findViewById(R.id.textView_MaThietBi);
//        TextView statustv = (TextView) view.findViewById(R.id.textView_status);
//        TextView nametv = (TextView) view.findViewById(R.id.textView_name);

        // assign value


        return view;
    }

    private class ViewHolder{
        TextView matv;
        TextView statustv;
        TextView nametv;
    }


}
