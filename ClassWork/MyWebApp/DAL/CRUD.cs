using Microsoft.Data.SqlClient;
using MyWebApp.Models;
using System.Configuration;
using System.Data;
namespace MyWebApp.DAL;

public class CRUD
{
    private readonly string _config;
    private readonly string _connStr;

    public CRUD(IConfiguration config)
    {
        _connStr = config.GetConnectionString("AzureDB");
    }

    public void AddPatient(Patient p)
    {
        using SqlConnection con = new SqlConnection(_connStr);
        con.Open();


        SqlCommand cmd = new SqlCommand(
            "INSERT INTO Patients (MRN, PatientName, Age, BodyWeight, Height) " +
            "VALUES (@MRN, @PatientName, @Age, @BodyWeight, @Height)", con
        );


        cmd.Parameters.AddWithValue("@MRN", p.MRN);
        cmd.Parameters.AddWithValue("@PatientName", p.PatientName);
        cmd.Parameters.AddWithValue("@Age", p.Age);
        cmd.Parameters.AddWithValue("@BodyWeight", p.BodyWeight);
        cmd.Parameters.AddWithValue("@Height", p.Height);

        cmd.ExecuteNonQuery();
        con.Close();
    }
    public List<Patient> GetPatientsList()
    {
        using SqlConnection con = new SqlConnection(_connStr);
        SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Patients", con);
        DataTable dt = new DataTable();
        da.Fill(dt);
        List<Patient> list = new List<Patient>();
        foreach (DataRow drow in dt.Rows)
        {
            Patient p = new Patient();

            p.MRN = int.Parse(drow["MRN"].ToString());
            p.PatientName = drow["PatientName"].ToString();
            p.Age = int.Parse(drow["Age"].ToString());
            p.BodyWeight = float.Parse(drow["BodyWeight"].ToString());
            p.Height = float.Parse(drow["Height"].ToString());

            list.Add(p);
        }
        return list;
    }
    public Patient GetPatientByMRN(int MRN)
    {
        using SqlConnection con = new SqlConnection(_connStr);
        SqlCommand cmd = new SqlCommand("SELECT * FROM Patients WHERE MRN = @MRN", con);
        cmd.Parameters.AddWithValue("@MRN", MRN);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);

        if (dt.Rows.Count == 0)
        {
            return null;
        }

        DataRow row = dt.Rows[0];

        Patient p = new Patient
        {
            MRN = int.Parse(row["MRN"].ToString()),
            PatientName = row["PatientName"].ToString(),
            Age = int.Parse(row["Age"].ToString()),
            BodyWeight = float.Parse(row["BodyWeight"].ToString()),
            Height = float.Parse(row["Height"].ToString())
        };

        return p;
    }
    public string UpdatePatient(Patient p)
    {
        using SqlConnection con = new SqlConnection(_connStr);
        con.Open();
        try
        {
            SqlCommand cmd = new SqlCommand(
               "UPDATE Patients SET " +
               "PatientName = @PatientName, " +
               "Age = @Age, " +
               "BodyWeight = @BodyWeight, " +
               "Height = @Height " +
               "WHERE MRN = @MRN", con
            );
            cmd.Parameters.AddWithValue("@PatientName", p.PatientName);
            cmd.Parameters.AddWithValue("@Age", p.Age);
            cmd.Parameters.AddWithValue("@BodyWeight", p.BodyWeight);
            cmd.Parameters.AddWithValue("@Height", p.Height);
            cmd.Parameters.AddWithValue("@MRN", p.MRN);

            cmd.ExecuteNonQuery();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            con.Close();
        }

    }
    public string DeletePatient(int MRN)
    {
        using SqlConnection con = new SqlConnection(_connStr);
        con.Open();
        try
        {


            SqlCommand cmd = new SqlCommand(
                "DELETE FROM Patients WHERE MRN = @MRN", con
            );
            cmd.Parameters.AddWithValue("@MRN", MRN);
            cmd.ExecuteNonQuery();
            return "Success";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            con.Close();
        }

    }     
        
}
