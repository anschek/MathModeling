using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MathMod
{
    //универсальные методы решения задач линейной математики
    internal class LinearProblem
    {
        public enum problemType { min, max }

        List<List<double>> restrictions_;
        List<double> free_variables_;
        List<double> objective_fun_;
        readonly List<double> values_in_result_;
        double objective_fun_value;
        problemType type_;
        List<int> basis_indexes_;
        List<string> comparison_signs_;

        void ReduceToCanonicalForm()
        {
            int balance_variable_count = restrictions_.Count;

            for (int i = 0; i < balance_variable_count; i++)
            {
                if (comparison_signs_[i] == ">=" || comparison_signs_[i] == "<=")
                {
                    for (int j = 0; j < balance_variable_count; j++)
                        restrictions_[i].Add(0);
                    if (comparison_signs_[i] == "<=" || comparison_signs_[i] == ">=")
                    {
                        int basis_ind = objective_fun_.Count + i;
                        restrictions_[i][basis_ind] = 1;
                        //Console.WriteLine($"basis one: [{i}][{basis_ind}]");
                        if (comparison_signs_[i] == ">=")
                        {
                            restrictions_[i][basis_ind] = -1;
                            restrictions_[i] = restrictions_[i].Select(x => -x).ToList();
                            //foreach (int el in restrictions_[i])
                            //    Console.WriteLine($"{el} ");
                            free_variables_[i] *= -1;
                        }
                    }

                }
                else if (comparison_signs_[i] == "=") continue;
                else throw new FormatException($"Ожидаются символы: >=, <=, =. Фактический: {comparison_signs_[i]}");
            }
            for (int j = 0; j < balance_variable_count; j++)
                objective_fun_.Add(0);
        }

        List<int> PickOutBasis()
        {
            List<int> basis = new List<int>();
            for (int j = 0; j < restrictions_[0].Count; ++j)
            {
                bool onlyCanonicalSigns = true;
                int ones = 0;
                for (int i = 0; i < restrictions_.Count; ++i)
                {
                    if (restrictions_[i][j] != 0 && restrictions_[i][j] != 1)
                    {
                        onlyCanonicalSigns = false;
                        break;
                    }
                    if (restrictions_[i][j] == 1) ++ones;
                }
                if (onlyCanonicalSigns && ones == 1) basis.Add(j);
            }
            if (basis.Count != restrictions_.Count) throw new ArgumentException($"Ожидаемая размерность базиса {restrictions_.Count}. Фактическая: {basis.Count}");
            return basis;
        }
        public LinearProblem(List<List<double>> a, List<string> comparison_signs, List<double> b, List<double> c, problemType type)
        {
            restrictions_ = a;
            comparison_signs_ = comparison_signs;
            free_variables_ = b;
            objective_fun_ = c;
            values_in_result_ = new List<double> { };
            for (int i = 0; i < objective_fun_.Count; ++i) if (objective_fun_[i] != 0) values_in_result_.Add(i);
            type_ = type;
            ReduceToCanonicalForm();
            basis_indexes_ = PickOutBasis();
            objective_fun_ = objective_fun_.Select(x => -x).ToList();
        }
        //возвращает -1, если в базис вводить нечего, иначе индекс вводимой переменной
        int EnteringIntoBasis()
        {
            if (type_ == problemType.min)
                return objective_fun_.FindIndex(x => x == objective_fun_.Max() && x > 0);
            else//type_ == problemType.max
                return objective_fun_.FindIndex(x => x == objective_fun_.Min() && x < 0);
        }

        int DerivationFromBasis(int leading_col)
        {
            List<double> divided_right_side = free_variables_.Select(x => x).ToList();
            for (int i = 0; i < divided_right_side.Count; ++i)
            {
                if (restrictions_[i][leading_col] <= 0) divided_right_side[i] = 0;
                else divided_right_side[i] /= restrictions_[i][leading_col];
            }
            return divided_right_side.FindIndex(x => x == divided_right_side.FindAll(x => x > 0).Min());
        }

        public void SimplexMethod()
        {
            bool is_optimal = false;
            while (!is_optimal)
            {
                int leading_col = EnteringIntoBasis();
                if (leading_col == -1)
                {
                    is_optimal = true;
                    break;
                }
                int leading_row = DerivationFromBasis(leading_col);
                double leading_elem = restrictions_[leading_row][leading_col];
                basis_indexes_[leading_row] = leading_col;
                for (int j = 0; j < restrictions_[0].Count; ++j)
                    restrictions_[leading_row][j] /= leading_elem;

                free_variables_[leading_row] /= leading_elem;

                double divider;
                for (int i = 0; i < restrictions_.Count; ++i)
                {
                    if (i == leading_row) continue;
                    divider = -restrictions_[i][leading_col];
                    for (int j = 0; j < restrictions_[0].Count; ++j)
                        restrictions_[i][j] += divider * restrictions_[leading_row][j];
                    free_variables_[i] += divider * free_variables_[leading_row];
                }
                divider = -objective_fun_[leading_col];
                for (int j = 0; j < restrictions_[0].Count; ++j)
                    objective_fun_[j] += divider * restrictions_[leading_row][j];

                objective_fun_value += divider * free_variables_[leading_row];
            }
        }
        public string GetObjectiveFun()
        {
            string text_func = "";
            for (int i = 0; i < basis_indexes_.Count; ++i)
                if (-1 != values_in_result_.FindIndex(x => x == basis_indexes_[i]))
                    text_func += $"x{basis_indexes_[i] + 1}={Math.Round(free_variables_[i], 3)} ";
            text_func += $"\nL(x)={Math.Round(objective_fun_value, 3)}";
            return text_func;
        }
    }
}
