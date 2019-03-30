using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace urna_eletronica
{
    public partial class ViewCandidatos : Form
    {
        Dictionary<string, Image> listaCandidatos;
        Dictionary<Image, string> nomeCandidatos;
        Dictionary<string, string> cargos;
        public ViewCandidatos(Dictionary<string, Image> listaCandidatos, Dictionary<string, string> cargos, Dictionary<Image, string> nomeCandidatos)
        {
            InitializeComponent();
            this.listaCandidatos = listaCandidatos;
            this.cargos = cargos;
            this.nomeCandidatos = nomeCandidatos;
        }

        private void ViewCandidatos_Load(object sender, EventArgs e)
        {
            if (listaCandidatos != null)
            {
                foreach (string numero in listaCandidatos.Keys)
                {
                    if (cargos.ContainsKey(numero) && cargos[numero] == "presidente")
                    {

                        foreach (Control control in groupBoxPresidente.Controls)
                        {
                            Image img = listaCandidatos[numero];
                            if (control is PictureBox)
                                if (((PictureBox)control).Image == null && !containImage(groupBoxPresidente, listaCandidatos[numero]))
                                    ((PictureBox)control).Image = img;
                            if (control is Label)
                                if (((Label)control).Text.Contains("label") &&!containLabel(groupBoxPresidente, nomeCandidatos[img] + ": " + numero))
                                    ((Label)control).Text = nomeCandidatos[img] + ": " + numero;
                        }
                    }
                    else if (cargos.ContainsKey(numero) && cargos[numero] == "governador")
                    {
                        Image img = listaCandidatos[numero];
                        foreach (Control control in groupBoxGovernador.Controls)
                        {
                            if (control is PictureBox)
                                if (((PictureBox)control).Image == null && !containImage(groupBoxGovernador, listaCandidatos[numero]))
                                    ((PictureBox)control).Image = img;
                            if (control is Label)
                                if (((Label)control).Text.Contains("label") && !containLabel(groupBoxGovernador, nomeCandidatos[img] + ": " + numero))
                                    ((Label)control).Text = nomeCandidatos[img] + ": " + numero;
                        }
                    }
                    else if (cargos.ContainsKey(numero) && cargos[numero] == "senador")
                    {
                        Image img = listaCandidatos[numero];
                        foreach (Control control in groupBoxSenador.Controls)
                        {
                            if (control is PictureBox)
                                if (((PictureBox)control).Image == null && !containImage(groupBoxSenador, listaCandidatos[numero]))
                                    ((PictureBox)control).Image = img;
                            if (control is Label)
                                if (((Label)control).Text.Contains("label") && !containLabel(groupBoxSenador, nomeCandidatos[img] + ": " + numero))
                                    ((Label)control).Text = nomeCandidatos[img] + ": " + numero;
                        }
                    }
                    else if (cargos.ContainsKey(numero) && cargos[numero] == "estadual")
                    {
                        Image img = listaCandidatos[numero];
                        foreach (Control control in groupBoxEstadual.Controls)
                        {
                            if (control is PictureBox)
                                if (((PictureBox)control).Image == null && !containImage(groupBoxEstadual, listaCandidatos[numero]))
                                    ((PictureBox)control).Image = img;
                            if (control is Label)
                                if (((Label)control).Text.Contains("label") && !containLabel(groupBoxEstadual, nomeCandidatos[img] + ": " + numero))
                                    ((Label)control).Text = nomeCandidatos[img] + ": " + numero;
                        }

                    }
                    else if (cargos.ContainsKey(numero) && cargos[numero] == "federal")
                    {
                        Image img = listaCandidatos[numero];
                        foreach (Control control in groupBoxFederal.Controls)
                        {
                            if (control is PictureBox)
                                if (((PictureBox)control).Image == null && !containImage(groupBoxFederal, listaCandidatos[numero]))
                                    ((PictureBox)control).Image = img;
                            if (control is Label)
                                if (((Label)control).Text.Contains("label") && !containLabel(groupBoxFederal, nomeCandidatos[img] + ": " + numero))
                                    ((Label)control).Text = nomeCandidatos[img] + ": " + numero;
                        }
                    }
                }//fim do for
            }//fim list null
        }

        private bool containImage(GroupBox groupBox, Image img)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is PictureBox)
                    if (((PictureBox)control).Image == img)
                        return true;
            }
            return false;
        }

        private bool containLabel(GroupBox groupBox, string numero)
        {
            foreach (Control control in groupBox.Controls)
            {
                if (control is Label)
                    if (((Label)control).Text == numero)
                        return true;
            }
            return false;
        }
    }
}
