using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Wasla.DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Courier Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courier Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Merchants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WalletBalance = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                    table.CheckConstraint("CK_Product_WalletBalance_NonNegative", "[WalletBalance] >= 0");
                });

            migrationBuilder.CreateTable(
                name: "CompanyPhones",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPhones", x => x.CompanyId);
                    table.ForeignKey(
                        name: "FK_CompanyPhones_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rate Cards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DestinationCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MinWeight = table.Column<double>(type: "float", nullable: false),
                    MaxWeight = table.Column<double>(type: "float", nullable: false),
                    BaseFee = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    ExtraKiloPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rate Cards", x => x.Id);
                    table.CheckConstraint("CK_RateCard_BaseFee_NonNegative", "[BaseFee] >= 0");
                    table.CheckConstraint("CK_RateCard_EffectiveDate_ExpiryDate", "[EffectiveDate] < [ExpiryDate]");
                    table.CheckConstraint("CK_RateCard_MaxWeight_NonNegative", "[MaxWeight] >= 0");
                    table.CheckConstraint("CK_RateCard_MinWeight_MaxWeight", "[MinWeight] < [MaxWeight]");
                    table.CheckConstraint("CK_RateCard_MinWeight_NonNegative", "[MinWeight] >= 0");
                    table.ForeignKey(
                        name: "FK_Rate Cards_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Client_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MerchantPhones",
                columns: table => new
                {
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MerchantPhones", x => x.MerchantId);
                    table.ForeignKey(
                        name: "FK_MerchantPhones_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ratings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Stars = table.Column<double>(type: "float", nullable: false),
                    RatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ratings", x => x.Id);
                    table.CheckConstraint("CK_Rating_Score_Range", "[Stars] >= 1 AND [Stars] <= 5");
                    table.ForeignKey(
                        name: "FK_Ratings_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ratings_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DriverPhones",
                columns: table => new
                {
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPhones", x => x.DriverId);
                    table.ForeignKey(
                        name: "FK_DriverPhones_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerPhone = table.Column<string>(type: "char(11)", maxLength: 100, nullable: false),
                    TrackingUuid = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CustomerAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CityFrom = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CityTo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    isClaimingRequired = table.Column<bool>(type: "bit", nullable: false),
                    isBreakable = table.Column<bool>(type: "bit", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    PaymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeliveredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.CheckConstraint("CK_Order_TotalPrice", "TotalPrice >= 0");
                    table.ForeignKey(
                        name: "FK_Orders_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Orders_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicensePlate = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capacity = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClientPhones",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false),
                    Label = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientPhones", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_ClientPhones_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Driver-Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    isActive = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver-Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Driver-Orders_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Driver-Orders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Order-Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Qty = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order-Products", x => x.Id);
                    table.CheckConstraint("CK_OrderProduct_Price", "Price >= 0");
                    table.CheckConstraint("CK_OrderProduct_Quantity", "QTY >= 0");
                    table.ForeignKey(
                        name: "FK_Order-Products_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShippingFee = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AcceptanceState = table.Column<bool>(type: "bit", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderCompany", x => x.Id);
                    table.CheckConstraint("CK_OrderCompany_ShippingFee_NonNegative", "[ShippingFee] >= 0");
                    table.ForeignKey(
                        name: "FK_OrderCompany_Courier Company_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Courier Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderCompany_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Tracking Histories of Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracking Histories of Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracking Histories of Orders_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Driver-Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ReturnedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Driver-Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Driver-Vehicles_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Driver-Vehicles_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict
                        );
                });

            migrationBuilder.InsertData(
                table: "Courier Company",
                columns: new[] { "Id", "CompanyEmail", "CompanyName", "Password" },
                values: new object[,]
                {
                    { 1, "info@wasla.com", "Wasla Express", "pass123" },
                    { 2, "hello@fastship.com", "FastShip", "secret" },
                    { 3, "contact@citycouriers.com", "CityCouriers", "pwd123" }
                });

            migrationBuilder.InsertData(
                table: "Merchants",
                columns: new[] { "Id", "Email", "Name", "Password", "StoreName", "WalletBalance" },
                values: new object[,]
                {
                    { 1, "alfa@store.com", "Alfa Store", "m1npass", "Alfa Store", 1000m },
                    { 2, "beta@shop.com", "Beta Shop", "m2npass", "Beta Shop", 250m },
                    { 3, "gamma@market.com", "Gamma Market", "m3npass", "Gamma Market", 500m }
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Email", "MerchantId", "Name" },
                values: new object[,]
                {
                    { 1, "ahmed.client@example.com", 1, "Ahmed Client" },
                    { 2, "layla.client@example.com", 2, "Layla Client" },
                    { 3, "hassan.client@example.com", 3, "Hassan Client" }
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "CompanyId", "Email", "Name", "Password" },
                values: new object[,]
                {
                    { 1, 1, "ziad@example.com", "Ziad Ahmed", "drv1" },
                    { 2, 2, "omar@example.com", "Omar Khalid", "drv2" },
                    { 3, 1, "sara@example.com", "Sara Ali", "drv3" }
                });

            migrationBuilder.InsertData(
                table: "Rate Cards",
                columns: new[] { "Id", "BaseFee", "CompanyId", "DestinationCity", "EffectiveDate", "ExpiryDate", "ExtraKiloPrice", "MaxWeight", "MinWeight", "OriginCity" },
                values: new object[,]
                {
                    { 1, 50m, 1, "Giza", new DateTime(2026, 6, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 10m, 5.0, 0.0, "Cairo" },
                    { 2, 80m, 2, "Cairo", new DateTime(2026, 6, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2027, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 8m, 10.0, 0.0, "Giza" },
                    { 3, 150m, 1, "Alexandria", new DateTime(2026, 6, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 10, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 12m, 20.0, 0.0, "Cairo" }
                });

            migrationBuilder.InsertData(
                table: "Ratings",
                columns: new[] { "Id", "Comment", "CompanyId", "CreatedAt", "MerchantId", "RatedBy", "Stars" },
                values: new object[,]
                {
                    { 1, "Good service", 1, new DateTime(2026, 6, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, "merchant", 4.5 },
                    { 2, "Excellent", 1, new DateTime(2026, 6, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, "merchant", 5.0 },
                    { 3, "Average", 2, new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, "merchant", 3.5 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "CityFrom", "CityTo", "CompanyId", "CreatedAt", "CustomerAddress", "CustomerName", "CustomerPhone", "DeliveredAt", "DriverId", "MerchantId", "PaymentType", "TotalPrice", "TrackingUuid", "UpdatedAt", "isBreakable", "isClaimingRequired" },
                values: new object[,]
                {
                    { 1, "Cairo", "Giza", 1, new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), "Cairo", "Mohamed Ali", "01001234567", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 1, "COD", 120m, "TRK1001", new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), false, false },
                    { 2, "Giza", "Cairo", 1, new DateTime(2026, 6, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), "Giza", "Nora Hussein", "01007654321", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 2, "Online", 80m, "TRK1002", new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), true, false },
                    { 3, "Cairo", "Alexandria", 1, new DateTime(2026, 6, 29, 12, 0, 0, 0, DateTimeKind.Unspecified), "Alexandria", "Omar Said", "01009998877", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, 3, "COD", 200m, "TRK1003", new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), false, true }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "Capacity", "CompanyId", "DriverId", "IsActive", "LicensePlate", "Type" },
                values: new object[,]
                {
                    { 1, 500.0, 1, 1, "Active", "ABC-123", "Car" },
                    { 2, 50.0, 2, 1, "Active", "XYZ-987", "Motorcycle" },
                    { 3, 1200.0, 1, 1, "Active", "LMN-456", "Van" }
                });

            migrationBuilder.InsertData(
                table: "Driver-Orders",
                columns: new[] { "Id", "DriverId", "OrderId", "isActive" },
                values: new object[,]
                {
                    { 1, 1, 1, "Active" },
                    { 2, 2, 2, "Active" },
                    { 3, 3, 3, "Active" }
                });

            migrationBuilder.InsertData(
                table: "Driver-Vehicles",
                columns: new[] { "Id", "AssignedAt", "DriverId", "ReturnedAt", "VehicleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 6, 21, 12, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2026, 6, 26, 12, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, new DateTime(2026, 6, 30, 12, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 }
                });

            migrationBuilder.InsertData(
                table: "Order-Products",
                columns: new[] { "Id", "Name", "OrderId", "Price", "Qty" },
                values: new object[,]
                {
                    { 1, "Item A", 1, 50m, 1 },
                    { 2, "Item B", 2, 35m, 2 },
                    { 3, "Item C", 3, 200m, 1 }
                });

            migrationBuilder.InsertData(
                table: "Tracking Histories of Orders",
                columns: new[] { "Id", "Location", "OrderId", "Status", "Timestamp" },
                values: new object[,]
                {
                    { 1, "Merchant", 1, "ReadyForPickup", new DateTime(2026, 6, 29, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Warehouse", 2, "Packed", new DateTime(2026, 6, 30, 12, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "System", 3, "Created", new DateTime(2026, 7, 1, 12, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Client_MerchantId",
                table: "Client",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver-Orders_DriverId",
                table: "Driver-Orders",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver-Orders_OrderId",
                table: "Driver-Orders",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Driver-Vehicles_DriverId",
                table: "Driver-Vehicles",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Driver-Vehicles_VehicleId",
                table: "Driver-Vehicles",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CompanyId",
                table: "Drivers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Order-Products_OrderId",
                table: "Order-Products",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCompany_CompanyId",
                table: "OrderCompany",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderCompany_OrderId",
                table: "OrderCompany",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CompanyId",
                table: "Orders",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DriverId",
                table: "Orders",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MerchantId",
                table: "Orders",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Rate Cards_CompanyId",
                table: "Rate Cards",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_CompanyId",
                table: "Ratings",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Ratings_MerchantId",
                table: "Ratings",
                column: "MerchantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracking Histories of Orders_OrderId",
                table: "Tracking Histories of Orders",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_DriverId",
                table: "Vehicles",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientPhones");

            migrationBuilder.DropTable(
                name: "CompanyPhones");

            migrationBuilder.DropTable(
                name: "Driver-Orders");

            migrationBuilder.DropTable(
                name: "Driver-Vehicles");

            migrationBuilder.DropTable(
                name: "DriverPhones");

            migrationBuilder.DropTable(
                name: "MerchantPhones");

            migrationBuilder.DropTable(
                name: "Order-Products");

            migrationBuilder.DropTable(
                name: "OrderCompany");

            migrationBuilder.DropTable(
                name: "Rate Cards");

            migrationBuilder.DropTable(
                name: "Ratings");

            migrationBuilder.DropTable(
                name: "Tracking Histories of Orders");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Merchants");

            migrationBuilder.DropTable(
                name: "Courier Company");
        }
    }
}
