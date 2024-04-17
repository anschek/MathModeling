namespace MathMod
{
    internal class TravelingSalesmanProblem
    {
        List<List<double>> mainMatrix_;
        List<PathInfo> graphOfSolutions_;
        class PathInfo
        {
            public List<(int, int)> edges;
            public double lowerLevel;
            public List<List<double>> matrix;
            public PathInfo()
            {
                edges = new List<(int, int)>();
                lowerLevel = 0;
                matrix = new List<List<double>>();
                rowIndexes = new List<int>();
                colIndexes = new List<int>();
            }
            public PathInfo(List<(int, int)> edges, double lowerLevel, List<List<double>> matrix, List<int> rowBase, List<int> colBase)
            {
                this.edges = edges;
                this.lowerLevel = lowerLevel;
                this.matrix = matrix;
                rowIndexes = rowBase;
                colIndexes = colBase;
            }
            public PathInfo(double lowerLevel, List<List<double>> mainMatrix)
            {
                this.edges = new List<(int, int)>();
                this.lowerLevel = lowerLevel;
                this.matrix = new List<List<double>>();
                rowIndexes = Enumerable.Range(0, mainMatrix.Count).ToList();
                colIndexes = Enumerable.Range(0, mainMatrix[0].Count).ToList();
            }
            public List<int> rowIndexes;
            public List<int> colIndexes;
        }
        int curLeadingBranch_;
        double result_;

        public TravelingSalesmanProblem(List<List<double>> adjacency_matrix)
        {
            mainMatrix_ = adjacency_matrix;
            graphOfSolutions_ = new List<PathInfo> ();
        }

        public void Solve()
        {
            List<List<double>> curMatrix = Program.Init2DList(mainMatrix_);
            curLeadingBranch_ = 0;
            double curLowerLimit = 0;
            bool isFirstIteration = true;
            while(curMatrix.Count > 1 )
            {
                if (isFirstIteration)
                {//H0
                    double lowerLimit = RowAndColumnsReduction(curMatrix);
                    curLowerLimit = lowerLimit;
                    graphOfSolutions_.Add(new PathInfo(lowerLimit, mainMatrix_));
                    isFirstIteration = false;
                }
                else curLowerLimit += RowAndColumnsReduction(curMatrix);

                CreateNewBranchOnBase(curLeadingBranch_);
                ((int from, int to), double maxValue) = ZeroCellsAssessment(curMatrix, graphOfSolutions_.Count-1);
                List<List<double>> reducedMatrix = MatrixReduction(curMatrix, from, to, graphOfSolutions_.Count - 1);
                //H
                graphOfSolutions_.Last().edges.Add((from,to));
                graphOfSolutions_.Last().lowerLevel += RowAndColumnsReduction(reducedMatrix);
                graphOfSolutions_.Last().matrix = reducedMatrix;
                //H*
                graphOfSolutions_[curLeadingBranch_].matrix = Program.Init2DList(curMatrix);
                graphOfSolutions_[curLeadingBranch_].lowerLevel += maxValue;
                graphOfSolutions_[curLeadingBranch_].matrix[graphOfSolutions_[curLeadingBranch_].rowIndexes.FindIndex(x => x == from)]
                    [graphOfSolutions_[curLeadingBranch_].colIndexes.FindIndex(x => x == to)] = Program.inf;//a[from][to]=m
                Console.WriteLine($"\n{from} -> {to}, с ребром: {graphOfSolutions_.Last().lowerLevel}, без ребра: {graphOfSolutions_[curLeadingBranch_].lowerLevel} ");
                //for next iteration
                curLowerLimit = graphOfSolutions_.Where(x => !double.IsNaN(x.lowerLevel)).Min(x => x.lowerLevel);
                curLeadingBranch_ = graphOfSolutions_.FindIndex(x => x.lowerLevel == curLowerLimit);
                curMatrix = Program.Init2DList(graphOfSolutions_[curLeadingBranch_].matrix);
                Console.WriteLine("Матрица, выбранная для следующего шага: ");
                Program.PrintMatrix(curMatrix);
            }
            (int lastFrom, int LastTo) = (graphOfSolutions_[curLeadingBranch_].rowIndexes.Last(), graphOfSolutions_[curLeadingBranch_].colIndexes.Last());
            graphOfSolutions_[curLeadingBranch_].edges.Add((lastFrom,LastTo));
            result_ = graphOfSolutions_[curLeadingBranch_].lowerLevel;
        }

        double RowAndColumnsReduction(List<List<double>> curMatrix)
        {
            double sumOfMinimums = 0;
            double curMin;
            for(int i = 0; i < curMatrix.Count; ++i)//rows reduction
            {
                curMin = curMatrix[i].Min();
                for (int j = 0; j < curMatrix[i].Count; ++j) curMatrix[i][j] -= curMin;
                sumOfMinimums += curMin;
            }
            for (int j = 0; j < curMatrix[0].Count; ++j)//columns reduction
            {
                curMin = Enumerable.Range(0,curMatrix.Count)
            .Select(rowInd => curMatrix[rowInd][j]).Min();
                for (int i = 0; i < curMatrix.Count; ++i) curMatrix[i][j] -= curMin;
                sumOfMinimums += curMin;
            }
            return sumOfMinimums;
        }

        ((int,int),double) ZeroCellsAssessment(List<List<double>> curMatrix, int pathInd)
        {
            Dictionary<(int,int),double> zeroCellsAssessment = new Dictionary<(int,int),double>();
            for (int i = 0;i < curMatrix.Count; ++i)
                for(int j =0; j < curMatrix[i].Count; ++j)
                {
                    if (curMatrix[i][j] == 0)
                    {
                        curMatrix[i][j] = Program.inf;
                        zeroCellsAssessment.Add((i,j), curMatrix[i].Min() + Enumerable.Range(0, curMatrix.Count).Select(rowInd => curMatrix[rowInd][j]).Min());
                        curMatrix[i][j] = 0;
                    }
                }
            KeyValuePair<(int, int), double> maxValue = zeroCellsAssessment.First(x => x.Value == zeroCellsAssessment.Max(x => x.Value));
            return ((graphOfSolutions_[pathInd].rowIndexes[maxValue.Key.Item1], graphOfSolutions_[pathInd].colIndexes[maxValue.Key.Item2]), maxValue.Value);
        }

        List<List<double>> MatrixReduction(List<List<double>> originalMatrix, int from, int to, int pathInd) 
        {
            List<List<double>> reducedMatrix = Program.Init2DList(originalMatrix);
            PathInfo curPath = graphOfSolutions_[pathInd];
            (int deletedRow, int deletedCol) = (curPath.rowIndexes.FindIndex(rowInd => rowInd == from),
               curPath.colIndexes.FindIndex(colInd => colInd == to));
            int findRowTo = curPath.rowIndexes.FindIndex(rowInd => rowInd == to);
            int findColFrom = curPath.colIndexes.FindIndex(colInd => colInd == from);
            if(findRowTo >=0 && findColFrom >=0)  reducedMatrix[findRowTo][findColFrom] = Program.inf;

            reducedMatrix.RemoveAt(curPath.rowIndexes.FindIndex(x => x==from));
            foreach (List<double> row in reducedMatrix) row.RemoveAt(curPath.colIndexes.FindIndex(x => x == to));
            graphOfSolutions_[pathInd].rowIndexes.Remove(from);
            graphOfSolutions_[pathInd].colIndexes.Remove(to);
            return reducedMatrix;
        }

        void CreateNewBranchOnBase(int baseIndex)
        {
            graphOfSolutions_.Add(new PathInfo(
                Program.Init1DList(graphOfSolutions_[baseIndex].edges),
                graphOfSolutions_[baseIndex].lowerLevel, 
                Program.Init2DList(graphOfSolutions_[baseIndex].matrix),
                Program.Init1DList(graphOfSolutions_[baseIndex].rowIndexes), 
                Program.Init1DList(graphOfSolutions_[baseIndex].colIndexes)));
        }

        public double GetResult() => result_;

        public List<(int,int)> GetPath() => graphOfSolutions_[curLeadingBranch_].edges;
    }
}
