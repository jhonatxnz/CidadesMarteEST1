//Jhonatan Willian dos Santos Silva 21686
//Yngrid Vitória Sa Baeta           21694
using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhos
{
    public partial class FrmCidades : Form
    {
        ListaDupla<Cidade> lista1; //instanciamos a classe para utilizar seus metódos
        public FrmCidades()
        {
            InitializeComponent();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void FrmAgenda_Load(object sender, EventArgs e)
        {

            int indice = 0;
            udTempo.ImageList = imlBotoes;
            foreach (ToolStripItem item in udTempo.Items)
                if (item is ToolStripButton)
                    (item as ToolStripButton).ImageIndex = indice++;

            lista1 = new ListaDupla<Cidade>();
            if (dlgAbrir.ShowDialog() == DialogResult.OK)
            {
                lista1.LerDados(dlgAbrir.FileName); //le os dados do arquivo que o usuario escolheu
                lista1.ExibirDados(listBox2);       //exibe  os dados nos listBox2
            }
            btnSalvar.Enabled = false;  // botão salvar só é enable true quando está incluindo
        }

        private void FrmCidades_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dlgAbrir.FileName != "")    //ao fechar o programa grava os dados no txt e ordena a lista
            {
                lista1.Ordenar();
                lista1.GravarDados(dlgAbrir.FileName);
            }

        }

        private void tsInicio_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.navegando;
            lista1.PosicionarNoPrimeiro();  //posicionamos DadoAtual no primeiro no
            AtualizarTela();                //chamamos metodo AtualizarTela
        }

        private void tsAnterior_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.navegando;
            if (lista1.EstaNoInicio) //se está no primeiro nó significa que ele não possui antecessor
            {
                MessageBox.Show("Primeiro nó não possui um antecessor");
                lista1.PosicionarNoPrimeiro();
            }
            else
                lista1.RetrocederPosicao(); //retrocedemos uma posição
            AtualizarTela();            //chamamos metodo AtualizarTela
        }

        private void tsProx_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.navegando;
            if (lista1.EstaNoFim)   //se está no ultimo no significa que ele não possui sucessor
            {
                MessageBox.Show("Último nó não possui um sucessor");
                lista1.PosicionarNoUltimo();
            }
            else
                lista1.AvancarPosicao();    //avançamos uma posição
            AtualizarTela();            //chamamos metodo AtualizarTela
        }

        private void tsUltimo_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.navegando;
            lista1.PosicionarNoUltimo();    //posicionamos DadoAtual no ultimo no
            AtualizarTela();                //chamamos metodo AtualizarTela
        }
        private void btnProcurar_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.pesquisando;  // entramos no modo de busca
            LimpaTela();
            txtCodigo.ReadOnly = false;
            txtCodigo.Focus();
            slMensagem.Text = "Digite o codigo da cidade.";
        }

        private void btnNovo_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.incluindo;
            btnSalvar.Enabled = true;  // botão salvar agora é enable true pois está em situação de inclusão
            LimpaTela();
            txtCodigo.Focus();
            slMensagem.Text = "Digite as informções da cidade.";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.editando;
            LimpaTela();//limpa a tela
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.incluindo;
            if (txtNome.Text == "" || udX.Text == "" || udY.Text == "")// se algum dos campos estiver vazio não incluimos um novo dado
            {
                MessageBox.Show("Digite as informações corretamente");
                btnSalvar.Enabled = false;
                LimpaTela();
            }
            else
            {
                var dadoAserIncluido = new Cidade(txtCodigo.Text.PadLeft(3, ' '), txtNome.Text.PadRight(15, ' '), (udX.Value), (udY.Value)); //cria o dado a ser incluido
                lista1.Incluir(dadoAserIncluido);//inclui novo dado que o usuario forneceu
                lista1.ExibirDados(listBox2);//exibe os dados no listbox
                btnSalvar.Enabled = false;  //btnSalvar recebe false pois a inclusão ja foi efetuada
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            lista1.SituacaoAtual = Situacao.excluindo;
            if (MessageBox.Show("Deseja realmente excluir?", "Exclusão",
            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {

                lista1.Excluir(lista1.DadoAtual());//exclui o dado atualmente visitado
                lista1.ExibirDados(listBox2);//exibe os dados no listbox
                lista1.PosicionarNoUltimo();//posiciona no ultimo dado
                AtualizarTela();//atualiza a tela
            }
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void AtualizarTela() //metodo criado para evitar repetição de codigo
        {
            if (!lista1.EstaVazio) //se a lista não está vazia
            {
                Cidade cidade = lista1.DadoAtual();

                txtCodigo.Text = cidade.Codigo; //txtCodigo escreve o DadoAtual.Codigo
                txtNome.Text = cidade.Nome;     //txtNome escreve o DadoAtual.Nome
                udX.Value = cidade.X; //udX escreve o DadoAtual.X
                udY.Value = cidade.Y; //udY escreve o DadoAtual.Y
            }
        }
        private void LimpaTela() //metodo criado para evitar repetição de codigo
        {
            txtCodigo.Clear();
            txtNome.Clear();
            udX.Value = 0;
            udY.Value = 0;
        }
        private void listBox2_Click(object sender, EventArgs e)
        {
            lista1.PosicionarEm(listBox2.SelectedIndex); //se o usuario clicar em uma cidade da lista posicionamos o dadoAtual onde foi clicado
            AtualizarTela();                             //e exibimos na tela
        }
        private void txtCodigo_Leave(object sender, EventArgs e)
        {
            {
                /* if (!int.TryParse(txtCodigo.Text, out _))
                {
                    MessageBox.Show("Digite um valor válido!"); //um aviso será disparado
                    LimpaTela();
                    txtCodigo.Focus();
                } */
                for (int i = 0; i < txtCodigo.Text.Length; i++) //percorre todos os valores que estao no txtCodigo
                {
                    char numero = txtCodigo.Text[i]; //numero recebe o que foi digitado no txtCodigo

                    if (!char.IsNumber(numero)) //se o que foi digitado no txtCodigo não seja numero
                    {
                        MessageBox.Show("Digite um valor válido!"); //um aviso será disparado
                        LimpaTela();
                        txtCodigo.Focus();
                    }
                }
                if (lista1.SituacaoAtual == Situacao.incluindo || lista1.SituacaoAtual == Situacao.pesquisando) //se a situacao for incluindo ou pesquisando
                    if (txtCodigo.Text == "") //se o usuario não digitou no txtCodigo
                    {
                        MessageBox.Show("Digite informações válidas!");
                        txtCodigo.Focus();
                    }
                    else  // temos um valor digitado no txtCodigo
                    {
                        switch (lista1.SituacaoAtual)
                        {
                            case Situacao.incluindo:
                                btnSalvar.Enabled = true;  // situação de inclusão botao de salvar fica enable true
                                break;
                            case Situacao.pesquisando:
                                string codigo = txtCodigo.Text.PadLeft(3, ' ');
                                LimpaTela();
                                var dadoProcurado = new Cidade(codigo, txtNome.Text.PadRight(15, ' '), (udX.Value), (udY.Value)); //dado que vai ser procurado
                                if (lista1.Existe(dadoProcurado, out _)) //verifica se existe o dado passado por parametro
                                {
                                    AtualizarTela();
                                }

                                else
                                {
                                    MessageBox.Show("Dado não encontrado!");
                                }
                                break;

                        }
                    }
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Font font = new Font("Arial", 10); //define a font como Arial,tamanho 10 (utilizado para passar de parâmetro no e.Graphics.DrawString)
           
            lista1.PosicionarNoPrimeiro(); //posiciona o atua no primeiro
            while (lista1.DadoAtual() != null)//percorre a lista
            {
                Cidade cidade = lista1.DadoAtual();
                int x = (int)(cidade.X * pictureBox1.Width); //multiplica o valor de X pela largura do pictureBox
                int y = (int)(cidade.Y * pictureBox1.Height);//multiplica o valor de Y pela altura do pictureBox
                e.Graphics.FillEllipse(Brushes.Black, new Rectangle(x, y, 10, 10)); //pinta X e Y no pictureBox
                e.Graphics.DrawString(cidade.Nome, font, Brushes.Black, x, y + 10);//escreve o nome da cidade no pictureBox(y + 10 para o nome da cidade não ficar em cima da bolinha)
                lista1.AvancarPosicao(); //avança posicão
            }
            lista1.PosicionarNoPrimeiro();// posiciona no primeiro novamente por conta de um erro que tivemos zz
        }    
    }
}
