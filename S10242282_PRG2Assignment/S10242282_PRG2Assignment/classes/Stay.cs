// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

class Stay
{
    public DateTime CheckinDate { get; set; }
    public DateTime CheckoutDate { get; set;}
    public List<Room> RoomList { get; set; } = new();

    public Stay() { }
    public Stay(DateTime ci, DateTime co)
    {
        CheckinDate = ci;
        CheckoutDate = co;
        RoomList = new();
    }

    public void AddRoom(Room room)
    {
        if (RoomList.Contains(room))
        {
            Console.WriteLine("Room has already been added");
        }
        else
        {
            RoomList.Add(room);
        }
    }

    public double CalculateTotal()
    {
        TimeSpan t = CheckoutDate - CheckinDate;
        int days = t.Days;
        double total = 0;
        
        foreach (Room r in RoomList)
        {
            double cost = (r.DailyRate + r.CalculateCharges()) * days;
            total += cost;
        }

        return total;
    }

    public override string ToString()
    {
        return "CheckinDate: " + CheckinDate.ToString("dd/MM/yyyy") +
            "\tCheckoutDate: " + CheckoutDate.ToString("dd/MM/yyyy");
    }
}