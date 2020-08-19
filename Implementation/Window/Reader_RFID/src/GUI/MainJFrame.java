/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package GUI;

import DataBase.Fields;
import java.awt.Color;
import java.awt.Dimension;
import java.util.Enumeration;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.TableColumn;
import javax.swing.table.TableModel;
import javax.swing.table.TableRowSorter;
import DataBase.RequestGETDataBase;
import DataBase.SheetsQuickstart;
import MultiTable.MultiLineHeaderRenderer;
import MultiTable.MultiLineTableCellRenderer;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.IOException;
import java.net.MalformedURLException;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import reader_rfid.ThemThietBiMoi;

/**
 *
 * @author bao13
 */
public class MainJFrame extends javax.swing.JFrame {

    /**
     * Creates new form MainJFrame
     */
    private static JTable jTableShowDataBase;
    private static int ii = 1;
    private JScrollPane scrollpane;
    private static DefaultTableModel dm;

    public static ThemThietBiMoi bAddDevice = new ThemThietBiMoi();
    public static AddNewDeviceGUI addNewDeviceGUI = new AddNewDeviceGUI();
    public static EditDeviceGUI editDeviceGUI = new EditDeviceGUI();
    int iRow = 0;

    private final LoggingGUI loggingGUI;
    private SheetsQuickstart quickstart;
    private List<String> lAccount = new ArrayList<>();
    public MainJFrame() {

        getContentPane().setBackground(Color.WHITE);
        scrollpane = new JScrollPane(jTableData);
        initComponents();
        setExtendedState(this.MAXIMIZED_BOTH);
        eventWindowClose();   
        try {
            quickstart = new SheetsQuickstart();
            lAccount = quickstart.getListAccount();
        } catch (GeneralSecurityException ex) {
            Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IOException ex) {
            Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
        }
        
        loggingGUI = new LoggingGUI(lAccount);
        loggingGUI.setAlwaysOnTop(true);
        loggingGUI.setVisible(true);
        jPanel1Modify.setVisible(false);

        jButtonChinhSua.setBackground(Color.green);
        jButtonThemMoi.setBackground(Color.green);
        jButtonXoa.setBackground(Color.green);

        setTitle("Giam Sat Thiet Bi Y Te");
        configTable();
        clearAllRow();
        loadtable();
        System.out.println();

    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {
        java.awt.GridBagConstraints gridBagConstraints;

        jPanel1 = new javax.swing.JPanel();
        jPanelTable = new javax.swing.JPanel();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTableData = new javax.swing.JTable();
        jPanel1Modify = new javax.swing.JPanel();
        jButtonThemMoi = new javax.swing.JButton();
        jButtonChinhSua = new javax.swing.JButton();
        jButtonXoa = new javax.swing.JButton();
        jPanel2 = new javax.swing.JPanel();
        jButtonReload = new javax.swing.JButton();
        jLabelTitle = new javax.swing.JLabel();

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 100, Short.MAX_VALUE)
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGap(0, 100, Short.MAX_VALUE)
        );

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setBackground(new java.awt.Color(255, 255, 255));

        jPanelTable.setBackground(new java.awt.Color(255, 255, 255));
        jPanelTable.setBorder(javax.swing.BorderFactory.createEmptyBorder(1, 1, 1, 1));

