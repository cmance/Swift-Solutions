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
        public const string BaseUrl = "http://localhost:5145";     // copied from launchSettings.json
        

        // File to store browser cookies in
        // public const string CookieFile = "..\\..\\..\\..\\..\\..\\..\\StandupsCookies.txt";

        // Page names that everyone should use
        // A handy way to look these up
        public static readonly Dictionary<string, string> Paths = new()
        {
            { "Home" , "/" },
<<<<<<< HEAD
            { "Login", "/Identity/Account/Login" },
            { "Explore", "/Home/Explore"},
=======
            { "Admin", "/Admin" },
            { "Notifications", "/Admin/ScheduledNotifications" },
            { "Login", "/Identity/Account/Login" },
>>>>>>> 38cbe6f2e35377718afec52d456aebcf81f5045e
            { "SelectGroup", "/MyAccount/SelectGroup" },
            { "AdminQuestions", "/Admin/Questions" },
            { "AdminQuestionsCreate", "/Admin/Questions/Create" },
            { "Favorites", "/Favorites" }
        };

        public static string PathFor(string pathName) => Paths[pathName];
        public static string UrlFor(string pathName) => BaseUrl + Paths[pathName];
    }
}
