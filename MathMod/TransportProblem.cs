using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Runtime;
using System.Reflection.Metadata;
using System.Xml;

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
            Close,                  //закрытая
            ShortageOfSupply,      //нехваток поставок 
            SurplusOfSupply       //избыток поставок 
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
        //свести задачу к закрытой
        void ReduceProblemToClosed()
        {
            int SumA = ListSum(a_);
            int SumB = ListSum(b_);
            int diff = Math.Abs(SumA - SumB);
            int max_elem_to_add = GetMaxElement(rates_);
            //не хватает груза
            if (SumA < SumB)
            {//добавляем фиктивного поставщика - строку
                a_.Add(diff);
                rates_.Add(new List<int> { });
                for (int j = 0; j < rates_[0].Count(); ++j)
                    rates_[rates_.Count - 1].Add(max_elem_to_add);
            }
            else //не хаватет потребителей
            {//добавляем фиктивного потребителя - столбец
                b_.Add(diff);
                type_ = ClosednessType.SurplusOfSupply;
                for (int i = 0; i < rates_.Count(); ++i)
                    rates_[i].Add(max_elem_to_add);
            }
        }

        public static void PrintMatrix(List<List<int>> matrix)
        {
            for (int i = 0; i < matrix.Count; ++i)
            {
                for (int j = 0; j < matrix[0].Count; ++j)
                {
                    Console.Write(matrix[i][j] + "  ");
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
                (int i, int j) = GetMinIndex(cur_rates);
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
            //штрафы пос строкам
            for(int i = 0;i < cur_rates.Count; ++i)
            {
                for(int j = 0;j < cur_rates[0].Count; ++j)
                {
                    if (cur_rates[i][j] < min)
                    {
                        premin = min;
                        min = cur_rates[i][j];
                    }//разность должна рассчитываться между разными элементами
                    else if (cur_rates[i][j] < premin && cur_rates[i][j] !=min) premin = cur_rates[i][j];

                }
                //если значения явл-ся исходными, строка уже закрыта, если есть только один минимум, он и является штрафом
                //если минимума два - штраф явл-ся их разностью
                if (min == max_elem_to_add && premin == max_elem_to_add) column_of_penalty[i] = -1;
                else if (premin == max_elem_to_add) column_of_penalty[i] = min;
                else column_of_penalty[i] = premin - min;
                min = max_elem_to_add; premin = max_elem_to_add;
            }
            //штрафы по столбцам
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
        //Индексы минимального элемента с максимальным штрафом
        static (int,int) IndexesOfMinElementWithMaxPenalty(List<List<int>> cur_rates, List<int> column_of_penalty, List<int> row_of_penalty)
        {
            //если макс элемент находится в столбце штрафов
            if (column_of_penalty.Max() > row_of_penalty.Max())
            {//возвращаем i-такой индекс столбца штрафов (строки матрицы), что штраф максимален
             //j-такой индекс этой строки матрицы, что элемент на строке минимален
                int ind_max_in_column = column_of_penalty.FindIndex(x => x == column_of_penalty.Max());
                return (ind_max_in_column, (
                    cur_rates[ind_max_in_column].FindIndex(x => x == cur_rates[ind_max_in_column].Min())));
            }
            else
            {//находим индекс максимального штрафа в строке, это итоговый j
             //i находим перебором по столбцу с индексом j
                int ind_max_in_row = row_of_penalty.FindIndex(x => x == row_of_penalty.Max());
                int min_element_in_column = cur_rates[0][ind_max_in_row];
                int min_ind = 0;
                for (int i = 1; i < cur_rates.Count; ++i)
                    if (cur_rates[i][ind_max_in_row] < min_element_in_column) 
                    {
                        min_element_in_column = cur_rates[i][ind_max_in_row];
                        min_ind = i;
                    }
                return (min_ind, ind_max_in_row);
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
            //найти макс элемент матрицы, которым будут заменяться ячейки, чтобы их нельзя было определить, как минимальные
            int max_element_to_add = GetMaxElement(cur_rates);
            while(cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {//расчет штрафов, поиск индексов нужного эл-та
                CalculatePenalties(cur_rates, ref column_of_penalty, ref row_of_penalty);
                (int i, int j)= IndexesOfMinElementWithMaxPenalty(cur_rates, column_of_penalty, row_of_penalty);
                int supply = Math.Min(cur_a[i], cur_b[j]);
                //если в ряде минимум, в текущих тарифах заменяем элемент, чтоб к нему не возвращаться
                if(cur_a[i] == supply)for (int jk = 0; jk < cur_rates.Count; jk++)cur_rates[i][jk] = max_element_to_add;
                if(cur_b[j] == supply)for (int ik = 0; ik < cur_rates.Count; ik++) cur_rates[ik][j] = max_element_to_add;
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }
            optimal_ = cur_func;
        }        
        public void NorthWestAngleMethod()
        {
            //инициализация списков
            List<List<int>> cur_func = Init2DList(rates_.Count, rates_[0].Count, 0);
            List<int> cur_a = Init1DList(a_), cur_b = Init1DList(b_);
            //проходимя по ячейкам, сдивагая их по степени того, как заканчиваются грузы/потребности
            int i=0, j=0;
            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                int cur_i = i, cur_j = j;
                int supply = Math.Min(cur_a[cur_i], cur_b[cur_j]);
                cur_func[cur_i][cur_j] = supply;
                if (cur_a[cur_i] == supply) ++i;
                if (cur_b[cur_j] == supply) ++j;
                cur_a[cur_i] -= supply;
                cur_b[cur_j] -= supply;
            }
            optimal_ = cur_func;
        }
        //Поиск словарей (индекс строки, индекс столбца) элемент для ячеек с ++ и +
        (Dictionary<(int, int), int>, Dictionary<(int, int), int>) FindPreferredElements(List<List<int>> cur_rates)
        {
            List<List<int>> preferred = Init2DList(rates_.Count, rates_[0].Count, 0);
            for (int i = 0; i < cur_rates.Count; ++i)
            {
                int cur_min = cur_rates[i][0];
                int min_ind = 0;
                for (int j = 1; j < cur_rates[0].Count; ++j)
                {
                    if (cur_rates[i][j] < cur_min) 
                    { 
                        cur_min = cur_rates[i][j];
                        min_ind = j;
                    }
                }
                ++preferred[i][min_ind];
            }
            for (int j = 0; j < cur_rates[0].Count; ++j)
            {
                int cur_min = cur_rates[0][j];
                int min_ind = 0;
                for (int i = 1; i < cur_rates.Count; ++i)
                {
                    if (cur_rates[i][j] < cur_min)
                    {
                        cur_min = cur_rates[i][j];
                        min_ind = i;
                    }
                }
                ++preferred[min_ind][j];
            }
            Dictionary<(int, int), int> preferred_plus_plus= new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> preferred_plus = new Dictionary<(int, int), int>();
            for(int i = 0; i < preferred.Count; ++i)
            {
                for(int j=0; j < preferred[0].Count; ++j)
                {
                    if (preferred[i][j] == 2) preferred_plus_plus[(i,j)] = cur_rates[i][j]; 
                    else if (preferred[i][j] ==1)preferred_plus_plus[(i, j)] = cur_rates[i][j]; 
                }
            }
            return (preferred_plus_plus.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value), 
                preferred_plus.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value));
        }
        
        public void DoublePreferenceMethod()
        {
            //инициализация списков
            List<List<int>> cur_rates = Init2DList(rates_),
                            cur_func = Init2DList(rates_.Count, rates_[0].Count, 0);
            List<int> cur_a = Init1DList(a_), cur_b = Init1DList(b_);
            //метод
            int max_element_to_add = GetMaxElement(cur_rates);
            (Dictionary<(int, int), int> plus_plus, Dictionary<(int, int), int> plus) = FindPreferredElements(cur_rates);
            foreach (((int i, int j), _) in plus_plus)
            {
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }            
            foreach (((int i, int j), _) in plus)
            {
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }
            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                (int i, int j) = GetMinIndex(cur_rates);
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = GetMaxElement(cur_rates);
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
        //получить целевую функцию
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
