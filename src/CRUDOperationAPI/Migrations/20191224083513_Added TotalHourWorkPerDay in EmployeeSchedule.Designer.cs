using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CRUDOperationAPI.Contexts;

namespace CRUDOperationAPI.Migrations
{
    [DbContext(typeof(EmployeeDbContext))]
    [Migration("20191224083513_Added TotalHourWorkPerDay in EmployeeSchedule")]
    partial class AddedTotalHourWorkPerDayinEmployeeSchedule
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CRUDOperationAPI.Models.ClientProject", b =>
                {
                    b.Property<int>("ClientProjectID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ClientID");

                    b.Property<int>("ProjectID");

                    b.HasKey("ClientProjectID");

                    b.HasIndex("ClientID");

                    b.HasIndex("ProjectID");

                    b.ToTable("ClientProject");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Clients", b =>
                {
                    b.Property<int>("ClientID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClientContactNumber");

                    b.Property<string>("ClientFirstName");

                    b.Property<string>("ClientLastName");

                    b.Property<string>("ClientOffice");

                    b.Property<string>("OfficeAddress");

                    b.HasKey("ClientID");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Company", b =>
                {
                    b.Property<int>("CompanyID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CompanyName");

                    b.HasKey("CompanyID");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Contacts", b =>
                {
                    b.Property<int>("ContactID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<string>("ContactNumber")
                        .IsRequired();

                    b.Property<DateTime>("CreatedTimeStamp");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("EmergencyContactNumber")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<DateTime>("ModifiedTimeStamp");

                    b.Property<string>("ProfilePicture");

                    b.HasKey("ContactID");

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.DepartmentEmployee", b =>
                {
                    b.Property<int>("DepartmentEmployeeID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("DepartmentID");

                    b.Property<int>("EmployeeID");

                    b.HasKey("DepartmentEmployeeID");

                    b.HasIndex("DepartmentID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("DepartmentEmployee");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Departments", b =>
                {
                    b.Property<int>("DepartmentID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CompanyID");

                    b.Property<string>("DepartmentName");

                    b.HasKey("DepartmentID");

                    b.HasIndex("CompanyID");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Employee", b =>
                {
                    b.Property<int>("EmployeeId")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("ContactId");

                    b.Property<DateTime>("CreatedTimeStamp");

                    b.Property<string>("Designation");

                    b.Property<bool>("IsFullTimer");

                    b.Property<bool>("IsWorking");

                    b.Property<DateTime>("ModifiedTimeStamp");

                    b.Property<decimal>("Salary");

                    b.Property<int?>("UserID");

                    b.HasKey("EmployeeId");

                    b.HasIndex("ContactId");

                    b.HasIndex("UserID");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.EmployeeProject", b =>
                {
                    b.Property<int>("EmployeeProjectID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("EmployeeID");

                    b.Property<int>("ProjectID");

                    b.HasKey("EmployeeProjectID");

                    b.HasIndex("EmployeeID");

                    b.HasIndex("ProjectID");

                    b.ToTable("EmployeeProject");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.EmployeeSchedule", b =>
                {
                    b.Property<int>("ScheduleID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTimeStamp");

                    b.Property<int>("EmployeeID");

                    b.Property<DateTime?>("InTime");

                    b.Property<DateTime>("ModifiedTimeStamp");

                    b.Property<DateTime?>("OutTime");

                    b.Property<TimeSpan>("TotalHourWorkPerday");

                    b.HasKey("ScheduleID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("EmployeeSchedule");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Leaves", b =>
                {
                    b.Property<int>("LeaveID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArrovedBy");

                    b.Property<DateTime>("CreatedTimeStamp");

                    b.Property<int>("EmployeeID");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("LeaveReason");

                    b.Property<string>("LeaveType");

                    b.Property<DateTime>("ModifiedTimeStamp");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Status");

                    b.HasKey("LeaveID");

                    b.HasIndex("EmployeeID");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Projects", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTimeStamp");

                    b.Property<bool>("IsActive");

                    b.Property<DateTime>("ModifiedTimeStamp");

                    b.Property<string>("ProjectDescription");

                    b.Property<DateTime>("ProjectEndDate");

                    b.Property<string>("ProjectName");

                    b.Property<DateTime>("ProjectStartDate");

                    b.HasKey("ProjectID");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Roles", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("RoleName");

                    b.HasKey("RoleID");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Users", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Password");

                    b.Property<int>("RoleID");

                    b.Property<string>("UserName");

                    b.HasKey("UserID");

                    b.HasIndex("RoleID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.ClientProject", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Clients", "Clients")
                        .WithMany()
                        .HasForeignKey("ClientID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CRUDOperationAPI.Models.Projects", "Projects")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.DepartmentEmployee", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Departments", "Departments")
                        .WithMany()
                        .HasForeignKey("DepartmentID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CRUDOperationAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Departments", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Employee", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Contacts", "Contacts")
                        .WithMany()
                        .HasForeignKey("ContactId");

                    b.HasOne("CRUDOperationAPI.Models.Users", "Users")
                        .WithMany()
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.EmployeeProject", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CRUDOperationAPI.Models.Projects", "Projects")
                        .WithMany()
                        .HasForeignKey("ProjectID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.EmployeeSchedule", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Leaves", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CRUDOperationAPI.Models.Users", b =>
                {
                    b.HasOne("CRUDOperationAPI.Models.Roles", "Roles")
                        .WithMany()
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
