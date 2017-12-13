﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using myTestApp.Models;
using myTestApp.DatabaseHelp;
using static myTestApp.Session.SessionHelper;
using Microsoft.AspNetCore.Http;

namespace myTestApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.SetString(USERKEY, "");
            HttpContext.Session.SetString(PROJECTKEY, "");
            HttpContext.Session.SetString(GROUPKEY, "");
            HttpContext.Session.SetString(SELECTEDUSER, "");
            return RedirectToAction("myTestView", "Home");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult myTestView()
        {
            ViewData["Message"] = "This is my test page.";

            return View();
        }

        public IActionResult dashboard()
        {
            //Initialize session variables
            var sessionUserID = HttpContext.Session.GetString(USERKEY);
            var sessionProjectID = HttpContext.Session.GetString(PROJECTKEY);
            var sessionGroupID = HttpContext.Session.GetString(GROUPKEY);
            var sessionSelectedUserID = HttpContext.Session.GetString(SELECTEDUSER);
            //Initialize Database Helper
            DBHelper dbhelp = new DBHelper();

            //Data Structures
            //List<TimeCard> timeCardList = new List<TimeCard>();
                //<User Full Name, Users Total Worked Hours>
            Dictionary<String, String> groupUserHours = new Dictionary<string, string>();
            String projectHoursHigh;
            String projectHoursAverage;
            String projectHoursLow;
            String projectHoursSelectedUser;
                //<Group Name, Group Hours>
            Dictionary<String, String> projectGroupHours = new Dictionary<string, string>();
            bool isLoggedInUserSelected;
            if (sessionUserID.Equals(sessionSelectedUserID))
            {
                isLoggedInUserSelected = true;
            }
            else
            {
                isLoggedInUserSelected = false;
            }

            bool isLoggedInUserAdmin = true;
            ViewBag.isLoggedInUserAdmin = isLoggedInUserAdmin;


            List<Project> allProjects;
            allProjects = dbhelp.getAllProject();
            ViewBag.allProjects = allProjects;
            List<Group> projectGroups;
            projectGroups = new List<Group>();
            for (int i = 0; i < 3; i++){
                Group g = new Group();
                g.name = "Group " + i;
                projectGroups.Add(g);
            }
            ViewBag.projectGroups = projectGroups;
            List<User> groupUsers;

            //projectGroups = getProjcetGroups();
            //groupUsers = getGroupUsers();
            //timeCardList = dbhelp.getAllUserTimeCard(sessionSelectedUserID);
            //projectHoursHigh = getProjectHoursHigh();
            //projectHoursLow = getProjectHoursLow();
            //groupUserHours = getGroupUserHours();
            List<TimeCard> timeCardList = new List<TimeCard>();

            string Message = "LOGGED IN USER ID " + sessionUserID;
            ViewBag.userID = Message;
            Message = "CURRENT GROUP ID " + sessionGroupID;
            ViewBag.groupID = Message;
            Message = "CURRENT PROJECT ID " + sessionProjectID;
            ViewBag.projectID = Message;
            timeCardList = dbhelp.getAllUserTimeCard("13");
            ViewBag.timeCardList = timeCardList;
            return View();
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
