namespace KmTravels.Core.Enums;

public static class UserRoles
{
    public const string SuperAdmin = "SuperAdmin";
    public const string StateAdmin = "StateAdmin";
    public const string DistrictAdmin = "DistrictAdmin";
    public const string Member = "Member";

    public static readonly string[] All = [SuperAdmin, StateAdmin, DistrictAdmin, Member];
}

public enum NewsCategory
{
    LatestNews = 1,
    PressRelease = 2,
    IndustryUpdate = 3,
    GovernmentAnnouncement = 4
}

public enum MemberType
{
    Operator = 1,
    CableWorker = 2
}

public enum MemberStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Expired = 4
}

public enum AdvertisementPosition
{
    HomeBanner = 1,
    Sidebar = 2,
    Footer = 3,
    Popup = 4
}

public enum OfficeBearerRole
{
    President = 1,
    Secretary = 2,
    Treasurer = 3,
    DistrictCoordinator = 4,
    CommitteeMember = 5
}

public enum PublicationType
{
    Magazine = 1,
    Circular = 2,
    Newsletter = 3
}

public enum VideoSource
{
    Upload = 1,
    YouTube = 2
}
