using System.Globalization;

namespace P1
{
    public static class Methods
    {
        /// <summary>
        /// Метод считывает коэффициенты a, b, c, d
        /// Проверяет корректность ввода
        /// </summary>
        /// <param name="coefficients">Возвращает массив коэффициентов, нужных для дальнейшей работы программы</param>
        public static void ReadAllCoefficients(out double[] coefficients)
        {
            Console.WriteLine("Сейчас Вам нужно будет ввести значения коэффициентов a, b, c, d, которые лежат в интервале (0, 1):");
            
            //Массив нужен для понимания пользователем, какой именно коэф. вводится
            char[] coefficientsNames =  ['a', 'b', 'c', 'd'];
            coefficients = new double[4];
            
            //Цикл проходит по каждому элементу возвращаемого массива, заполняя его
            for (int i = 0; i < coefficientsNames.Length; i++)
            {
                Console.WriteLine($"Введите коэффициент {coefficientsNames[i]}: ");
                while (true)
                {
                    try
                    {
                        coefficients[i] = double.Parse(Console.ReadLine()?.Replace('.', ',') ??
                                                       throw new InvalidOperationException());
                        if (coefficients[i] <= 0 || coefficients[i] >= 1)
                        {
                            Console.WriteLine("Значение коэффицинта не входит в интервал (0, 1), попробуйте ещё раз: ");
                        }
                        else
                        {
                            break;
                        }
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Коэффициент введён неверно, попробуйте ещё раз: ");
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Коэффициент введён неверно, попробуйте ещё раз: ");
                    }
                }
            }
        }
        
        /// <summary>
        /// Метод проверяет существование и возможность чтения файла input.txt
        /// При возникновении проблем метод выдает ошибку в консоль
        /// </summary>
        /// <returns>возвращает true, если файл существует и с ним нет проблем, иначе false</returns>
        public static bool CheckInputFile()
        {
            if (!File.Exists("input.txt"))
            {
                Console.WriteLine("Входной файл на диске отсутсвует");
                return false;
            }

            try
            {
                File.ReadLines("input.txt");
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Проблемы с открытием файла");
            }
            catch (IOException)
            {
                Console.WriteLine($"Проблема c чтением данных из файла");
            }

            return false;
        }

        /// <summary>
        /// Метод считывает числовую информацию из файла, при помощи метода ConvertData преобразует ее и записывает в массив данных
        /// </summary>
        /// <param name="arrays">Массив со всеми данными из файла input</param>
        /// <returns>Возвращает true, если пользователь хочет продолжить выполнение программы с некорректными данными, иначе false</returns>
        public static bool ReadInputData(out double[][] arrays)
        {
            string[] input = File.ReadAllLines("input.txt");
            if (input.Length % 2 != 0)
            {
                Console.WriteLine(
                    "Количество строк неккоректно, нажмите enter, если хотите продолжить выполнение программы (последняя строка не будет учтена):");
                ConsoleKeyInfo continueIndicator = Console.ReadKey();
                if (continueIndicator.Key != ConsoleKey.Enter)
                {
                    arrays = [];
                    return false;
                }
            }

            arrays = new double[input.Length / 2 * 2][];
            for (int i = 0; i < input.Length / 2; i++)
            {
                ConvertData(input[i * 2], input[(i * 2) + 1], out double[] arrayX, out double[] arrayY);
                arrays[i * 2] = arrayX;
                arrays[(i * 2) + 1] = arrayY;
            }

            return true;
        }

        private static void ConvertData(string input1, string input2, out double[] arrayX, out double[] arrayY)
        {
            string[] formattedInput1 = input1.Split(' ');
            string[] formattedInput2 = input2.Split(' ');
            int correctValues1 = 0;
            int correctValues2 = 0;

            foreach (string t in formattedInput1)
            {
                if (double.TryParse(t, out double _))
                {
                    correctValues1++;
                }
            }

            foreach (string t in formattedInput2)
            {
                if (double.TryParse(t, out double _))
                {
                    correctValues2++;
                }
            }

            if (correctValues1 != correctValues2)
            {
                arrayX = [0.0];
                arrayY = [0.0];
            }
            else
            {
                arrayX = new double[correctValues1];
                arrayY = new double[correctValues1];
                int counter1 = 0;
                for (int i = 0; i < formattedInput1.Length; i++)
                {
                    if (double.TryParse(formattedInput1[i], out double x))
                    {
                        arrayX[counter1] = x;
                        counter1++;
                    }
                }

                int counter2 = 0; 
                for (int i = 0; i < formattedInput2.Length; i++)
                {
                    if (double.TryParse(formattedInput2[i], out double x))
                    {
                        arrayY[counter2] = x;
                        counter2++;
                    }
                }
            }
        }

        public static bool CreateResultArray(double[] coefficients, double[][] arrays, out double[][] arrayResult)
        {
            arrayResult = new double[arrays.Length / 2][];
            for (int i = 0; i < arrays.Length / 2; i++)
            {
                arrayResult[i] = new double[arrays[i].Length];
                for (int j = 0; j < arrays[i].Length; j++)
                {
                    if ((arrays[i + 1][j] * coefficients[2]) + coefficients[3] != 0)
                    {
                        arrayResult[i][j] = ((coefficients[0] * arrays[i][j]) + coefficients[1]) / ((arrays[i + 1][j] * coefficients[2]) + coefficients[3]);
                    }
                    else
                    {
                        Console.WriteLine("В файле присутсвует деление на 0, вы хотите продолжить выполнение программы, тогда в соответсвующую ячейку будет записан 0?");
                        Console.WriteLine("Для продолжения нажмите enter, иначе - любую клавишу: ");
                        ConsoleKeyInfo continueIndicator = Console.ReadKey();
                        if (continueIndicator.Key != ConsoleKey.Enter)
                        {
                            
                            arrayResult[i][j] = 0;
                        }
                        else
                        {
                            arrayResult = [];
                            return false;
                        }
                        
                    }
                }
            }
            return true;
        }

        
        public static bool CreateOutputFiles(double[][] arrayResult)
        {
            try
            {
                for (int i = 0; i < arrayResult.Length; i++)
                {
                    string[] lines = Array.ConvertAll(arrayResult[i], x => x.ToString(CultureInfo.CurrentCulture));
                    File.WriteAllLines($"output-{i}.txt", lines);
                    File.AppendAllText("config.txt", $"Создан файл output-{i}.txt");
                }
                return true;
            }
            catch
            {
                Console.WriteLine("Проблемы с записью данных в файл");
                return false;
            }
        }
    }
}