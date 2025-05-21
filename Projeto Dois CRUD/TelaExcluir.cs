using MySql.Data.MySqlClient;
using SeuProjeto;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Projeto_Dois_CRUD
{
    public partial class TelaExcluir : UserControl
    {
        private int produtoId = -1;
        private string caminhoImagem = "";

        public TelaExcluir()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtId.Text.Trim(), out int id))
            {
                MessageBox.Show("ID inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                produtoId = id;
                                lblNome.Text = "Nome: " + reader["nome"].ToString();
                                lblTipo.Text = "Tipo: " + reader["tipo"].ToString();
                                lblTeor.Text = "Teor Alcoólico: " + reader["teor_alcoolico"].ToString();
                                lblPreco.Text = "Preço: R$ " + reader["preco"].ToString();
                                lblEstoque.Text = "Estoque: " + reader["estoque"].ToString();
                                caminhoImagem = reader["imagem"].ToString();

                                if (!string.IsNullOrEmpty(caminhoImagem) && File.Exists(caminhoImagem))
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
                                MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                LimparCampos();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (produtoId == -1)
            {
                MessageBox.Show("Nenhum produto selecionado. Busque pelo ID antes de excluir.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmar = MessageBox.Show("Tem certeza que deseja excluir este produto?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                using (MySqlConnection conn = Conexao.ObterConexao())
                {
                    string sql = "DELETE FROM produtos WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", produtoId);

                        int rows = cmd.ExecuteNonQuery();

                        if (rows > 0)
                        {
                            MessageBox.Show("Produto excluído com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LimparCampos();
                        }
                        else
                        {
                            MessageBox.Show("Produto não encontrado para exclusão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao excluir produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparCampos();
        }

        private void LimparCampos()
        {
            txtId.Clear();
            lblNome.Text = "Nome:";
            lblTipo.Text = "Tipo:";
            lblTeor.Text = "Teor Alcoólico:";
            lblPreco.Text = "Preço:";
            lblEstoque.Text = "Estoque:";
            picFoto.Image = null;
            produtoId = -1;
            caminhoImagem = "";
        }
    }
}
