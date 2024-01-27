using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Program;

namespace MathMod
{
    internal class TransportProblem
    {
        int[] a_;
        int[] b_;
        int[,] rates_;
        int[,] fun_;

        public TransportProblem(int[]a, int[]b, int[,] rates)
        {
            a_=a; b_=b; rates_=rates;   
        }

        static int GetIndexesOfMinElemets(int[,] cur_rates)
        {
            int min_element = cur_rates[0, 0];
            for(int i=0; i < cur_rates.GetLength(0); ++i)
            {
                for(int j=0; j < cur_rates.GetLength(1); ++j)
                {
                    if(cur_rates[i, j] < min_element)
                    {
                        min_element = cur_rates[i, j];
                    }
                }
            }
            return min_element;
        }

        public void MethodOfMinElement()
        {
            int[,] cur_rates = rates_;
            int[] cur_a = a_;
            int[] cur_b = b_;

            int[,] cur_func = new int[a_.GetLength(0), b_.GetLength(0)];

            while (cur_a.Sum() > 0 || cur_b.Sum() > 0)
            {
                int min_element = GetIndexesOfMinElemets(cur_rates);

                for (int i = 0; i < cur_rates.GetLength(0); ++i)
                {
                    for (int j = 0; j < cur_rates.GetLength(1); ++j)
                    {
                        if (cur_rates[i, j] == min_element)
                        {
                            int supply = Math.Min(cur_a[i], cur_b[j]);
                            
                            cur_rates[i, j] = 500;

                            cur_func[i, j] = supply;
                            cur_a[i] -= supply;
                            cur_b[j] -= supply;
                        }
                    }
                }

            }
            fun_ = cur_func;
        }

        public int[,] GetFunc()
        {
            return fun_;
        }

    }
}
