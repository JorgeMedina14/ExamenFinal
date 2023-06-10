using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Examenfinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlConnection con = new SqlConnection(@"Data Source=GMN\SQLEXPRESS;Initial Catalog=ExamenFinal;Integrated Security=True");

        public MainWindow()
        {
            InitializeComponent();
            CargarGrid();
        }

        public void CargarGrid()
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM Productos", con);
                DataTable dt = new DataTable();
                SqlDataReader sdr = cmd.ExecuteReader();
                dt.Load(sdr);
                datagrid.ItemsSource = dt.DefaultView;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        public bool ValidarDatos()
        {
            if (txtNombre.Text == string.Empty || txtDescripcion.Text == string.Empty || txtPrecio.Text == string.Empty || txtDisponible.Text == string.Empty)
            {
                MessageBox.Show("Porfavor rellenar todos los campos");
                return false;
            }
            return true;
        }

        private void BtnCrear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidarDatos())
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Productos VALUES(@Nombre, @Descripcion, @Precio, @Disponible)", con);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@Nombre", txtNombre.Text);
                    cmd.Parameters.AddWithValue("@Descripcion", txtDescripcion.Text);
                    cmd.Parameters.AddWithValue("@Precio", txtPrecio.Text);
                    cmd.Parameters.AddWithValue("@Disponible", txtDisponible.Text);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    CargarGrid();
                    MessageBox.Show("Datos Registrados Correctamente");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void BtnEliminar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM Productos WHERE ID = @ID", con);
                cmd.Parameters.AddWithValue("@ID", txtBuscar.Text);
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("El Productos ha sido eliminado correctamente");
                    CargarGrid();
                }
                else
                {
                    MessageBox.Show("El Productos no ha podido ser eliminado");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error al eliminar el Productos: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        

       
    }
}
