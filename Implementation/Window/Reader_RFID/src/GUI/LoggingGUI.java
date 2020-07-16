/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package GUI;

import DataBase.SheetsQuickstart;
import java.io.IOException;
import java.security.GeneralSecurityException;
import java.util.ArrayList;
import java.util.List;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import reader_rfid.Reader_RFID;

/**
 *
 * @author ohbao
 */
public class LoggingGUI extends javax.swing.JFrame {

    
    //private MainJFrame mainJFrame = new MainJFrame();
    /**
     * Creates new form LoggingGUI
     */
    private int isStatus = 0; 
    private SheetsQuickstart quickstart;
    private List<String> arrList = new ArrayList<>();
    public LoggingGUI() {
        try {
            quickstart = new SheetsQuickstart();
            arrList = quickstart.getListAccount();
            initComponents();
        } catch (GeneralSecurityException ex) {
            Logger.getLogger(LoggingGUI.class.getName()).log(Level.SEVERE, null, ex);
        } catch (IOException ex) {
            Logger.getLogger(LoggingGUI.class.getName()).log(Level.SEVERE, null, ex);
        }
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jPanel1 = new javax.swing.JPanel();
        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jTextField1NguoiDung = new javax.swing.JTextField();
        jLabel3 = new javax.swing.JLabel();
        jTextField2MatKhau = new javax.swing.JTextField();
        jButtonTruyCap = new javax.swing.JButton();
        jButtonHuyBo = new javax.swing.JButton();
        jButton1ReadOnly = new javax.swing.JButton();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);

        jLabel1.setFont(new java.awt.Font("Monospaced", 0, 24)); // NOI18N
        jLabel1.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        jLabel1.setText("Đăng Nhập");

        jLabel2.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N
        jLabel2.setText("Người dùng");

        jTextField1NguoiDung.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N

        jLabel3.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N
        jLabel3.setText("Mật khẩu");

        jTextField2MatKhau.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N

        jButtonTruyCap.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N
        jButtonTruyCap.setText("Truy cập");
        jButtonTruyCap.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonTruyCapActionPerformed(evt);
            }
        });

        jButtonHuyBo.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N
        jButtonHuyBo.setText("Hủy bỏ");
        jButtonHuyBo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonHuyBoActionPerformed(evt);
            }
        });

        jButton1ReadOnly.setFont(new java.awt.Font("Monospaced", 0, 15)); // NOI18N
        jButton1ReadOnly.setText("Read Only");
        jButton1ReadOnly.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButton1ReadOnlyActionPerformed(evt);
            }
        });

        javax.swing.GroupLayout jPanel1Layout = new javax.swing.GroupLayout(jPanel1);
        jPanel1.setLayout(jPanel1Layout);
        jPanel1Layout.setHorizontalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addComponent(jLabel1, javax.swing.GroupLayout.Alignment.TRAILING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
            .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                .addContainerGap(65, Short.MAX_VALUE)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                        .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                            .addComponent(jLabel2, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel3, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                        .addGap(18, 18, 18))
                    .addGroup(javax.swing.GroupLayout.Alignment.TRAILING, jPanel1Layout.createSequentialGroup()
                        .addComponent(jButtonTruyCap)
                        .addGap(36, 36, 36)))
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING, false)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jButton1ReadOnly)
                        .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                        .addComponent(jButtonHuyBo, javax.swing.GroupLayout.PREFERRED_SIZE, 86, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addComponent(jTextField1NguoiDung)
                    .addComponent(jTextField2MatKhau, javax.swing.GroupLayout.PREFERRED_SIZE, 213, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(56, 56, 56))
        );
        jPanel1Layout.setVerticalGroup(
            jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(jPanel1Layout.createSequentialGroup()
                .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 47, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jTextField1NguoiDung, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(11, 11, 11)
                        .addComponent(jTextField2MatKhau, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                    .addGroup(jPanel1Layout.createSequentialGroup()
                        .addComponent(jLabel2)
                        .addGap(21, 21, 21)
                        .addComponent(jLabel3)))
                .addGap(18, 18, 18)
                .addGroup(jPanel1Layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jButtonHuyBo)
                    .addComponent(jButtonTruyCap)
                    .addComponent(jButton1ReadOnly))
                .addContainerGap(27, Short.MAX_VALUE))
        );

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jPanel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonTruyCapActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonTruyCapActionPerformed
        // TODO add your handling code here:         
        if(jTextField1NguoiDung.getText().contains("admin") && 
           arrList.contains(jTextField2MatKhau.getText())){
            System.out.println("Success loggin");
            isStatus = 1;
        }
        else{
            JOptionPane.showMessageDialog(this,"Người dùng hoặc mật khẩu không chính xác","Error", JOptionPane.ERROR_MESSAGE);  
            isStatus = 0;
        }
    }//GEN-LAST:event_jButtonTruyCapActionPerformed

    private void jButtonHuyBoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonHuyBoActionPerformed
      
        this.isStatus = -1;       // TODO add your handling code here:
    }//GEN-LAST:event_jButtonHuyBoActionPerformed

    private void jButton1ReadOnlyActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButton1ReadOnlyActionPerformed
        // TODO add your handling code here:
        
        this.isStatus = 2;
    }//GEN-LAST:event_jButton1ReadOnlyActionPerformed
    
    public int getStatus(){
        return this.isStatus;
    }
    /**
     * @param args the command line arguments
     */
//    public static void main(String args[]) {
//        /* Set the Nimbus look and feel */
//        //<editor-fold defaultstate="collapsed" desc=" Look and feel setting code (optional) ">
//        /* If Nimbus (introduced in Java SE 6) is not available, stay with the default look and feel.
//         * For details see http://download.oracle.com/javase/tutorial/uiswing/lookandfeel/plaf.html 
//         */
//        try {
//            for (javax.swing.UIManager.LookAndFeelInfo info : javax.swing.UIManager.getInstalledLookAndFeels()) {
//                if ("Nimbus".equals(info.getName())) {
//                    javax.swing.UIManager.setLookAndFeel(info.getClassName());
//                    break;
//                }
//            }
//        } catch (ClassNotFoundException ex) {
//            java.util.logging.Logger.getLogger(LoggingGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
//        } catch (InstantiationException ex) {
//            java.util.logging.Logger.getLogger(LoggingGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
//        } catch (IllegalAccessException ex) {
//            java.util.logging.Logger.getLogger(LoggingGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
//        } catch (javax.swing.UnsupportedLookAndFeelException ex) {
//            java.util.logging.Logger.getLogger(LoggingGUI.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);
//        }
//        //</editor-fold>
//
//        /* Create and display the form */
//        java.awt.EventQueue.invokeLater(new Runnable() {
//            public void run() {
//                new LoggingGUI().setVisible(true);
//            }
//        });
//    }

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButton1ReadOnly;
    private javax.swing.JButton jButtonHuyBo;
    private javax.swing.JButton jButtonTruyCap;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JPanel jPanel1;
    private javax.swing.JTextField jTextField1NguoiDung;
    private javax.swing.JTextField jTextField2MatKhau;
    // End of variables declaration//GEN-END:variables
}
