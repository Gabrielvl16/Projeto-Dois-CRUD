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
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;

        public Form1()
        {
            InitializeComponent();

            this.MouseDown += Form1_MouseDown;
        }

        // Botão de fechar e Minimizar
        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        // btn Cadastrar
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            AbrirTela(new TelaCadastrar());
        }

        private void btnCadastrar_MouseEnter(object sender, EventArgs e)
        {
            btnCadastrar.BackColor = ColorTranslator.FromHtml("#004C99");
        }

        private void btnCadastrar_MouseLeave(object sender, EventArgs e)
        {
            btnCadastrar.BackColor = ColorTranslator.FromHtml("#0066CC");
        }

        // btn Listar
        private void btnListar_Click(object sender, EventArgs e)
        {
            AbrirTela(new TelaListar());
        }

        private void btnListar_MouseEnter(object sender, EventArgs e)
        {
            btnListar.BackColor = ColorTranslator.FromHtml("#004C99");
        }

        private void btnListar_MouseLeave(object sender, EventArgs e)
        {
            btnListar.BackColor = ColorTranslator.FromHtml("#0066CC");
        }

        // btn Editar
        private void btnEditar_Click(object sender, EventArgs e)
        {
            AbrirTela(new TelaEditar());
        }

        private void btnEditar_MouseEnter(object sender, EventArgs e)
        {
            btnEditar.BackColor = ColorTranslator.FromHtml("#004C99");
        }

        private void btnEditar_MouseLeave(object sender, EventArgs e)
        {
            btnEditar.BackColor = ColorTranslator.FromHtml("#0066CC");
        }

        // btn Excluir
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            AbrirTela(new TelaExcluir());
        }

        private void btnExcluir_MouseEnter(object sender, EventArgs e)
        {
            btnExcluir.BackColor = ColorTranslator.FromHtml("#004C99");
        }

        private void btnExcluir_MouseLeave(object sender, EventArgs e)
        {
            btnExcluir.BackColor = ColorTranslator.FromHtml("#0066CC");
        }

        // btn Login
        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin formLogin = new FormLogin();
            formLogin.Show();
        }

        private void btnLogin_MouseEnter(object sender, EventArgs e)
        {
            btnLogin.BackColor = ColorTranslator.FromHtml("#004C99");
        }

        private void btnLogin_MouseLeave(object sender, EventArgs e)
        {
            btnLogin.BackColor = ColorTranslator.FromHtml("#0066CC");
        }

        // Animação do Menu
        private void Form1_Load(object sender, EventArgs e)
        {
            btnCadastrar.Visible = false;
            btnListar.Visible = false;
            btnEditar.Visible = false;
            btnExcluir.Visible = false;
            btnLogin.Visible = false;
        }

        bool menuExpandido = false;

        private void btnMenu_Click(object sender, EventArgs e)
        {
            timerMenu.Start();
        }

        private void timerMenu_Tick(object sender, EventArgs e)
        {
            if (menuExpandido)
            {
                panelMenu.Width -= 10;
                if (panelMenu.Width <= 59)
                {
                    timerMenu.Stop();
                    menuExpandido = false;

                    btnCadastrar.Visible = false;
                    btnListar.Visible = false;
                    btnEditar.Visible = false;
                    btnExcluir.Visible = false;
                    btnLogin.Visible = false;

                }
            }
            else
            {
                panelMenu.Width += 10;
                if (panelMenu.Width >= 148)
                {
                    timerMenu.Stop();
                    menuExpandido = true;

                    btnCadastrar.Visible = true;
                    btnListar.Visible = true;
                    btnEditar.Visible = true;
                    btnExcluir.Visible = true;
                    btnLogin.Visible = true;
                }
            }
        }

        //User controls
        private void AbrirTela(UserControl tela)
        {
            panelContainer.Controls.Clear();
            tela.Dock = DockStyle.Fill;
            panelContainer.Controls.Add(tela);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
            }
        }
    }
}
