using MathMod;
using System;
using System.Globalization;

internal class Program
{
    public static List<LinearProblem> ConverToLinearProblemList(List<string> lines)
    {
        int step = 1;//1-L(x), 2-type, 3-restrictions, comparison, free
        List<LinearProblem> linear_problems = new List<LinearProblem>();

        List<List<double>> a = new List<List<double>> { };
        List<double> b = new List<double> {};
        List<string> comparison = new List<string> { };
        List<double> c = new List<double> { };
        LinearProblem.problemType type = LinearProblem.problemType.max;


        for (int i=0; i<lines.Count; ++i)
        {
            if (lines[i].Trim() == string.Empty)
            {
                linear_problems.Add(new LinearProblem (a.Select(x => x.ToList()).ToList(), comparison.Select(x=>x).ToList(), b.Select(x=>x).ToList(), c.Select(x=>x).ToList(), type));
                step = 1;
            }
            else
            {
                if(step == 1)
                {
                    c.Clear();
                    c = new List<double> { };
                    c = lines[i].Split().Select(x => double.Parse(x, NumberStyles.AllowLeadingSign)).ToList();
                    //c = lines[i].Split().Select(double.Parse).ToList();
                    step = 2;
                }else if(step == 2)
                {
                    if (lines[i].Trim().ToLower() == "max") type = LinearProblem.problemType.max;
                    else if (lines[i].Trim().ToLower() == "min") type = LinearProblem.problemType.min;
                    step = 3;

                    a.Clear();b.Clear();comparison.Clear();
                    a = new List<List<double>> { };
                    b = new List<double> { };
                    comparison = new List<string> { };
                }
                else if(step ==3)
                {
                    a.Add(new List<double>());
                    List<string> temp_a_comparsion_b = lines[i].Split().ToList();
                    int line_count = temp_a_comparsion_b.Count;
                    for(int j=0; j<line_count-2; ++j)
                        a[a.Count-1].Add(Convert.ToDouble(temp_a_comparsion_b[j]));
                    comparison.Add(temp_a_comparsion_b[line_count-2]);
                    b.Add(Convert.ToDouble(temp_a_comparsion_b[line_count - 1]));
                }
            }
        }
        return linear_problems;
    }

