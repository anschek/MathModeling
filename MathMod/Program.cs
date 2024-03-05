using MathMod;


//Console.WriteLine("-----Задача№1-----------------------------------------------------------------");

//List<int> a1 = new List<int> { 90, 400, 110 };
//List<int> b1 = new List<int> { 140, 300, 160 };
//List<List<int>> c1 = new List<List<int>> { };
//c1.Add(new List<int> { 2, 5, 2 });
//c1.Add(new List<int> { 4, 1, 5 });
//c1.Add(new List<int> { 3, 6, 8 });

//TransportProblem transport_problem1 = new TransportProblem(a1, b1, c1);

//transport_problem1.MethodOfMinElement();
//transport_problem1.PrintReferencePlan();

//bool is_optimal = transport_problem1.AssessOptimality();
//if (is_optimal)
//{
//    Console.WriteLine("L(x) = " + transport_problem1.GetObjectiveFunction());
//    Console.WriteLine("Матрица оптимального решения:");
//    transport_problem1.PrintReferencePlan();
//}

//Console.WriteLine("\n-----Задача№2. Вырожденный опорный план----------------------------------------");

//List<int> a2 = new List<int> { 6000, 3000, 4000 };
//List<int> b2 = new List<int> { 4000, 5000, 1000, 3000 };
//List<List<int>> c2 = new List<List<int>> { };
//c2.Add(new List<int> { 6, 4, 9, 8 });
//c2.Add(new List<int> { 5, 3, 2, 8 });
//c2.Add(new List<int> { 2, 3, 6, 8 });

//TransportProblem transport_problem2 = new TransportProblem(a2, b2, c2);

//transport_problem2.MethodOfMinElement();
//transport_problem2.PrintReferencePlan();

//is_optimal = transport_problem2.AssessOptimality();
//if (is_optimal)
//{
//    Console.WriteLine("L(x) = " + transport_problem2.GetObjectiveFunction());
//    Console.WriteLine("Матрица оптимального решения:");
//    transport_problem2.PrintReferencePlan();
//}


//Console.WriteLine("\n-----Задача№3. Открытая задача------------------------------------------------");

//List<int> a3 = new List<int> { 40, 40, 60 };
//List<int> b3 = new List<int> { 27, 25, 30, 35 };
//List<List<int>> c3 = new List<List<int>> { };
//c3.Add(new List<int> { 70, 85, 55, 120 });
//c3.Add(new List<int> { 110, 90, 75, 110 });
//c3.Add(new List<int> { 115, 115, 70, 90, 120 });

//TransportProblem transport_problem3 = new TransportProblem(a3, b3, c3);

//transport_problem3.MethodOfVogelApproximation();
//transport_problem3.PrintReferencePlan();

//is_optimal = transport_problem3.AssessOptimality();
//if (is_optimal)
//{
//    Console.WriteLine("L(x) = " + transport_problem3.GetObjectiveFunction());
//    Console.WriteLine("Матрица оптимального решения:");
//    transport_problem3.PrintReferencePlan();
//}

//Console.WriteLine("\n-----Задача№4. Двойной ноль-----------------------------------------------------");

//List<int> a4 = new List<int> { 10,8,7 };
//List<int> b4 = new List<int> { 6,7,8,5 };
//List<List<int>> c4 = new List<List<int>> { };
//c4.Add(new List<int> { 100,150,500,150 });
//c4.Add(new List<int> { 420,180,60,120 });
//c4.Add(new List<int> { 200,250,120,150 });

//TransportProblem transport_problem4 = new TransportProblem(a4, b4, c4);

//transport_problem4.MethodOfVogelApproximation();
//transport_problem4.PrintReferencePlan();

//is_optimal = transport_problem4.AssessOptimality();
//if (is_optimal)
//{
//    Console.WriteLine("L(x) = " + transport_problem4.GetObjectiveFunction());
//    Console.WriteLine("Матрица оптимального решения:");
//    transport_problem4.PrintReferencePlan();
//}


//СИМПЛЕКС

List<List<double>> a = new List<List<double>>{};
a.Add(new List<double> {1,2,1,0,0 });
a.Add(new List<double> {1,1,0,1,0 });
a.Add(new List<double> {2,1,0,0,1 });
List<double> b = new List<double> { 4,3,8 };
List<double> c = new List<double> {-3,-4,0,0,0 };

List<int> basis = new List<int> { 2, 3, 4 };

LinearProblem lp1 = new LinearProblem (a,b,c, LinearProblem.problemType.max, basis );
List<double> obj_fun = lp1.SimplexMethod();
//divided_right_side = new List<double>(b.Count);
//List<double> divided_right_side = b.Select(x => x).ToList();

//Console.WriteLine($" 1:{b[1]} 2:{divided_right_side[1]}");
//divided_right_side[1] = 10;
//Console.WriteLine($" 1:{b[1]} 2:{divided_right_side[1]}");