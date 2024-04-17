using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathMod
{
    internal class DijkstrasAlgorithm
    {
        List<List<double>> mainMatrix_;
        public DijkstrasAlgorithm(List<List<double>> adjacencyMatrix)
        {
            mainMatrix_ = adjacencyMatrix;
        }

        public List<double> Solve(int from)
        {
            List<List<double>> mainMatrix = Program.Init2DList(mainMatrix_);
            List<double> results = Enumerable.Repeat(Program.inf, mainMatrix.Count).ToList();
            results[0] = 0;
            

            for (int i=0; i<mainMatrix.Count; i++)
            {
                List<bool> visited = Enumerable.Repeat(false, mainMatrix.Count).ToList();
                for (int j=0; j < mainMatrix[i].Count; j++)
                {
                    if (!visited[j] && mainMatrix[i][j]< Program.inf)
                    {
                        results[j] =  results[j] < results[i]+mainMatrix[i][j]?results[j]:results[i] + mainMatrix[i][j];
                    }
                }
                visited[i] = true;
            }
            return results;          
        }
    }
}
