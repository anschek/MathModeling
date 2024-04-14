using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMod
{
    internal class TravelingSalesmanProblem
    {
        List<List<double>> mainMatrix_;
        List<List<(int, int)>> graphOfSolutions_;
        int curLeadingBranch_;
        double result_;

        public TravelingSalesmanProblem(List<List<double>> adjacency_matrix)
        {
            mainMatrix_ = adjacency_matrix;
            Solve();
        }

        void Solve()
        {
            double sumOfMinimums;
            RowAndColumnsReduction(out sumOfMinimums);

        }

        void RowAndColumnsReduction(out double sumOfMinimums)
        {
            sumOfMinimums = 0;
            double curMin;
            for(int i = 0; i < mainMatrix_.Count; ++i)
            {
                curMin = mainMatrix_[i].Min();
                mainMatrix_[i].ForEach(x => {x -= curMin;});
                sumOfMinimums += curMin;
            }

            for (int j = 0; j < mainMatrix_[0].Count; ++j)
            {
                curMin = Enumerable.Range(0, mainMatrix_.Count)
            .Select(rowInd => mainMatrix_[rowInd][j]).Min();
                mainMatrix_.ForEach(x => { x[j] -= curMin; });
                sumOfMinimums += curMin;
            }
        }

        public double GetResult() => result_;
    }
}
