﻿namespace MathMod
{
    internal class TransportProblem
    {
        List<int> a_;
        List<int> b_;
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
            return list.Select(x => x).Sum();
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
                type_ = ClosednessType.ShortageOfSupply;
                rates_.Add(new List<int> { });
                for (int j = 0; j < rates_[0].Count(); ++j)
                    rates_[rates_.Count - 1].Add(max_elem_to_add + 1 + j);
            }
            else //не хаватет потребителей
            {//добавляем фиктивного потребителя - столбец
                b_.Add(diff);
                type_ = ClosednessType.SurplusOfSupply;
                for (int i = 0; i < rates_.Count(); ++i)
                    rates_[i].Add(max_elem_to_add + 1 + i);
            }
        }

        public static void PrintMatrix(List<List<int>> matrix)
        {
            for (int i = 0; i < matrix.Count; ++i)
            {
                for (int j = 0; j < matrix[0].Count; ++j)
                    Console.Write(matrix[i][j] + "  ");
                Console.WriteLine();
            }
        }
        public void PrintReferencePlan()
        {
            switch (type_)
            {
                case ClosednessType.Close:
                    PrintMatrix(optimal_);
                    return;
                case ClosednessType.SurplusOfSupply:
                    for (int i = 0; i < optimal_.Count; ++i)
                    {
                        for (int j = 0; j < optimal_[0].Count - 1; ++j)
                            Console.Write(optimal_[i][j] + "  ");
                        Console.WriteLine();
                    }
                    return;
                case ClosednessType.ShortageOfSupply:
                    for (int i = 0; i < optimal_.Count - 1; ++i)
                    {
                        for (int j = 0; j < optimal_[0].Count; ++j)
                            Console.Write(optimal_[i][j] + "  ");
                        Console.WriteLine();
                    }
                    return;
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
                for (int j = 0; j < cur_rates[0].Count; ++j)
                    if (cur_rates[i][j] == min_element)
                        return (i, j);
            return (-1, -1);//в случае ошибки
        }

        static List<int> Init1DList(int size, int fill)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < size; ++i)
                list.Add(fill);
            return list;
        }

        static List<int> Init1DList(List<int> orig)
        {
            List<int> copy = new List<int>();
            for (int i = 0; i < orig.Count; ++i)
                copy.Add(orig[i]);
            return copy;
        }

        static List<List<int>> Init2DList(int size0, int size1, int fill)
        {
            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < size0; ++i)
                list.Add(Init1DList(size1, fill));
            return list;
        }

        static List<List<int>> Init2DList(List<List<int>> orig)
        {
            List<List<int>> copy = new List<List<int>>();
            for (int i = 0; i < orig.Count; ++i)
                copy.Add(Init1DList(orig[i]));
            return copy;
        }

        public void MethodOfMinElement()
        {
            //инициализация списков
            List<List<int>> cur_rates = Init2DList(rates_),
                            cur_func = Init2DList(rates_.Count, rates_[0].Count, 0);
            List<int> cur_a = Init1DList(a_), cur_b = Init1DList(b_);
            int max_element_to_add = GetMaxElement(rates_) + 1;
            //алгоритм
            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {
                (int i, int j) = GetMinIndex(cur_rates);
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }
            optimal_ = cur_func;
        }

        static void CalculatePenalties(List<List<int>> cur_rates, ref List<int> column_of_penalty, ref List<int> row_of_penalty)
        {
            int max_elem_to_add = GetMaxElement(cur_rates);
            int min = max_elem_to_add, premin = max_elem_to_add;
            //штрафы пос строкам
            for (int i = 0; i < cur_rates.Count; ++i)
            {
                for (int j = 0; j < cur_rates[0].Count; ++j)
                {
                    if (cur_rates[i][j] < min)
                    {
                        premin = min;
                        min = cur_rates[i][j];
                    }//разность должна рассчитываться между разными элементами
                    else if (cur_rates[i][j] < premin && cur_rates[i][j] != min) premin = cur_rates[i][j];
                }
                //если значения явл-ся исходными, строка уже закрыта, если есть только один минимум, он и является штрафом
                //если минимума два - штраф явл-ся их разностью
                if (min == max_elem_to_add && premin == max_elem_to_add) column_of_penalty[i] = -1;
                else if (premin == max_elem_to_add) column_of_penalty[i] = min;
                else column_of_penalty[i] = premin - min;
                min = max_elem_to_add; premin = max_elem_to_add;
            }
            //штрафы по столбцам
            for (int j = 0; j < cur_rates[0].Count; ++j)
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
        static (int, int) IndexesOfMinElementWithMaxPenalty(List<List<int>> cur_rates, List<int> column_of_penalty, List<int> row_of_penalty)
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
            List<int> row_of_penalty = Init1DList(b_.Count, 0),
                      column_of_penalty = Init1DList(a_.Count, 0);
            //найти макс элемент матрицы, которым будут заменяться ячейки, чтобы их нельзя было определить, как минимальные
            int max_element_to_add = GetMaxElement(cur_rates) + 1;
            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {//расчет штрафов, поиск индексов нужного эл-та
                CalculatePenalties(cur_rates, ref column_of_penalty, ref row_of_penalty);
                (int i, int j) = IndexesOfMinElementWithMaxPenalty(cur_rates, column_of_penalty, row_of_penalty);
                int supply = Math.Min(cur_a[i], cur_b[j]);
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
            int i = 0, j = 0;
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
            //выбираем минимумы строк
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
            }//выбираем минимумы столбцов
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
            }//добавляем словари для сортировки по элементам
            Dictionary<(int, int), int> preferred_plus_plus = new Dictionary<(int, int), int>();
            Dictionary<(int, int), int> preferred_plus = new Dictionary<(int, int), int>();
            for (int i = 0; i < preferred.Count; ++i)
            {
                for (int j = 0; j < preferred[0].Count; ++j)
                {
                    if (preferred[i][j] == 2) preferred_plus_plus[(i, j)] = cur_rates[i][j];
                    else if (preferred[i][j] == 1) preferred_plus_plus[(i, j)] = cur_rates[i][j];
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
            //найти макс элемент матрицы, которым будут заменяться ячейки, чтобы их нельзя было определить, как минимальные
            int max_element_to_add = GetMaxElement(cur_rates);
            (Dictionary<(int, int), int> plus_plus, Dictionary<(int, int), int> plus) = FindPreferredElements(cur_rates);
            foreach (((int i, int j), _) in plus_plus)
            {//проходимся по ++
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = max_element_to_add + 1;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }

            foreach (((int i, int j), _) in plus)
            {//проходимся по +
                int supply = Math.Min(cur_a[i], cur_b[j]);
                cur_rates[i][j] = max_element_to_add;
                cur_func[i][j] = supply;
                cur_a[i] -= supply;
                cur_b[j] -= supply;
            }


            while (cur_a.Sum() > 0 && cur_b.Sum() > 0)
            {//из остальных эл-тов выбираем минимальные
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
                == optimal_.SelectMany(x => x).Count(x => x != 0));
        }        
        public bool IsNonDegenerate(List<List<int>> reference_plan)
        {
            return (a_.Count + b_.Count - 1
                == reference_plan.SelectMany(x => x).Count(x => x != 0));
        }
        //получить целевую функцию
        public int GetObjectiveFunction()
        {
            int objective_func = 0;
            int rows = rates_.Count;
            int columns = rates_[0].Count;
            if (type_ == ClosednessType.ShortageOfSupply) --rows;
            if (type_ == ClosednessType.SurplusOfSupply) --columns;
            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < columns; ++j)
                    objective_func += rates_[i][j] * optimal_[i][j];
            return objective_func;
        }

