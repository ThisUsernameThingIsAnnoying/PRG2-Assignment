// ====================================================================
// Student Number : S10242282
// Student Name   : Teo Keng Hwee Sherwyn
// ====================================================================

class Guest
{
    public string Name { get; set; }
    public string PassportNum { get; set; }
    public Stay HotelStay { get; set; }
    public Membership Member { get; set; }
    public bool IsCheckedin { get; set; }

    public Guest() { }
    public Guest(string n, string p, Stay h, Membership m)
    {
        Name = n;
        PassportNum = p;
        HotelStay = h;
        Member = m;
    }

    public override string ToString()
    {
        return "Name: " + Name +
            "PassportNum: " + PassportNum +
            "HotelStay: " + HotelStay +
            "Member: " + Member;
    }
}