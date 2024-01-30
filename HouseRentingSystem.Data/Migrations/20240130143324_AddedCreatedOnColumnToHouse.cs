using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedOnColumnToHouse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("2d956d3b-df02-46dd-9b90-c0fd043b0777"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("3d4bb33c-6634-4f35-a485-355a4e21c8ae"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("c577cbbb-7fd6-4482-be47-dfe34c6fb7d8"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Houses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 30, 14, 33, 23, 658, DateTimeKind.Utc).AddTicks(6550));

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("1a92462a-f10e-47fa-896e-626816ed9e0b"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("2fd3f436-0d86-4f77-b843-43defcac8160"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a & o = &hp = 1", 1200.00m, null, "Family House Comfort" },
                    { new Guid("44a813cd-cfbb-4f32-a9fa-dfd85c585d44"), "North London, UK (near the border)", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("6b515eec-45ff-47bd-9805-08dc2191a455"), "Big House Marina" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("1a92462a-f10e-47fa-896e-626816ed9e0b"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("2fd3f436-0d86-4f77-b843-43defcac8160"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("44a813cd-cfbb-4f32-a9fa-dfd85c585d44"));

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Houses");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("2d956d3b-df02-46dd-9b90-c0fd043b0777"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("3d4bb33c-6634-4f35-a485-355a4e21c8ae"), "North London, UK (near the border)", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("6b515eec-45ff-47bd-9805-08dc2191a455"), "Big House Marina" },
                    { new Guid("c577cbbb-7fd6-4482-be47-dfe34c6fb7d8"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a & o = &hp = 1", 1200.00m, null, "Family House Comfort" }
                });
        }
    }
}
