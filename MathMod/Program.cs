using MathMod;


Console.WriteLine("-----Задача№1-----------------------------------------------------------------");
List<int> a1 = new List<int> { 14, 14, 14, 14 };
List<int> b1 = new List<int> { 13, 5, 13, 12, 13 };
List<List<int>> c1 = new List<List<int>> { };
c1.Add(new List<int> { 16, 26, 12, 24, 3 });
c1.Add(new List<int> { 5, 2, 19, 27, 2 });
c1.Add(new List<int> { 29, 23, 25, 16, 8 });
c1.Add(new List<int> { 2, 25, 14, 15, 21 });

Console.WriteLine("Метод северо-западного угла");
TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);
transport_problem1.NorthWestAngleMethod();
transport_problem1.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

Console.WriteLine("Метод минимального элемента");
transport_problem1.MethodOfMinElement();
transport_problem1.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

Console.WriteLine("Метод двойного предпочтения");
transport_problem1.DoublePreferenceMethod();
transport_problem1.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");

Console.WriteLine("Метод аппроксимации Фогеля");
transport_problem1.MethodOfVogelApproximation();
transport_problem1.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem1.IsNonDegenerate() + "\n");


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
transport_problem2.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem2.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem2.IsNonDegenerate() + "\n");

List<int> a3 = new List<int> { 40, 40, 60 };
List<int> b3 = new List<int> { 27, 25, 30, 35 };
List<List<int>> c3 = new List<List<int>> { };
c3.Add(new List<int> {70, 85, 55, 120 });
c3.Add(new List<int> { 110, 90, 75, 110 });
c3.Add(new List<int> { 135, 95, 80, 75 });
TransportProblem transport_problem3 = new TransportProblem(a3, b3, c3);
Console.WriteLine("Метод аппроксимации Фогеля");
transport_problem3.MethodOfVogelApproximation();
transport_problem3.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem3.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem3.IsNonDegenerate() + "\n");

List<int> a4 = new List<int> { 20000,30000,30000 };
List<int> b4 = new List<int> { 40000,24000,10000 };
List<List<int>> c4 = new List<List<int>> { };
c4.Add(new List<int> {12,13,11 });
c4.Add(new List<int> { 14,12,15 });
c4.Add(new List<int> { 11,10,13 });
TransportProblem transport_problem4 = new TransportProblem(a4, b4, c4);
Console.WriteLine("Метод минимального элемента");
transport_problem4.MethodOfMinElement();
transport_problem4.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem4.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem4.IsNonDegenerate() + "\n");

