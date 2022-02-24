using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Futbol2
{
    public partial class Form1 : Form
    {

        SqlConnection conn = new SqlConnection("Data Source = localhost; Initial Catalog = futbol; Integrated Security = True");

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn.Open();

            cargarTablaJug();
            cargarPosDis();
            //agregarJug("Cristiano Ronaldo", 7, 91);
        }

        public void cargarTablaJug()
        {
            SqlCommand comando = new SqlCommand("SELECT * FROM jugadores;", conn);
            SqlDataAdapter adaptador = new SqlDataAdapter();
            adaptador.SelectCommand = comando;
            DataTable tabla = new DataTable();
            adaptador.Fill(tabla);
            dataGridView1.DataSource = tabla;
        }
        
        public void agregarJug (string nombre, int dorsal, int puntuacion)
        {
            int idJug = 0;

            string query = "INSERT INTO jugadores (nombre, dorsal, puntuacion, created_at, updated_at) VALUES ('" + nombre + "'," +
                "'" + dorsal + "', '" + puntuacion + "', CURRENT_TIMESTAMP, CURRENT_TIMESTAMP);";
            SqlCommand comando = new SqlCommand(query, conn);
            comando.ExecuteNonQuery();
            cargarTablaJug();

            string query2 = "SELECT id FROM jugadores ORDER BY id DESC";
            SqlCommand comando2 = new SqlCommand(query2, conn);
            SqlDataReader Reader = comando2.ExecuteReader();

            if (Reader.Read())
            {
                idJug = Int32.Parse(Reader[0].ToString());
            } 
            else
            {
                MessageBox.Show("ERROR: no se encontraron datos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Reader.Close();

            SqlCommand comando3;

            for (int i = 0; i < checkedListBox1.CheckedIndices.Count; i++)
            {
                string query3 = "INSERT INTO pivote_puestos_jugadores(puesto_id, jugador_id) VALUES ('" + Int32.Parse((checkedListBox1.CheckedIndices[i] + 1).ToString()) + "', '" + idJug.ToString() + "');";
                comando3 = new SqlCommand(query3, conn);
                comando3.ExecuteNonQuery();
            }

        }

        public void cargarPosDis ()
        {
            string query = "SELECT puesto FROM puestos;";
            SqlCommand comando = new SqlCommand(query, conn);
            SqlDataReader Reader = comando.ExecuteReader();
            while (Reader.Read())
            {
                checkedListBox1.Items.Add(Reader[0].ToString());
            }

            Reader.Close();
        }

        private void btnAddJug_Click(object sender, EventArgs e)
        {
            agregarJug("Messi", 10, 93);
        }
    }
}
