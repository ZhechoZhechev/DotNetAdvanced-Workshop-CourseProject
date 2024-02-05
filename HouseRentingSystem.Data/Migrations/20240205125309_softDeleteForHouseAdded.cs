using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HouseRentingSystem.Data.Migrations
{
    /// <inheritdoc />
    public partial class softDeleteForHouseAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("16c645b5-0184-4928-8569-179eccba1e32"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("9335caf4-8afb-4f24-9742-b0e33e09044d"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("d203cb82-2622-40c8-b98f-b47d2b49ebb0"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Houses",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("1c885594-652c-4dc7-8d2f-b1b8f66efdf4"), "North London, UK (near the border)", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("6b515eec-45ff-47bd-9805-08dc2191a455"), "Big House Marina" },
                    { new Guid("6ec9b0b3-aec3-4e2d-a153-7b3eeb5543fd"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("dee87c31-e470-453e-8f14-bc1cbb420103"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a & o = &hp = 1", 1200.00m, null, "Family House Comfort" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("1c885594-652c-4dc7-8d2f-b1b8f66efdf4"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("6ec9b0b3-aec3-4e2d-a153-7b3eeb5543fd"));

            migrationBuilder.DeleteData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: new Guid("dee87c31-e470-453e-8f14-bc1cbb420103"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Houses");

            migrationBuilder.InsertData(
                table: "Houses",
                columns: new[] { "Id", "Address", "AgentId", "CategoryId", "Description", "ImageUrl", "PricePerMonth", "RenterId", "Title" },
                values: new object[,]
                {
                    { new Guid("16c645b5-0184-4928-8569-179eccba1e32"), "Near the Sea Garden in Burgas, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "It has the best comfort you will ever ask for. With two bedrooms, it is great for your family.", "https://cf.bstatic.com/xdata/images/hotel/max1024x768/179489660.jpg?k=2029f6d9589b49c95dcc9503a265e292c2cdfcb5277487a0050397c3f8dd545a & o = &hp = 1", 1200.00m, null, "Family House Comfort" },
                    { new Guid("9335caf4-8afb-4f24-9742-b0e33e09044d"), "Boyana Neighbourhood, Sofia, Bulgaria", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 2, "This luxurious house is everything you will need. It is just excellent.", "https://i.pinimg.com/originals/a6/f5/85/a6f5850a77633c56e4e4ac4f867e3c00.jpg", 2000.00m, null, "Grand House" },
                    { new Guid("d203cb82-2622-40c8-b98f-b47d2b49ebb0"), "North London, UK (near the border)", new Guid("d686a26f-6deb-48fd-899a-c8161cedbfd4"), 3, "A big house for your whole family. Don't miss to buy a house with three bedrooms.", "https://www.luxury-architecture.net/wpcontent/uploads/2017/12/1513217889-7597-FAIRWAYS-010.jpg", 2100.00m, new Guid("6b515eec-45ff-47bd-9805-08dc2191a455"), "Big House Marina" }
                });
        }
    }
}
