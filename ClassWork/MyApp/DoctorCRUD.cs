// using System;
// using System.Data;

// using Microsoft.Data.SqlClient;
// using System.Configuration;
// using System.ComponentModel;

// namespace MyApp
// {
//     internal class DoctorCRUD
//     {
//         SqlConnection con = new SqlConnection(
//             ConfigurationManager.ConnectionStrings["MyConnection"].ToString()
//         );

//         public void AddDoctor(Doctor d)
//         {
//             con.Open();

//             SqlCommand cmd = new SqlCommand(
//                 "INSERT INTO Doctors (DoctorId, Name, Specialisation, MRN) " +
//                 "VALUES (@DoctorId, @Name, @Specialisation, @MRN)", con
//             );


//             cmd.Parameters.AddWithValue("@DoctorId", d.DoctorId);
//             cmd.Parameters.AddWithValue("@Name", d.Name);
//             cmd.Parameters.AddWithValue("@Specialisation", d.Specialisation);
//             cmd.Parameters.AddWithValue("@MRN", d.MRN);
        
//             cmd.ExecuteNonQuery();
//             con.Close();
//         }

//         public string UpdateDoctor(Doctor d)
//         {
//             try
//             {
//                 con.Open();

//                 SqlCommand cmd = new SqlCommand(
//                    "UPDATE Doctors SET " +
//                    "DoctorId= @PatientName, " +
//                    "Age = @Age, " +
//                    "BodyWeight = @BodyWeight, " +
//                    "Height = @Height " +
//                    "WHERE MRN = @MRN", con
//                 );
//                 cmd.Parameters.AddWithValue("@PatientName", d.DoctorId);
//                 cmd.Parameters.AddWithValue("@Age", d.Name);
//                 cmd.Parameters.AddWithValue("@BodyWeight", p.BodyWeight);
//                 cmd.Parameters.AddWithValue("@Height", p.Height);
//                 cmd.Parameters.AddWithValue("@MRN", p.MRN);

//                 cmd.ExecuteNonQuery();
//                 return "Success";
//             }
//             catch (Exception ex)
//             {
//                 return ex.Message;
//             }
//             finally
//             {
//                 con.Close();
//             }

//         }

//         public string DeletePatient(int MRN)
//         {
//             try
//             {
//                 con.Open();

//                 SqlCommand cmd = new SqlCommand(
//                     "DELETE FROM Patients WHERE MRN = @MRN", con
//                 );
//                 cmd.Parameters.AddWithValue("@MRN", MRN);
//                 cmd.ExecuteNonQuery();
//                 return "Success";
//             }
//             catch (Exception ex)
//             {
//                 return ex.Message;
//             }
//             finally
//             {
//                 con.Close();
//             }

//         }
//         public List<Patient> GetPatientsList()
//         {
//             SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Patients", con);
//             DataTable dt = new DataTable();
//             da.Fill(dt);
//             List<Patient> list = new List<Patient>();
//             foreach (DataRow drow in dt.Rows)
//             {
//                 Patient p = new Patient();

//                 p.MRN = int.Parse(drow["MRN"].ToString());
//                 p.PatientName = drow["PatientName"].ToString();
//                 p.Age = int.Parse(drow["Age"].ToString());
//                 p.BodyWeight = float.Parse(drow["BodyWeight"].ToString());
//                 p.Height = float.Parse(drow["Height"].ToString());

//                 list.Add(p);
//             }
//             return list;
//         }

//     }
// }

