// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

class Membership 
{
    public string Status { get; set; }
    public int Points { get; set; }

    public Membership() { }
    public Membership(string s, int p)
    {
        Status = s;
        Points = p;
    }

    public void EarnPoints(double amt)
    {
        Points += Convert.ToInt32(amt) / 10;

        if (Status != "Gold" || Status != "Silver")
        {
            if (Points >= 100)
            {
                Status = "Silver";
            }
        }

        if (Status != "Gold")
        {
            if (Points >= 200)
            {
                Status = "Gold";
            }
        }
    }

    public bool RedeemPoints(int amt)
    {
        if (amt <= Points)
        {
            Points -= amt;
            return true;
        }
        else
        {
            Console.WriteLine("Your points are not enough.");
            return false;
        }
    }

    public override string ToString()
    {
        return "Status: " + Status +
            "\tPoints: " + Points;
    }
}