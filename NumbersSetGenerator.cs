using System.Text;
using static LottoGeneratorService.RandomNumberProcedures;

namespace LottoGeneratorService;

public class NumbersSetGenerator
{
    //DEPRECATED
    //public static async Task<string> GenerateSets(int min, int max, int numbersPerGroup, int sets, decimal divergence)
    //{
    //    int bits = 4;
    //    string results = "";
    //    int[] NumberSets = new int[sets];
    //    await Task.Run(() =>
    //    {
    //        //increases the bits to the minimum required to represent the number
    //        while ((Math.Pow(2, bits)) < max + 1)
    //        {
    //            bits++;
    //        }

    //        StringBuilder sbSets = new();
    //        StringBuilder sbNumbers = new();

    //        //outer loop : Sets
    //        for (int i = 0; i < sets; i++)
    //        {
    //            int[] NumberSet = ComputeNumberSet(numbersPerGroup, min, max, bits, divergence, true);
    //            for (int j = 0; j < NumberSet.Length; j++)
    //            {
    //                sbNumbers.AppendLine(NumberSet[j].ToString());
    //            }
    //            sbSets.AppendLine(sbNumbers.ToString().ReplaceLineEndings(",").TrimEnd(','));
    //            sbNumbers.Clear();
    //        }
    //        results = sbSets.ToString();
    //    });
    //    return results;
    //}


    public static async Task<List<int[]>> GenerateSetsAsListOfIntArray(int[] min, int[] max, int[] numbersPerGroup, decimal[] divergence, int sets, bool[] sumcheck, bool[] oecheck, bool sort = true)
    {
        //this one takes in multiple groups as input
        int bits = 4;
        List<int[]> results = new List<int[]>();

        await Task.Run(() =>
        {
            while ((Math.Pow(2, bits)) < max.Max() + 1) //use the maximum number of the request group
            {
                bits++;
            }
            //get the length from either min, max or numbersPerGroup int array (should all be the same length)
            int groupsCount = numbersPerGroup.Length;

            //outer loop : Sets
            for (int i = 0; i < sets; i++)
            {
                // int[] combinedGroups = new int[numbersPerGroup.Sum()]; //say for certain lottery, 5 numbers are 1-70,2 numbers are 1-25 and 1 number is 1-5, there will be 8 numbers total
                int[] combinedGroups = new int[0]; //using Array Append below, so setting this as array with 0 elements

                // int ctStart = numbersPerGroup.Sum(); //for alternate method below, decided not to use
                for (int j = 0; j < groupsCount; j++)
                {
                    int[] NumberSet = ComputeNumberSet2(numbersPerGroup[j], min[j], max[j], bits, divergence[j], sort, sumcheck[j], oecheck[j]);

                    //  NumberSet.CopyTo(combinedGroups,0);  //alternate method, but need to track CopyTo starting point, so not using this  
                    foreach (int number in NumberSet)
                    {
                        combinedGroups = combinedGroups.Append(number).ToArray(); //as always, don't forget the ToArray() after appending!
                    }
                }

                results.Add(combinedGroups);
            }
        });
        return results;
    }
}
