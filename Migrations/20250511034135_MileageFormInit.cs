using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FormsBoard.Migrations
{
    /// <inheritdoc />
    public partial class MileageFormInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FormStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MileageForm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FormStatusId = table.Column<int>(type: "int", nullable: false),
                    ManagerId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ManagerDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AccountantId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountantEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountantDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountingApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentReference = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectedByName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionPhase = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MileageRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LocationCorporate = table.Column<bool>(type: "bit", nullable: false),
                    LocationLenoirCity = table.Column<bool>(type: "bit", nullable: false),
                    LocationClarksville = table.Column<bool>(type: "bit", nullable: false),
                    LocationMaryville = table.Column<bool>(type: "bit", nullable: false),
                    LocationCookeville = table.Column<bool>(type: "bit", nullable: false),
                    LocationMtJuliet = table.Column<bool>(type: "bit", nullable: false),
                    LocationDickson = table.Column<bool>(type: "bit", nullable: false),
                    LocationWeisgarber = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyCorporate = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyInfusion = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyNursePractitioners = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyPhysicians = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtySpecialtyMeds = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyAllergy = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyMarketing = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyPharmacy = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyScheduling = table.Column<bool>(type: "bit", nullable: false),
                    SpecialtyVaccine = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileageForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageForm_FormStatus_FormStatusId",
                        column: x => x.FormStatusId,
                        principalTable: "FormStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MileageLineItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MileageFormId = table.Column<int>(type: "int", nullable: false),
                    TravelDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EndLocation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    TotalMiles = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MileageLineItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MileageLineItem_MileageForm_MileageFormId",
                        column: x => x.MileageFormId,
                        principalTable: "MileageForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "FormStatus",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Form is being prepared by employee", "Draft" },
                    { 2, "Form submitted for manager review", "Submitted" },
                    { 3, "Approved by manager, awaiting accounting review", "Manager Approved" },
                    { 4, "Accounting approved and Completed", "Completed" },
                    { 5, "Form has been discarded", "Discarded" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MileageForm_FormStatusId",
                table: "MileageForm",
                column: "FormStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_MileageLineItem_MileageFormId",
                table: "MileageLineItem",
                column: "MileageFormId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MileageLineItem");

            migrationBuilder.DropTable(
                name: "MileageForm");

            migrationBuilder.DropTable(
                name: "FormStatus");
        }
    }
}
