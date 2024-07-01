// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

List<Guest> guestList = new();
List<Room> roomList = new();

void Menu()
{
    Console.WriteLine("---------------- M E N U ----------------" +
        "\n[1] List all Guests" +
        "\n[2] List available Rooms" +
        "\n[3] Register Guest" +
        "\n[4] Check-in Guest" +
        "\n[5] Display Guest's stay detail" +
        "\n[6] Extend stay" +
        "\n[7] Display Bill" +
        "\n[8] Check-out Guest" +        
        "\n[0] Exit" +
        "\n-----------------------------------------");
}

void InitRoom(List<Room> roomList)
{
    using StreamReader sr = new("csv/Rooms.csv");
    string? s = sr.ReadLine();

    while ((s = sr.ReadLine()) != null)
    {
        string[] info = s.Split(',');

        if (info[0] == "Standard")
        {
            StandardRoom room = new(Convert.ToInt32(info[1]), info[2], Convert.ToDouble(info[3]), true);
            roomList.Add(room);
        }

        else
        {
            DeluxeRoom room = new(Convert.ToInt32(info[1]), info[2], Convert.ToDouble(info[3]), true);
            roomList.Add(room);
        }
    }
}

void InitGuest(List<Guest> guestList, List<Room> roomList)
{
    using StreamReader sr = new("csv/Guests.csv");
    string? s = sr.ReadLine();

    while ((s = sr.ReadLine()) != null)
    {
        string[] guestInfo = s.Split(",");

        string name = guestInfo[0];
        string passport = guestInfo[1];
        Membership member = new(guestInfo[2], Convert.ToInt32(guestInfo[3]));

        using StreamReader sr1 = new("csv/Stays.csv");
        string? s1 = sr1.ReadLine();

        while ((s1 = sr1.ReadLine()) != null)
        {
            string[] stayInfo = s1.Split(",");

            if (passport == stayInfo[1])
            {
                DateTime checkin = Convert.ToDateTime(stayInfo[3]);
                DateTime checkout = Convert.ToDateTime(stayInfo[4]);

                Stay stay = new(checkin, checkout);
                Guest guest = new(name, passport, stay, member);

                guestList.Add(guest);

                for (int i = 5; i < 10;)
                {
                    if (stayInfo[i] != "")
                    {
                        foreach (Room r in roomList)
                        {
                            if (r.RoomNumber == Convert.ToInt32(stayInfo[i]))
                            {
                                if (r is StandardRoom standardroom)
                                {
                                    standardroom.IsAvail = false;
                                    standardroom.RequireWifi = Convert.ToBoolean(stayInfo[i + 1]);
                                    standardroom.RequireBreakfast = Convert.ToBoolean(stayInfo[i + 2]);

                                    guest.HotelStay.AddRoom(standardroom);
                                }

                                else
                                {
                                    DeluxeRoom deluxeroom = (DeluxeRoom)r;
                                    {
                                        deluxeroom.IsAvail = false;
                                        deluxeroom.AdditionalBed = Convert.ToBoolean(stayInfo[i + 3]);
                                        guest.HotelStay.AddRoom(deluxeroom);
                                    }
                                }
                            }
                        }
                    }
                    i += 4;
                }
            }
        }
    }
}

void DisplayGuest(List<Guest> guestList)
{
    Console.WriteLine("{0,-10} {1,-13} {2,-14} {3,-14} {4,-13} {5,-12} {6}", 
        "Name", "Passport", "Check-in", "Check-out", "Room", "Status", "Points");
    
    foreach (Guest g in guestList)
    {
        string roomNum = "";

        if (g.HotelStay.RoomList != null)
        {
            foreach (Room r in g.HotelStay.RoomList)
            {
                roomNum += r.RoomNumber + ", ";
            }

            Console.WriteLine("{0,-10} {1,-13} {2,-14} {3,-14} {4,-13} {5,-12} {6}", 
                g.Name, g.PassportNum, g.HotelStay.CheckinDate.ToString("dd/MM/yyyy"), g.HotelStay.CheckoutDate.ToString("dd/MM/yyyy"), roomNum, g.Member.Status, g.Member.Points);
        }
    }
}

