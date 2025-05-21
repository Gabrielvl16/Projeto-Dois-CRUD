using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_Dois_CRUD
{
    public partial class FormLogin : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        public FormLogin()
        {
            InitializeComponent();

            this.MouseDown += FormLogin_MouseDown;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string usuario = "Admin";
            string senha = "123";

            if (txtUsuario.Text == usuario && txtSenha.Text == senha)
            {
                MessageBox.Show("Login realizado com sucesso!",
                "Acesso liberado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
                );

                this.Hide();
                Form1 form1 = new Form1();
                form1.Show();
            }
            else if (txtUsuario.Text == "" || txtSenha.Text == "")
            {
                MessageBox.Show("Preecha ambos os campos para continuar!",
                "Erro",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error
                );
            }
            else
            {
                MessageBox.Show("Usuário ou senha icorretos!",
                "Acesso negado",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
                );
            }
        }

        private void FormLogin_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
    }
}
