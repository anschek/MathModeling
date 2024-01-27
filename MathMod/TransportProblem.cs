using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Collections;

namespace MathMod
{
    internal class TransportProblem
    {
        List<int> a_;
        List<int> b_;
        //тарифы и целевая функция
        List<List<int>> rates_;
        List<List<int>> objective_func_;

        public TransportProblem(List<int> a, List<int> b, List<List<int>> c)
        {
            a_ = a; b_ = b; rates_ = c;
        }

        public static void PrintMatrix(List<List<int>> matrix)
        {
            for (int i = 0; i < matrix.Count; ++i)
            {
                for (int j = 0; j < matrix[0].Count; ++j)
                {
                    Console.Write(matrix[i][j]);
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
        }
        static int GetMaxElement(List<List<int>> cur_rates)
        {
            int max_element = cur_rates[0][0];
            for (int i = 0; i < cur_rates.Count; ++i)
            {
                for (int j = 0; j < cur_rates[0].Count; ++j)
                {
                    int cur_max = cur_rates[i][j];
                    if (cur_max > max_element)
                    {
                        max_element = cur_max;
                    }
                }
            }
            return max_element;
        }

        static int GetMinElement(List<List<int>> cur_rates)
        {
            int min_element = cur_rates[0][0];
            for (int i = 0; i < cur_rates.Count; ++i)
            {
                for (int j = 0; j < cur_rates[0].Count; ++j)
                {
                    int cur_min = cur_rates[i][j];
                    if (cur_min < min_element)
                    {
                        min_element = cur_min;
                    }
                }
            }
            return min_element;
        }

        static (int, int) GetMinIndex(List<List<int>> cur_rates)
        {
            int min_element = GetMinElement(cur_rates);
            for (int i = 0; i < cur_rates.Count; ++i)
            {
                for (int j = 0; j < cur_rates[0].Count; ++j)
                {
                    if (cur_rates[i][j] == min_element)
                    {
                        return (i, j);
                    }
                }
            }
            return (-1, -1);//в случае ошибки
        }

        public void MethodOfMinElement()
        {
            List<List<int>> cur_rates = new List<List<int>> { };
            List<List<int>> cur_func = new List<List<int>> { };
            for (int i = 0; i < rates_.Count; ++i)
            {
                cur_rates.Add(new List<int> { });
                cur_func.Add(new List<int> { });
                for (int j = 0; j < rates_[0].Count; ++j)
                {
                    cur_func[i].Add(0);
                    cur_rates[i].Add(rates_[i][j]);
                }
            }

            List<int> cur_a = a_;
            List<int> cur_b = b_;

            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                (int, int) min_indexes = GetMinIndex(cur_rates);

                int i = min_indexes.Item1; int j = min_indexes.Item2;

                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = GetMaxElement(cur_rates) + 1;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;

            }
            objective_func_ = cur_func;
        }

        public List<List<int>> GetObjectiveFunction()
        {
            return objective_func_;
        }

        public int GetResult()
        {
            int result = 0;
            for (int i = 0; i < rates_.Count; ++i)
            {
                for (int j = 0; j < rates_[0].Count; ++j)
                {
                    result += rates_[i][j] * objective_func_[i][j];
                }
            }
            return result;

        }


    }
}
