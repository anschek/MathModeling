using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime;

namespace MathMod
{
    internal class TransportProblem
    {
        List<int> a_;
        List<int> b_;
        //тарифы и целевая функция
        List<List<int>> rates_;
        List<List<int>> optimal_;

        ClosednessType type_;

        enum ClosednessType
        {
            Close,
            ShortageOfSupply,
            SurplusOfSupply
        }

        public TransportProblem(List<int> a, List<int> b, List<List<int>> c)
        {
            NewTransportProblem(a, b, c);
        }

        public void NewTransportProblem(List<int> a, List<int> b, List<List<int>> c)
        {
            a_ = a; b_ = b; rates_ = c;

            if (ListSum(a_) == ListSum(b_)) type_ = ClosednessType.Close;
            else ReduceProblemToClosed();
        }

        static int ListSum(List<int> list)
        {
            return list.Select(x => x).Sum(); ;
        }
        
        void ReduceProblemToClosed()
        {
            int SumA = ListSum(a_);
            int SumB = ListSum(b_);
            int diff = Math.Abs(SumA - SumB);
            int max_elem_to_add = GetMaxElement(rates_);
            //не хватает груза
            if (SumA < SumB)
            {
                a_.Add(diff);
                //добавляем фиктивного поставщика - строку
                rates_.Add(new List<int> { });
                for (int j = 0; j < rates_[0].Count(); ++j)
                {
                    rates_[rates_.Count - 1].Add(max_elem_to_add);
                    //rates_[rates_.Count - 1][j] = max_elem_to_add;
                }
            }
            else
            {
                b_.Add(diff);
                //добавляем фиктивного потребителя - столбец
                type_ = ClosednessType.SurplusOfSupply;
                for (int i = 0; i < rates_.Count(); ++i)
                {
                    rates_[i].Add(max_elem_to_add);
                    //rates_[rates_[0].Count - 1] = max_elem_to_add;
                }
            }
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
            return cur_rates.Select(x => x.Max()).Max();
        }

        static int GetMinElement(List<List<int>> cur_rates)
        {
            return cur_rates.Select(x => x.Min()).Min();
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

        static List<int> Init1DList( int size, int fill)
        {
            List<int> list = new List<int>();
            for(int i = 0;i < size; ++i)
            {
                list.Add(fill);
            }
            return list;
        }

        static List<int> Init1DList(List<int> orig)
        {
            List<int> copy = new List<int>();
            for (int i = 0; i < orig.Count; ++i)
            {
                copy.Add(orig[i]);
            }
            return copy;
        }

        static List<List<int>> Init2DList(int size0, int size1, int fill)
        {
            List < List<int> >list = new List<List<int>>();
            for (int i = 0; i < size0; ++i)
            {
                list.Add(Init1DList(size1, fill));
            }
            return list;
        }

        static List<List<int>> Init2DList(List<List<int>> orig)
        {
            List<List<int>> copy = new List<List<int>>();
            for (int i = 0; i < orig.Count; ++i)
            {
                copy.Add(Init1DList(orig[i]));
            }
            return copy;
        }

        public void MethodOfMinElement()
        {
            //инициализация списков
            List<List<int>> cur_rates = Init2DList(rates_), 
                            cur_func = Init2DList(rates_.Count, rates_[0].Count, 0);
            List<int> cur_a = Init1DList(a_), cur_b = Init1DList(b_);

            //алгоритм
            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                (int, int) min_indexes = GetMinIndex(cur_rates);

                int i = min_indexes.Item1; int j = min_indexes.Item2;

                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = GetMaxElement(cur_rates);
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;

            }
            optimal_ = cur_func;
        }

