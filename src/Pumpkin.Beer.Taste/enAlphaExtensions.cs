namespace Pumpkin.Beer.Taste;

using System;
using Ardalis.GuardClauses;

public static class EnAlphaExtensions
{
    private static readonly string[] Columns = [
        "A",
        "B",
        "C",
        "D",
        "E",
        "F",
        "G",
        "H",
        "I",
        "J",
        "K",
        "L",
        "M",
        "N",
        "O",
        "P",
        "Q",
        "R",
        "S",
        "T",
        "U",
        "V",
        "W",
        "X",
        "Y",
        "Z",
    ];

    public static string IndexToColumn(int index)
    {
        Guard.Against.Negative(index);
        return Columns[index];
    }
}
