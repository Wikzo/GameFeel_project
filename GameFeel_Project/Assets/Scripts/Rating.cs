using System;

public class Rating
{
    string timeFormat = "MM/dd/yyyy-h:mm";            // Use this timeFormat


    public string Description = "*write a description here*";
    public int Confidence = 5;
    public int HowXIsThis = 2;
    public int HowMuchILikedThis = 6;
    public int HowDifficult = 1;
    public int HowEnjoyable = 4;

    private string Date = "*time*";

    public string ToStringDatabaseFormat()
    {
        DateTime time = DateTime.Now;
        Date = time.ToString(timeFormat);

        return
            string.Format(
                "&Date={0}&Description={1}&Confidence={2}&HowXIsThis={3}&HowMuchILikedThis={4}&HowDifficult={5}&HowEnjoyable={6}",
                Date,
                Description,
                Confidence,
                HowXIsThis,
                HowMuchILikedThis,
                HowDifficult,
                HowEnjoyable);
    }

}