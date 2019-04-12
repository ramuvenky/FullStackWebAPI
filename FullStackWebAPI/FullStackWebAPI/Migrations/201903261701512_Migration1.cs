namespace FullStackWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ParentTasks",
                c => new
                    {
                        ParentTaskId = c.Int(nullable: false, identity: true),
                        Parent_Task = c.String(),
                    })
                .PrimaryKey(t => t.ParentTaskId);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        ProjectId = c.Int(nullable: false, identity: true),
                        Project_Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProjectId);
            
            CreateTable(
                "dbo.Tasks",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        ParentTaskId = c.Int(nullable: false),
                        ProjectId = c.Int(nullable: false),
                        Task_Name = c.String(),
                        Start_Date = c.DateTime(nullable: false),
                        End_date = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                        Status = c.String(),
                    })
                .ForeignKey("dbo.ParentTasks", t => t.ParentTaskId, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .PrimaryKey(t => t.TaskId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmployeeId = c.String(),
                        ProjectId = c.Int(nullable: false),
                        TaskId = c.Int(nullable: false),
                    })
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.Tasks", t => t.TaskId, cascadeDelete: false)
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tasks", "ParentTaskId", "dbo.ParentTasks");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "TaskId", "dbo.Tasks");
            DropTable("dbo.Users");
            DropTable("dbo.Tasks");
            DropTable("dbo.Projects");
            DropTable("dbo.ParentTasks");
        }
    }
}