//------ОЦЕНКА ОПТИМАЛЬНОСТИ---------------------------------------------------------------------------------------------------------------------------------------

        (List<int>, List<int>) PotentialMethod(List<List<int>> reference_plan)
        {
            List<int> potential_column = Init1DList(a_.Count, 0);
            List<int> potential_row = Init1DList(b_.Count, 0);
            int filled_count = 1;//т.к. изначально первый потенциал столбца равен нулю
            int step_in_degenerate = 1;
            //проход по первой строке
            for (int j = 0; j < reference_plan[0].Count; ++j)
                if (reference_plan[0][j] != 0)
                {
                    potential_row[j] = rates_[0][j];//-0
                    ++filled_count;
                }
            int non_degenerate_plan= potential_column.Count + potential_row.Count;//изначально кол-во потенциалов для невырожденной задачи
            int degenerate_plan = 0;
            int potential_count_without_nuls = non_degenerate_plan;
            if(!IsNonDegenerate(reference_plan)){//из нач. кол-ва вычитается: ожидаемое m+n-1 - реальное, умноженное на 2 потенциала
                potential_count_without_nuls -= (a_.Count+b_.Count-1-reference_plan.SelectMany(x => x).Count(x => x != 0))*2;
                degenerate_plan = potential_count_without_nuls;
            }
            //хз есть ли подтвержденная форма, я эмпирически так посчитала..

            while (filled_count < potential_count_without_nuls)
            {//когда все потенциалы будут заполнены, цикл завершится
                for (int i = 1; i < reference_plan.Count; ++i)//проходимся по занятым ячейкам
                {//если заполнили ячейку, вощвращаемся к началу
                    bool reload_iteration = false;
                    for (int j = 0; j < reference_plan[0].Count; ++j)
                        if (reference_plan[i][j] != 0)
                        {
                            if (potential_column[i] != 0 && potential_row[j] != 0) continue;//потенциалы уже рассчитаны
                            else if (potential_column[i] != 0 || potential_row[j] != 0)
                            {
                                if (potential_column[i] != 0) potential_row[j] = rates_[i][j] - potential_column[i];
                                if (potential_row[j] != 0) potential_column[i] = rates_[i][j] - potential_row[j];
                                ++filled_count;
                                reload_iteration = true;
                                break;
                            }//else - два потенциала пустые - ничего не делаем, идем дальше
                        }
                    if (reload_iteration) break;
                }
                
                //если задача вырождена, а мы заполнили все потенциалы, что могли
                if (!IsNonDegenerate(reference_plan) && filled_count == degenerate_plan && (degenerate_plan != non_degenerate_plan))
                {
                    (int min_i, int min_j) = GetMinElementInFreeCell(reference_plan, step_in_degenerate)[step_in_degenerate - 1];
                    if (potential_column[min_i] == 0 && potential_row[min_j] == 0)
                    {
                        --filled_count;//для повтора итерации
                        ++step_in_degenerate;//нужна другая пустая ячейка
                    }
                    else
                    {
                        if (potential_column[min_i] != 0)
                            potential_row[min_j] = rates_[min_i][min_j] - potential_column[min_i];
                        if (potential_row[min_j] != 0)
                            potential_column[min_i] = rates_[min_i][min_j] - potential_row[min_j];
                        Console.WriteLine($"Введен 0 в ячейку[{min_i}][{min_j}] с минимальным тарифом {rates_[min_i][min_j]}");
                        ++filled_count;//потенциал заполнен
                        potential_count_without_nuls+=2;//если план вырожденный, а мы заполнили ячейку, то в нем стало на возможную заполненную ячейку больше
                    }
                }
            }
            return (potential_column, potential_row);
        }
        //минимальный тариф у пустой ячейки
        List<(int, int)> GetMinElementInFreeCell(List<List<int>> reference_plan, int step)
        {
            int min_el = GetMaxElement(rates_);
            List<(int, int)> min_elems = new List<(int, int)>(step);
            min_elems.Add((0, 0));
            for (int i = 0; i < reference_plan.Count; ++i)
                for (int j = 0; j < reference_plan.Count; ++j)
                    if (min_el > rates_[i][j] && reference_plan[i][j] == 0)
                    {
                        min_el = rates_[i][j];
                        min_elems.Insert(0, (i, j));
                    }
            return min_elems;
        }


        (bool, List<(int, int)>) FreeCellEstimation((List<int>, List<int>) potentials, List<List<int>> reference_plan)
        {
            Console.WriteLine("Оценка свободных ячеек:");
            (List<int> potential_column, List<int> potential_row) = potentials;
            List<(int, int)> positive_delta = new List<(int, int)>();
            for (int i = 0; i < reference_plan.Count; ++i)
            {
                for (int j = 0; j < reference_plan[0].Count; ++j)
                {
                    if (reference_plan[i][j] == 0)
                    {
                        int cur_delta = potential_column[i] + potential_row[j] - rates_[i][j];
                        Console.WriteLine("d[" + i + "][" + j + "] = " + cur_delta);
                        if (cur_delta > 0) positive_delta.Add((i, j));
                    }
                }
            }
            return (positive_delta.Count == 0, positive_delta);
        }

        //проверка на оптимальность(вывод)
        public bool PrintPotentialsInfo(List<List<int>> reference_plan)
        {
            (List<int> potential_column, List<int> potential_row) = PotentialMethod(reference_plan);
            Console.WriteLine("Столбец потенциалов:");
            for (int i = 0; i < potential_column.Count; ++i)
                Console.WriteLine("u[" + i + "] = " + potential_column[i]);
            Console.WriteLine("Строка потенциалов:");
            for (int j = 0; j < potential_row.Count; ++j)
                Console.WriteLine("v[" + j + "] = " + potential_row[j]);

            (bool is_optimal, List<(int, int)> positive_delta) = FreeCellEstimation((potential_column, potential_row), reference_plan);
            if (is_optimal) Console.WriteLine("Опорный план оптимален");
            else
            {
                Console.WriteLine("Опорный план не оптимален. Дельта со следующими индексами положительна:");
                foreach ((int i, int j) in positive_delta)
                    Console.WriteLine($"d[{i}][{j}]>0");
            }
            return is_optimal;
        }

        List<List<int>> GetNewReferencePlan()
        {
            List<List<int>> new_reference_plan = new List<List<int>>();
            int rows_num = optimal_.Count, clmns_num = optimal_[0].Count;
            Console.WriteLine($"Введите новую матрицу размерностью {rows_num}x{clmns_num}");
            for (int i = 0; i < rows_num; ++i)
            {
                List<int> new_row = Console.ReadLine().Split().Select(int.Parse).ToList();
                new_reference_plan.Add(Init1DList(new_row));
            }
            return new_reference_plan;
        }

        public bool AssessOptimality()
        {
            bool continue_ = true;
            bool is_first_itaration = true;
            bool is_optimal = false;
            List<List<int>> cur_reference_plan = optimal_;
            string answer;
            do
            {
                if (!is_first_itaration)
                {
                    Console.WriteLine("Хотите ввести новый опорный план для проверки на оптимальность?(Да/Нет)");
                    answer = Console.ReadLine();
                }
                else answer = "да";

                switch (answer.ToUpper())
                {
                    case "ДА":
                        if (!is_first_itaration) cur_reference_plan = GetNewReferencePlan();
                        is_optimal = PrintPotentialsInfo(cur_reference_plan);
                        if (is_optimal) continue_ = false;
                        break;
                    case "НЕТ":
                        continue_ = false;
                        break;
                    default:
                        Console.WriteLine("Некорректный ввод");
                        break;
                }
                if (is_first_itaration) is_first_itaration = false;
            } while (continue_);
            optimal_ = cur_reference_plan;
            return is_optimal;
        }
    }
}