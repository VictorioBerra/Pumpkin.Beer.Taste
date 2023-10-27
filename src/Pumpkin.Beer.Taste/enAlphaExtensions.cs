namespace Pumpkin.Beer.Taste;

using System;
using Ardalis.GuardClauses;

public static class EnAlphaExtensions
{
    private static readonly string[] Columns = new[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK", "AL", "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU", "AV", "AW", "AX", "AY", "AZ", "BA", "BB", "BC", "BD", "BE", "BF", "BG", "BH" };

    public static string IndexToColumn(int index)
    {
        Guard.Against.Negative(index);
        return Columns[index];
    }
}
