﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PopNGo_BDD_Tests.Shared
{
    // Sitewide definitions and useful methods
    public class Common
    {
        public const string BaseUrl = "https://localhost:5145";     // copied from launchSettings.json
        

        // File to store browser cookies in
        // public const string CookieFile = "..\\..\\..\\..\\..\\..\\..\\StandupsCookies.txt";

        // Page names that everyone should use
        // A handy way to look these up
        public static readonly Dictionary<string, string> Paths = new()
        {
            { "Home" , "/" },
            { "Explore", "/Home/Explore"},
            { "Favorites", "/Favorites" },
            { "History", "/Home/History" },
            { "Itinerary", "/Home/Itinerary"},
            { "Login", "/Identity/Account/Login" },
            { "Profile", "/Identity/Account/Manage"},
            { "Recommendation", "/Home/Recommendation"},
            { "Admin", "/Admin" },
            { "Notifications", "/Admin/ScheduledNotifications" },
            { "Metrics", "/Admin/Metrics" },
        };

        public static string PathFor(string pathName) => Paths[pathName];
        public static string UrlFor(string pathName) => BaseUrl + Paths[pathName];

        
    }
}
