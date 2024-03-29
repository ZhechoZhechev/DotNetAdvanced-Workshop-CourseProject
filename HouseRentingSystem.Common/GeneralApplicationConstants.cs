﻿namespace HouseRentingSystem.Common;

public static class GeneralApplicationConstants
{
    public const int ReleaseYear = 2024;

    public const int CurrentPageConst = 1;
    public const int HousesPerPageConst = 3;

    public const string AdminAreaName = "Admin";
    public const string AdminRoleName = "Administrator";
    public const string AdminEmail = "Admin@houserentingsystem.com";

    public const string UsersCacheKey = "UsersCache";
    public const int UserMemoryCacheExpDuration = 5;

    public const string RentedHousesCacheKey = "RentedHouses";
    public const int RentedHousesMemoryCacheExpDuration = 5;

    public const string OnlineUsersCookieName = "IsOnline";
    public const int LastActivityBeforeOfflineMinutes = 10;
}
