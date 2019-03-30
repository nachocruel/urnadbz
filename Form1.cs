using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace urna_eletronica
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private bool estadual = true;
        private bool federal = false;
        private bool senado = false;
        private bool governo = false;
        private bool presidente = false;
        List<Image> imgCandidatos = new List<Image>();
        Dictionary<string, Image> candidatosNumeros = new Dictionary<string, Image>();
        Dictionary<Image, string> nomeCandidatos = new Dictionary<Image, string>();
        Dictionary<string, string> cargos = new Dictionary<string, string>();

        private void reinicia()
        {
            estadual = true;
            federal = false;
            senado = false;
            governo = false;
            pictureBoxFim.Visible = false;
            presidente = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            insertNumero(button1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            insertNumero(button2.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            insertNumero(button3.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            insertNumero(button4.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            insertNumero(button5.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            insertNumero(button6.Text);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            insertNumero(button7.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            insertNumero(button8.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            insertNumero(button9.Text);
        }

        private void button0_Click(object sender, EventArgs e)
        {
            insertNumero(button0.Text);
        }

        private void buttonCorrigir_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void insertNumero(string numero)
        {
            if (federal && checarTamanhoFederal())
            {
                richTextBox1.Text = richTextBox1.Text + numero;
            }
            else if (estadual && checarTamanhoEstadual())
            {
                richTextBox1.Text = richTextBox1.Text + numero;
            }
            else if (senado && checarTamanhoSenador())
            {
                richTextBox1.Text = richTextBox1.Text + numero;
            }
            else if (governo && checarTamanhoGevernadorPresidente())
            {
                richTextBox1.Text = richTextBox1.Text + numero;
            }
            else if (presidente && checarTamanhoGevernadorPresidente())
            {
                richTextBox1.Text = richTextBox1.Text + numero;
            }
        }

        //Verifica tamanho do numeros
        private bool checarTamanhoEstadual()
        {
            return richTextBox1.Text.Length < 5;
        }

        private bool checarTamanhoFederal()
        {
            return richTextBox1.Text.Length < 4;
        }

        private bool checarTamanhoSenador()
        {
            return richTextBox1.Text.Length < 3;
        }

        private bool checarTamanhoGevernadorPresidente()
        {
            return richTextBox1.Text.Length < 2;
        }

        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 5 && this.estadual)
            {
                this.estadual = false;
                this.federal = true;
                gravarVoto(richTextBox1.Text);
                labelEscolha.Text = "Escolha seu Deputado Federal";
                buttonCorrigir.PerformClick();
            }
            if (richTextBox1.Text.Length == 4 && this.federal)
            {
                this.federal = false;
                this.senado = true;
                gravarVoto(richTextBox1.Text);
                labelEscolha.Text = "Escolha seu Senador";
                buttonCorrigir.PerformClick();
            }
            if (richTextBox1.Text.Length == 3 && this.senado)
            {
                this.senado = false;
                this.governo = true;
                gravarVoto(richTextBox1.Text);
                labelEscolha.Text = "Escolha seu Governador";
                buttonCorrigir.PerformClick();
            }

            if (richTextBox1.Text.Length == 2 && governo && !presidente)
            {
                governo = false;
                presidente = true;
                gravarVoto(richTextBox1.Text);
                labelEscolha.Text = "Escolha seu Presidente";
                buttonCorrigir.PerformClick();
            }

            if (richTextBox1.Text.Length == 2 && presidente && !governo)
            {
                gravarVoto(richTextBox1.Text);
                Thread t = new Thread(() =>
                {
                    Invoke(new Action(() =>
                    {
                        pictureBoxFim.Visible = true;
                        labelEscolha.Text = "Escolha seu Deputado Estadual";
                    }));
                    Invoke(new Action(() =>
                    {
                        Thread.Sleep(5000);
                        buttonCorrigir.PerformClick();
                        reinicia();
                    }));

                });
                t.Start();
            }
        }

        public void gravarVoto(string voto)
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\votos_candidatos.txt";
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine(voto);
            }
        }

        public void apurarVotos()
        {
            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\votos_candidatos.txt";
            string apuracaoPath = Directory.GetCurrentDirectory() + "\\resultado.txt";

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Image imgCandidato = candidatosNumeros[line];
                    string nome = nomeCandidatos[imgCandidato];
                    bool encontrou = false;
                    string linhaParaGravar = "";
                    int count = 0;

                    using (StreamReader novaSr = new StreamReader(apuracaoPath))
                    {
                        string novaLinha;
                        while ((novaLinha = novaSr.ReadLine()) != null)
                        {
                            string[] arrayLinha = novaLinha.Split(':');
                            if (arrayLinha[0] == nome)
                            {
                                encontrou = true;
                                int quantidadeVotos = int.Parse(arrayLinha[1]) + 1;
                                linhaParaGravar = nome + ":" + quantidadeVotos;
                            }
                            if (!encontrou)
                                count++;
                        }
                    }

                    if (!encontrou)
                    {
                        using (StreamWriter sw = new StreamWriter(apuracaoPath, true))
                        {
                            sw.WriteLine(nome + ":" + 1);
                        }
                    }
                    else
                    {
                        using (StreamWriter sw = new StreamWriter(apuracaoPath, true))
                        {
                            sw.WriteLine(linhaParaGravar, count);
                        }
                    }
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Random random = new Random();
            carregarCandidatos();
            string filePath = Directory.GetCurrentDirectory();
            filePath += "\\votos_candidatos.txt";
            if (!File.Exists(filePath))
                File.Create(filePath);
            else
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.Write("");
                }

            string apuracaoPath = Directory.GetCurrentDirectory() + "\\resultado.txt";
            if (!File.Exists(apuracaoPath))
                File.Create(apuracaoPath);
            else
                using (StreamWriter sw = new StreamWriter(apuracaoPath))
                {
                    sw.Write("");
                }

            int quantidadeDigitos = 2;
            int reapet = 0;
            while (reapet != 5)
            {
                for (int i = 0; i < 4; i++)
                {
                    string numero = gerarNumeroCandidato(quantidadeDigitos);
                    while (candidatosNumeros.ContainsKey(numero))
                    {
                        numero = gerarNumeroCandidato(quantidadeDigitos);
                    }


                    Image imgCandidato = imgCandidatos[random.Next(0, 29)];
                    while (candidatosNumeros.ContainsValue(imgCandidato))
                    {
                        imgCandidato = imgCandidatos[random.Next(0, 29)];
                    }

                    candidatosNumeros.Add(numero, imgCandidato);

                    if (candidatosNumeros.Count <= 4 && numero.Length == 2)
                        cargos.Add(numero, "presidente");
                    else if (candidatosNumeros.Count > 4 && candidatosNumeros.Count < 9 && numero.Length == 2)
                        cargos.Add(numero, "governador");
                    else if (numero.Length == 3)
                        cargos.Add(numero, "senador");
                    else if (numero.Length == 4)
                        cargos.Add(numero, "federal");
                    else if (numero.Length == 5)
                        cargos.Add(numero, "estadual");
                }

                if (candidatosNumeros.Count > 4)
                    quantidadeDigitos++;
                reapet++;
            }
        }

        private void carregarCandidatos()
        {
            Image android17 = Properties.Resources.android17;
            nomeCandidatos.Add(android17, "Androide nº17");
            imgCandidatos.Add(android17);
            //androi 18
            Image android18 = Properties.Resources.android18;
            nomeCandidatos.Add(android18, "Androide nº18");
            imgCandidatos.Add(android18);
            //bills
            Image bills = Properties.Resources.bills;
            nomeCandidatos.Add(bills, "Bills");
            imgCandidatos.Add(bills);
            //bulma
            Image bulma = Properties.Resources.bulma;
            nomeCandidatos.Add(bulma, "Bulma");
            imgCandidatos.Add(bulma);
            //freeza
            Image freeza = Properties.Resources.freeza;
            nomeCandidatos.Add(freeza, "Freeza");
            imgCandidatos.Add(freeza);
            //gohan
            Image gohan = Properties.Resources.gohan;
            nomeCandidatos.Add(gohan, "Gohan");
            imgCandidatos.Add(gohan);
            //goku
            Image goku = Properties.Resources.goku;
            nomeCandidatos.Add(goku, "Goku");
            imgCandidatos.Add(goku);
            //jiren
            Image jiren = Properties.Resources.jiren;
            nomeCandidatos.Add(jiren, "Jiren");
            imgCandidatos.Add(jiren);
            //krillin
            Image krillin = Properties.Resources.krillin;
            nomeCandidatos.Add(krillin, "Krillin");
            imgCandidatos.Add(krillin);
            //majinboo
            Image majinboo = Properties.Resources.majinboo;
            nomeCandidatos.Add(majinboo, "Majin Boo");
            imgCandidatos.Add(majinboo);
            //mestre kame
            Image mestrekame = Properties.Resources.mestrekame;
            nomeCandidatos.Add(mestrekame, "Mestre Kame");
            imgCandidatos.Add(mestrekame);
            //piccolo
            Image piccolo = Properties.Resources.piccolo;
            nomeCandidatos.Add(piccolo, "Piccolo");
            imgCandidatos.Add(piccolo);
            //raditz
            Image raditz = Properties.Resources.raditz;
            nomeCandidatos.Add(raditz, "Raditz");
            imgCandidatos.Add(raditz);
            //rikum
            Image rikum = Properties.Resources.rikum;
            nomeCandidatos.Add(rikum, "Rikum");
            imgCandidatos.Add(rikum);
            //sr popo
            Image sr_popo = Properties.Resources.sr_popo;
            nomeCandidatos.Add(sr_popo, "Sr. Popo");
            imgCandidatos.Add(sr_popo);
            //tal pai pai
            Image talpaipai = Properties.Resources.talpaipai;
            nomeCandidatos.Add(talpaipai, "Tal Pai Pai");
            imgCandidatos.Add(talpaipai);
            //tenchinjan
            Image tenchinjan = Properties.Resources.tenchinjan;
            nomeCandidatos.Add(tenchinjan, "Tenchinjan");
            imgCandidatos.Add(tenchinjan);
            //titi
            Image titi = Properties.Resources.titi;
            nomeCandidatos.Add(titi, "Titi");
            imgCandidatos.Add(titi);
            //trunks
            Image trunks = Properties.Resources.trunks;
            nomeCandidatos.Add(trunks, "Trunks");
            imgCandidatos.Add(trunks);
            //boter
            Image boter = Properties.Resources.boter;
            nomeCandidatos.Add(boter, "Boter");
            imgCandidatos.Add(boter);
            //capitão ginyu
            Image capitaoginyu = Properties.Resources.capitaoginyu;
            nomeCandidatos.Add(capitaoginyu, "Capitão Ginyu");
            imgCandidatos.Add(capitaoginyu);
            //dodoria
            Image dodoria = Properties.Resources.dodoria;
            nomeCandidatos.Add(dodoria, "Dodoria");
            imgCandidatos.Add(dodoria);
            //goten
            Image goten = Properties.Resources.goten;
            nomeCandidatos.Add(goten, "Goten");
            imgCandidatos.Add(goten);
            //gotenks
            Image gotenks = Properties.Resources.gotenks;
            nomeCandidatos.Add(gotenks, "Gotenks");
            imgCandidatos.Add(gotenks);
            //gurdo
            Image gurdo = Properties.Resources.gurdo;
            nomeCandidatos.Add(gurdo, "Gurdo");
            imgCandidatos.Add(gurdo);
            //vegetto
            Image vegetto = Properties.Resources.vegetto;
            nomeCandidatos.Add(vegetto, "Vegetto");
            imgCandidatos.Add(vegetto);
            //vegeta
            Image vegeta = Properties.Resources.vegeta;
            nomeCandidatos.Add(vegeta, "Vegeta");
            imgCandidatos.Add(vegeta);
            //yuz
            Image yuz = Properties.Resources.yuz;
            nomeCandidatos.Add(yuz, "Yuz");
            imgCandidatos.Add(yuz);
            //zarbon
            Image zarbon = Properties.Resources.zarbon;
            nomeCandidatos.Add(zarbon, "Zarbon");
            imgCandidatos.Add(zarbon);
        }

        private string gerarNumeroCandidato(int quantidadeDedigitos)
        {
            Random random = new Random();
            string numeroCandidato = "";
            for (int i = 0; i < quantidadeDedigitos; i++)
            {
                if (numeroCandidato.Length == 0)
                    numeroCandidato += random.Next(1, 9);
                else
                    numeroCandidato += random.Next(0, 9);
            }
            return numeroCandidato;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Length == 2 && (presidente || governo))
            {
                pictureImgFundo.Visible = false;
                labelEscolha.Visible = false;
                pictureImgCandidato.Visible = true;
                labelCargo.Visible = true;
                labelInfoCargo.Visible = true;
                labelCandidato.Visible = true;
                labelNome.Visible = true;

                if (candidatosNumeros.ContainsKey(richTextBox1.Text) && cargos.ContainsKey(richTextBox1.Text))
                {
                    pictureImgCandidato.Image = candidatosNumeros[richTextBox1.Text];
                    labelCargo.Text = cargos[richTextBox1.Text];
                    labelNome.Text = nomeCandidatos[candidatosNumeros[richTextBox1.Text]];
                }
                else
                {
                    pictureImgCandidato.Image = Properties.Resources.avatar;
                    labelCargo.Text = "Nulo";
                    labelNome.Text = "";
                }
            }
            else if (richTextBox1.Text.Length == 3 && senado)
            {
                pictureImgFundo.Visible = false;
                labelEscolha.Visible = false;
                pictureImgCandidato.Visible = true;
                labelCargo.Visible = true;
                labelInfoCargo.Visible = true;
                labelCandidato.Visible = true;
                labelNome.Visible = true;
                if (candidatosNumeros.ContainsKey(richTextBox1.Text) && cargos.ContainsKey(richTextBox1.Text))
                {
                    pictureImgCandidato.Image = candidatosNumeros[richTextBox1.Text];
                    labelCargo.Text = cargos[richTextBox1.Text];
                    labelNome.Text = nomeCandidatos[candidatosNumeros[richTextBox1.Text]];
                }
                else
                {
                    pictureImgCandidato.Image = Properties.Resources.avatar;
                    labelCargo.Text = "Nulo";
                    labelNome.Text = "";
                }
            }
            else if (richTextBox1.Text.Length == 4 && federal)
            {
                pictureImgFundo.Visible = false;
                labelEscolha.Visible = false;
                pictureImgCandidato.Visible = true;
                labelCargo.Visible = true;
                labelInfoCargo.Visible = true;
                labelCandidato.Visible = true;
                labelNome.Visible = true;
                if (candidatosNumeros.ContainsKey(richTextBox1.Text) && cargos.ContainsKey(richTextBox1.Text))
                {
                    pictureImgCandidato.Image = candidatosNumeros[richTextBox1.Text];
                    labelCargo.Text = "Deputado " + cargos[richTextBox1.Text];
                    labelNome.Text = nomeCandidatos[candidatosNumeros[richTextBox1.Text]];
                }
                else
                {
                    pictureImgCandidato.Image = Properties.Resources.avatar;
                    labelCargo.Text = "Nulo";
                    labelNome.Text = "";
                }
            }
            else if (richTextBox1.Text.Length == 5 && estadual)
            {
                pictureImgFundo.Visible = false;
                labelEscolha.Visible = false;
                pictureImgCandidato.Visible = true;
                labelCargo.Visible = true;
                labelInfoCargo.Visible = true;
                labelCandidato.Visible = true;
                labelNome.Visible = true;
                if (candidatosNumeros.ContainsKey(richTextBox1.Text) && cargos.ContainsKey(richTextBox1.Text))
                {
                    pictureImgCandidato.Image = candidatosNumeros[richTextBox1.Text];
                    labelCargo.Text = "Deputado " + cargos[richTextBox1.Text];
                    labelNome.Text = nomeCandidatos[candidatosNumeros[richTextBox1.Text]];
                }
                else
                {
                    pictureImgCandidato.Image = Properties.Resources.avatar;
                    labelCargo.Text = "Nulo";
                    labelNome.Text = "";
                }
            }
            else
            {
                pictureImgFundo.Visible = true;
                labelEscolha.Visible = true;
                pictureImgCandidato.Visible = false;
                labelCargo.Visible = false;
                labelInfoCargo.Visible = false;
                labelCandidato.Visible = false;
                labelNome.Visible = false;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ViewCandidatos vc = new ViewCandidatos(candidatosNumeros, cargos, nomeCandidatos);
            vc.ShowDialog();
        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            apurarVotos();
        }
    }
}
