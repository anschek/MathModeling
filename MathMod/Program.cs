// See https://aka.ms/new-console-template for more information
using MathMod;



//int[] a = { 90, 400, 110 };
//int[] b = { 140, 300, 160 };
//int[,] c = { { 2, 5, 2 }, { 4, 1, 5 }, { 3, 6, 8 } };

//TransportProblem transport_problem0 = new TransportProblem(a, b, c);
//transport_problem0.MethodOfMinElement();

//PrintMatrix(transport_problem0.GetFunc());


List<int> a1 = new List<int> { 14, 14, 14, 14 };
List<int> b1 = new List<int> { 13, 5, 13, 12, 13 };
List<List<int>> c1 = new List<List<int>> { };
c1.Add(new List<int> { 16, 26, 12, 24, 3 });
c1.Add(new List<int> { 5, 2, 19, 27, 2 });
c1.Add(new List<int> { 29, 23, 25, 16, 8 });
c1.Add(new List<int> { 2, 25, 14, 15, 21 });

//TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);
//transport_problem1.MethodOfVogelApproximation();
//TransportProblem.PrintMatrix(transport_problem1.GetOptimalPlan());

//Console.WriteLine(transport_problem1.GetObjectiveFunction());
//Console.WriteLine(transport_problem1.IsNonDegenerate());
//Console.WriteLine();

////TransportProblem transportProblem = new TransportProblem(a1, b1, c1);
////TransportProblem.PrintMatrix(c1);


//List<int> a2 = new List<int> { 90, 400, 110 };
//List<int> b2 = new List<int> { 140, 300, 160 };
//List<List<int>> c2 = new List<List<int>> { };
//c2.Add(new List<int> { 2, 5, 2 });
//c2.Add(new List<int> { 4, 1, 5});
//c2.Add(new List<int> { 3, 6, 8 });

TransportProblem transport_problem2 = new TransportProblem(a1, b1, c1);
transport_problem2.DoublePreferenceMethod();
TransportProblem.PrintMatrix(transport_problem2.GetOptimalPlan());
Console.WriteLine(transport_problem2.GetObjectiveFunction());
Console.WriteLine(transport_problem2.IsNonDegenerate());

