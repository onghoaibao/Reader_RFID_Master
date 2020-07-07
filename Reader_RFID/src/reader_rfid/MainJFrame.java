/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package reader_rfid;

import static com.sun.xml.internal.fastinfoset.alphabet.BuiltInRestrictedAlphabets.table;
import java.awt.Color;
import java.awt.Component;
import java.awt.Dimension;
import java.awt.Font;
import java.util.Enumeration;
import java.util.List;
import java.util.concurrent.Executors;
import java.util.concurrent.ScheduledExecutorService;
import java.util.concurrent.TimeUnit;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.BorderFactory;
import javax.swing.JComponent;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.border.Border;
import javax.swing.event.ListSelectionEvent;
import javax.swing.event.ListSelectionListener;
import javax.swing.table.DefaultTableCellRenderer;
import javax.swing.table.DefaultTableModel;
import javax.swing.table.JTableHeader;
import javax.swing.table.TableCellEditor;
import javax.swing.table.TableColumn;
import javax.swing.table.TableColumnModel;
import javax.swing.table.TableModel;
import javax.swing.table.TableRowSorter;

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
    public static ThemMoiGUI themMoiGui = new ThemMoiGUI();

    public MainJFrame() {
        getContentPane().setBackground(Color.WHITE);
        scrollpane = new JScrollPane(jTableData);
        initComponents();
        jButtonChinhSua.setBackground(Color.green);
        jButtonThemMoi.setBackground(Color.green);
        jButtonXoa.setBackground(Color.green);

        jLabelTitle.setText("Chương Trình Quản Lý Thiết Bị Y Tế");
        jLabelTitle.setFont(new Font("Times New Roman", Font.BOLD, 28));

        setTitle("Giam Sat Thiet Bi Y Te");
        configTable();
        
        Reader_RFID rFID = new Reader_RFID();
        LoadDatatable(rFID.getListData());
        //ableData.
//        for (int y = 0; y < 10; y++) {
//            setRowDataToTable(y, String.valueOf(y + 1), "Máy siêu âm màu (01 máy)\n112233/logicE\nGE/Mỹ",
//                    "15/06/2000;\n999.999.000;\nMediruop",
//                    "Tot",
//                    "Nội tổng quát\n01/011/2020",
//                    "01/02/2020",
//                    "01/01/2020: Nội Tổng quát\n01/02/2020 trả\n15/02/2020:\nNội thần kinh mượn",
//                    "Nội thần kinh");
//            ii = y;
//        }

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

        jPanelTable = new javax.swing.JPanel();
        jScrollPane1 = new javax.swing.JScrollPane();
        jTableData = new javax.swing.JTable();
        jPanel1 = new javax.swing.JPanel();
        jButtonThemMoi = new javax.swing.JButton();
        jButtonChinhSua = new javax.swing.JButton();
        jButtonXoa = new javax.swing.JButton();
        jLabelTitle = new javax.swing.JLabel();

        setDefaultCloseOperation(javax.swing.WindowConstants.EXIT_ON_CLOSE);
        setBackground(new java.awt.Color(255, 255, 255));

        jPanelTable.setBackground(new java.awt.Color(255, 255, 255));
        jPanelTable.setBorder(javax.swing.BorderFactory.createLineBorder(new java.awt.Color(0, 0, 0)));

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

        jPanel1.setBorder(javax.swing.BorderFactory.createLineBorder(new java.awt.Color(0, 0, 0)));

        jButtonThemMoi.setBackground(new java.awt.Color(4, 174, 1));
        jButtonThemMoi.setFont(new java.awt.Font("Ubuntu", 1, 15)); // NOI18N
        jButtonThemMoi.setText("Them moi");
        jButtonThemMoi.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonThemMoiActionPerformed(evt);
            }
        });

        jButtonChinhSua.setBackground(new java.awt.Color(248, 249, 248));
        jButtonChinhSua.setFont(new java.awt.Font("Ubuntu", 1, 15)); // NOI18N
        jButtonChinhSua.setText("Sua");
        jButtonChinhSua.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonChinhSuaActionPerformed(evt);
            }
        });

        jButtonXoa.setFont(new java.awt.Font("Ubuntu", 1, 15)); // NOI18N
        jButtonXoa.setText("Xoa");

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jButtonThemMoi, javax.swing.GroupLayout.DEFAULT_SIZE, 161, Short.MAX_VALUE)
            .addComponent(jButtonChinhSua, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addComponent(jButtonXoa, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addGap(78, 78, 78)
                .addComponent(jButtonThemMoi, javax.swing.GroupLayout.PREFERRED_SIZE, 37, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addComponent(jButtonChinhSua, javax.swing.GroupLayout.PREFERRED_SIZE, 38, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(26, 26, 26)
                .addComponent(jButtonXoa, javax.swing.GroupLayout.PREFERRED_SIZE, 38, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap(javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        javax.swing.GroupLayout jPanelTableLayout = new javax.swing.GroupLayout(jPanelTable);
        jPanelTable.setLayout(jPanelTableLayout);
        jPanelTableLayout.setHorizontalGroup(
            jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanelTableLayout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 910, Short.MAX_VALUE)
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addComponent(jPanel1, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addContainerGap())
        );
        jPanelTableLayout.setVerticalGroup(
            jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanelTableLayout.createSequentialGroup()
                .addContainerGap()
                .addGroup(jPanelTableLayout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jScrollPane1, javax.swing.GroupLayout.DEFAULT_SIZE, 490, Short.MAX_VALUE)
                    .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                .addContainerGap())
        );

        jLabelTitle.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        jLabelTitle.setText("text");

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
                .addComponent(jLabelTitle, javax.swing.GroupLayout.PREFERRED_SIZE, 58, javax.swing.GroupLayout.PREFERRED_SIZE)
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
        themMoiGui.setVisible(true);
        reloadJTable();
    }//GEN-LAST:event_jButtonThemMoiActionPerformed

    private void jButtonChinhSuaActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonChinhSuaActionPerformed
        // TODO add your handling code here:
        int row = jTableData.getSelectedRow();
        int col = jTableData.getSelectedColumnCount();
        if (row != -1 && col != -1) {
            String s = "";
            for (int i = 0; i < 8; i++) {
                s = s + (String.valueOf(jTableData.getValueAt(row, i)) + " - ");
            }
            System.out.print(s.replace("/n", ""));
        }
    }//GEN-LAST:event_jButtonChinhSuaActionPerformed

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
    private javax.swing.JButton jButtonThemMoi;
    private javax.swing.JButton jButtonXoa;
    private javax.swing.JLabel jLabelTitle;
    private javax.swing.JPanel jPanel1;
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
                new Object[]{"STT",
                    "Tên thiết bị, số lượng\nSố seri/model\nHãng/nước sx",
                    "Ngày sx;\nNguyên giá;\nCty nhập",
                    "Tình trạng\nhoạt động",
                    "Đơn vị  mượn\nNgày mượn",
                    "Ngày trả",
                    "Lịch sử di chuyển", "Hiện tại"});

        jTableData.setModel(dm);

        MultiLineHeaderRenderer renderer = new MultiLineHeaderRenderer();
        Enumeration e = jTableData.getColumnModel().getColumns();
        while (e.hasMoreElements()) {
            ((TableColumn) e.nextElement()).setHeaderRenderer(renderer);
        }

        jTableData.getColumnModel().getColumn(0).setPreferredWidth(5);
        jTableData.getColumnModel().getColumn(1).setPreferredWidth(250);
        jTableData.getColumnModel().getColumn(2).setPreferredWidth(100);
        jTableData.getColumnModel().getColumn(3).setPreferredWidth(90);
        jTableData.getColumnModel().getColumn(4).setPreferredWidth(90);
        jTableData.getColumnModel().getColumn(5).setPreferredWidth(120);
        jTableData.getColumnModel().getColumn(6).setPreferredWidth(250);
        jTableData.getColumnModel().getColumn(7).setPreferredWidth(300);
        jTableData.setDefaultRenderer(String.class, new MultiLineTableCellRenderer());
        TableRowSorter<? extends TableModel> sort = new TableRowSorter<DefaultTableModel>(dm);
        jTableData.setRowSorter(sort);
        getContentPane().add(scrollpane);
    }

    public void setRowDataToTable(int row,
            String stt, String dv, String sx,
            String tthd, String dvm_nm, String nt,
            String lsdc, String ht) {
        dm.addRow(new Object[]{});
        jTableData.setModel(dm);
        jTableData.setValueAt(stt, row, 0);
        jTableData.setValueAt(dv, row, 1);
        jTableData.setValueAt(sx, row, 2);
        jTableData.setValueAt(tthd, row, 3);
        jTableData.setValueAt(dvm_nm, row, 4);
        jTableData.setValueAt(nt, row, 5);
        jTableData.setValueAt(lsdc, row, 6);
        jTableData.setValueAt(ht, row, 7);
    }

    private void LoadDatatable(List<Fields> listData) {
        int i = 0;
        
        for (Fields  e : listData) {
            setRowDataToTable(i, e.getField1(), e.getField2(),
                    e.getField3(), e.getField4(), e.getField5(), e.getField6(),
                    e.getField7(), e.getField8());
            i++;
        }
    }

    public void reloadJTable() {
        ScheduledExecutorService executorService = Executors.newSingleThreadScheduledExecutor();
        executorService.scheduleAtFixedRate(new Runnable() {
            @Override
            public void run() {
                System.out.println("----------------");
                if (themMoiGui.iStatus == 1) {
                    System.out.println("---- 123213213213213 -------");
                    System.out.println("--Name: " + themMoiGui.getContent().getName_seri().toString());
                    System.out.println("--NgaySX: " + themMoiGui.getContent().getNgaySX().toString());
                    System.out.println("--TinhTrang: " + themMoiGui.getContent().getTinhTrang().toString());
                    System.out.println("--Code: " + themMoiGui.getContent().getCode().toString());
                    executorService.shutdown();
                } else if (themMoiGui.iStatus == -1) {
                    System.out.println("---- Khong lam gi ca FALSE -----------");
                    executorService.shutdown();
                }
                repaint();
            }
        }, 0, 500, TimeUnit.MILLISECONDS);

        if (themMoiGui.iStatus == 1) {
            executorService.shutdown();
        }

    }

}
