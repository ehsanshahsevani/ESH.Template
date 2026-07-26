namespace Persistence.Tools;

public static class AnnouncementMethods
{
    public static bool IsPalindromeBetween(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2)
        {
            return false;
        }

        if (input.First() == input.Last())
        {
            return true;
        }

        return false;
    }

    public static bool HasConsecutiveNumbers(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2)
        {
            return false;
        }

        foreach (char c in input)
        {
            if (char.IsDigit(c) == false)
            {
                return false;
            }
        }

        int first = input[0] - '0';
        int second = input[1] - '0';
        int difference = second - first;

        if (Math.Abs(difference) != 1)
        {
            return false;
        }

        for (int i = 1; i < input.Length - 1; i++)
        {
            int current = input[i] - '0';
            int next = input[i + 1] - '0';
        
            if (next - current != difference)
            {
                return false;
            }
        }

        return true;
    }

    public static bool HasRepeatedDigits(string input)
    {
        if (string.IsNullOrEmpty(input) || input.Length < 2)
        {
            return false;
        }

        char firstChar = input[0];
        for (int i = 1; i < input.Length; i++)
        {
            if (input[i] != firstChar)
            {
                return false;
            }
        }

        return true;
    }
}