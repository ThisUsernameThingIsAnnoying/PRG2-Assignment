// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

class StandardRoom : Room
{
    public bool RequireWifi { get; set; }
    public bool RequireBreakfast { get; set; }

    public StandardRoom() : base() { }
    public StandardRoom(int r, string b, double d, bool i) : base(r, b, d, i) { }

    public override double CalculateCharges()
    {
        if (RequireWifi == true)
        {
            if (RequireBreakfast == true)
            {
                return 10 + 20;
            }

            else
            {
                return 10;
            }
        }

        else
        {
            if (RequireBreakfast == true)
            {
                return 20;
            }
        }

        return 0;
    }

    public override string ToString()
    {
        return base.ToString() + "RequireWifi: " + RequireWifi +
            "\tRequireBreakfast: " + RequireBreakfast;
    }
}