using MySql.Data.MySqlClient;
using SeuProjeto;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto_Dois_CRUD
{
    public partial class TelaCadastrar : UserControl
    {
        public TelaCadastrar()
        {
            InitializeComponent();
            cbTipo.Items.AddRange(new string[] { "Cachaça", "Vinho", "Vodka", "Licor", "Cerveja", "Outros" });

            // Configura a PictureBox para exibir a imagem ajustada
            picFoto.SizeMode = PictureBoxSizeMode.Zoom;
        }

        string caminhoImagem = "";

        private void btnSelecionar_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Imagens (*.jpg;*.png)|*.jpg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                caminhoImagem = open.FileName;

                // Carrega a imagem usando Bitmap para liberar o arquivo
                using (var imgTemp = Image.FromFile(caminhoImagem))
                {
                    picFoto.Image = new Bitmap(imgTemp);
                }
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string tipo = cbTipo.SelectedItem?.ToString();
            string teor = txtTeor.Text.Trim();
            string preco = txtPreco.Text.Trim();
            int estoque = (int)numEstoque.Value;

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(tipo))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios.");
                return;
            }

            try
            {
                using (MySqlConnection conn = Conexao.ObterConexao())
                {
                    string sql = @"INSERT INTO produtos 
                (nome, tipo, teor_alcoolico, preco, estoque, imagem) 
                VALUES (@nome, @tipo, @teor, @preco, @estoque, @imagem)";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", nome);
                        cmd.Parameters.AddWithValue("@tipo", tipo);

                        object valorTeor = DBNull.Value;
                        if (!string.IsNullOrEmpty(teor))
                            valorTeor = Convert.ToDecimal(teor);

                        object valorPreco = DBNull.Value;
                        if (!string.IsNullOrEmpty(preco))
                            valorPreco = Convert.ToDecimal(preco);

                        cmd.Parameters.AddWithValue("@teor", valorTeor);
                        cmd.Parameters.AddWithValue("@preco", valorPreco);

                        cmd.Parameters.AddWithValue("@estoque", estoque);
                        cmd.Parameters.AddWithValue("@imagem", caminhoImagem);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Produto cadastrado com sucesso!");

                        LimparCampos();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message);
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            cbTipo.SelectedIndex = -1;
            txtTeor.Clear();
            txtPreco.Clear();
            numEstoque.Value = 0;
            picFoto.Image = null;
            caminhoImagem = "";
        }
    }
}