        static void CalculatePenalties(List<List<int>> cur_rates,  ref List<int> column_of_penalty, ref List<int> row_of_penalty)
        {
            int max_elem_to_add = GetMaxElement(cur_rates);
            int min = max_elem_to_add, premin = max_elem_to_add;
            for(int i = 0;i < cur_rates.Count; ++i)
            {
                for(int j = 0;j < cur_rates[0].Count; ++j)
                {
                    if (cur_rates[i][j] < min)
                    {
                        premin = min;
                        min = cur_rates[i][j];
                    }
                    else if (cur_rates[i][j] < premin && cur_rates[i][j] !=min) premin = cur_rates[i][j];

                }
                if (min == max_elem_to_add && premin == max_elem_to_add) column_of_penalty[i] = -1;
                else if (premin == max_elem_to_add) column_of_penalty[i] = min;
                else column_of_penalty[i] = premin - min;
                min = max_elem_to_add; premin = max_elem_to_add;
            }

            for (int j = 0;j < cur_rates[0].Count; ++j)
            {
                min = max_elem_to_add; premin = max_elem_to_add;
                for (int i = 0; i < cur_rates.Count; ++i)
                {
                    if (cur_rates[i][j] < min)
                    {
                        premin = min;
                        min = cur_rates[i][j];
                    }
                    else if (cur_rates[i][j] < premin && cur_rates[i][j] != min) premin = cur_rates[i][j];
                }
                if (min == max_elem_to_add && premin == max_elem_to_add) row_of_penalty[j] = -1;
                else if (premin == max_elem_to_add) row_of_penalty[j] = min;
                else row_of_penalty[j] = premin - min;
            }
        }

        static (int,int) IndexesOfMinElementWithMaxPenalty(List<List<int>> cur_rates, List<int> column_of_penalty, List<int> row_of_penalty)
        {
            if (column_of_penalty.Max() > row_of_penalty.Max())
            {
                int ind_max_in_column = column_of_penalty.FindIndex(x => x == column_of_penalty.Max());
                return (ind_max_in_column, (
                    cur_rates[ind_max_in_column].FindIndex(x => x == cur_rates[ind_max_in_column].Min())
                    ));

            }
            else
            {
                int ind_max_in_row = row_of_penalty.FindIndex(x => x == row_of_penalty.Max());
                int min_element_in_column = cur_rates[0][ind_max_in_row];
                int min_ind = 0;
                for (int i = 1; i < cur_rates.Count; ++i)
                {
                    if (cur_rates[i][ind_max_in_row] < min_element_in_column) 
                    {
                        min_element_in_column = cur_rates[i][ind_max_in_row];
                        min_ind = i;
                    }
                }
                return (min_ind, ind_max_in_row) ;
            }
        }
        

        public void MethodOfVogelApproximation()
        {
            //инициализация списков
            List<List<int>> cur_rates = Init2DList(rates_),
                            cur_func = Init2DList(rates_.Count, rates_[0].Count, 0);
            List<int> cur_a = Init1DList(a_), cur_b = Init1DList(b_);
            //штрафы
            List<int> row_of_penalty = Init1DList(b_.Count,0), 
                      column_of_penalty = Init1DList(a_.Count,0);

            int max_element_to_add = GetMaxElement(cur_rates);
            while(cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                CalculatePenalties(cur_rates, ref column_of_penalty, ref row_of_penalty);
                (int, int) min_indexes = IndexesOfMinElementWithMaxPenalty(cur_rates, column_of_penalty, row_of_penalty);

                int i = min_indexes.Item1; int j = min_indexes.Item2;
                int supply = Math.Min(cur_a[i], cur_b[j]);
                //это можно на функции разбить
                if(cur_a[i]==cur_b[j]) 
                { 
                    for(int jk=0; jk< cur_rates.Count; jk++)cur_rates[i][jk] = max_element_to_add;
                    for(int ik=0; ik< cur_rates.Count; ik++)cur_rates[ik][j]=  max_element_to_add;
                }
                else if(cur_a[i] == supply)
                {
                    for (int jk = 0; jk < cur_rates.Count; jk++)cur_rates[i][jk] = max_element_to_add;
                }
                else
                {
                    for (int ik = 0; ik < cur_rates.Count; ik++) cur_rates[ik][j] = max_element_to_add;
                }
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }
            optimal_ = cur_func;
        }

        public List<List<int>> GetOptimalPlan()
        {
            return optimal_;
        }
        //этот план невырожденный
        public bool IsNonDegenerate()
        {
            return (a_.Count + b_.Count - 1
                == optimal_.SelectMany(x => x).Count(x => x!=0));
        }

        public int GetObjectiveFunction()
        {
            int objective_func = 0;
            int rows = rates_.Count;
            int columns = rates_[0].Count;
            if (type_ == ClosednessType.ShortageOfSupply) --rows;
            if (type_ == ClosednessType.SurplusOfSupply) --columns;
            for (int i = 0;i <rows;  ++i)
            {
                for (int j = 0; j < columns; ++j)
                {
                   objective_func += rates_[i][j] * optimal_[i][j];
                }
            }
            return objective_func;

        }


    }
}
