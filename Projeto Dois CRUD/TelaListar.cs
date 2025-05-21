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
    public partial class TelaListar : UserControl
    {
        public TelaListar()
        {
            InitializeComponent();

            // Configurar tipos no ComboBox ao inicializar
            cbTipo.Items.Add("Todos");
            cbTipo.Items.AddRange(new string[]
            {
                "Cachaça",
                "Vodka",
                "Whisky",
                "Rum",
                "Gin",
                "Tequila",
                "Vinho",
                "Cerveja",
                "Hidromel",
                "Licor de Amêndoa",
                "Licor de Menta",
                "Licor de Chocolate",
                "Energético",
                "Refrigerante",
                "Água Tônica",
                "Água Mineral",
                "Suco Natural"
            });

            cbTipo.SelectedIndex = 0;

            // Eventos para carregar os dados ao alterar os filtros
            cbTipo.SelectedIndexChanged += Filtros_Alterados;
            txtNomeFiltro.TextChanged += Filtros_Alterados;

            // Evento para quando o usuário selecionar uma linha no DataGridView
            DgvProdutos.SelectionChanged += DgvProdutos_SelectionChanged;

            // Configurações básicas do DataGridView
            DgvProdutos.ReadOnly = true;
            DgvProdutos.AllowUserToAddRows = false;
            DgvProdutos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Carregar produtos inicialmente
            CarregarProdutos();
        }

        private void Filtros_Alterados(object sender, EventArgs e)
        {
            CarregarProdutos();
        }

        private void CarregarProdutos()
        {
            string tipoSelecionado = cbTipo.SelectedItem?.ToString();
            string nomeFiltro = txtNomeFiltro.Text.Trim();

            try
            {
                using (MySqlConnection conn = Conexao.ObterConexao())
                {
                    string sql = @"SELECT id, nome, tipo, teor_alcoolico, preco, estoque, imagem 
                                   FROM produtos WHERE 1=1";

                    if (!string.IsNullOrEmpty(nomeFiltro))
                    {
                        sql += " AND nome LIKE @nome";
                    }
                    if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
                    {
                        sql += " AND tipo = @tipo";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                    {
                        if (!string.IsNullOrEmpty(nomeFiltro))
                            cmd.Parameters.AddWithValue("@nome", $"%{nomeFiltro}%");
                        if (!string.IsNullOrEmpty(tipoSelecionado) && tipoSelecionado != "Todos")
                            cmd.Parameters.AddWithValue("@tipo", tipoSelecionado);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);

                            DgvProdutos.DataSource = dt;

                            AtualizarEstatisticas(dt);
                            LimparImagemProduto();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar produtos: " + ex.Message);
            }
        }

        private void AtualizarEstatisticas(DataTable dt)
        {
            int totalProdutos = dt.Rows.Count;
            int somaEstoque = 0;
            decimal somaPreco = 0;

            foreach (DataRow row in dt.Rows)
            {
                int estoque = row["estoque"] != DBNull.Value ? Convert.ToInt32(row["estoque"]) : 0;
                decimal preco = row["preco"] != DBNull.Value ? Convert.ToDecimal(row["preco"]) : 0;

                somaEstoque += estoque;
                somaPreco += preco;
            }

            decimal mediaPreco = totalProdutos > 0 ? somaPreco / totalProdutos : 0;

            lblTotal.Text = $"Total de Produtos: {totalProdutos}";
            lblEstoque.Text = $"Estoque Total: {somaEstoque}";
            lblMediaPreco.Text = $"Preço Médio: R$ {mediaPreco:F2}";
        }

        private void DgvProdutos_SelectionChanged(object sender, EventArgs e)
        {
            // Seu código para tratar a seleção, por exemplo:
            if (DgvProdutos.SelectedRows.Count > 0)
            {
                DataGridViewRow row = DgvProdutos.SelectedRows[0];
                string caminhoImagem = row.Cells["imagem"].Value?.ToString();

                if (!string.IsNullOrEmpty(caminhoImagem) && System.IO.File.Exists(caminhoImagem))
                {
                    try
                    {
                        using (var imgTemp = Image.FromFile(caminhoImagem))
                        {
                            picProduto.Image = new Bitmap(imgTemp);
                            picProduto.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    catch
                    {
                        picProduto.Image = null;
                    }
                }
                else
                {
                    picProduto.Image = null;
                }
            }
            else
            {
                picProduto.Image = null;
            }
        }

        private void LimparImagemProduto()
        {
            picProduto.Image = null;
        }
    }
}
