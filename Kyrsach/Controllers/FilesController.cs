using Kyrsach.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Kyrsach.Controllers
{
    public class FilesController : Controller
    {
        public ActionResult Index()
        {
            return View(GetFiles());
        }

        [HttpPost]
        public ActionResult Index(IFormFile postedFile)
        {
            if (postedFile != null && postedFile.Length > 0)
            {
                byte[] bytes;
                using (var ms = new MemoryStream())
                {
                    postedFile.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                string constr = "Server=(LocalDB)\\MSSQLLocalDB;Database=Serova4;Trusted_Connection=True;MultipleActiveResultSets=true";
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "INSERT INTO tbrFile VALUES (@Name, @ContentType, @Data)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Name", Path.GetFileName(postedFile.FileName));
                        cmd.Parameters.AddWithValue("@ContentType", postedFile.ContentType);
                        cmd.Parameters.AddWithValue("@Data", bytes);
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }

            return View(GetFiles());
        }
    


[HttpPost]
        public JsonResult GetWordDocument(string fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = "Server=(LocalDB)\\MSSQLLocalDB;Database=Serova4;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Data, ContentType FROM tbrFile WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Data"];
                        contentType = sdr["ContentType"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }
            JsonResult jsonResult = Json(new { FileName = fileName, ContentType = contentType, Data = bytes });
            //jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        private static List<FileModel> GetFiles()
        {
            List<FileModel> files = new List<FileModel>();
            string constr = "Server=(LocalDB)\\MSSQLLocalDB;Database=Serova4;Trusted_Connection=True;MultipleActiveResultSets=true";
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM tbrFile"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new FileModel
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }
    }
}
