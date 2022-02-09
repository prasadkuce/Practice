try
{
    Console.WriteLine("Enter a Number");
    string? number = Console.ReadLine();
    number = NumberToWords.ConvertAmount(double.Parse(number));
    Console.WriteLine($"Number in words is {number}");
    Console.ReadKey();

}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
public class NumberToWords
{
    private static string[] units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Ninteen" };
    private static string[] tens = { "", "", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

    public static string ConvertAmount(double amount)
    {
        try
        {
            long amountLong = (long)amount;
            long amountDecimal = (long) Math.Round((amount - (double)(amount)) * 100);
            if (amountDecimal == 0)
                return Convert(amountLong) + " Only";
            else
                return Convert(amountLong) + " Point " + Convert(amountDecimal) + " Only.";
        }
        catch (Exception e)
        {
            throw;
        }
    }
    private static string Convert(long amount)
    {
        if (amount < 20)
            return units[amount];
        if (amount < 100)
            return tens[amount / 10] + ((amount % 10 > 0) ? Convert(amount % 10) : " ");
        if(amount < 1000)
            return units[amount / 100] + " Hundred " + ((amount % 100 > 0) ? Convert(amount % 100) : " ");
        if(amount < 100000)
            return units[amount / 1000] + " Thousand " + ((amount % 1000 > 0) ? Convert(amount % 1000) : " ");
        if (amount < 10000000)
            return units[amount / 100000] + " Lakh " + ((amount % 100000 > 0) ? Convert(amount % 100000) : " ");
        if (amount < 1000000000)
            return units[amount / 10000000] + " Crore " + ((amount % 10000000 > 0) ? Convert(amount % 10000000) : " ");
        return units[amount / 1000000000] + " Arab"  + ((amount % 1000000000 > 0) ? Convert(amount % 1000000000) : " ");
    }

}