namespace My_Calculator.Helpers
{
    public interface IFileSizeConversion
    {
        double Bits { get; }
        double Bytes { get; }
        double Gigabytes { get; }
        double Kilobytes { get; }
        double Megabytes { get; }
        double Petabytes { get; }
        double Terabytes { get; }
    }
}