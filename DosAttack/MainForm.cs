using System;
using System.Windows.Forms;

namespace DosAttack
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnSynAttack_Click(object sender, EventArgs e)
        {
            try
            {
                var synSender = new SynSender();
                synSender.Send(tbIpAddress.Text, (short)nudPort.Value, attack: false);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }
    }
}
