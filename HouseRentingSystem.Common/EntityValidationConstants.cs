namespace HouseRentingSystem.Common;

public static class EntityValidationConstants
{
    public static class CategoryConstants
    {
        public const int NameMinLength = 2;
        public const int NameMaxLength = 50;
    }

    public static class HouseConstants
    {
        public const int TitleMinLength = 10;
        public const int TitleMaxLength = 50;

        public const int AddressMinLength = 15;
        public const int AddressMaxLength = 150;

        public const int DesciptionMinLength = 50;
        public const int DesciptionMaxLength = 500;

        public const int ImgURLMaxLength = 2048;

        public const string PricePerMonthMinRange = "0";
        public const string PricePerMonthMaxRange = "5000";
    }

    public static class AgentConstants
    {
        public const int PhoneNumberMinLength = 7;
        public const int PhoneNumberMaxLength = 15;
    }

    public static class UserConstants
    {
        public const int FirstNameMinLength = 1;
        public const int FirstNameMaxLength = 12;

        public const int LastNameMinLength = 3;
        public const int LastNameMaxLength = 15;
    }
}
