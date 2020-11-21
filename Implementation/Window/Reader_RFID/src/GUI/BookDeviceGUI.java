package GUI;

import GUI.*;
import DataBase.RequestGETDataBase;
import DataBase.SheetsQuickstart;
import DataBase.SheetsQuickstart;
import java.awt.Color;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.KeyAdapter;
import java.awt.event.KeyEvent;
import java.awt.event.WindowAdapter;
import java.awt.event.WindowEvent;
import java.io.IOException;
import java.net.MalformedURLException;
import java.security.GeneralSecurityException;
import java.util.logging.Level;
import java.util.logging.Logger;
import javax.swing.JOptionPane;
import javax.swing.JTextField;
import reader_rfid.ThemThietBiMoi;

/**
 *
 * @author ohbao
 */
public class BookDeviceGUI extends javax.swing.JFrame {

    /**
     * Creates new form ThemMoi
     */
    private ThemThietBiMoi a = new ThemThietBiMoi();
    public int iStatus = 0;

    

    public BookDeviceGUI() {
        initComponents();
        eventWindowClose();
        getContentPane().setBackground(new Color(45, 211, 50));

        jTextFieldMaTB.setEditable(false);
        jText_Ten.setEditable(false);
        jTextNgaySX.setEditable(false);
        jTextTinhTrang.setEditable(false);
        jTextFieldNgayTra.setEditable(false);
        jTextFieldLSDC.setEditable(false);
        jTextFieldHienTai.setEditable(false);

        eventButtonChapNhan();
        eventButtonHuyBo();
    }

    public void clearText() {
        jTextDVM.setText("");
        jTextNgaySX.setText("");
        jTextTinhTrang.setText("");
        jText_Ten.setText("");
    }

    public void setContentInfo(String MTB, String TTTB, String NSX, String TTHD,
            String BR, String RT, String LSDC, String POS) {
        jTextFieldMaTB.setText(MTB);
        jText_Ten.setText(TTTB);
        jTextNgaySX.setText(NSX);
        jTextTinhTrang.setText(TTHD);
        jTextDVM.setText(BR);
        jTextFieldNgayTra.setText(RT);
        jTextFieldLSDC.setText(LSDC);
        jTextFieldHienTai.setText(POS);
    }

    public ThemThietBiMoi getContent() {
        return this.a;
    }

