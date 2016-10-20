using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using MySql.Data.MySqlClient;
using System.Data;


namespace Estacionamiento_Entrada_Android
{//le agrega el color establecido 
    [Activity(Theme = "@style/Theme.Custom", Label = "Estacionamiento Entrada", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.button1);
            EditText folio = FindViewById<EditText>(Resource.Id.editText1);

            button.Click += delegate
            {//conector
                string connsqlstring = "Server=192.168.0.31;Port=3306;database=estacionamiento;User Id=root;Password=root;charset=utf8";
                int cont = 0;
                try
                {
                    MySqlConnection sqlconn = new MySqlConnection(connsqlstring);
                    sqlconn.Open();

                    string Query = "SELECT COUNT(0) from android;";

                    DataTable t = new DataTable();
                    try
                    {
                        MySqlDataAdapter da = new MySqlDataAdapter(Query, sqlconn);
                        da.Fill(t);
                        DataRow row = t.Rows[0];
                        folio.Text = row["COUNT(0)"].ToString();
                        cont = Convert.ToInt32(row["COUNT(0)"].ToString());
                    }
                    catch (MySqlException ex)
                    {

                    }
                    if (cont <= 3)
                    {
                        try
                        {
                            DateTime thisDay = DateTime.Now;

                            MySqlConnection db = new MySqlConnection(connsqlstring);
                            db.Open();

                            string consulta = "INSERT INTO android(hora_entrada,minutos_entrada,segundos_entrada,dia_entrada,mes_entrada,año_entrada) VALUES ({0},{1},{2},{3},{4},{5});";
                            consulta = string.Format(consulta, thisDay.Hour, thisDay.Minute, thisDay.Second, thisDay.Day, thisDay.Month, thisDay.Year);
                            MySqlCommand instruccion = new MySqlCommand(consulta, db);
                            instruccion.ExecuteNonQuery();
                            db.Close();
                            Toast.MakeText(this, "Entrada Registrada Con Exito", ToastLength.Long).Show();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                        }
                        try
                        {

                            string Querym = "SELECT MAX(idfolio) from android;";

                            DataTable tm = new DataTable();
                            try
                            {
                                MySqlDataAdapter da = new MySqlDataAdapter(Querym, sqlconn);
                                da.Fill(tm);
                                DataRow row = tm.Rows[0];
                                folio.Text = row["MAX(idfolio)"].ToString();

                            }
                            catch (MySqlException ex)
                            {

                            }
                        }
                        catch (MySqlException ex)
                        {

                        }
                    }
                    else
                    {
                        Toast.MakeText(this, "No hay mas cupo de Vehiculos", ToastLength.Long).Show();
                    }
                }
                catch (Exception ex) { }


            };
        }
    }
}

