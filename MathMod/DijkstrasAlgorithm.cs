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

        public Dictionary<int, double> Solve(int from)
        {
            List<List<double>> mainMatrix = Program.Init2DList(mainMatrix_);
            //List<double> results = Enumerable.Repeat(Program.inf, mainMatrix.Count).ToList();
            //List<bool> notVisited = Enumerable.Repeat(true, mainMatrix.Count).ToList();

            Dictionary<int, double> results = Enumerable.Range(0, mainMatrix_.Count).Select(v => new { v, Program.inf }).ToDictionary(v => v.v, v => v.inf);
            Dictionary<int, List<int>> paths = new Dictionary<int, List<int>> ();
            Dictionary<int, bool> notVisited = Enumerable.Range(0, mainMatrix_.Count).Select(v => (v, true)).ToDictionary(v => v.v, v => v.Item2);

            results[from] = 0;
            paths[from] = new List<int>();
            while (notVisited.Any(x=> x.Value==true))
            {
                int nearestVertex = notVisited.Where(v => v.Value == true).OrderBy(v => results[v.Key]).FirstOrDefault().Key;
                notVisited[nearestVertex] = false;
                
                for(int j=0; j < mainMatrix_[0].Count; ++j) 
                {//если ребро существует и сосед не посещен
                    if (mainMatrix[nearestVertex][j] < Program.inf && notVisited[j])
                    {//если вершину надо продлить, продливаем, добавляем путь
                        if (results[j] > results[nearestVertex] + mainMatrix[nearestVertex][j])
                        {
                            results[j] = results[nearestVertex] + mainMatrix[nearestVertex][j];
                            paths[j] = Program.Init1DList(paths[nearestVertex]);
                            paths[j].Add(j);
                        }
                       
                    }
                    //смотрим на соседа
                    //если сосед не посещен
                    //+ создать массив хранящий ребра

                }

            }

            return results;          
        }
    }
}
