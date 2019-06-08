namespace My_Calculator.Helpers
{
    public interface INumeralSystemConversion
    {
        string Binary { get; }
        string Hexadecimal { get; }
        string Octal { get; }
    }
}