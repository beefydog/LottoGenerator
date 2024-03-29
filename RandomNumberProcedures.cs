﻿using System.Text;

namespace LottoGeneratorService;
/// <summary>
/// some goodies for interlock based random numbers 
/// NOTE: The GenerateBoolean() method uses CPU core "noise" instead of algo  - not my idea, but an ex-EE from Intel determined that truly random numbers can be generated
/// Apparently, the more cores, the more randomness, but even 2 cores are sufficiently random. This is running on a 16 core cpu, so no worries :)
/// </summary>
internal static class RandomNumberProcedures
{

    internal static bool OddEvenRatioGood(Int32[] testArray)
    {
        bool retval = false;
        //if (testArray.Length == 5)
        //{
        //    // this is to determine if there are either 2 or 3 even numbers (3 or 2 odd numbers) in a five element array - KEEP for explanation
        //    int ModResult = (testArray[0] % 2) + (testArray[1] % 2) + (testArray[2] % 2) + (testArray[3] % 2) + (testArray[4] % 2);
        //    if (ModResult == 2 || ModResult == 3) //if ModResult
        //    {
        //        retval = true;
        //    }
        //}

        // new version for test arrays of any size
        // the ModResult will be a function of the array size, for example if the array has 25 elements, we want to see if the odd/even ratio is about
        // half - 25 is an odd number, so 12.5 is not an absolute answer, we need to look for 12<>13 Mod results (12 odds and 13 evens, or 12 even and 13 odds)
        // so create a low and a high number from 25 (take 25/2  and get the floor and the ceiling)
        int ElementsCount = testArray.Length;
        decimal DividedCount = ElementsCount / 2;
        int LowRange = Convert.ToInt32(Math.Floor(DividedCount));
        int HighRange = Convert.ToInt32(Math.Ceiling(DividedCount));
        int ModSum = 0;
        foreach (Int32 element in testArray)
        {
            ModSum += (element % 2);
        }
        if (ModSum == LowRange || ModSum == HighRange)
        {
            retval = true;
        }
        return retval;
    }

    //basic version - keep for reference in tutorial
    //public static int GetNumFromOneToN(int N, int bits)
    //{
    //    string x = string.Empty;
    //    StringBuilder sb = new();
    //    int num = 0;

    //    while (num == 0 || num > N)
    //    {
    //        for (int i = 0; i < bits; i++)
    //        {
    //            sb.Append(GenerateBoolean() ? '1' : '0');
    //        }
    //        x = sb.ToString();

    //        //random reversal of string
    //        if (GenerateBoolean()) x.Reverse();

    //        sb.Clear();
    //        num = Convert.ToInt32(x, 2);
    //    }
    //    return num;
    //}

    /// <summary>
    /// Returns a single random integer between min and max via the random boolean generator
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="bits">predetermined/passed by caller: the minimum # of bits required to represent max</param>
    /// <returns>32 bit integer</returns>
    internal static int GetNumFromMinToMax(int min, int max, int bits)
    {
        string x;
        StringBuilder sb = new();
        int num = 0;

        while (num < min || num > max)
        {
            for (int i = 0; i < bits; i++)
            {
                sb.Append(GenerateBoolean() ? '1' : '0');
            }
            x = sb.ToString();

            //random reversal of string for extra randomness - though it looks like this is not really contributing much - needs testing
            if (GenerateBoolean()) _ = x.Reverse();

            sb.Clear();
            num = Convert.ToInt32(x, 2);
        }
        return num;
    }

    /// <summary>
    /// This randomly generates a boolean value (1 or 0) from CPU hardware via Interlocks 
    /// </summary>
    /// <returns>bool</returns>
    internal static bool GenerateBoolean()
    {
        UInt64 gen1 = 0;
        UInt64 gen2 = 0;
        Task.Run(() =>
        {
            while (gen1 < 1 || gen2 < 1)
                Interlocked.Increment(ref gen1);
        });
        while (gen1 < 1 || gen2 < 1)
            Interlocked.Increment(ref gen2);
        return (gen1 + gen2) % 2 == 0;
    }

    //DEPRECATED
    ///// <summary>
    ///// Generates a single set of numbers - DEPRECATED
    ///// </summary>
    ///// <param name="NumbersPerGroup"></param>
    ///// <param name="Min"></param>
    ///// <param name="Max"></param>
    ///// <param name="Bits"></param>
    ///// <param name="divergence"></param>
    ///// <param name="Sort"></param>
    ///// <returns></returns>
    //public static Int32[] ComputeNumberSet(int NumbersPerGroup, int Min, int Max, int Bits, decimal divergence = 10, bool Sort = true)
    //{
    //    Int32[] FinalArray = new Int32[NumbersPerGroup];
    //    Int32[] NumArray = new Int32[NumbersPerGroup];

    //    Int32 total = FinalArray.Sum();

    //    int MiddleSum = (NumbersPerGroup * Max) / 2; //compute the maximum sum for the numbers in the set, then divide by 2
    //    int LowBound = Convert.ToInt32(MiddleSum * (1 - divergence / 100)); //if, say 10%, then take 10/100 = 0.1 and subtract from 1 to get 0.9, then multiply
    //    int HighBound = Convert.ToInt32(MiddleSum * (1 + divergence / 100));//if, say 10%, then take 10/100 = 0.1 and add to 1 to get 1.1, then multiply
    //    int num = 0;

