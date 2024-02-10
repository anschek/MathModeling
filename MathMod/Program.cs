using MathMod;


//Console.WriteLine("-----Задача№1-----------------------------------------------------------------");
//List<int> a1 = new List<int> { 14, 14, 14, 14 };
//List<int> b1 = new List<int> { 13, 5, 13, 12, 13 };
//List<List<int>> c1 = new List<List<int>> { };
//c1.Add(new List<int> { 16, 26, 12, 24, 3 });
//c1.Add(new List<int> { 5, 2, 19, 27, 2 });
//c1.Add(new List<int> { 29, 23, 25, 16, 8 });
//c1.Add(new List<int> { 2, 25, 14, 15, 21 });

//Console.WriteLine("Метод северо-западного угла");
//TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);
//transport_problem1.NorthWestAngleMethod();
//TransportProblem.PrintMatrix(transport_problem1.GetOptimalPlan());
//Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
//Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

//Console.WriteLine("Метод минимального элемента");
//transport_problem1.MethodOfMinElement();
//TransportProblem.PrintMatrix(transport_problem1.GetOptimalPlan());
//Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
//Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

//Console.WriteLine("Метод двойного предпочтения");
//transport_problem1.DoublePreferenceMethod();
//TransportProblem.PrintMatrix(transport_problem1.GetOptimalPlan());
//Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
//Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

//Console.WriteLine("Метод аппроксимации Фогеля");
//transport_problem1.MethodOfVogelApproximation();
//TransportProblem.PrintMatrix(transport_problem1.GetOptimalPlan());
//Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
//Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");


Console.WriteLine("-----Задача№2. Свести открытую к закрытой-------------------------------------\n");
List<int> a2 = new List<int> { 240, 40, 110 };
List<int> b2 = new List<int> { 90, 190, 40, 130 };
List<List<int>> c2 = new List<List<int>> { };
c2.Add(new List<int> { 7, 13, 9, 8 });
c2.Add(new List<int> { 14, 8, 7, 10 });
c2.Add(new List<int> { 3, 15, 20, 6 });
TransportProblem transport_problem2 = new TransportProblem(a2, b2, c2);
Console.WriteLine("Метод аппроксимации Фогеля");
transport_problem2.MethodOfVogelApproximation();
TransportProblem.PrintMatrix(transport_problem2.GetOptimalPlan());
Console.WriteLine("L(x) = " + transport_problem2.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem2.IsNonDegenerate() + "\n");

