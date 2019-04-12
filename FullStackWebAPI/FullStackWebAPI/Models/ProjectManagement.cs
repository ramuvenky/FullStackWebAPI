using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FullStackWebAPI.Models
{
    public class ProjectManagementContext : DbContext
    {
        public ProjectManagementContext() : base("SQLServerConnection")
        {

        }

        public DbSet<ParentTask> ParentTasks { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> Users { get; set; }        
    }
    
    public class ParentTask
    {
        public int ParentTaskId { get; set; }
        public string Parent_Task { get; set; }
        public virtual ICollection<Task> Task { get; set; }
    }

    public class Project
    {
        
        public Project()
        {

        }

        public Project(ProjectUI projectUI)
        {
            this.Project_Name = projectUI.Project_Name;
            this.StartDate = projectUI.StartDate;
            this.EndDate = projectUI.EndDate;
            this.Priority = projectUI.Priority;
        }
        

        public int ProjectId { get; set; }
        public string Project_Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Priority { get; set; }
        public virtual ICollection<Task> Task { get; set; }
        public virtual ICollection<User> User { get; set; }
    }

    public class ProjectUI
    {
        public ProjectUI()
        {

        }

        public ProjectUI(Project project)
        {
            this.ProjectId = project.ProjectId;
            this.Project_Name = project.Project_Name;
            this.StartDate = project.StartDate;
            this.EndDate = project.EndDate;
            this.Priority = project.Priority;
        }

        public int TotalTasks { get; set; }
        public int TotalCompleted { get; set; }
        public int ProjectId { get; set; }
        public string Project_Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Priority { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
    }

    public class Task
    {
        
        public Task() { }

        public Task(TaskUI taskUI)
        {
            this.TaskId = taskUI.TaskId;
            
            this.Task_Name = taskUI.Task_Name;
            this.Start_Date = taskUI.Start_Date;
            this.End_date = taskUI.End_date;
            this.Priority = taskUI.Priority;
            this.Status = taskUI.Status;
        }
        

        public int TaskId { get; set; }
        //public virtual ICollection<ParentTask> ParentTask { get; set; }
        //public virtual ICollection<Project> Project { get; set; }
        public string Task_Name { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_date { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public virtual ICollection<User> User { get; set; }
    }

    public class TaskUI
    {
        public TaskUI() { }

        public TaskUI(Task task)
        {
            this.TaskId = task.TaskId;
            this.Task_Name = task.Task_Name;
            this.Start_Date = task.Start_Date;
            this.End_date = task.End_date;
            this.Priority = task.Priority;
            this.Status = task.Status;
        }

        public int TaskId { get; set; }
        public int? ParentTaskId { get; set; }
        public string Parent_task { get; set; }
        public int? ProjectId { get; set; }
        public string Project_Name { get; set; }
        public string Task_Name { get; set; }
        public DateTime Start_Date { get; set; }
        public DateTime End_date { get; set; }
        public int Priority { get; set; }
        public string Status { get; set; }
        public int? UserId { get; set; }
        public string User_name { get; set; }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmployeeId { get; set; }
        
        //public ICollection<Project> Project { get; set; }
        //public virtual ICollection<Task> Task { get; set; }
    }
}