        jTableData.setBorder(javax.swing.BorderFactory.createLineBorder(new java.awt.Color(0, 0, 0)));
        jTableData.setModel(new javax.swing.table.DefaultTableModel(
            new Object [][] {
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null},
                {null, null, null, null, null, null, null, null}
            },
            new String [] {
                "Title 1", "Title 2", "Title 3", "Title 4", "Title 5", "Title 6", "Title 7", "Title 8"
            }
        ));
        jTableData.setGridColor(new java.awt.Color(255, 255, 255));
        jScrollPane1.setViewportView(jTableData);

        jPanel1Modify.setBackground(new java.awt.Color(254, 254, 254));
        jPanel1Modify.setBorder(javax.swing.BorderFactory.createEmptyBorder(1, 1, 1, 1));
        jPanel1Modify.setLayout(new java.awt.GridBagLayout());

        jButtonThemMoi.setBackground(new java.awt.Color(4, 174, 1));
        jButtonThemMoi.setFont(new java.awt.Font("Arial", 1, 16)); // NOI18N
        jButtonThemMoi.setText("Tạo mới");
        jButtonThemMoi.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonThemMoiActionPerformed(evt);
            }
        });
        gridBagConstraints = new java.awt.GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 0;
        gridBagConstraints.ipadx = 79;
        gridBagConstraints.ipady = 5;
        gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
        gridBagConstraints.insets = new java.awt.Insets(115, 1, 0, 1);
        jPanel1Modify.add(jButtonThemMoi, gridBagConstraints);

        jButtonChinhSua.setBackground(new java.awt.Color(248, 249, 248));
        jButtonChinhSua.setFont(new java.awt.Font("Arial", 1, 16)); // NOI18N
        jButtonChinhSua.setText("Cập nhật");
        jButtonChinhSua.setVerifyInputWhenFocusTarget(false);
        jButtonChinhSua.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonChinhSuaActionPerformed(evt);
            }
        });
        gridBagConstraints = new java.awt.GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 1;
        gridBagConstraints.ipadx = 73;
        gridBagConstraints.ipady = 6;
        gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
        gridBagConstraints.insets = new java.awt.Insets(18, 1, 0, 1);
        jPanel1Modify.add(jButtonChinhSua, gridBagConstraints);

        jButtonXoa.setFont(new java.awt.Font("Arial", 1, 16)); // NOI18N
        jButtonXoa.setText("Xóa");
        jButtonXoa.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonXoaActionPerformed(evt);
            }
        });
        gridBagConstraints = new java.awt.GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 2;
        gridBagConstraints.ipadx = 111;
        gridBagConstraints.ipady = 6;
        gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
        gridBagConstraints.insets = new java.awt.Insets(18, 1, 210, 1);
        jPanel1Modify.add(jButtonXoa, gridBagConstraints);

        jPanel2.setBackground(new java.awt.Color(255, 255, 255));
        jPanel2.setLayout(new java.awt.GridBagLayout());

        jButtonReload.setBackground(new java.awt.Color(51, 255, 0));
        jButtonReload.setFont(new java.awt.Font("Monospaced", 1, 18)); // NOI18N
        jButtonReload.setText("Reload");
        jButtonReload.setPreferredSize(new java.awt.Dimension(63, 12));
        jButtonReload.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonReloadActionPerformed(evt);
            }
        });
        gridBagConstraints = new java.awt.GridBagConstraints();
        gridBagConstraints.gridx = 0;
        gridBagConstraints.gridy = 0;
        gridBagConstraints.ipadx = 207;
        gridBagConstraints.ipady = 24;
        gridBagConstraints.anchor = java.awt.GridBagConstraints.NORTHWEST;
        gridBagConstraints.insets = new java.awt.Insets(11, 315, 19, 313);
        jPanel2.add(jButtonReload, gridBagConstraints);

        javax.swing.GroupLayout jPanelTableLayout = new javax.swing.GroupLayout(jPanelTable);
        jPanelTable.setLayout(jPanelTableLayout);
        jPanelTableLayout.setHorizontalGroup(
            jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanelTableLayout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(jPanelTableLayout.createSequentialGroup()
                        .addComponent(jPanel2, javax.swing.GroupLayout.DEFAULT_SIZE, 1018, Short.MAX_VALUE)
                        .addGap(37, 37, 37))
                    .addGroup(jPanelTableLayout.createSequentialGroup()
                        .addComponent(jScrollPane1)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)))
                .addComponent(jPanel1Modify, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
        );
        jPanelTableLayout.setVerticalGroup(
            jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanelTableLayout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jScrollPane1)
                    .addComponent(jPanel1Modify, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addGap(18, 18, 18)
                .addComponent(jPanel2, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap())
        );

        jLabelTitle.setBackground(new java.awt.Color(0, 204, 102));
        jLabelTitle.setFont(new java.awt.Font("Monospaced", 1, 36)); // NOI18N
        jLabelTitle.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        jLabelTitle.setText("Chương Trình Quản Lí Thiết Bị Y Tế");
        jLabelTitle.setOpaque(true);

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jPanelTable, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addComponent(jLabelTitle, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jLabelTitle, javax.swing.GroupLayout.PREFERRED_SIZE, 90, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jPanelTable, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addContainerGap())
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonThemMoiActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonThemMoiActionPerformed
        // TODO add your handling code here:
        ii++;
        jTableData.setValueAt(String.valueOf(ii), 0, 0);
        addNewDeviceGUI.iStatus = 0;
        addNewDeviceGUI.clearText();
        addNewDeviceGUI.setVisible(true);
        reloadJTable();
    }//GEN-LAST:event_jButtonThemMoiActionPerformed
    int y = 0;
    private void jButtonChinhSuaActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonChinhSuaActionPerformed
        // TODO add your handling code here:
        int row = jTableData.getSelectedRow();
        int col = jTableData.getSelectedColumnCount();
        System.out.println("selected row: " + String.valueOf(row)
                + "  selected col: " + String.valueOf(col));
        if (row != -1 && col != -1) {
            String s = "";
            for (int i = 0; i < 8; i++) {
                s = jTableData.getValueAt(row, i).toString();
                System.out.println("row: " + String.valueOf(i) + "=" + s);
            }
            //setContentInfo();
            editDeviceGUI.setContentInfo(jTableData.getValueAt(row, 1).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 2).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 3).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 4).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 5).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 6).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 7).toString().replace("\n", ","),
                    jTableData.getValueAt(row, 8).toString().replace("\n", ","));
            editDeviceGUI.setVisible(true);
            //System.out.print(s.replace("/n", ""));
        } else {
            JOptionPane.showMessageDialog(null, "Chọn một hàng để chỉnh sữa ", "Error", JOptionPane.WARNING_MESSAGE);
        }
    }//GEN-LAST:event_jButtonChinhSuaActionPerformed

    private void clearAllRow() {
        y = 0;
        System.out.println("row: " + dm.getRowCount());
        for (int i = dm.getRowCount() - 1; i >= 0; i--) {
            dm.removeRow(i);
        }
        //jTableData.repaint();
    }

    //Reload table
    private void jButtonReloadActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonReloadActionPerformed
        try {
            clearAllRow();
            SheetsQuickstart quickstart = new SheetsQuickstart();
            try {
                quickstart = new SheetsQuickstart();
            } catch (GeneralSecurityException ex) {
                Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
            } catch (IOException ex) {
                Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
            }
            List<Fields> lField = quickstart.getDeviceInformation();
            for (Fields f : lField) {
                setRowDataToTable(y, f.getMTB(),
                        f.getField1(),
                        f.getField2(),
                        f.getField3(),
                        f.getField4(),
                        f.getField5(),
                        f.getField6(),
                        f.getField7(),
                        f.getField8());
                y++;
            }
        } catch (GeneralSecurityException ex) {
            Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IOException ex) {
            Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
        }
    }//GEN-LAST:event_jButtonReloadActionPerformed

    // Xoa row in the sheet
    private void jButtonXoaActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonXoaActionPerformed
        int row = jTableData.getSelectedRow();
        int col = jTableData.getSelectedColumnCount();
        System.out.println("selected row: " + String.valueOf(row)
                + "  selected col: " + String.valueOf(col));
        if (row != -1 && col != -1) {
            try {
                String s = "";
                for (int i = 0; i < 8; i++) {
                    s = jTableData.getValueAt(row, i).toString();
                    System.out.println("row: " + String.valueOf(i) + "=" + s);
                }
                SheetsQuickstart sheetAPI = new SheetsQuickstart();
                boolean b = sheetAPI.deleteRowInSheet(jTableData.getValueAt(row, 1).toString());
                if (!b) {
                    JOptionPane.showMessageDialog(null, "Chọn hàng cần xóa", "Error", JOptionPane.ERROR_MESSAGE);
                }              
            } catch (GeneralSecurityException | IOException ex) {
                Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
            }
        } else {
            JOptionPane.showMessageDialog(null, "Chọn một hàng để chỉnh sữa ", "Error", JOptionPane.WARNING_MESSAGE);
        }
    }//GEN-LAST:event_jButtonXoaActionPerformed

    /**
     * @param args the command line arguments
     */
    public static void main(String args[]) {
        /* Set the Nimbus look and feel */
        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
         */
        try {
            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
                if ("Nimbus".equals(info.getName())) {
                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
                    break;
                }
            }
        } catch (ClassNotFoundException ex) {
            java.util.logging.Logger.getLogger(MainJFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (InstantiationException ex) {
            java.util.logging.Logger.getLogger(MainJFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (IllegalAccessException ex) {
            java.util.logging.Logger.getLogger(MainJFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
            java.util.logging.Logger.getLogger(MainJFrame.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
        }
        //</editor-fold>

        /* Create and display the form */
        java.awt.EventQueue.invokeLater(new Runnable() {
            public void run() {
                new MainJFrame().setVisible(true);
            }
        });
    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButtonChinhSua;
    private javax.swing.JButton jButtonReload;
    private javax.swing.JButton jButtonThemMoi;
    private javax.swing.JButton jButtonXoa;
    private javax.swing.JLabel jLabelTitle;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JPanel jPanel1Modify;
    private javax.swing.JPanel jPanel2;
    private javax.swing.JPanel jPanelTable;
    private javax.swing.JScrollPane jScrollPane1;
    private javax.swing.JTable jTableData;
    // End of variables declaration//GEN-END:variables

    private void configTable() {
        //configTable();
        jTableData.setBackground(Color.WHITE);
        jTableData.getTableHeader().setPreferredSize(new Dimension(50, 70));
        jTableData.setShowGrid(true);
        jTableData.setRowHeight(30);
        jTableData.setShowHorizontalLines(true); // only HorizontalLines
        jTableData.setShowVerticalLines(true); //  only VerticalLines
        jTableData.setShowGrid(true);          // show Horizontal and Vertical
        jTableData.getTableHeader().disable();
        dm = new DefaultTableModel() {
            public Class<String> getColumnClass(int columnIndex) {
                return String.class;
            }

            public boolean isCellEditable(int row, int column) {
                return false;
            }
        };

        dm.setDataVector(
                new Object[][]{
                    {}
                },
                new Object[]{"ID", "Ma Thiet Bi",
                    "Tên thiết bị;\nsố lượng;\nSố seri/model\nHãng/nước sx",
                    "Ngày sx;\nNguyên giá;\nCty nhập",
                    "Tình trạng\nhoạt động",
                    "Đơn vị  mượn;\nNgày mượn",
                    "Ngày trả",
                    "Lịch sử di chuyển", "Hiện tại"});

        jTableData.setModel(dm);

        MultiLineHeaderRenderer renderer = new MultiLineHeaderRenderer();
        Enumeration e = jTableData.getColumnModel().getColumns();
        while (e.hasMoreElements()) {
            ((TableColumn) e.nextElement()).setHeaderRenderer(renderer);
        }

        jTableData.getColumnModel().getColumn(0).setPreferredWidth(5);
        jTableData.getColumnModel().getColumn(1).setPreferredWidth(100);
        jTableData.getColumnModel().getColumn(2).setPreferredWidth(350);
        jTableData.getColumnModel().getColumn(3).setPreferredWidth(200);
        jTableData.getColumnModel().getColumn(4).setPreferredWidth(90);
        jTableData.getColumnModel().getColumn(5).setPreferredWidth(200);
        jTableData.getColumnModel().getColumn(6).setPreferredWidth(150);
        jTableData.getColumnModel().getColumn(7).setPreferredWidth(270);
        jTableData.getColumnModel().getColumn(8).setPreferredWidth(150);
        jTableData.setDefaultRenderer(String.class, new MultiLineTableCellRenderer());
        TableRowSorter<? extends TableModel> sort = new TableRowSorter<DefaultTableModel>(dm);
        jTableData.setRowSorter(sort);
        getContentPane().add(scrollpane);
    }

    public void setRowDataToTable(int row, String mtb,
            String stt, String name, String sx,
            String tthd, String dvm_nm, String ngaytra,
            String lsdc, String ht) {
        dm.addRow(new Object[]{});
        jTableData.setModel(dm);
        jTableData.setValueAt(stt, row, 0);
        jTableData.setValueAt(mtb, row, 1);
        jTableData.setValueAt(name, row, 2);
        jTableData.setValueAt(sx, row, 3);
        jTableData.setValueAt(tthd, row, 4);
        jTableData.setValueAt(dvm_nm, row, 5);
        jTableData.setValueAt(ngaytra, row, 6);
        jTableData.setValueAt(lsdc, row, 7);
        jTableData.setValueAt(ht, row, 8);
    }

    public void reloadJTable() {
        ScheduledExecutorService executorService = Executors.newSingleThreadScheduledExecutor();
        executorService.scheduleAtFixedRate(new Runnable() {
            @Override
            public void run() {
                System.out.println("----------------");
                if (addNewDeviceGUI.iStatus == 1) {
                    System.out.println("---- 123213213213213 -------");
                    System.out.println("--Name: " + addNewDeviceGUI.getContent().getName_seri().toString());
                    System.out.println("--NgaySX: " + addNewDeviceGUI.getContent().getNgaySX().toString());
                    System.out.println("--TinhTrang: " + addNewDeviceGUI.getContent().getTinhTrang().toString());
                    System.out.println("--Code: " + addNewDeviceGUI.getContent().getCode().toString());

                    setRowDataToTable(iRow, String.valueOf(iRow + 1),
                            addNewDeviceGUI.getContent().getCode().toString(),
                            addNewDeviceGUI.getContent().getName_seri().toString().replace("; ", "\n"),
                            addNewDeviceGUI.getContent().getNgaySX().toString().replace("; ", "\n"),
                            addNewDeviceGUI.getContent().getTinhTrang().toString().replace("; ", "\n"),
                            "Khong",
                            "Khong",
                            "Khong",
                            "Trong Kho");
                    iRow++;
                    executorService.shutdown();
                } else if (addNewDeviceGUI.iStatus == -1) {
                    System.out.println("---- Khong lam gi ca FALSE -----------");
                    executorService.shutdown();
                }
                repaint();
            }
        }, 0, 500, TimeUnit.MILLISECONDS);
    }

    private void loadtable() {
        clearAllRow();
        ScheduledExecutorService service__ = Executors.newSingleThreadScheduledExecutor();
        service__.scheduleAtFixedRate(new Runnable() {
            @Override
            public void run() {
                if (loggingGUI.getStatus() == 1) {
                    try {
                        jPanel1Modify.setVisible(true);
                        SheetsQuickstart quickstart = new SheetsQuickstart();
                        List<Fields> lField = quickstart.getDeviceInformation();
                        for (Fields f : lField) {
                            setRowDataToTable(y, f.getMTB(),
                                    f.getField1(),
                                    f.getField2(),
                                    f.getField3(),
                                    f.getField4(),
                                    f.getField5(),
                                    f.getField6(),
                                    f.getField7(),
                                    f.getField8());
                            y++;
                        }
                        loggingGUI.dispose();
                        service__.shutdown();
                    } catch (GeneralSecurityException ex) {
                        Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
                    } catch (IOException ex) {
                        Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
                    }
                } else if (loggingGUI.getStatus() == 2) {
                    try {
                        SheetsQuickstart quickstart = new SheetsQuickstart();
                        List<Fields> lField = quickstart.getDeviceInformation();
                        for (Fields f : lField) {
                            setRowDataToTable(y, f.getMTB(),
                                    f.getField1(),
                                    f.getField2(),
                                    f.getField3(),
                                    f.getField4(),
                                    f.getField5(),
                                    f.getField6(),
                                    f.getField7(),
                                    f.getField8());
                            y++;
                        }
                        loggingGUI.dispose();
                        jTableData.disable();
                        service__.shutdown();
                    } catch (GeneralSecurityException ex) {
                        Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
                    } catch (IOException ex) {
                        Logger.getLogger(MainJFrame.class.getName()).log(Level.SEVERE, null, ex);
                    }
                } else if (loggingGUI.getStatus() == -1) {
                    dispatchEvent(new WindowEvent(MainJFrame.this, WindowEvent.WINDOW_CLOSING));
                }
            }
        }, 0, 200, TimeUnit.MILLISECONDS);
    }

    private void eventWindowClose() {
        this.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {

                System.out.print("Da Dong");
                System.exit(0);

            }
        });
    }

    private void keyEnterPressed() {
        this.addKeyListener(new KeyAdapter() {
            @Override
            public void keyPressed(KeyEvent e) {
                if (e.getKeyCode() == KeyEvent.VK_ENTER) {
                    System.out.println("Enter is pressed ");
                }
            }
        });
    }

}
