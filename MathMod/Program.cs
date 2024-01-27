// See https://aka.ms/new-console-template for more information
using MathMod;

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
int[,] c = { { 2, 5, 2 }, { 4, 1, 5 }, { 3, 6, 8 } };

TransportProblem transport_problem0 = new TransportProblem(a, b, c);
transport_problem0.MethodOfMinElement();

PrintMatrix(transport_problem0.GetFunc());


int[] a1 = { 14,14,14,14 };
int[] b1 = { 13,5,13,12,13 };
int[,] c1 = { { 16,26,12,24,3 }, {5,2,19,27,2 }, { 29,23,25,16,8 }, {2,25,14,15,21 } };

TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);
transport_problem1.MethodOfMinElement();

Console.WriteLine();
PrintMatrix(transport_problem1.GetFunc());
Console.WriteLine(transport_problem1.CalculateObjectiveFunction());