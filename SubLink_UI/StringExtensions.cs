﻿using System.Collections.Generic;

namespace SubLink_UI;

public static class StringExtensions
{
    public static string GetUnique(this ICollection<string> values, in string baseValue) {
        int counter = 1;
        string result = baseValue;

        while (values.Contains(result)) {
            result = $"{baseValue}{counter}";
            counter++;
        }

        return result;
    }
}
