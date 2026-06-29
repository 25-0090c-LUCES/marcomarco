namespace marcomarco
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    internal class Program
    {
        // marcowowowow
        //did it work?
        // USERNAMES AND PASSWORDS ONLY
        static List<string> UserNames = new List<string>();
        static List<string> UserPasswords = new List<string>();
        static List<string> UserLoc = new List<string>();
        static List<string> UserConNum = new List<string>();

        // PARALLEL LISTS FOR JOBS
        static List<string> JobIDs = new List<string>();
        static List<string> JobTitles = new List<string>();
        static List<string> JobBudgets = new List<string>();
        static List<string> JobEmployers = new List<string>();
        static List<string> JobWorkers = new List<string>();
        static List<string> JobStatuses = new List<string>();
        static List<string> JobRatings = new List<string>();
        static List<string> JobLocations = new List<string>();
        static List<string> JobDatePosted = new List<string>();

        // Engine queues and stacks for simple text parsing
        static Queue<string> LiveNotifications = new Queue<string>();
        static Stack<string> TransactionHistory = new Stack<string>();

        static void Main(string[] args)
        {
            LoadData();

            while (true)
            {
                string currentUser = LoginPortal();
                if (currentUser == "") continue;

                RunUniversalDashboard(currentUser);
            }
        }
        static string MaskPassword()
        {
            string pass = "";
            ConsoleKeyInfo key;
            do
            {
                key = Console.ReadKey(true);
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                {
                    pass = pass.Substring(0, (pass.Length - 1));
                    Console.Write("\b \b");
                }
            } while (key.Key != ConsoleKey.Enter);

            return pass;
        }

        static string LoginPortal()
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkGreen;

                Console.WriteLine(@"
                     ████████╗ █████╗ ███████╗██╗  ██╗██╗  ██╗ █████╗ ██╗   ██╗ █████╗ 
                     ╚══██╔══╝██╔══██╗██╔════╝██║ ██╔╝██║ ██╔╝██╔══██╗╚██╗ ██╔╝██╔══██╗
                        ██║   ███████║███████╗█████╔╝ █████╔╝ ███████║ ╚████╔╝ ███████║
                        ██║   ██╔══██║╚════██║██╔═██╗ ██╔═██╗ ██╔══██║  ╚██╔╝  ██╔══██║
                        ██║   ██║  ██║███████║██║  ██╗██║  ██╗██║  ██║   ██║   ██║  ██║
                        ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝  ╚═╝╚═╝  ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝  ╚═╝
");

                Console.ResetColor();

                Console.WriteLine("[1]. Login");
                Console.WriteLine("[2]. Register");
                Console.WriteLine("[0]. Exit");
                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                if (choice == 1)
                {
                    int maxAttempts = 3;
                    int attempts = 0;

                    while (attempts < maxAttempts)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("(Enter / to return to main menu.)");
                        Console.WriteLine("===============================================================================");
                        Console.WriteLine("                              LOGIN");
                        Console.WriteLine("===============================================================================");
                        Console.ResetColor();

                        Console.Write("Enter Username: ");
                        string u = Console.ReadLine().Trim();
                        if (u == "/")
                        {
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(u))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username cannot be empty. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }


                        Console.Write("Enter Password: ");
                        string p = MaskPassword().Trim();
                        Console.WriteLine();
                        if (p == "/")
                        {
                            break;
                        }
                        if (string.IsNullOrWhiteSpace(p))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Password cannot be empty.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        bool found = false;
                        for (int i = 0; i < UserNames.Count; i++)
                        {
                            if (UserNames[i].Equals(u, StringComparison.OrdinalIgnoreCase) && UserPasswords[i] == p)
                            {
                                found = true;
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine("\n[SUCCESS] LoggedIn Successfully! Welcome back, " + UserNames[i] + "!");
                                Console.ResetColor();
                                Thread.Sleep(1200);
                                return UserNames[i];
                            }
                        }

                        if (!found)
                        {
                            attempts++;
                            int left = maxAttempts - attempts;

                            if (left == 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Too many failed attempts. Access temporarily locked. Returning to main menu.");
                                Console.ResetColor();
                                Thread.Sleep(1200);
                                break;

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] We couldn't sign you in. The username may not exist, or the password entered is incorrect. Please check your credentials and try again.");
                                Console.ResetColor();
                                Thread.Sleep(1200);
                            }
                        }
                    }
                }
                else if (choice == 2)
                {
                    string u = "";
                    string p = "";
                    string l = "";
                    string c = "";
                    bool cancelled = false;

                    // USERNAME
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("(Enter / to return to main menu.)");
                        Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");

                        Console.Write("Enter Username: ");
                        u = Console.ReadLine().Trim();
                        if (u == "/")
                        {
                            cancelled = true; break;
                        }
                        bool exists = false;
                        for (int i = 0; i < UserNames.Count; i++)
                        {
                            if (UserNames[i].Equals(u, StringComparison.OrdinalIgnoreCase))
                            { exists = true; break; }
                        }

                        if (exists)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username already exists. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        if (string.IsNullOrWhiteSpace(u))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Username cannot be empty. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        break;
                    }

                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.Write("Enter Password: ");
                            p = MaskPassword().Trim();
                            Console.WriteLine();
                            if (p == "/")
                            {
                                cancelled = true; break;
                            }
                            if (string.IsNullOrWhiteSpace(p))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Password cannot be empty. Please try again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }


                    // LOCATION
                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.WriteLine($"Enter Password: {new string('*', p.Length)}");
                            Console.Write("Enter Location: ");
                            l = Console.ReadLine().Trim();
                            if (l == "/")
                            {
                                cancelled = true; break;
                            }
                            if (string.IsNullOrWhiteSpace(l))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Location cannot be empty. Please try again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }

                    if (!cancelled)
                    {
                        while (true)
                        {
                            Console.Clear();
                            Console.WriteLine("(Enter / to return to main menu.)");
                            Console.WriteLine("=========== REGISTER NEW ACCOUNT ===========");
                            Console.WriteLine($"Enter Username: {u}");
                            Console.WriteLine($"Enter Password: {new string('*', p.Length)}");
                            Console.WriteLine($"Enter Location: {l}");
                            Console.Write("Enter Contact Number (Format: 09XXXXXXXXX): ");
                            c = Console.ReadLine().Trim();

                            if (c == "/") { cancelled = true; break; }

                            if (c.Length != 11 || !c.All(char.IsDigit))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Contact number must be exactly 11 digits.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            if (!c.StartsWith("09"))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Contact number must start with 09. Please try again.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            bool contactExists = false;
                            for (int i = 0; i < UserConNum.Count; i++)
                            {
                                if (UserConNum[i] == c) { contactExists = true; break; }
                            }

                            if (contactExists)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("\n[ERROR] Contact number is already registered.");
                                Console.ResetColor();
                                Thread.Sleep(1000);
                                continue;
                            }

                            break;
                        }
                    }
                    if (!cancelled)
                    {
                        UserNames.Add(u);
                        UserPasswords.Add(p);
                        UserLoc.Add(l);
                        UserConNum.Add(c);

                        SaveData();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUCCESS] Account created successfully!");
                        Console.WriteLine("You may now log in.");
                        Console.ResetColor();
                        Thread.Sleep(1500);
                    }
                }
                else if (choice == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nThank you for using TaskKaya!");
                    Console.ResetColor();

                    Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. PLease try again.");
                    Console.ResetColor();

                    Thread.Sleep(1000);
                }
            }

        }

        static void RunUniversalDashboard(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================================");
                Console.WriteLine("  DASHBOARD | USER: " + username.ToUpper() + " | RATING: " + GetUserAverageRating(username));
                Console.WriteLine("===============================================================================");
                Console.ResetColor();

                int alertCount = 0;
                foreach (string notif in LiveNotifications)
                {
                    string[] parts = notif.Split('|');
                    if (parts.Length >= 3 && parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) && parts[2] == "UNREAD")
                    {
                        alertCount++;
                    }
                }

                if (alertCount > 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(" 🔔 [ALERT] You have (" + alertCount + ") unread message(s) in your Inbox!\n");
                    Console.ResetColor();
                }

                Console.WriteLine(" [1] Find a Job / Apply");
                Console.WriteLine(" [2] Post And ManageJobs");
                Console.WriteLine(" [3] Track My Ongoing Work");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(" [4] Check Notifications Inbox (Approved/Finished/Declined)");
                Console.ResetColor();
                Console.WriteLine(" [5] View History Ledger");
                Console.WriteLine(" [0] Logout");
                Console.WriteLine("-------------------------------------------------------------------------------");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                switch (choice)
                {
                    case 1: BrowseJobsBoard(username); break;
                    case 2: PostAndManageJobs(username); break;
                    case 3: TrackWorkerContracts(username); break;
                    case 4: CheckAndDisplayNotifications(username); break;
                    case 5: ViewMyLedger(username); break;
                    case 0: return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] Invalid choice. Please try again.");
                        Console.ResetColor();
                        Thread.Sleep(1000);
                        break;
                }
            }
        }
        static void PostAndManageJobs(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================================================");
                Console.WriteLine("  JOB MANAGEMENT HUB | USER: " + username.ToUpper());
                Console.WriteLine("===============================================================================");
                Console.ResetColor();

                Console.WriteLine(" [1] Post a Job");
                Console.WriteLine(" [2] Manage Posted Jobs");
                Console.WriteLine(" [3] Verify Completions");
                Console.WriteLine(" [0] Return to Dashboard");
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input. Numbers only. Please try again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                if (choice == 0) return;

                // ── [1] POST A JOB ────────────────────────────────────────────────────────
                if (choice == 1)
                {
                    Console.Clear();
                    Console.WriteLine("(Enter / to return.)");
                    Console.WriteLine("=== POST A JOB ===");

                    string title = "";
                    while (true)
                    {
                        Console.Write("Job Title: ");
                        title = Console.ReadLine().Trim();
                        if (title == "/") break;
                        if (string.IsNullOrWhiteSpace(title))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Job title cannot be empty.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            Console.Clear();
                            Console.WriteLine("(Enter / to return.)");
                            Console.WriteLine("=== POST A JOB ===");
                            continue;
                        }
                        break;
                    }
                    if (title == "/") continue;

                    string location = "";
                    while (true)
                    {
                        Console.Write("Job Location: ");
                        location = Console.ReadLine().Trim();
                        if (location == "/") break;
                        if (string.IsNullOrWhiteSpace(location))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Job location cannot be empty.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            Console.Clear();
                            Console.WriteLine("(Enter / to return.)");
                            Console.WriteLine("=== POST A JOB ===");
                            Console.WriteLine($"Job Title: {title}");
                            continue;
                        }
                        break;
                    }
                    if (location == "/") continue;

                    string budget = "";
                    while (true)
                    {
                        Console.Write("Budget (PHP): ");
                        budget = Console.ReadLine().Trim();
                        if (budget == "/") break;
                        if (string.IsNullOrWhiteSpace(budget))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Budget cannot be empty.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            Console.Clear();
                            Console.WriteLine("(Enter / to return.)");
                            Console.WriteLine("=== POST A JOB ===");
                            Console.WriteLine($"Job Title: {title}");
                            Console.WriteLine($"Job Location: {location}");
                            continue;
                        }
                        if (!decimal.TryParse(budget, out decimal budgetValue) || budgetValue <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Please enter a valid amount greater than 0.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            Console.Clear();
                            Console.WriteLine("(Enter / to return.)");
                            Console.WriteLine("=== POST A JOB ===");
                            Console.WriteLine($"Job Title: {title}");
                            Console.WriteLine($"Job Location: {location}");
                            continue;
                        }
                        break;
                    }
                    if (budget == "/") continue;

                    string newID = "JOB" + new Random().Next(100, 999);
                    JobIDs.Add(newID);
                    JobTitles.Add(title);
                    JobLocations.Add(location);
                    JobBudgets.Add(budget);
                    JobEmployers.Add(username);
                    JobWorkers.Add("None");
                    JobStatuses.Add("AVAILABLE");
                    JobRatings.Add("N/A");
                    JobDatePosted.Add(DateTime.Now.ToString("MM/dd/yyyy hh:mm tt"));
                    SaveData();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n[SUCCESS] Job {newID} posted successfully!");
                    Console.ResetColor();
                    Pause();
                }

                // ── [2] REVIEW APPLICANTS ─────────────────────────────────────────────────
                else if (choice == 2)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("                     MY POSTED JOBS");
                        Console.WriteLine("=========================================================================================");
                        Console.WriteLine("{0,-8} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-10} | {6,-10}",
                            "Job ID", "Title", "Location", "Budget", "Worker", "Status", "Applicants");
                        Console.WriteLine("=========================================================================================");
                        Console.ResetColor();

                        int count = 0;
                        for (int i = 0; i < JobIDs.Count; i++)
                        {
                            if (JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                            {
                                string workerDisplay = JobWorkers[i] == "None" ? "No worker" : JobWorkers[i];

                                int applicantCount = 0;
                                foreach (string notif in LiveNotifications)
                                {
                                    string[] parts = notif.Split('|');
                                    if (parts.Length >= 4 &&
                                        parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) &&
                                        parts[1] == "APPLIED")
                                    {
                                        string[] payload = parts[3].Split(';');
                                        if (payload.Length >= 2 && payload[1] == JobIDs[i])
                                            applicantCount++;
                                    }
                                }

                                Console.WriteLine("{0,-8} | {1,-10} | {2,-10} | {3,-12} | {4,-10} | {5,-10} | {6,-10}",
                                    JobIDs[i],
                                    JobTitles[i],
                                    JobLocations[i],
                                    "PHP " + JobBudgets[i],
                                    workerDisplay,
                                    JobStatuses[i],
                                    applicantCount);
                                count++;
                            }
                        }

                        if (count == 0)
                        {
                            Console.WriteLine("\nYou have not posted any jobs yet.");
                            Pause();
                            break;
                        }

                        Console.WriteLine("=========================================================================================");
                        Console.Write("\nEnter Job ID to view applicants (or / to return): ");
                        string selectedJobId = Console.ReadLine().Trim().ToUpper();

                        if (selectedJobId == "/") break;

                        if (string.IsNullOrWhiteSpace(selectedJobId))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Job ID cannot be empty.");
                            Console.ResetColor();
                            Pause();
                            continue;
                        }

                        // find the job
                        int jobIdx = -1;
                        for (int i = 0; i < JobIDs.Count; i++)
                        {
                            if (JobIDs[i] == selectedJobId &&
                                JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase))
                            {
                                jobIdx = i;
                                break;
                            }
                        }

                        if (jobIdx == -1)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Job ID not found or not yours.");
                            Console.ResetColor();
                            Pause();
                            continue;
                        }

                        // gather applicants for selected job
                        List<string> applicantMapping = new List<string>();
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("===============================================================================");
                        Console.WriteLine($" JOB: {JobIDs[jobIdx]} — {JobTitles[jobIdx]} | Status: {JobStatuses[jobIdx]}");
                        Console.WriteLine("===============================================================================");
                        Console.WriteLine("{0,-5} | {1,-15} | {2,-22} | {3,-15} | {4,-15}",
                            "No.", "Applicant", "Rating", "Location", "Contact");
                        Console.WriteLine("===============================================================================");
                        Console.ResetColor();

                        int appCount = 0;
                        foreach (string notif in LiveNotifications)
                        {
                            string[] parts = notif.Split('|');
                            if (parts.Length >= 4 &&
                                parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) &&
                                parts[1] == "APPLIED")
                            {
                                string[] payload = parts[3].Split(';');
                                if (payload.Length >= 2 && payload[1] == selectedJobId &&
                                    JobStatuses[jobIdx] == "AVAILABLE")
                                {
                                    string workerName = payload[0];
                                    string workerScore = GetUserAverageRating(workerName);
                                    string workerLocation = "";
                                    string workerContact = "";

                                    for (int k = 0; k < UserNames.Count; k++)
                                    {
                                        if (UserNames[k].Equals(workerName, StringComparison.OrdinalIgnoreCase))
                                        {
                                            workerLocation = UserLoc[k];
                                            workerContact = UserConNum[k];
                                            break;
                                        }
                                    }

                                    Console.WriteLine("{0,-5} | {1,-15} | {2,-10} | {3,-15} | {4,-15}",
                                        appCount + 1,
                                        workerName,
                                        workerScore,
                                        workerLocation,
                                        workerContact);
                                    applicantMapping.Add(parts[3]);
                                    appCount++;
                                }
                            }
                        }
                        Console.WriteLine("===============================================================================");


                        if (appCount == 0)
                        {
                            Console.WriteLine("\nNo pending applicants for this job.");
                            Pause();
                            continue;
                        }

                        int appIndex;
                        while (true)
                        {
                            Console.Write("\nEnter applicant number to evaluate (or / to return): ");
                            string input = Console.ReadLine().Trim();

                            if (input == "/") { appIndex = -1; break; }

                            if (!int.TryParse(input, out appIndex))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Numbers only.");
                                Console.ResetColor();
                                continue;
                            }

                            if (appIndex >= 1 && appIndex <= applicantMapping.Count) break;

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Number out of range.");
                            Console.ResetColor();
                        }

                        if (appIndex == -1) continue;

                        appIndex--;
                        string[] selectedPayload = applicantMapping[appIndex].Split(';');
                        string selectedWorker = selectedPayload[0];

                        Console.Clear();
                        Console.WriteLine($"Job:      {JobIDs[jobIdx]} — {JobTitles[jobIdx]}");
                        Console.WriteLine($"Applicant: {selectedWorker}");
                        Console.WriteLine($"Rating:   {GetUserAverageRating(selectedWorker)}");
                        Console.WriteLine("-------------------------------------------------------------------------------");
                        Console.WriteLine(" [1] Accept — move to ONGOING");
                        Console.WriteLine(" [2] Decline applicant");

                        int decision;
                        while (true)
                        {
                            Console.Write("Enter action: ");
                            if (int.TryParse(Console.ReadLine(), out decision)) break;
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[ERROR] Numbers only.");
                            Console.ResetColor();
                        }

                        if (decision == 1)
                        {
                            JobStatuses[jobIdx] = "ONGOING";
                            JobWorkers[jobIdx] = selectedWorker;
                            AddNotification(selectedWorker, "APPROVED",
                                $"Employer '{username}' accepted you for '{JobTitles[jobIdx]}' (ID: {JobIDs[jobIdx]}). You can start work now.");
                            RemoveApplicationNotification(username, selectedWorker, selectedJobId);
                            AutoDeclineOtherApplicants(username, selectedJobId, JobTitles[jobIdx], selectedWorker);
                            SaveData();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n[SUCCESS] Contract finalized. Worker can now track their ongoing duties.");
                            Console.ResetColor();
                        }
                        else if (decision == 2)
                        {
                            AddNotification(selectedWorker, "DECLINED",
                                $"Your application for '{JobTitles[jobIdx]}' (ID: {JobIDs[jobIdx]}) was declined.");
                            RemoveApplicationNotification(username, selectedWorker, selectedJobId);
                            SaveData();
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n[INFO] Applicant declined.");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Invalid choice.");
                            Console.ResetColor();
                        }

                        Pause();
                    }
                }

                // ── [3] VERIFY COMPLETIONS ────────────────────────────────────────────────
                else if (choice == 3)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("===============================================================================");
                    Console.WriteLine("  VERIFY COMPLETIONS");
                    Console.WriteLine("===============================================================================");
                    Console.WriteLine("{0,-8} | {1,-20} | {2,-15} | {3,-10}", "Job ID", "Title", "Worker", "Budget");
                    Console.WriteLine("===============================================================================");
                    Console.ResetColor();

                    int pendingCount = 0;
                    for (int i = 0; i < JobIDs.Count; i++)
                    {
                        if (JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase) &&
                            JobStatuses[i] == "PENDING_VERIFICATION")
                        {
                            Console.WriteLine("{0,-8} | {1,-20} | {2,-15} | {3,-10}",
                                JobIDs[i],
                                JobTitles[i],
                                JobWorkers[i],
                                "PHP " + JobBudgets[i]);
                            pendingCount++;
                        }
                    }

                    if (pendingCount == 0)
                    {
                        Console.WriteLine("\nNo completions pending verification.");
                        Pause();
                        continue;
                    }

                    Console.Write("\nEnter Job ID to verify (or / to return): ");
                    string targetId = Console.ReadLine().Trim().ToUpper();
                    if (targetId == "/") continue;

                    int matchIndex = -1;
                    for (int i = 0; i < JobIDs.Count; i++)
                    {
                        if (JobIDs[i] == targetId &&
                            JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase) &&
                            JobStatuses[i] == "PENDING_VERIFICATION")
                        {
                            matchIndex = i;
                            break;
                        }
                    }

                    if (matchIndex == -1)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] Job ID not found or not eligible for verification.");
                        Console.ResetColor();
                        Pause();
                        continue;
                    }

                    Console.Clear();
                    Console.WriteLine($"Job:    {JobIDs[matchIndex]} — {JobTitles[matchIndex]}");
                    Console.WriteLine($"Worker: {JobWorkers[matchIndex]}");
                    Console.WriteLine($"Pay:    PHP {JobBudgets[matchIndex]}");
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    Console.WriteLine(" [1] Approve & release payment");
                    Console.WriteLine(" [2] Reject & send back for revision");

                    int verifyChoice;
                    while (true)
                    {
                        Console.Write("Enter action: ");
                        if (int.TryParse(Console.ReadLine(), out verifyChoice)) break;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[ERROR] Invalid input. Numbers only.");
                        Console.ResetColor();
                    }

                    if (verifyChoice == 1)
                    {
                        int stars;
                        while (true)
                        {
                            Console.Write("Rate worker (1-5 stars): ");
                            if (!int.TryParse(Console.ReadLine(), out stars))
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Numbers only.");
                                Console.ResetColor();
                                continue;
                            }
                            if (stars < 1 || stars > 5)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("[ERROR] Enter a number between 1 and 5.");
                                Console.ResetColor();
                                continue;
                            }
                            break;
                        }

                        JobStatuses[matchIndex] = "COMPLETED";
                        JobRatings[matchIndex] = new string('★', stars);

                        string stackPayload = $"{JobIDs[matchIndex]}|{JobTitles[matchIndex]}|{JobEmployers[matchIndex]}|{JobWorkers[matchIndex]}|{JobBudgets[matchIndex]}|{JobRatings[matchIndex]}";
                        TransactionHistory.Push(stackPayload);

                        AddNotification(JobWorkers[matchIndex], "FINISHED",
                            $"Payment released! '{username}' approved your work on '{JobTitles[matchIndex]}'. Rating: {JobRatings[matchIndex]}");
                        SaveData();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUCCESS] Job finalized and payment released.");
                        Console.ResetColor();
                    }
                    else if (verifyChoice == 2)
                    {
                        JobStatuses[matchIndex] = "ONGOING";
                        AddNotification(JobWorkers[matchIndex], "DECLINED",
                            $"[!] Revision requested by '{username}' for job '{JobTitles[matchIndex]}' (ID: {JobIDs[matchIndex]}).");
                        SaveData();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("\n[INFO] Job sent back to worker for revision.");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] Invalid choice.");
                        Console.ResetColor();
                    }

                    Pause();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. Please try again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                }
            }
        }
        static void BrowseJobsBoard(string username)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== FIND A JOB ===");
                Console.WriteLine("[1] View All Jobs");
                Console.WriteLine("[2] Filter by Location");
                Console.WriteLine("[0] Return");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                if (choice == 0)
                    return;

                string filterLocation = "";

                if (choice == 1)
                {
                    filterLocation = "";
                }
                else if (choice == 2)
                {
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine("(Enter / to return to main menu.)");
                        Console.Write("Enter Location: ");
                        filterLocation = Console.ReadLine().Trim();

                        if (filterLocation == "/")
                        {
                            filterLocation = "";
                            break;
                        }

                        if (string.IsNullOrWhiteSpace(filterLocation))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n[ERROR] Location cannot be empty. Please try again.");
                            Console.ResetColor();
                            Thread.Sleep(1000);
                            continue;
                        }

                        break;
                    }

                    if (filterLocation == "")
                        continue;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. Please Try Again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("======================================================================================================================");
                Console.WriteLine("                                                     AVAILABLE JOBS");
                Console.WriteLine("======================================================================================================================");
                Console.WriteLine("{0,-8} | {1,-10} | {2,-10} | {3,-10} | {4,-10} | {5,-13} | {6,-15} | {7,-16}",
                    "Job ID", "Title", "Location", "Budget", "Employer", "Emp. Location", "Contact No.", "Date Posted");
                Console.WriteLine("======================================================================================================================");
                Console.ResetColor();

                int count = 0;

                for (int i = 0; i < JobIDs.Count; i++)
                {
                    bool available =
                        JobStatuses[i] == "AVAILABLE" &&
                        !JobEmployers[i].Equals(username, StringComparison.OrdinalIgnoreCase);

                    bool locationMatch =
                        choice == 1 ||
                        JobLocations[i].Equals(filterLocation, StringComparison.OrdinalIgnoreCase);

                    if (available && locationMatch)
                    {
                        string employerLocation = "";
                        string employerContact = "";

                        for (int j = 0; j < UserNames.Count; j++)
                        {
                            if (UserNames[j].Equals(JobEmployers[i], StringComparison.OrdinalIgnoreCase))
                            {
                                employerLocation = UserLoc[j];
                                employerContact = UserConNum[j];
                                break;
                            }
                        }

                        Console.WriteLine("{0,-8} | {1,-10} | {2,-10} | {3,-10} | {4,-10} | {5,-13} | {6,-15} | {7,-16}",
                            JobIDs[i],
                            JobTitles[i],
                            JobLocations[i],
                            "PHP " + JobBudgets[i],
                            JobEmployers[i],
                            employerLocation,
                            employerContact,
                            JobDatePosted[i]);
                        count++;
                    }
                }


                if (count == 0)
                {
                    Console.WriteLine("\nNo jobs found.");
                    Pause();
                    continue;
                }

                Console.Write("\nEnter Job ID to apply for(or enter / to return to main menu.): ");
                string targetId = Console.ReadLine().Trim().ToUpper();

                if (targetId == "/")
                    continue;

                if (targetId == "")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Job ID cannot be empty.");
                    Console.ResetColor();
                    Pause();
                    continue;
                }

                int matchIndex = -1;

                for (int i = 0; i < JobIDs.Count; i++)
                {
                    if (JobIDs[i] == targetId &&
                        JobStatuses[i] == "AVAILABLE")
                    {
                        matchIndex = i;
                        break;
                    }
                }

                if (matchIndex != -1)
                {
                    bool alreadyApplied = false;
                    foreach (string notif in LiveNotifications)
                    {
                        string[] parts = notif.Split('|');
                        if (parts.Length >= 4 &&
                            parts[0].Equals(JobEmployers[matchIndex], StringComparison.OrdinalIgnoreCase) &&
                            parts[1] == "APPLIED" &&
                            parts[3] == username + ";" + JobIDs[matchIndex])
                        {
                            alreadyApplied = true;
                            break;
                        }
                    }

                    if (alreadyApplied)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n[ERROR] You have already applied for this job.");
                        Console.ResetColor();
                    }
                    else
                    {
                        AddNotification(
                            JobEmployers[matchIndex],
                            "APPLIED",
                            username + ";" + JobIDs[matchIndex]
                        );
                        SaveData();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n[SUCCESS] Application submitted! Waiting for Employer review.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Job ID not found or unavailable.");
                    Console.ResetColor();
                }

                Pause();
            }
        }
        static string GetUserAverageRating(string workerName)
        {
            int totalStars = 0;
            int jobCount = 0;

            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobWorkers[i].Equals(workerName, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "COMPLETED")
                {
                    string stars = JobRatings[i];
                    if (!string.IsNullOrEmpty(stars) && stars != "N/A")
                    {
                        totalStars += stars.Length;
                        jobCount++;
                    }
                }
            }

            if (jobCount == 0) return "No performance reviews";
            int average = (int)Math.Round((double)totalStars / jobCount);
            return new string('*', average);
        }

        static void RemoveApplicationNotification(string employer, string worker, string jobId)
        {
            List<string> retainedNotifs = new List<string>();
            string targetPayload = worker + ";" + jobId;
            foreach (string n in LiveNotifications)
            {
                string[] parts = n.Split('|');
                if (parts.Length >= 4 &&
                    parts[0].Equals(employer, StringComparison.OrdinalIgnoreCase) &&
                    parts[1] == "APPLIED" &&
                    parts[3] == targetPayload)
                {
                    continue;
                }
                retainedNotifs.Add(n);
            }

            LiveNotifications.Clear();
            foreach (string n in retainedNotifs) LiveNotifications.Enqueue(n);
        }
        static void AutoDeclineOtherApplicants(string employer, string jobId, string jobTitle, string acceptedWorker)
        {
            List<string> retainedNotifs = new List<string>();
            List<string> workersToNotify = new List<string>();

            foreach (string n in LiveNotifications)
            {
                string[] parts = n.Split('|');
                bool isOtherApplicant = false;

                if (parts.Length >= 4 &&
                    parts[0].Equals(employer, StringComparison.OrdinalIgnoreCase) &&
                    parts[1] == "APPLIED")
                {
                    string[] payload = parts[3].Split(';');
                    if (payload.Length >= 2 &&
                        payload[1] == jobId &&
                        !payload[0].Equals(acceptedWorker, StringComparison.OrdinalIgnoreCase))
                    {
                        isOtherApplicant = true;
                        workersToNotify.Add(payload[0]);
                    }
                }

                if (isOtherApplicant)
                {
                    continue;
                }
                retainedNotifs.Add(n);
            }

            LiveNotifications.Clear();
            foreach (string n in retainedNotifs) LiveNotifications.Enqueue(n);

            foreach (string worker in workersToNotify)
            {
                AddNotification(worker, "DECLINED", "Sorry! The job '" + jobTitle + "' (ID: " + jobId + ") has already been filled by another applicant.");
            }
        }
        static void TrackWorkerContracts(string username)
        {
            int ongoingCount = 0;
            Console.Clear();
            Console.WriteLine("===============================================================================");
            Console.WriteLine("                      MY ONGOING WORK");
            Console.WriteLine("===============================================================================");
            Console.WriteLine("{0,-8} | {1,-20} | {2,-10} | {3,-15}", "Job ID", "Title", "Earnings", "Client");
            Console.WriteLine("===============================================================================");

            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobWorkers[i].Equals(username, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "ONGOING")
                {
                    Console.WriteLine("{0,-8} | {1,-20} | {2,-10} | {3,-15}",
                        JobIDs[i],
                        JobTitles[i],
                        "PHP " + JobBudgets[i],
                        JobEmployers[i]);
                    ongoingCount++;
                }
            }

            if (ongoingCount == 0)
            {
                Console.WriteLine("\nYou have no active approved jobs currently in active progress state.");
                Pause();
                return;
            }

            Console.Write("\nEnter Job ID to submit for approval: ");
            string targetId = Console.ReadLine().Trim().ToUpper();

            int matchIndex = -1;
            for (int i = 0; i < JobIDs.Count; i++)
            {
                if (JobIDs[i] == targetId && JobWorkers[i].Equals(username, StringComparison.OrdinalIgnoreCase) && JobStatuses[i] == "ONGOING")
                {
                    matchIndex = i;
                    break;
                }
            }

            if (matchIndex != -1)
            {
                JobStatuses[matchIndex] = "PENDING_VERIFICATION";
                AddNotification(JobEmployers[matchIndex], "FINISHED", "Review Required: '" + username + "' completed '" + JobTitles[matchIndex] + "' (ID: " + JobIDs[matchIndex] + ").");
                SaveData();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n[SUCCESS] Job submitted! Waiting for employer approval.");
                Console.ResetColor();
                Pause();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n[ERROR] Job ID not found or not yours.");
                Console.ResetColor();
                Pause();
            }
        }
        static void CheckAndDisplayNotifications(string username)
        {
            while (true)
            {
                List<string> personalNotifications = new List<string>();
                int unreadCount = 0;
                foreach (string notif in LiveNotifications)
                {
                    string[] parts = notif.Split('|');
                    if (parts[0].Equals(username, StringComparison.OrdinalIgnoreCase))
                    {
                        personalNotifications.Add(notif);
                        if (parts[2] == "UNREAD") unreadCount++;
                    }
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("===============================================================================");
                Console.WriteLine("    NOTIFICATIONS INBOX | TOTAL: " + personalNotifications.Count + " | UNREAD: " + unreadCount);
                Console.WriteLine("===============================================================================");
                Console.ResetColor();
                Console.WriteLine(" [1] View Folder: Approved/Applied Jobs");
                Console.WriteLine(" [2] View Folder: Jobs Finished / Submission Reviews");
                Console.WriteLine(" [3] View Folder: Declined / Returned Job Tasks");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" [4] Clear ALL My Notifications");
                Console.ResetColor();
                Console.WriteLine(" [0] Return to Main Menu");
                Console.WriteLine("-------------------------------------------------------------------------------");

                Console.Write("Enter number of choice: ");
                if (!int.TryParse(Console.ReadLine(), out int choice))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid input numbers only. Please try again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }
                if (choice == 0) return;

                if (choice == 4)
                {
                    List<string> retainedNotifs = new List<string>();
                    foreach (string n in LiveNotifications)
                    {
                        string[] parts = n.Split('|');

                        // Keep other users' notifications AND keep APPLIED
                        // notifications addressed to you (those are job applications, not inbox messages)
                        if (!parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) ||
                            parts[1] == "APPLIED")
                        {
                            retainedNotifs.Add(n);
                        }
                    }

                    LiveNotifications.Clear();
                    foreach (string n in retainedNotifs) LiveNotifications.Enqueue(n);

                    SaveData();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n[SUCCESS] Inbox cleared. Pending job applications were kept.");
                    Console.ResetColor();
                    Pause();
                    continue;
                }

                string targetedCategoryCode = "";
                if (choice == 1) { targetedCategoryCode = "APPROVED"; }
                else if (choice == 2) { targetedCategoryCode = "FINISHED"; }
                else if (choice == 3) { targetedCategoryCode = "DECLINED"; }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n[ERROR] Invalid choice. Please try again.");
                    Console.ResetColor();
                    Thread.Sleep(1000);
                    continue;
                }

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("=== FOLDER INBOX: " + targetedCategoryCode + " ===");
                Console.WriteLine("-------------------------------------------------------------------------------");

                int displayCount = 0;
                List<string> updatedNotifs = new List<string>();

                foreach (string n in LiveNotifications)
                {
                    string[] parts = n.Split('|');

                    if (parts[0].Equals(username, StringComparison.OrdinalIgnoreCase) && parts[1] == targetedCategoryCode)
                    {
                        Console.WriteLine(" * " + parts[3]);
                        displayCount++;
                        updatedNotifs.Add(parts[0] + "|" + parts[1] + "|READ|" + parts[3]);
                    }
                    else
                    {
                        updatedNotifs.Add(n);
                    }
                }

                LiveNotifications.Clear();
                foreach (string n in updatedNotifs) LiveNotifications.Enqueue(n);
                SaveData();

                if (displayCount == 0)
                {
                    Console.WriteLine("No notifications in this folder.");
                }

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.ResetColor();
                Pause();
            }
        }
        static void AddNotification(string target, string category, string message)
        {
            LiveNotifications.Enqueue(target + "|" + category + "|UNREAD|" + message);
            SaveData();
        }
        static void ViewMyLedger(string username)
        {
            int displayCount = 0;
            Console.Clear();
            Console.WriteLine("====================================================================================");
            Console.WriteLine("                        MY HISTORY LEDGER");
            Console.WriteLine("====================================================================================");
            Console.WriteLine("{0,-8} | {1,-10} | {2,-12} | {3,-12} | {4,-10} | {5,-8} | {6,-10}", "Job ID", "Title", "Employer", "Worker", "Paid", "Rating", "Your Role");
            Console.WriteLine("====================================================================================");

            foreach (string log in TransactionHistory)
            {
                string[] p = log.Split('|');
                if (p.Length >= 6)
                {
                    bool isEmployer = p[2].Equals(username, StringComparison.OrdinalIgnoreCase);
                    bool isWorker = p[3].Equals(username, StringComparison.OrdinalIgnoreCase);

                    if (isEmployer || isWorker)
                    {
                        string role = isEmployer ? "Employer" : "Worker";
                        Console.WriteLine("{0,-8} | {1,-10} | {2,-12} | {3,-12} | {4,-10} | {5,-8} | {6,-10}",
                            p[0], p[1], p[2], p[3], "PHP " + p[4], p[5], role);
                        displayCount++;
                    }
                }
            }

            if (displayCount == 0)
            {
                Console.WriteLine("\nNo completed transactions in your history yet.");
            }

            Pause();
        }
        static void SaveData()
        {
            List<string> userLines = new List<string>();
            for (int i = 0; i < UserNames.Count; i++)
                userLines.Add($"{UserNames[i]}|{UserPasswords[i]}|{UserLoc[i]}|{UserConNum[i]}");
            File.WriteAllLines("users_universal.txt", userLines);

            List<string> jobLines = new List<string>();
            for (int i = 0; i < JobIDs.Count; i++)
                jobLines.Add(JobIDs[i] + "|" + JobTitles[i] + "|" + JobLocations[i] +
             "|" + JobBudgets[i] + "|" + JobEmployers[i] +
             "|" + JobWorkers[i] + "|" + JobStatuses[i] +
             "|" + JobRatings[i] + "|" + JobDatePosted[i]);
            File.WriteAllLines("jobs_universal.txt", jobLines);

            File.WriteAllLines("notifications_universal.txt", LiveNotifications.ToArray());
            File.WriteAllLines("history_universal.txt", TransactionHistory.ToArray());
        }
        static void LoadData()
        {
            if (!File.Exists("users_universal.txt")) File.WriteAllText("users_universal.txt", "");
            if (!File.Exists("jobs_universal.txt")) File.WriteAllText("jobs_universal.txt", "");
            if (!File.Exists("notifications_universal.txt")) File.WriteAllText("notifications_universal.txt", "");
            if (!File.Exists("history_universal.txt")) File.WriteAllText("history_universal.txt", "");
            if (File.Exists("users_universal.txt"))
            {
                UserNames.Clear();
                UserPasswords.Clear();
                UserLoc.Clear();
                UserConNum.Clear();
                foreach (string line in File.ReadAllLines("users_universal.txt"))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string cleanLine = line.Replace(',', '|');
                    string[] p = cleanLine.Split('|');
                    if (p.Length == 4)
                    {
                        UserNames.Add(p[0]);
                        UserPasswords.Add(p[1]);
                        UserLoc.Add(p[2]);
                        UserConNum.Add(p[3]);
                    }
                }
            }
            if (File.Exists("jobs_universal.txt"))
            {
                JobIDs.Clear();
                JobTitles.Clear();
                JobBudgets.Clear();
                JobEmployers.Clear();
                JobWorkers.Clear();
                JobStatuses.Clear();
                JobRatings.Clear();
                JobLocations.Clear();
                foreach (string line in File.ReadAllLines("jobs_universal.txt"))
                {
                    string[] p = line.Split('|');
                    if (p.Length == 9)
                    {
                        JobIDs.Add(p[0]);
                        JobTitles.Add(p[1]);
                        JobLocations.Add(p[2]);
                        JobBudgets.Add(p[3]);
                        JobEmployers.Add(p[4]);
                        JobWorkers.Add(p[5]);
                        JobStatuses.Add(p[6]);
                        JobRatings.Add(p[7]);
                        JobDatePosted.Add(p[8]);
                    }
                }
            }
            if (File.Exists("notifications_universal.txt"))
            {
                LiveNotifications.Clear();
                foreach (string line in File.ReadAllLines("notifications_universal.txt"))
                    if (!string.IsNullOrWhiteSpace(line)) LiveNotifications.Enqueue(line);
            }
            if (File.Exists("history_universal.txt"))
            {
                TransactionHistory.Clear();
                string[] lines = File.ReadAllLines("history_universal.txt");
                for (int i = lines.Length - 1; i >= 0; i--)
                {
                    if (!string.IsNullOrWhiteSpace(lines[i]))
                        TransactionHistory.Push(lines[i]);
                }
            }
        }
        static void Pause()
        {
            Console.WriteLine("\nPress any key to return...");
            Console.ReadKey();
        }
    }

}