void DisplayRoom(List<Room> roomList)
{
    Console.WriteLine("{0,-9}  {1,-11}  {2,-17}  {3}",
            "RoomType", "RoomNumber", "BedConfiguration", "DailyRate");

    foreach (Room r in roomList)
    {
        if (r is StandardRoom)
        {
            Console.WriteLine("{0,-9}  {1,-11}  {2,-17}  {3}",
            "Standard", r.RoomNumber, r.BedConfiguration, r.DailyRate.ToString("$0"));
        }

        else
        {
            Console.WriteLine("{0,-9}  {1,-11}  {2,-17}  {3}",
            "Deluxe", r.RoomNumber, r.BedConfiguration, r.DailyRate.ToString("$0"));
        }
    }
}

void RegisterGuest(List<Guest> guestList)
{
    while (true)
    {
        Console.Write("Please enter your name: ");
        string name = Console.ReadLine();

        if (name.All(Char.IsLetter) == true && name != "")
        {
            Console.Write("\nPlease enter your passport number: ");
            string passport = Console.ReadLine();

            bool found = false;

            foreach (Guest g in guestList)
            {
                if (g.PassportNum == passport.ToUpper())
                {
                    found = true;

                    Console.WriteLine("\nPassport number has already been entered.");
                    continue;
                }
            }

            if (found == false)
            {
                if (passport.Length == 9 && Char.IsLetter(passport[0]) == true && Char.IsLetter(passport[8]) == true)
                {
                    bool pass = true;

                    for (int i = 1; i < 8; i++)
                    {
                        if (char.IsNumber(passport[i]) == false)
                        {
                            pass = false;
                            continue;
                        }
                    }

                    if (pass == false)
                    {
                        Console.WriteLine("\nPassport number between the first and last alphabets are not all digits.");
                        continue;
                    }

                    else
                    {
                        Membership member = new("Ordinary", 0);

                        Stay stay = new();

                        Guest guest = new(name, passport.ToUpper(), stay, member);

                        guestList.Add(guest);

                        string guests = name + "," + passport.ToUpper() + "," + member.Status + "," + "0";

                        using (StreamWriter sw = new("Guests.csv", true))
                        {
                            sw.WriteLine(guests);
                        }

                        Console.WriteLine("\nYou have been registered.");

                        break;
                    }
                }

                else
                {
                    Console.WriteLine("\nPassport length is not 9 digits or starting and ending characters are not alphabets.");
                    continue;
                }
            }
        }

        else
        {
            Console.WriteLine("Invalid name entered. Please try again.");
            continue;
        }
    }
}

