using MySql.Data.MySqlClient;
using SeuProjeto;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Projeto_Dois_CRUD
{
    public partial class TelaEditar : UserControl
    {
        private string caminhoImagem = "";
        private int produtoIdAtual = -1;

        public TelaEditar()
        {
            InitializeComponent();

            cbTipo.Items.AddRange(new string[]
           {
                "Cachaça", "Vodka", "Whisky", "Rum", "Gin", "Tequila", "Vinho", "Cerveja", "Hidromel",
                "Licor de Amêndoa", "Licor de Menta", "Licor de Chocolate", "Energético", "Refrigerante",
                "Água Tônica", "Água Mineral", "Suco Natural"
           });
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text.Trim(), out int id))
            {
                MessageBox.Show("ID inválido.");
                return;
            }

            try
            {
                using (var conn = Conexao.ObterConexao())
                {
                    string sql = "SELECT * FROM produtos WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                produtoIdAtual = id;
                                txtNome.Text = reader["nome"].ToString();
                                cbTipo.SelectedItem = reader["tipo"].ToString();
                                txtTeor.Text = reader["teor_alcoolico"].ToString();
                                txtPreco.Text = reader["preco"].ToString();
                                numEstoque.Value = Convert.ToInt32(reader["estoque"]);
                                caminhoImagem = reader["imagem"].ToString();

                                if (File.Exists(caminhoImagem))
                                {
                                    picFoto.Image = Image.FromFile(caminhoImagem);
                                    picFoto.SizeMode = PictureBoxSizeMode.Zoom;
                                }
                                else
                                {
                                    picFoto.Image = null;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Produto não encontrado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar produto: " + ex.Message);
            }
        }

        private void btnSelecionarImagem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Imagens (*.jpg;*.png)|*.jpg;*.png";

            if (open.ShowDialog() == DialogResult.OK)
            {
                caminhoImagem = open.FileName;
                picFoto.Image = Image.FromFile(caminhoImagem);
                picFoto.SizeMode = PictureBoxSizeMode.Zoom;
            }
        }

        private void btnRemoverImagem_Click(object sender, EventArgs e)
        {
            caminhoImagem = "";
            picFoto.Image = null;
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            lblMensagem.Text = "";

            if (!int.TryParse(txtId.Text.Trim(), out int id))
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "ID inválido.";
                return;
            }
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "Informe o nome.";
                return;
            }
            if (cbTipo.SelectedIndex < 0)
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "Selecione o tipo.";
                return;
            }

            decimal teor = 0;
            if (!string.IsNullOrWhiteSpace(txtTeor.Text) && !decimal.TryParse(txtTeor.Text, out teor))
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "Teor alcoólico inválido.";
                return;
            }
            decimal preco = 0;
            if (!string.IsNullOrWhiteSpace(txtPreco.Text) && !decimal.TryParse(txtPreco.Text, out preco))
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "Preço inválido.";
                return;
            }

            try
            {
                using (MySqlConnection conn = Conexao.ObterConexao())
                {
                    string sql = @"UPDATE produtos SET 
                           nome = @nome, tipo = @tipo, teor_alcoolico = @teor, 
                           preco = @preco, estoque = @estoque, imagem = @imagem
                           WHERE id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", txtNome.Text.Trim());
                        cmd.Parameters.AddWithValue("@tipo", cbTipo.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@teor", string.IsNullOrWhiteSpace(txtTeor.Text) ? (object)DBNull.Value : teor);
                        cmd.Parameters.AddWithValue("@preco", string.IsNullOrWhiteSpace(txtPreco.Text) ? (object)DBNull.Value : preco);
                        cmd.Parameters.AddWithValue("@estoque", (int)numEstoque.Value);
                        cmd.Parameters.AddWithValue("@imagem", caminhoImagem);
                        cmd.Parameters.AddWithValue("@id", id);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            lblMensagem.ForeColor = Color.Green;
                            lblMensagem.Text = "Produto atualizado com sucesso!";
                        }
                        else
                        {
                            lblMensagem.ForeColor = Color.Red;
                            lblMensagem.Text = "Produto não encontrado para atualização.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMensagem.ForeColor = Color.Red;
                lblMensagem.Text = "Erro ao atualizar: " + ex.Message;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtId.Clear();
            txtNome.Clear();
            cbTipo.SelectedIndex = -1;
            txtTeor.Clear();
            txtPreco.Clear();
            numEstoque.Value = 0;
            picFoto.Image = null;
            caminhoImagem = "";
            lblMensagem.Text = "";
        }
    }
}
