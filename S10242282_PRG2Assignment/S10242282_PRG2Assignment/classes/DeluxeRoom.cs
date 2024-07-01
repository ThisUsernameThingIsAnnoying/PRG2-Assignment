// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

class DeluxeRoom : Room
{
    public bool AdditionalBed { get; set; }

    public DeluxeRoom() : base() { }
    public DeluxeRoom(int r, string b, double d, bool i) : base(r, b, d, i) { }

    public override double CalculateCharges()
    {
        if (AdditionalBed == true)
        {
            return 25;
        }

        return 0;
    }

    public override string ToString()
    {
        return base.ToString() + "AdditionalBed: " + AdditionalBed;
    }
}