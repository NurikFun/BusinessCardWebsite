using BusinessCardWebsite.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace BusinessCardWebsite.DB
{
    public class SiteDb
    {
        private MySqlConnection con;
        private MySqlCommand cmd { get; set; }

        public SiteDb(string connectionString)
        {
            con = new MySqlConnection(connectionString);
        }

        public List<Request> getRequests()
        {
            List<Request> requests = new List<Request>();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("SELECT * FROM nii_test.requests;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        requests.Add(new Request
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["name"].ToString(),
                            email = reader["email"].ToString(),
                            phone = reader["phone"].ToString(),
                            company_name = reader["company_name"].ToString(),
                            estimated_budget = reader["estimated_budget"].ToString(),
                            field = reader["field"].ToString(),
                            description = reader["description"].ToString(),
                            date_request = Convert.ToDateTime(reader["date_request"]),
                            status = Convert.ToInt32(reader["status"]),
                            resulCode = true
                        });
                    }
                    return requests;
                }
            }
            catch (Exception e)
            {
                return ListResultFalse(e, requests);
            }
        }

        public QueryResult insertRequest(Request request)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("INSERT INTO nii_test.requests (name, email, phone, company_name, estimated_budget, field, description) VALUES (@name, @email, @phone, @company_name, @estimated_budget, @field, @description)"))
                {
                    cmd.Parameters.AddWithValue("@name", request.name);
                    cmd.Parameters.AddWithValue("@email", request.email);
                    cmd.Parameters.AddWithValue("@phone", request.phone);
                    cmd.Parameters.AddWithValue("@company_name", request.company_name);
                    cmd.Parameters.AddWithValue("@estimated_budget", request.estimated_budget);
                    cmd.Parameters.AddWithValue("@field", request.field);
                    cmd.Parameters.AddWithValue("@description", request.description);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }

        public Request getRequest(int id)
        {
            Request request = new Request();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("select * from nii_test.requests where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    request.id = Convert.ToInt32(reader["id"]);
                    request.name = reader["name"].ToString();
                    request.email = reader["email"].ToString();
                    request.phone = reader["phone"].ToString();
                    request.company_name = reader["company_name"].ToString();
                    request.estimated_budget = reader["estimated_budget"].ToString();
                    request.field = reader["field"].ToString();
                    request.description = reader["description"].ToString();
                    request.date_request = Convert.ToDateTime(reader["date_request"]);
                    request.status = Convert.ToInt32(reader["status"]);

                    return request;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, request);
            }
        }

        public QueryResult deleteRequest(int id)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("delete from nii_test.requests where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }



        public List<Project> getProjects(int type)
        {
            string filter = "";
            if (type != 0)
            {
                filter = "where direction_id=" + type;
            }
            List<Project> projects = new List<Project>();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("SELECT id, direction_id, name, description, img_path FROM nii_test.projects " + filter + ";"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        projects.Add(new Project
                        {
                            id = Convert.ToInt32(reader["id"]),
                            direction_id = Convert.ToInt32(reader["direction_id"]),
                            name = reader["name"].ToString(),
                            description = reader["description"].ToString(),
                            img_path = reader["img_path"].ToString(),
                            resulCode = true
                        });
                    }

                    return projects;
                }
            }
            catch (Exception e)
            {
                return ListResultFalse(e, projects);
            }
        }



        public Project getProject(int id)
        {
            Project project = new Project();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("call nii_test.getProject(@id);"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    bool first = true;
                    while (reader.Read())
                    {
                        if (first == true)
                        {
                            project.id = Convert.ToInt32(reader["id"]);
                            project.direction_id = Convert.ToInt32(reader["direction_id"]);
                            project.name = reader["name"].ToString();
                            project.description = reader["description"].ToString();
                            project.content = reader["content"].ToString();
                            project.img_path = reader["img"].ToString();
                            project.doc_path = reader["doc_path"].ToString();

                            if (!reader["img_path"].ToString().IsEmpty())
                                project.images.Add(new Images
                                {
                                    img_path = reader["img_path"].ToString()
                                });

                            project.resulCode = true;
                            first = false;
                        }
                        else
                        {
                            project.images.Add(new Images
                            {
                                img_path = reader["img_path"].ToString()
                            });
                        }
                    }
                    return project;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, project);
            }

        }


        public QueryResult insertProject(Project project)
        {
            QueryResult qr = new QueryResult();
            string select = "select max(id) from nii_test.projects;";
            string command = "INSERT INTO nii_test.projects (direction_id, name, description, content, img_path, doc_path) VALUES " +
                "(" + project.direction_id + ", '" + project.name + "', '" + project.description + "', '" + project.content + "', '" + project.img_path + "', '" + project.doc_path + "'); " + select;

            try
            {
                using (con)
                using (cmd = new MySqlCommand(command))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();
                    int project_id = Convert.ToInt32(reader[0]);

                    cmd.Dispose();
                    con.Close();

                    if (project.images != null)
                    {
                        insertImages(project.images, project_id);
                    }

                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }

        private QueryResult insertImages(List<Images> images, int project_id)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                {
                    con.Open();
                    foreach (Images img in images)
                    {
                        cmd = new MySqlCommand("INSERT INTO nii_test.images (project_id, img_path) VALUES (" + project_id + ", '" + img.img_path + "');")
                        {
                            CommandType = CommandType.Text,
                            Connection = con
                        };
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
                return qr;
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }

        public QueryResult deleteImage(string filePath)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("delete from nii_test.images where img_path=@filePath;"))
                {
                    cmd.Parameters.AddWithValue("@filePath", filePath);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }



        public QueryResult updateProject(Project project)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("update nii_test.projects set direction_id=@direction_id, name=@name, description=@description, content=@content, img_path=@img_path, doc_path=@doc_path, date_updated=now() where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", project.id);
                    cmd.Parameters.AddWithValue("@direction_id", project.direction_id);
                    cmd.Parameters.AddWithValue("@name", project.name);
                    cmd.Parameters.AddWithValue("@description", project.description);
                    cmd.Parameters.AddWithValue("@content", project.content);
                    cmd.Parameters.AddWithValue("@img_path", project.img_path);
                    cmd.Parameters.AddWithValue("@doc_path", project.doc_path);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    con.Close();

                    if (project.images != null)
                    {
                        insertImages(project.images, project.id);
                    }
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }


        public QueryResult deleteProject(int id)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("delete from nii_test.images where project_id=@id; delete from nii_test.projects where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }






        public List<Direction> getDirections()
        {
            List<Direction> directions = new List<Direction>();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("SELECT * FROM nii_test.directions;"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        directions.Add(new Direction
                        {
                            id = Convert.ToInt32(reader["id"]),
                            name = reader["name"].ToString(),
                            description = reader["description"].ToString(),
                            content = reader["content"].ToString(),
                            img_path = reader["img_path"].ToString()
                        });
                    }
                    return directions;
                }
            }
            catch (Exception e)
            {
                return ListResultFalse(e, directions);
            }
        }



        public Direction getDirection(int id)
        {
            Direction direction = new Direction();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("select * from nii_test.directions where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    MySqlDataReader reader = cmd.ExecuteReader();
                    reader.Read();

                    direction.id = Convert.ToInt32(reader["id"]);
                    direction.name = reader["name"].ToString();
                    direction.description = reader["description"].ToString();
                    direction.content = reader["content"].ToString();
                    direction.img_path = reader["img_path"].ToString();

                    return direction;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, direction);
            }

        }


        public QueryResult insertDirection(Direction direction)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("INSERT INTO nii_test.directions (name, description, content, img_path) VALUES (@name, @description, @content, @img_path)"))
                {
                    cmd.Parameters.AddWithValue("@name", direction.name);
                    cmd.Parameters.AddWithValue("@description", direction.description);
                    cmd.Parameters.AddWithValue("@content", direction.content);
                    cmd.Parameters.AddWithValue("@img_path", direction.img_path);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }




        public QueryResult updateDirection(Direction direction)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("update nii_test.directions set name=@name, description=@description, content=@content, img_path=@img_path, date_updated=now() where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", direction.id);
                    cmd.Parameters.AddWithValue("@name", direction.name);
                    cmd.Parameters.AddWithValue("@description", direction.description);
                    cmd.Parameters.AddWithValue("@content", direction.content);
                    cmd.Parameters.AddWithValue("@img_path", direction.img_path);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }


        public QueryResult deleteDirection(int id)
        {
            QueryResult qr = new QueryResult();
            try
            {
                using (con)
                using (cmd = new MySqlCommand("delete from nii_test.directions where id=@id;"))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;

                    con.Open();
                    cmd.ExecuteNonQuery();
                    return qr;
                }
            }
            catch (Exception e)
            {
                return ObjectResultFalse(e, qr);
            }
        }




        private T ObjectResultFalse<T>(Exception e, T t) where T : QueryResult
        {
            t.resulCode = false;
            t.resultMessage = e.Message;
            return t;
        }

        private List<T> ListResultFalse<T>(Exception e, List<T> list) where T : QueryResult, new()
        {
            list.Add(new T { resulCode = false, resultMessage = e.Message });
            return list;
        }

    }
}