    private static void Main(string[] args)
    {
        //ПОТЕНЦИАЛЫ

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

        //СИМПЛЕКС

internal class Program
{
    public static List<LinearProblem> ConverToLinearProblemList(List<string> lines)
    {
        int step = 1;//1-L(x), 2-type, 3-restrictions, comparison, free
        List<LinearProblem> linear_problems = new List<LinearProblem>();

        List<List<double>> a = new List<List<double>> { };
        List<double> b = new List<double> {};
        List<string> comparison = new List<string> { };
        List<double> c = new List<double> { };
        LinearProblem.problemType type = LinearProblem.problemType.max;


        for (int i=0; i<lines.Count; ++i)
        {
            if (lines[i].Trim() == string.Empty)
            {
                linear_problems.Add(new LinearProblem (a.Select(x => x.ToList()).ToList(), comparison.Select(x=>x).ToList(), b.Select(x=>x).ToList(), c.Select(x=>x).ToList(), type));
                step = 1;
            }
            else
            {
                if(step == 1)
                {
                    c.Clear();
                    c = new List<double> { };
                    c = lines[i].Split().Select(x => double.Parse(x, NumberStyles.AllowLeadingSign)).ToList();
                    //c = lines[i].Split().Select(double.Parse).ToList();
                    step = 2;
                }else if(step == 2)
                {
                    if (lines[i].Trim().ToLower() == "max") type = LinearProblem.problemType.max;
                    else if (lines[i].Trim().ToLower() == "min") type = LinearProblem.problemType.min;
                    step = 3;

                    a.Clear();b.Clear();comparison.Clear();
                    a = new List<List<double>> { };
                    b = new List<double> { };
                    comparison = new List<string> { };
                }
                else if(step ==3)
                {
                    a.Add(new List<double>());
                    List<string> temp_a_comparsion_b = lines[i].Split().ToList();
                    int line_count = temp_a_comparsion_b.Count;
                    for(int j=0; j<line_count-2; ++j)
                        a[a.Count-1].Add(Convert.ToDouble(temp_a_comparsion_b[j]));
                    comparison.Add(temp_a_comparsion_b[line_count-2]);
                    b.Add(Convert.ToDouble(temp_a_comparsion_b[line_count - 1]));
                }
            }
        }
        return linear_problems;
    }

    static void PrintMatrix<T>(List<List<T>> matrix)
    {
        for (int i = 0; i < matrix.Count; ++i)
        {
            for (int j = 0; j < matrix[0].Count; ++j)
                Console.Write(matrix[i][j] + "  ");
            Console.WriteLine();
        }
    }

    private static void Main(string[] args)
    {
        //List<List<double>> a = new List<List<double>> { };
        //a.Add(new List<double> { 1, 2, 1, 0, 0 });
        //a.Add(new List<double> { 1, 1, 0, 1, 0 });
        //a.Add(new List<double> { 2, 1, 0, 0, 1 });
        //List<double> b = new List<double> { 4, 3, 8 };
        //List<double> c = new List<double> { -3, -4, 0, 0, 0 };

        //List<int> basis = new List<int> { 2, 3, 4 };

        ////LinearProblem lp = new LinearProblem(a, b, c, LinearProblem.problemType.max, basis);
        //LinearProblem lp = new LinearProblem(a, b, c, LinearProblem.problemType.max);
        //lp.SimplexMethod();
        //Console.WriteLine(lp.GetObjectiveFun()+"\n");

        //List<List<double>> a1 = new List<List<double>> { };
        //a1.Add(new List<double> { 2, -1, 1, 1, 0, 0 });
        //a1.Add(new List<double> { -4, 2, -1, 0, 1, 0 });
        //a1.Add(new List<double> { 3, 0, 1, 0, 0, 1 });
        //List<double> b1 = new List<double> { 1, 2, 5 };
        //List<double> c1 = new List<double> { -1, 1, 3, 0, 0, 0 };

        //List<int> basis1 = new List<int> { 3,4,5 };

        ////LinearProblem lp1 = new LinearProblem(a1, b1, c1, LinearProblem.problemType.min, basis1);        
        //LinearProblem lp1 = new LinearProblem(a1, b1, c1, LinearProblem.problemType.min);
        //lp1.SimplexMethod();
        //Console.WriteLine(lp1.GetObjectiveFun());

        //List<List<double>> a1 = new List<List<double>> { };
        //a1.Add(new List<double> { 2, -1, 1 });
        //a1.Add(new List<double> { 4, -2, 1 });
        //a1.Add(new List<double> { 3, 0, 1 });
        //List<double> b1 = new List<double> { 1, -2, 5 };
        //List<string> comparison_signs = new List<string> { "<=", ">=", "<=" };
        //List<double> c1 = new List<double> { 1, -1, -3 };//!!Invert signs!!!

        ////LinearProblem lp1 = new LinearProblem(a1, b1, c1, LinearProblem.problemType.min, basis1);        
        //LinearProblem lp1 = new LinearProblem(a1, comparison_signs, b1, c1, LinearProblem.problemType.min);
        //lp1.SimplexMethod();
        //Console.WriteLine(lp1.GetObjectiveFun());

        //const string input = "symplex_data.txt";
        //List<string> lines = File.ReadAllLines(input).ToList();
        //List<LinearProblem> linear_problems = new List<LinearProblem>();
        //linear_problems = ConverToLinearProblemList(lines);

        //for (int i = 0; i < linear_problems.Count; ++i)
        //{
        //    linear_problems[i].SimplexMethod();
        //    Console.WriteLine($"{i}:\n{linear_problems[i].GetObjectiveFun()}");
        //}

        const double inf = double.PositiveInfinity;

        List<List<double>> m0 = new List<List<double>> {
        new List<double>{inf, 20,  18,  12,  8},
        new List<double>{5,   inf, 14,  7,   11},
        new List<double>{12,  18,  inf, 6,   11},
        new List<double>{11,  17,  11,  inf, 12},
        new List<double>{5,   5,   5,   5,   inf}
        };

        TravelingSalesmanProblem p0 = new TravelingSalesmanProblem(m0);

        //третья - фигня

        //записать в отдельный файл (и в консоль) ответы
        //третья - фигня

        //записать в отдельный файл (и в консоль) ответы
        //третья - фигня

        //записать в отдельный файл (и в консоль) ответы
    }
}