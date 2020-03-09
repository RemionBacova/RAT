using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;

namespace Client
{
    public partial class frmClient : Form
    {
        Process Server;

        Service.SoapServer a;

       
        public frmClient()
        {
            InitializeComponent();

            a = new Service.SoapServer();
      
            MerListen();


           
            
        }
        #region oldClient
        

      
        #endregion


     

   
   

      

        

        private void button2_Click(object sender, EventArgs e)// vendosi te gjithe qe te updaten infot
        {
           
            a.SetAllForUpdate();
         
        }

        

        private void button4_Click(object sender, EventArgs e)// vendos te zgjedhurin qe te update infot
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
             
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    a.SetForUpdate(dataGridView1.SelectedRows[i].Cells[0].Value.ToString());
                }
            }
        }

        private void button10_Click(object sender, EventArgs e)// refresh listen
        {
            MerListen();
        }

        private void MerListen()
        {
          
           dataGridView1.DataSource =  a.SelectUsers().Copy();
           dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        }

      

        private void button12_Click(object sender, EventArgs e)
        {
           
            a.SetAllForNoUpdate();
          
        }

      

        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
               
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    a.SetForNoUpdate(dataGridView1.SelectedRows[i].Cells[0].Value.ToString());
                }
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Process.Start(@"http://ip-api.com/#" + dataGridView1.SelectedRows[0].Cells[3].Value.ToString());
            }
        }


    }
   
}
