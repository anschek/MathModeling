using MathMod;


Console.WriteLine("-----Задача№1-----------------------------------------------------------------");

List<int> a1 = new List<int> { 90, 400, 110 };
List<int> b1 = new List<int> { 140, 300, 160 };
List<List<int>> c1 = new List<List<int>> { };
c1.Add(new List<int> { 2, 5, 2 });
c1.Add(new List<int> { 4, 1, 5 });
c1.Add(new List<int> { 3, 6, 8 });

TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);

transport_problem1.MethodOfMinElement();
transport_problem1.PrintReferencePlan();

bool is_optimal = transport_problem1.AssessOptimality();
if (is_optimal)
{
    Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
    Console.WriteLine("Матрица оптимального решения:");
    transport_problem1.PrintReferencePlan();
}

Console.WriteLine("\n-----Задача№2. Вырожденный опорный план----------------------------------------");

List<int> a2 = new List<int> { 6000, 3000, 4000 };
List<int> b2 = new List<int> { 4000, 5000, 1000, 3000 };
List<List<int>> c2 = new List<List<int>> { };
c2.Add(new List<int> { 6, 4, 9, 8 });
c2.Add(new List<int> { 5, 3, 2, 8 });
c2.Add(new List<int> { 2, 3, 6, 8 });

TransportProblem transport_problem2 = new TransportProblem(a2, b2, c2);

transport_problem2.MethodOfMinElement();
transport_problem2.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem2.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem2.IsNonDegenerate() + "\n");

is_optimal = transport_problem2.AssessOptimality();
if (is_optimal)
{
    Console.WriteLine("L(x) = " + transport_problem2.GetObjectiveFunction());
    Console.WriteLine("Матрица оптимального решения:");
    transport_problem2.PrintReferencePlan();
}


Console.WriteLine("\n-----Задача№3. Открытая задача------------------------------------------------");

List<int> a3 = new List<int> { 40, 40, 60 };
List<int> b3 = new List<int> { 27, 25, 30, 35 };
List<List<int>> c3 = new List<List<int>> { };
c3.Add(new List<int> { 70, 85, 55, 120 });
c3.Add(new List<int> { 110, 90, 75, 110 });
c3.Add(new List<int> { 115, 115, 70, 90, 120 });

TransportProblem transport_problem3 = new TransportProblem(a3, b3, c3);

transport_problem3.MethodOfVogelApproximation();
transport_problem3.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem3.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem3.IsNonDegenerate() + "\n");

is_optimal = transport_problem3.AssessOptimality();
if (is_optimal)
{
    Console.WriteLine("L(x) = " + transport_problem3.GetObjectiveFunction());
    Console.WriteLine("Матрица оптимального решения:");
    transport_problem3.PrintReferencePlan();
}

Console.WriteLine("\n-----Задача№4. Открытая задача + вырожденный план-----------------------------");

List<int> a4 = new List<int> { 240, 40, 110 };
List<int> b4 = new List<int> { 90, 190, 40, 130 };
List<List<int>> c4 = new List<List<int>> { };
c4.Add(new List<int> {7, 13, 9, 8 });
c4.Add(new List<int> {14, 8, 7, 10 });
c4.Add(new List<int> { 3, 15, 20, 6 });

TransportProblem transport_problem4 = new TransportProblem(a4, b4, c4);

transport_problem4.MethodOfVogelApproximation();
transport_problem4.PrintReferencePlan();
Console.WriteLine("L(x) = " + transport_problem4.GetObjectiveFunction());
Console.WriteLine("План невырожденный: " + transport_problem4.IsNonDegenerate() + "\n");

is_optimal = transport_problem4.AssessOptimality();
if (is_optimal)
{
    Console.WriteLine("L(x) = " + transport_problem4.GetObjectiveFunction());
    Console.WriteLine("Матрица оптимального решения:");
    transport_problem4.PrintReferencePlan();
}
