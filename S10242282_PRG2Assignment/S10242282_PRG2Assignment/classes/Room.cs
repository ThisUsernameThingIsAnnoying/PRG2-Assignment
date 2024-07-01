// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

abstract class Room
{
    public int RoomNumber { get; set; }
    public string BedConfiguration { get; set; }
    public double DailyRate { get; set; }
    public bool IsAvail { get; set; }

    public Room() { }
    public Room(int r, string b, double d, bool i)
    {
        RoomNumber = r;
        BedConfiguration = b;
        DailyRate = d;
        IsAvail = i;
    }

    public abstract double CalculateCharges();

    public override string ToString()
    {
        return "RoomNumber: " + RoomNumber +
            "\tBedConfiguration: " + BedConfiguration +
            "\tDailyRate: " + DailyRate +
            "\tIsAvail: " + IsAvail;
    }
}