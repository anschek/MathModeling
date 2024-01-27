// See https://aka.ms/new-console-template for more information
using MathMod;

//Console.WriteLine("Hello, World!");

static void PrintMatrix(int[,] matrix)
{
    for (int i = 0; i < matrix.GetLength(0); ++i)
    {
        for (int j = 0; j < matrix.GetLength(1); ++j)
        {
            Console.Write(matrix[i, j]);
            Console.Write("  ");
        }
        Console.WriteLine();
    }
}


int[] a = { 90, 400, 110 };
int[] b = { 140, 300, 160 };
int[,] c = { {2,5,2 }, {4,1,5 }, {3,6,8} };

TransportProblem transport_problem = new TransportProblem(a,b,c);
transport_problem.MethodOfMinElement();

//PrintMatrix(transport_problem.GetFunc());
