using System.Text;
using static LottoGeneratorService.RandomNumberProcedures;

namespace LottoGeneratorService
{
    public class NumbersSetGenerator
    {
        public static async Task<string> GenerateSets(int min, int max, int numbersPerSet, int sets, decimal SpreadPercent)
        {
            int bits = 4;
            string results = "";
            int[] NumberSets = new int[sets];
            await Task.Run(() =>
            {
                //increases the bits to the minimum required to represent the number
                while ((Math.Pow(2, bits)) < max + 1)
                {
                    bits++;
                }

                StringBuilder sbSets = new();
                StringBuilder sbNumbers = new();

                //outer loop : Sets
                for (int i = 0; i < sets; i++)
                {
                    int[] NumberSet = ComputeNumberSet(numbersPerSet, min, max, bits, SpreadPercent, true);
                    for (int j = 0; j < NumberSet.Length; j++)
                    {
                        sbNumbers.AppendLine(NumberSet[j].ToString());
                    }
                    sbSets.AppendLine(sbNumbers.ToString().ReplaceLineEndings(",").TrimEnd(','));
                    sbNumbers.Clear();
                }
                results = sbSets.ToString();
            });
            return results;
        }
    }
}