    /**
     * This method is called from within the constructor to initialize the form.
     * WARNING: Do NOT modify this code. The content of this method is always
     * regenerated by the Form Editor.
     */
    @SuppressWarnings("unchecked")
    // <editor-fold defaultstate="collapsed" desc="Generated Code">//GEN-BEGIN:initComponents
    private void initComponents() {

        jLabel1 = new javax.swing.JLabel();
        jLabel2 = new javax.swing.JLabel();
        jLabel3 = new javax.swing.JLabel();
        jLabel4 = new javax.swing.JLabel();
        jButtonChapNhan = new javax.swing.JButton();
        jButtonHuyBo = new javax.swing.JButton();
        jLabelDVM = new javax.swing.JLabel();
        jText_Ten = new javax.swing.JTextField();
        jTextNgaySX = new javax.swing.JTextField();
        jTextTinhTrang = new javax.swing.JTextField();
        jTextDVM = new javax.swing.JTextField();
        jLabelNgayTra = new javax.swing.JLabel();
        jTextFieldNgayTra = new javax.swing.JTextField();
        jLabelLSDC = new javax.swing.JLabel();
        jTextFieldLSDC = new javax.swing.JTextField();
        jLabel5 = new javax.swing.JLabel();
        jTextFieldHienTai = new javax.swing.JTextField();
        jLabel6 = new javax.swing.JLabel();
        jTextFieldMaTB = new javax.swing.JTextField();

        setDefaultCloseOperation(javax.swing.WindowConstants.DISPOSE_ON_CLOSE);
        setBackground(new java.awt.Color(51, 204, 0));
        setLocation(new java.awt.Point(500, 200));
        setResizable(false);

        jLabel1.setBackground(new java.awt.Color(254, 254, 254));
        jLabel1.setFont(new java.awt.Font("Ubuntu", 1, 24)); // NOI18N
        jLabel1.setForeground(new java.awt.Color(1, 1, 1));
        jLabel1.setHorizontalAlignment(javax.swing.SwingConstants.CENTER);
        jLabel1.setText("Đăng ký mượn thiết bị");
        jLabel1.setToolTipText("");
        jLabel1.setAutoscrolls(true);
        jLabel1.setCursor(new java.awt.Cursor(java.awt.Cursor.TEXT_CURSOR));
        jLabel1.setFocusable(false);
        jLabel1.setHorizontalTextPosition(javax.swing.SwingConstants.CENTER);

        jLabel2.setBackground(new java.awt.Color(255, 255, 255));
        jLabel2.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabel2.setText("Tên Thiết Bị, Số lượng, Số seri/model, Hãng, Nước sản xuất:");
        jLabel2.setAutoscrolls(true);

        jLabel3.setBackground(new java.awt.Color(255, 255, 255));
        jLabel3.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabel3.setText("Ngày SX, Nguyên giá, Cty Nhập:");

        jLabel4.setBackground(new java.awt.Color(255, 255, 255));
        jLabel4.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabel4.setText("Tình trạng hoạt động");

        jButtonChapNhan.setFont(new java.awt.Font("Ubuntu", 1, 15)); // NOI18N
        jButtonChapNhan.setText("Chấp nhận");
        jButtonChapNhan.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonChapNhanActionPerformed(evt);
            }
        });

        jButtonHuyBo.setFont(new java.awt.Font("Ubuntu", 1, 15)); // NOI18N
        jButtonHuyBo.setText("Hủy bỏ");
        jButtonHuyBo.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jButtonHuyBoActionPerformed(evt);
            }
        });

        jLabelDVM.setBackground(new java.awt.Color(255, 255, 255));
        jLabelDVM.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabelDVM.setText("Đơn vi mượn, Ngày mượn");

        jText_Ten.setFont(new java.awt.Font("Ubuntu", 0, 18)); // NOI18N

        jTextNgaySX.setFont(new java.awt.Font("Ubuntu", 0, 18)); // NOI18N
        jTextNgaySX.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jTextNgaySXActionPerformed(evt);
            }
        });

        jTextTinhTrang.setFont(new java.awt.Font("Ubuntu", 0, 18)); // NOI18N

        jTextDVM.setFont(new java.awt.Font("Ubuntu", 0, 18)); // NOI18N
        jTextDVM.addActionListener(new java.awt.event.ActionListener() {
            public void actionPerformed(java.awt.event.ActionEvent evt) {
                jTextDVMActionPerformed(evt);
            }
        });

        jLabelNgayTra.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabelNgayTra.setText("Ngày trả");

        jTextFieldNgayTra.setFont(new java.awt.Font("Arial", 0, 18)); // NOI18N

        jLabelLSDC.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabelLSDC.setText("Lịch sử di chuyển");

        jTextFieldLSDC.setFont(new java.awt.Font("Arial", 0, 18)); // NOI18N

        jLabel5.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabel5.setText("Vi trí hiện tại");

        jTextFieldHienTai.setFont(new java.awt.Font("Arial", 0, 18)); // NOI18N

        jLabel6.setFont(new java.awt.Font("Arial", 1, 18)); // NOI18N
        jLabel6.setText("Mã thiết bị");

        jTextFieldMaTB.setFont(new java.awt.Font("Arial", 0, 18)); // NOI18N

        javax.swing.GroupLayout layout = new javax.swing.GroupLayout(getContentPane());
        getContentPane().setLayout(layout);
        layout.setHorizontalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabel1, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                    .addGroup(layout.createSequentialGroup()
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING, false)
                            .addComponent(jLabel5, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabelNgayTra, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel3, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel2, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabel4, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabelDVM, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE)
                            .addComponent(jLabelLSDC, javax.swing.GroupLayout.Alignment.LEADING, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, Short.MAX_VALUE))
                        .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                            .addGroup(layout.createSequentialGroup()
                                .addGap(18, 18, 18)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                                    .addComponent(jText_Ten)
                                    .addComponent(jTextNgaySX)
                                    .addComponent(jTextTinhTrang)
                                    .addComponent(jTextDVM)))
                            .addGroup(layout.createSequentialGroup()
                                .addGap(18, 18, 18)
                                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                                    .addComponent(jTextFieldNgayTra)
                                    .addComponent(jTextFieldLSDC)
                                    .addComponent(jTextFieldHienTai)
                                    .addGroup(layout.createSequentialGroup()
                                        .addComponent(jButtonChapNhan, javax.swing.GroupLayout.PREFERRED_SIZE, 110, javax.swing.GroupLayout.PREFERRED_SIZE)
                                        .addGap(79, 79, 79)
                                        .addComponent(jButtonHuyBo, javax.swing.GroupLayout.PREFERRED_SIZE, 110, javax.swing.GroupLayout.PREFERRED_SIZE)
                                        .addGap(0, 488, Short.MAX_VALUE))
                                    .addComponent(jTextFieldMaTB)))))
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jLabel6)
                        .addGap(0, 0, Short.MAX_VALUE)))
                .addContainerGap())
        );
        layout.setVerticalGroup(
            layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
            .addGroup(layout.createSequentialGroup()
                .addContainerGap()
                .addComponent(jLabel1, javax.swing.GroupLayout.PREFERRED_SIZE, 41, javax.swing.GroupLayout.PREFERRED_SIZE)
                .addGap(18, 18, 18)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jLabel2, javax.swing.GroupLayout.PREFERRED_SIZE, 29, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jText_Ten, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jLabel3, javax.swing.GroupLayout.PREFERRED_SIZE, 29, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextNgaySX, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jLabel4, javax.swing.GroupLayout.PREFERRED_SIZE, 29, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextTinhTrang, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jLabelDVM, javax.swing.GroupLayout.PREFERRED_SIZE, 27, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextDVM, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.LEADING)
                    .addComponent(jLabelNgayTra, javax.swing.GroupLayout.PREFERRED_SIZE, 25, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextFieldNgayTra, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.TRAILING)
                    .addComponent(jLabelLSDC, javax.swing.GroupLayout.PREFERRED_SIZE, 32, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addGroup(layout.createSequentialGroup()
                        .addComponent(jTextFieldLSDC, javax.swing.GroupLayout.PREFERRED_SIZE, 28, javax.swing.GroupLayout.PREFERRED_SIZE)
                        .addGap(4, 4, 4)))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jLabel5, javax.swing.GroupLayout.PREFERRED_SIZE, 25, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextFieldHienTai, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.UNRELATED)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.BASELINE)
                    .addComponent(jLabel6, javax.swing.GroupLayout.PREFERRED_SIZE, 20, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jTextFieldMaTB, javax.swing.GroupLayout.PREFERRED_SIZE, javax.swing.GroupLayout.DEFAULT_SIZE, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addPreferredGap(javax.swing.LayoutStyle.ComponentPlacement.RELATED, 21, Short.MAX_VALUE)
                .addGroup(layout.createParallelGroup(javax.swing.GroupLayout.Alignment.CENTER)
                    .addComponent(jButtonChapNhan, javax.swing.GroupLayout.PREFERRED_SIZE, 30, javax.swing.GroupLayout.PREFERRED_SIZE)
                    .addComponent(jButtonHuyBo, javax.swing.GroupLayout.PREFERRED_SIZE, 30, javax.swing.GroupLayout.PREFERRED_SIZE))
                .addGap(77, 77, 77))
        );

        pack();
    }// </editor-fold>//GEN-END:initComponents

    private void jButtonHuyBoActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonHuyBoActionPerformed
        // TODO add your handling code here:
        //setContent("", "", "", "");
        dispose();
    }//GEN-LAST:event_jButtonHuyBoActionPerformed

    private void jTextNgaySXActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jTextNgaySXActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jTextNgaySXActionPerformed

    private void jTextDVMActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jTextDVMActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jTextDVMActionPerformed

    private void jButtonChapNhanActionPerformed(java.awt.event.ActionEvent evt) {//GEN-FIRST:event_jButtonChapNhanActionPerformed
        // TODO add your handling code here:
    }//GEN-LAST:event_jButtonChapNhanActionPerformed

    private void eventButtonChapNhan() {
        jButtonChapNhan.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                try {
                    iStatus = 1;
                    System.out.println("Tao da click Chap Nhan");
                    //setContent(jText_Ten.getText(), jTextNgaySX.getText(), jTextTinhTrang.getText(), jTextCode.getText());

                    SheetsQuickstart sheetAPI = new SheetsQuickstart();                    
                    String sName1 = jText_Ten.getText().replace(",", "\n");
                    String sName2 = jTextNgaySX.getText().replace(",", "\n");
                    String sName3 = jTextTinhTrang.getText().replace(",", "\n");
                    String sName4 = jTextDVM.getText().replace(",", "\n");
                    String sName5 = jTextFieldNgayTra.getText().replace(",", "\n");
                    String sName6 = jTextFieldLSDC.getText().replace(",", "\n");
                    String sName7 = jTextFieldHienTai.getText().replace(",", "\n");
                    String sName8 = jTextFieldMaTB.getText().replace(",", "\n");

                    boolean b = sheetAPI.bookkingDeviceOnSheet(sName8,
                            sName1, sName2, sName3, sName4, sName5, sName6,
                            sName7, "");
                    if (!b) {
                        JOptionPane.showMessageDialog(null, "Không tìm thấy mã thiết bị", "Error", JOptionPane.ERROR_MESSAGE);
                    } else {
                        dispose();
                    }
                    //
                } catch (MalformedURLException ex) {
                    Logger.getLogger(BookDeviceGUI.class.getName()).log(Level.SEVERE, null, ex);
                } catch (GeneralSecurityException ex) {
                    Logger.getLogger(BookDeviceGUI.class.getName()).log(Level.SEVERE, null, ex);
                } catch (IOException ex) {
                    Logger.getLogger(BookDeviceGUI.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
        });
    }

    private void eventButtonHuyBo() {
        jButtonHuyBo.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                iStatus = -1;
                System.out.println("Tao da click Huy Bo");
                dispose();
            }
        });
    }

    private void eventWindowClose() {
        this.addWindowListener(new WindowAdapter() {
            @Override
            public void windowClosing(WindowEvent e) {

                iStatus = -1;
                System.out.println("Tao da click Huy Bo");
                dispose();
                //System.exit(0);

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

    // Variables declaration - do not modify//GEN-BEGIN:variables
    private javax.swing.JButton jButtonChapNhan;
    private javax.swing.JButton jButtonHuyBo;
    private javax.swing.JLabel jLabel1;
    private javax.swing.JLabel jLabel2;
    private javax.swing.JLabel jLabel3;
    private javax.swing.JLabel jLabel4;
    private javax.swing.JLabel jLabel5;
    private javax.swing.JLabel jLabel6;
    private javax.swing.JLabel jLabelDVM;
    private javax.swing.JLabel jLabelLSDC;
    private javax.swing.JLabel jLabelNgayTra;
    private javax.swing.JTextField jTextDVM;
    private javax.swing.JTextField jTextFieldHienTai;
    private javax.swing.JTextField jTextFieldLSDC;
    private javax.swing.JTextField jTextFieldMaTB;
    private javax.swing.JTextField jTextFieldNgayTra;
    private javax.swing.JTextField jTextNgaySX;
    private javax.swing.JTextField jTextTinhTrang;
    private javax.swing.JTextField jText_Ten;
    // End of variables declaration//GEN-END:variables

}