void CheckIn(List<Guest> guestList, List<Room> roomList)
{
    while (true)
    {
        DisplayGuest(guestList);

        Console.Write("\nSelect the Guest's passport number: ");
        string passport = Console.ReadLine();

        Guest guest = null;

        foreach (Guest g in guestList)
        {
            if (passport == g.PassportNum)
            {
                guest = g;
            }            
        }

        if (guest == null)
        {
            Console.WriteLine("Guest can't be found. Please try again.\n");
            continue;
        }

        Console.Write("\nEnter the check-in date (dd/MM/yyyy): ");
        string datein = Console.ReadLine();
        
        try
        {
            DateTime CheckIn = Convert.ToDateTime(datein);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("");
            continue;
        }

        DateTime checkin = Convert.ToDateTime(datein);

        Console.Write("\nEnter the check-out date (dd/MM/yyyy): ");
        string dateout = Console.ReadLine();

        try
        {
            DateTime CheckIn = Convert.ToDateTime(dateout);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("");
            continue;
        }

        DateTime checkout = Convert.ToDateTime(dateout);

        if (checkout < checkin)
        {
            Console.WriteLine("\nInvalid check-out date. Must be later than check-in date. Please try again.\n");
            continue;
        }

        Stay stay = new(checkin, checkout);

        Console.WriteLine("\n{0,-9}  {1,-11}  {2,-17}  {3}",
            "RoomType", "RoomNumber", "BedConfiguration", "DailyRate");

        foreach (Room r in roomList)
        {
            if (r.IsAvail != false)
            {
                if (r is StandardRoom)
                {
                    Console.WriteLine("{0,-9}  {1,-11}  {2,-17}  {3}",
                    "Standard", r.RoomNumber, r.BedConfiguration, r.DailyRate.ToString("$0"));
                }

                else
                {
                    Console.WriteLine("{0,-9}  {1,-11}  {2,-17}  {3}",
                    "Deluxe", r.RoomNumber, r.BedConfiguration, r.DailyRate.ToString("$0"));
                }
            }
        }

        Console.Write("\nSelect a Room Number: ");
        string number = Console.ReadLine();

        try
        {
            int RoomNum = Convert.ToInt32(number);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("");
            continue;
        }

        int roomNum = Convert.ToInt32(number);

        Room room = null;

        foreach (Room r in roomList)
        {
            if (roomNum == r.RoomNumber)
            {
                room = r;
            }            
        }

        if (room == null)
        {
            Console.WriteLine("\nRoom can't be found. Please try again.\n");
            continue;
        }

        if (room is StandardRoom standardroom)
        {
            bool wifi = false;
            bool breakfast = false;

            Console.Write("\nRequire Wifi [Y/N]: ");
            string wantwifi = Console.ReadLine();

            if (wantwifi.ToUpper() == "Y")
            {
                wifi = true;
            }

            if (wantwifi.ToUpper() != "Y" || wantwifi.ToUpper() == "N")
            {
                Console.WriteLine("\nInvalid option. Please try again.\n");
                continue;
            }

            Console.Write("\nRequire breakfast [Y/N]: ");
            string wantbreakfast = Console.ReadLine();

            if (wantbreakfast.ToUpper() == "Y")
            {
                breakfast = true;
            }

            else if (wantbreakfast.ToUpper() == "N")
            {
                breakfast = false;
            }

            else
            {
                Console.WriteLine("\nInvalid option. Please try again.\n");
            }

            standardroom.RequireWifi = wifi;
            standardroom.RequireBreakfast = breakfast;
            standardroom.IsAvail = true;

            stay.AddRoom(standardroom);
        }

        else
        {
            DeluxeRoom deluxeroom = (DeluxeRoom)room;

            bool bed = false;

            Console.Write("\nRequire additional bed [Y/N]: ");
            string wantbed = Console.ReadLine();

            if (wantbed.ToUpper() == "Y")
            {
                bed = true;
            }

            else if (wantbed.ToUpper() == "N")
            {
                bed = false;
            }

            else
            {
                Console.WriteLine("\nInvalid option. Please try again.\n");
                continue;
            }

            deluxeroom.AdditionalBed = bed;
            deluxeroom.IsAvail = true;

            stay.AddRoom(deluxeroom);
        }

        Console.Write("\nSelect another room [Y/N]: ");
        string option = Console.ReadLine();

        if (option.ToUpper() == "Y")
        {
            guest.HotelStay = stay;
            guest.IsCheckedin = true;

            Console.WriteLine("");
        }

        else if (option.ToUpper() == "N")
        {
            guest.HotelStay = stay;
            guest.IsCheckedin = true;

            Console.WriteLine("\nYou have checked-in successfully. Enjoy your stay.");

            break;
        }

        else
        {
            Console.WriteLine("\nInvalid option. Your check-in has failed. Please try again.\n");
        }
    }
}

void CheckOut(List<Guest> guestList)
{
    while (true)
    {
        DisplayGuest(guestList);

        Console.Write("\nSelect the Guest's passport number: ");
        string passport = Console.ReadLine();

        Guest guest = null;

        foreach (Guest g in guestList)
        {
            if (passport == g.PassportNum)
            {
                guest = g;
            }
        }

        if (guest == null)
        {
            Console.WriteLine("\nGuest can't be found. Please try again.\n");
            continue;
        }        

        double total = guest.HotelStay.CalculateTotal();

        Console.WriteLine("\nTotal Bill:  $" + total.ToString("0.00"));

        Console.WriteLine("\n{0,-10} {1}",
            "Status", "Points");

        Console.WriteLine("{0,-10} {1}",
            guest.Member.Status, guest.Member.Points);

        if (guest.Member.Status == "Ordinary")
        {
            Console.WriteLine("\nYour status is not high enough to redeem points.");
        }

        else
        {
            Console.Write("\nEnter points to offset the bill: ");
            string number = Console.ReadLine();

            try
            {
                int Amt = Convert.ToInt32(number);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
                continue;
            }

            int amt = Convert.ToInt32(number);

            bool redeemPoints = guest.Member.RedeemPoints(amt);

            if (redeemPoints == true)
            {
                total -= amt;
            }
        }

        Console.WriteLine("\nTotal Bill:  $" + total.ToString("0.00"));

        Console.WriteLine("\nPress any key to make payment...");
        Console.ReadLine();

        Console.WriteLine("Payment successful. Thank you for staying with us!");

        guest.Member.EarnPoints(total);
        guest.IsCheckedin = false;
        
        break;
    }
}

