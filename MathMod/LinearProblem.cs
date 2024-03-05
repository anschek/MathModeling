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
        public enum problemType
        {
            min, max
        }

        List<List<double>> restrictions_;
        List<double> free_variables_;
        List<double> objective_fun_;
        double objective_fun_value;
        problemType type_;
        List<int> basis_indexes_;

        void PickOutBasis()
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
            basis_indexes_ = basis;
        }

        //public LinearProblem(List<List<double>> a, List<double> b, List<double> c, problemType type, List<int> basis)
        //{
        //    restrictions_ = a;
        //    free_variables_ = b;
        //    objective_fun_ = c;
        //    type_ = type;
        //    PickOutBasis();
        //}        
        public LinearProblem(List<List<double>> a, List<double> b, List<double> c, problemType type)
        {
            restrictions_ = a;
            free_variables_ = b;
            objective_fun_ = c;
            type_ = type;
            PickOutBasis();
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
            for (int i = 0; i < free_variables_.Count; ++i)
                text_func += $"x{basis_indexes_[i] + 1}={free_variables_[i]} ";
            text_func += $"\nL(x)={objective_fun_value}";
            return text_func;
        }

        /*
        TODO:
        
        дописать поиск базиса
        возможно сделать не канон (из файла)
        */
    }
}
