namespace FullStackWebAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migration2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tasks", "ParentTask_ParentTaskId", c => c.Int());
            AddColumn("dbo.Tasks", "Project_ProjectId", c => c.Int());
            AddColumn("dbo.Users", "Task_TaskId", c => c.Int());
            AddColumn("dbo.Users", "Project_ProjectId", c => c.Int());
            CreateIndex("dbo.Tasks", "ParentTask_ParentTaskId");
            CreateIndex("dbo.Tasks", "Project_ProjectId");
            CreateIndex("dbo.Users", "Task_TaskId");
            CreateIndex("dbo.Users", "Project_ProjectId");
            AddForeignKey("dbo.Users", "Task_TaskId", "dbo.Tasks", "TaskId");
            AddForeignKey("dbo.Tasks", "ParentTask_ParentTaskId", "dbo.ParentTasks", "ParentTaskId");
            AddForeignKey("dbo.Tasks", "Project_ProjectId", "dbo.Projects", "ProjectId");
            AddForeignKey("dbo.Users", "Project_ProjectId", "dbo.Projects", "ProjectId");
            DropForeignKey("dbo.Tasks", "ParentTaskId", "dbo.ParentTasks");
            DropForeignKey("dbo.Tasks", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Users", "TaskId", "dbo.Tasks");
            DropColumn("dbo.Tasks", "ParentTaskId");
            DropColumn("dbo.Tasks", "ProjectId");
            DropColumn("dbo.Users", "ProjectId");
            DropColumn("dbo.Users", "TaskId");
        }
        
        public override void Down()
        {
            
            AddColumn("dbo.Users", "TaskId", c => c.Int(nullable: false));
            AddColumn("dbo.Users", "ProjectId", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "ProjectId", c => c.Int(nullable: false));
            AddColumn("dbo.Tasks", "ParentTaskId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "Project_ProjectId", "dbo.Projects");
            DropForeignKey("dbo.Tasks", "ParentTask_ParentTaskId", "dbo.ParentTasks");
            DropForeignKey("dbo.Users", "Task_TaskId", "dbo.Tasks");
            DropIndex("dbo.Users", new[] { "Project_ProjectId" });
            DropIndex("dbo.Users", new[] { "Task_TaskId" });
            DropIndex("dbo.Tasks", new[] { "Project_ProjectId" });
            DropIndex("dbo.Tasks", new[] { "ParentTask_ParentTaskId" });
            DropColumn("dbo.Users", "Project_ProjectId");
            DropColumn("dbo.Users", "Task_TaskId");
            DropColumn("dbo.Tasks", "Project_ProjectId");
            DropColumn("dbo.Tasks", "ParentTask_ParentTaskId");
        }
    }
}
