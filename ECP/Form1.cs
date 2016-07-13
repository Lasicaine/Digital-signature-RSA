using System;
using System.Text;
using System.Windows.Forms;

namespace ECP
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private ECP A_ECP;
        private ECP B_ECP;
        private readonly Encoding enc = Encoding.Default;

        private void Form1_Load(object sender, EventArgs e)
        {
            //A_ECP = new RSA_Sign();
            //B_ECP = new RSA_Sign();

            //btnGen_A_Click(sender, e);
            //btnGen_B_Click(sender, e);

            //btn_PublicToA_Click(sender, e);
            //btn_PublicToB_Click(sender, e);
        }

        private void btn_PublicToB_Click(object sender, EventArgs e)
        {
            txtBoxPublic_Ae.Text = txtBoxA_e.Text;
            txtBoxPublic_An.Text = txtBoxA_n.Text;
        }

        private void btn_PublicToA_Click(object sender, EventArgs e)
        {
            txtBoxPublic_Be.Text = txtBoxB_e.Text;
            txtBoxPublic_Bn.Text = txtBoxB_n.Text;
        }  

        private void btnGen_A_Click(object sender, EventArgs e)
        {
            A_ECP = new ECP();
            txtBoxA_e.Text = A_ECP.E;
            txtBoxA_d.Text = A_ECP.D;
            txtBoxA_n.Text = A_ECP.N;
        }
        private void btnGen_B_Click(object sender, EventArgs e)
        {
            B_ECP = new ECP();
            txtBoxB_e.Text = B_ECP.E;
            txtBoxB_d.Text = B_ECP.D;
            txtBoxB_n.Text = B_ECP.N;
        }

        private void btnSignA_Click(object sender, EventArgs e)
        {
            try
            {
                if (rTxtBoxA.Text == "")
                {
                    MessageBox.Show("Сообщение отсутствует");
                    return;
                }
                byte[] bText = enc.GetBytes(rTxtBoxA.Text);
                txtBoxSIGN_A.Text = new BigInteger(A_ECP.CreateSignature(bText)).ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Не заполнены все поля");
            }

        }
        private void btnSignB_Click(object sender, EventArgs e)
        {
            try
            {
                if (rTxtBoxB.Text == "")
                {
                    MessageBox.Show("Сообщение отсутствует");
                    return;
                }
                byte[] bText = enc.GetBytes(rTxtBoxB.Text);
                txtBoxSIGN_B.Text = new BigInteger(B_ECP.CreateSignature(bText)).ToString();
            }
            catch (Exception)
            {
                MessageBox.Show("Не заполнены все поля");
            }
        }

        private void btnVerifySign_fromB_Click(object sender, EventArgs e)
        {
                if (txtBoxPublic_Bn.Text == "" || txtBoxPublic_Be.Text == "")
                {
                    MessageBox.Show("Не опубликованы криптографические параметры Абонента В");
                    return;
                }
                if (rTextBoxInputMessageA.Text == "")
                {
                    MessageBox.Show("Сообщение отсутствует");
                    return;
                }
                try
                {
                BigInteger S = new BigInteger(txtBoxSIGN_fromB.Text, 10);
                BigInteger E = new BigInteger(txtBoxPublic_Be.Text, 10);
                BigInteger n = new BigInteger(txtBoxPublic_Bn.Text, 10);
           
            if (A_ECP.VerifySignature(enc.GetBytes(rTextBoxInputMessageA.Text), S.getBytes(), E.getBytes(),
                                       n.getBytes()) && (txtBoxPublic_Be.Text == txtBoxB_e.Text))
            {
                MessageBox.Show("Цифровая подпись: действительна. \nПодписал: Абонент В.", "Проверка цифровой подписи", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Цифровая подпись: не действительна", "Проверка цифровой подписи", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
            catch (Exception)
            {
                MessageBox.Show("Не заполнены все поля");
            }


        }

        private void btnVerifySign_fromA_Click(object sender, EventArgs e)
        {
            if (txtBoxPublic_An.Text == "" || txtBoxPublic_Ae.Text == "")
            {
                MessageBox.Show("Не опубликованы криптографические параметры Абонента A");
                return;
            }
            if (rTextBoxInputMessageB.Text == "")
            {
                MessageBox.Show("Сообщение отсутствует");
                return;
            }
            try
            {
            BigInteger S = new BigInteger(txtBoxSIGN_fromA.Text, 10);
            BigInteger E = new BigInteger(txtBoxPublic_Ae.Text, 10);
            BigInteger n = new BigInteger(txtBoxPublic_An.Text, 10);

            if (B_ECP.VerifySignature(enc.GetBytes(rTextBoxInputMessageB.Text), S.getBytes(), E.getBytes(),
                                       n.getBytes()) && (txtBoxPublic_Ae.Text == txtBoxA_e.Text))
            {
                MessageBox.Show("Цифровая подпись: действительна. \nПодписал: Абонент А.", "Проверка цифровой подписи", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            MessageBox.Show("Цифровая подпись: не действительна", "Проверка цифровой подписи", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                MessageBox.Show("Не заполнены все поля");
            }
        }

        private void btnSendToASignMessage_Click(object sender, EventArgs e)
        {
            rTextBoxInputMessageA.Text = rTxtBoxB.Text;
            txtBoxSIGN_fromB.Text = txtBoxSIGN_B.Text;
        }

        private void btnSendToBSignMessage_Click(object sender, EventArgs e)
        {
            rTextBoxInputMessageB.Text = rTxtBoxA.Text;
            txtBoxSIGN_fromA.Text = txtBoxSIGN_A.Text;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnClose.PerformClick();
        }

        private void txtBoxSIGN_A_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm about = new AboutForm();
            about.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string msg = "Вы действительно хотите выйти?";
            DialogResult result = MessageBox.Show(msg,
            "Выход", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            } 
        }

        private void полученныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rTextBoxInputMessageA.Text = "";
            txtBoxSIGN_fromB.Text = "";
        }

        private void отправленныеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rTxtBoxB.Text = "";
            txtBoxSIGN_B.Text = "";
        }

        private void полученныеToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rTextBoxInputMessageB.Text = "";
            txtBoxSIGN_fromA.Text = "";
        }

        private void сообщенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rTxtBoxA.Text = "";
            txtBoxSIGN_A.Text = "";
        }
   
    }
}