void DisplayStay(List<Guest> guestList)
{
    while (true)
    {
        DisplayGuest(guestList);

        Console.Write("\nEnter the Guest's passport number: ");
        string passport = Console.ReadLine();

        bool passportFound = false;

        foreach (Guest g in guestList)
        {
            if (g.PassportNum == passport.ToUpper())
            {
                passportFound = true;

                Console.WriteLine("\n" + g.HotelStay.ToString());

                break;
            }
        }

        if (passportFound == false)
        {
            Console.WriteLine("\nPassport number not found. Please try again.\n");
            continue;
        }

        else
        {
            break;
        }
    }
}

void ExtendStay(List<Guest> guestList)
{
    while (true)
    {
        DisplayGuest(guestList);

        Console.Write("\nEnter the Guest's passport number: ");
        string passport = Console.ReadLine();

        Guest guest = null;

        foreach (Guest g in guestList)
        {
            if (passport == g.PassportNum)
            {
                guest = g;
            }
        }

        if (guest == null)
        {
            Console.WriteLine("\nGuest not found. Please try again.\n");
            continue;
        }

        if (guest.IsCheckedin == true)
        {
            Console.Write("\nEnter number of days to extend: ");
            string number = Console.ReadLine();

            try
            {
                int Days = Convert.ToInt32(number);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("");
                continue;
            }

            int days = Convert.ToInt32(number);

            guest.HotelStay.CheckoutDate.AddDays(days);

            Console.WriteLine("\nYou have successfully extended your stay. Have a nice stay!");

            break;
        }

        else
        {
            Console.WriteLine("\nYou are not checked-in. Please check in first.");
            break;
        }
    }
}

void DisplayBill(List<Guest> guestList)
{
    while (true)
    {
        var months = new Dictionary<int, double>
        {
            { 1, 0 },
            { 2, 0 },
            { 3, 0 },
            { 4, 0 },
            { 5, 0 },
            { 6, 0 },
            { 7, 0 },
            { 8, 0 },
            { 9, 0 },
            { 10, 0 },
            { 11, 0 },
            { 12, 0 }
        };

        List<string> monthList = new()
    {
        "Jan",
        "Feb",
        "Mar",
        "Apr",
        "May",
        "June",
        "July",
        "Aug",
        "Sep",
        "Oct",
        "Nov",
        "Dec"
    };

        double bill = 0;

        Console.Write("Enter the year: ");
        string stringYear = Console.ReadLine();

        try
        {
            int Year = Convert.ToInt32(stringYear);
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("");
            continue;
        }

        int year = Convert.ToInt32(stringYear);

        double total = 0;

        foreach (Guest g in guestList)
        {
            if (g.HotelStay.CheckoutDate.Year == year)
            {
                for (int i = 1; i < 13; i++)
                {
                    if (g.HotelStay.CheckoutDate.Month == i)
                    {
                        months[i] = g.HotelStay.CalculateTotal(); ;
                        total += g.HotelStay.CalculateTotal();
                    }
                }
            }
        }
        int x = 1;

        foreach (string m in monthList)
        {
            Console.WriteLine("{0,-5} {1}:    ${2}",
                m, year, months[x].ToString("0.00"));

            x++;
        }

        Console.WriteLine("\nTotal:         ${0}", total.ToString("0.00"));

        break;
    }
}

InitRoom(roomList);
InitGuest(guestList, roomList);

while (true)
{
    Menu();

    Console.Write("Please select an option: ");
    string choice = Console.ReadLine();

    Console.WriteLine("");

    if (choice == "1")
    {
        DisplayGuest(guestList);
    }

    else if (choice == "2")
    {
        DisplayRoom(roomList);
    }

    else if (choice == "3")
    {
        RegisterGuest(guestList);
    }

    else if (choice == "4")
    {
        CheckIn(guestList, roomList);
    }

    else if (choice == "5")
    {
        DisplayStay(guestList);
    }

    else if (choice == "6")
    {
        ExtendStay(guestList);
    }

    else if (choice == "7")
    {
        DisplayBill(guestList);
    }

    else if (choice == "8")
    {
        CheckOut(guestList);
    }

    else if (choice == "0")
    {
        Console.WriteLine("Goodbye!");
        break;
    }

    else
    {
        Console.WriteLine("Invalid choice. Please choose again.");
    }

    Console.WriteLine("\n");
}