    //    //test if set of numbers is not within the boundaries or does not have a good odd/even number ratio - if true generate a set until it meets all criteria 
    //    while (FinalArray.Sum() < LowBound || FinalArray.Sum() > HighBound || !OddEvenRatioGood(FinalArray))
    //    {
    //        for (int i = 0; i < NumbersPerGroup; i++)
    //        {
    //            num = GetNumFromMinToMax(Min, Max, Bits); //get next random number
    //            while (Array.Exists(NumArray, element => element == num)) //check to see if number is already picked, if so, try until it doesn't already exist in the array
    //            {
    //                num = GetNumFromMinToMax(Min, Max, Bits);
    //            }
    //            NumArray[i] = num;
    //            FinalArray[i] = num;
    //        }
    //    }

    //    if (Sort)
    //    {
    //        Array.Sort(FinalArray);
    //    }

    //    return FinalArray;
    //}


    /// <summary>
    /// Generates a single set of numbers - This version splits different cases for speed increases if options are/are not enabled, no point in crunching more
    /// REFACTOR ME
    /// </summary>
    /// <param name="NumbersPerGroup">Numbers per group (each "set" is made up of 1 or more groups)</param>
    /// <param name="Min">lower bound integer</param>
    /// <param name="Max">upper bound integer</param>
    /// <param name="Bits">min bits to represent "Max" integer - precomputed by calling function</param>
    /// <param name="Divergence">divergence from center "sums" in percent up to 50% each side</param>
    /// <param name="Sort">True to sort integers ascending</param>
    /// <param name="SumCheck">True to check the sums of a particular group so they fall in a statistically more likely range</param>
    /// <param name="OECheck">True to check the odd/even ratio of numbers - (e.g. for 6 numbers - the odds are higher that 3 numbers will be even and 3 odd)</param>
    /// <returns>Array of 32 bit integers</returns>
    internal static Int32[] ComputeNumberSet2(int NumbersPerGroup, int Min, int Max, int Bits, decimal Divergence = 10, bool Sort = true, bool SumCheck = true, bool OECheck = true)
    {
        Int32[] FinalArray = new Int32[NumbersPerGroup];
        Int32[] NumArray = new Int32[NumbersPerGroup];

        Int32 total = FinalArray.Sum();

        int MiddleSum = (NumbersPerGroup * Max) / 2; //compute the maximum sum for the numbers in the set, then divide by 2
        int LowBound = Convert.ToInt32(MiddleSum * (1 - Divergence / 100)); //if, say 10%, then take 10/100 = 0.1 and subtract from 1 to get 0.9, then multiply
        int HighBound = Convert.ToInt32(MiddleSum * (1 + Divergence / 100));//if, say 10%, then take 10/100 = 0.1 and add to 1 to get 1.1, then multiply
        int num = 0;

        if (SumCheck && OECheck)
        {
            //test if set of numbers is not within the boundaries or does not have a good odd/even number ratio - if true generate a set until it meets all criteria 
            while (FinalArray.Sum() < LowBound || FinalArray.Sum() > HighBound || !OddEvenRatioGood(FinalArray))
            {
                for (int i = 0; i < NumbersPerGroup; i++)
                {
                    num = GetNumFromMinToMax(Min, Max, Bits); //get next random number
                    while (Array.Exists(NumArray, element => element == num)) //check to see if number is already picked, if so, try until it doesn't already exist in the array
                    {
                        num = GetNumFromMinToMax(Min, Max, Bits);
                    }
                    NumArray[i] = num;
                    FinalArray[i] = num;
                }
            }
        }
        else if (SumCheck)
        {
            while (FinalArray.Sum() < LowBound || FinalArray.Sum() > HighBound)
            {
                for (int i = 0; i < NumbersPerGroup; i++)
                {
                    num = GetNumFromMinToMax(Min, Max, Bits); //get next random number
                    while (Array.Exists(NumArray, element => element == num)) //check to see if number is already picked, if so, try until it doesn't already exist in the array
                    {
                        num = GetNumFromMinToMax(Min, Max, Bits);
                    }
                    NumArray[i] = num;
                    FinalArray[i] = num;
                }
            }
        }
        else if (OECheck)
        {
            while (!OddEvenRatioGood(FinalArray))
            {
                for (int i = 0; i < NumbersPerGroup; i++)
                {
                    num = GetNumFromMinToMax(Min, Max, Bits); //get next random number
                    while (Array.Exists(NumArray, element => element == num)) //check to see if number is already picked, if so, try until it doesn't already exist in the array
                    {
                        num = GetNumFromMinToMax(Min, Max, Bits);
                    }
                    NumArray[i] = num;
                    FinalArray[i] = num;
                }
            }
        }
        else
        {
            for (int i = 0; i < NumbersPerGroup; i++)
            {
                num = GetNumFromMinToMax(Min, Max, Bits); //get next random number
                while (Array.Exists(NumArray, element => element == num)) //check to see if number is already picked, if so, try until it doesn't already exist in the array
                {
                    num = GetNumFromMinToMax(Min, Max, Bits);
                }
                NumArray[i] = num;
                FinalArray[i] = num;
            }
        }

        if (Sort)
        {
            Array.Sort(FinalArray);
        }

        return FinalArray;
    }
}
