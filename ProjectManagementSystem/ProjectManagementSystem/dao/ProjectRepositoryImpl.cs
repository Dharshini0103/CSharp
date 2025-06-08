using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using ProjectManagementSystem.entity;
using ProjectManagementSystem.myexceptions;


namespace ProjectManagementSystem.dao
{
    public class ProjectRepositoryImpl : IProjectRepository
    {
        static SqlConnection con = null;
        static SqlCommand cmd;
        static SqlDataReader dr;

        public static SqlConnection getConnection()
        {
            con = new SqlConnection("data source=DHARSH01\\SQLEXPRESS01;initial catalog=Project_Management_System;integrated security=true;");
            con.Open();
            return con;
        }
        public bool CreateProject(Project pj)
        {
            try
            {
                con = getConnection();
                cmd = new SqlCommand("INSERT INTO Project (projectName, description, start_date, status) VALUES (@ProjectName, @Description, @StartDate, @Status)", con);

                cmd.Parameters.AddWithValue("@ProjectName", pj.Projectname);
                cmd.Parameters.AddWithValue("@Description", pj.Description);
                cmd.Parameters.AddWithValue("@StartDate", pj.Start_date);
                cmd.Parameters.AddWithValue("@Status", pj.Status);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Project added successfully.." : "Unable to add project..");
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
        public bool CreateEmployee(Employee emp)
        {
            try
            {
                    con= getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Employee (name, designation, gender, salary, project_id) VALUES (@name, @designation, @gender, @salary, @project_id)", con);
                    cmd.Parameters.AddWithValue("@name", emp.Name);
                    cmd.Parameters.AddWithValue("@designation", emp.Designation);
                    cmd.Parameters.AddWithValue("@gender", emp.Gender);
                    cmd.Parameters.AddWithValue("@salary", emp.Salary);
                    cmd.Parameters.AddWithValue("@project_id", emp.Project_id);

                    int rows = cmd.ExecuteNonQuery();
                    Console.WriteLine(rows > 0 ? "Record added successfully.." : "Unable to add a record ..");
                    return rows > 0;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }


        public bool CreateTask(entity.Task tk)
        {
            try
            {
                con = getConnection();
                cmd = new SqlCommand("INSERT INTO Task (task_name, project_id, employee_id, status) VALUES (@task_name, @project_id, @employee_id, @status)", con);
                cmd.Parameters.AddWithValue("@task_name", tk.Task_name);
                cmd.Parameters.AddWithValue("@project_id", tk.Project_id);
                cmd.Parameters.AddWithValue("@employee_id", tk.Employee_id);
                cmd.Parameters.AddWithValue("@status", tk.Status);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Task added successfully.." : "Unable to add task..");
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool AssignProjectToEmployee(int projectId, int employeeId)
        {
            try
            {
                con = getConnection();

                cmd = new SqlCommand("SELECT id FROM Employee WHERE id = @employeeId", con);
                cmd.Parameters.AddWithValue("@employeeId", employeeId);
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    throw new EmployeeNotFoundException($"Employee with ID {employeeId} not found.");
                }
                dr.Close();
                cmd = new SqlCommand("SELECT id FROM Project WHERE id = @projectId", con);
                cmd.Parameters.AddWithValue("@projectId", projectId);
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    throw new ProjectNotFoundException($"Project with ID {projectId} not found.");
                }
                dr.Close();

                cmd = new SqlCommand("UPDATE Employee SET project_id = @projectId WHERE id = @employeeId", con);
                cmd.Parameters.AddWithValue("@projectId", projectId);
                cmd.Parameters.AddWithValue("@employeeId", employeeId);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Project assigned to employee successfully.." : "Failed to assign project..");
                return rows > 0;
            }
            catch (EmployeeNotFoundException enfe)
            {
                Console.WriteLine("Custom Error: " + enfe.Message);
                return false;
            }
            catch (ProjectNotFoundException pnfe)
            {
                Console.WriteLine("Custom Error: " + pnfe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool AssignTaskInProjectToEmployee(entity.Task t)
        {
            try
            {
                con = getConnection();
                cmd = new SqlCommand("UPDATE Task SET employee_id = @employeeId WHERE task_id = @taskId AND project_id = @projectId", con);
                cmd.Parameters.AddWithValue("@employeeId", t.Employee_id);
                cmd.Parameters.AddWithValue("@taskId", t.Task_id);
                cmd.Parameters.AddWithValue("@projectId", t.Project_id);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Task assigned successfully.." : "Failed to assign task..");
                return rows > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteEmployee(Employee emp)
        {
            try
            {
                con = getConnection();
                cmd = new SqlCommand("SELECT id FROM Employee WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id", emp.Id);
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    throw new EmployeeNotFoundException($"Employee with ID {emp.Id} not found.");
                }
                dr.Close();
                cmd = new SqlCommand("DELETE FROM Employee WHERE id = @id", con);
                cmd.Parameters.AddWithValue("@id",emp.Id);
                int rows = cmd.ExecuteNonQuery();
                Console.WriteLine(rows > 0 ? "Employee deleted successfully.." : "Failed to delete employee..");
                return rows > 0;
            }
            catch (EmployeeNotFoundException enfe)
            {
                Console.WriteLine("Custom Error: " + enfe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public bool DeleteProject(Project p)
        {
            try
            {
                con = getConnection();
                cmd = new SqlCommand("SELECT id FROM Project WHERE id = @projectId", con);
                cmd.Parameters.AddWithValue("@projectId", p.Id);
                dr = cmd.ExecuteReader();
                if (!dr.HasRows)
                {
                    dr.Close();
                    throw new ProjectNotFoundException($"Project with ID {p.Id} not found.");
                }
            }
            catch (ProjectNotFoundException pnfe)
            {
                Console.WriteLine("Custom Error: " + pnfe.Message);
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public List<entity.Task> GetAllTasks(entity.Task t)
        {
            List<entity.Task> taskList = new List<entity.Task>();
            try
            {
                con = getConnection();
                cmd = new SqlCommand("SELECT * FROM Task WHERE employee_id = @empId AND project_id = @projectId", con);
                cmd.Parameters.AddWithValue("@empId", t.Employee_id);
                cmd.Parameters.AddWithValue("@projectId", t.Project_id);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    entity.Task task = new entity.Task();
                    task.Task_id = Convert.ToInt32(dr["task_id"]);
                    task.Task_name = dr["task_name"].ToString();
                    task.Project_id = Convert.ToInt32(dr["project_id"]);
                    task.Employee_id = Convert.ToInt32(dr["employee_id"]);
                    task.Status = dr["status"].ToString();
                    taskList.Add(task);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return taskList;
        }

        public List<Project> GetAllProjects()
        {
            List<Project> projectList = new List<Project>();
            try
            {
                con = getConnection();
                cmd = new SqlCommand("SELECT * FROM Project", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Project p = new Project();
                    p.Id = Convert.ToInt32(dr["id"]);
                    p.Projectname = dr["Projectname"].ToString();
                    p.Description = dr["Description"].ToString();
                    p.Start_date = Convert.ToDateTime(dr["StartDate"]);
                    p.Status = dr["Status"].ToString();
                    projectList.Add(p);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return projectList;
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> empList = new List<Employee>();
            try
            {
                con = getConnection();
                cmd = new SqlCommand("SELECT * FROM Employee", con);
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    Employee e = new Employee();
                    e.Id = Convert.ToInt32(dr["id"]);
                    e.Name = dr["name"].ToString();
                    e.Designation = dr["designation"].ToString();
                    e.Gender = dr["gender"].ToString();
                    e.Salary = Convert.ToSingle(dr["salary"]);
                    e.Project_id = dr["project_id"] != DBNull.Value ? Convert.ToInt32(dr["project_id"]) : 0;
                    empList.Add(e);
                }
                dr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return empList;
        }
    }

}
