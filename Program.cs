//22МОиАИС184-1  Карпов Дмитрий Валерьевич
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Курсовой_Проект
{
    struct Access
    {
        public string Login;
        public string Password;
    }
    class Program
    {
        static string SPECIALPassword = "admin";//Особый пароль доступа
        static void Main(string[] args)
        {
            //Тестовые вопросы хранятся в .txt файлах,в папке Debug, без них программа не будет работать!
            CreatePasswordFolder();         //Создание папки с паролями,ответами, если вдруг их ещё нет
            CreateAnswerFolder();
            Console.SetWindowSize(100, 50); //РАЗМЕР КОНСОЛИ
            string Login = null;   //Логин в дальнейшем будет нужен! А именно логин, используется в название файлов к ответам, паролям 
            string Password = null;// Пароль нужен для того, что бы его изменить при желаниии(Проверка)
            //После завершениея метода  ProgramStart (вход пользователя) будут доступны основные методы
            ProgramStart(ref Login, ref Password);
            MainMethod(Login, Password); //Главная часть, в который реализовано меню с различным функционалом
        }
        static void MainMethod(string Login, string Password)
        {
            string MainMenu;
            while (true)
            {
                Console.Clear();
                MainMenu = Menu(Login);
                switch (MainMenu)
                {
                    case "1":
                        {
                            GetResult(Login);
                            break;
                        }
                    case "2":
                        {
                            GolomstockInstruction(Login);
                            break;
                        }
                    case "3":
                        {
                            BelovInstruction(Login);
                            break;
                        }
                    case "4":
                        {
                            PENInstruction(Login);
                            break;
                        }
                    case "5":
                        {
                            AllInfoAboutLoginAndPassword();
                            break;
                        }
                    case "6":
                        {
                            ChangeTextQuestion();
                            break;
                        }
                    case "7":
                        {
                            LearnSomeoneAnswers();
                            break;
                        }
                    case "8":
                        {
                            ChangePassword(Login, Password);
                            break;
                        }
                    case "9":
                        {
                            ProgramStart(ref Login, ref Password);
                            break;
                        }
                    case "0":
                        {
                            return;
                        }
                }
            }
        }
        static void ProgramStart(ref string Login, ref string Password)
        {
            Console.Clear();
            PSYTESTgraffiti();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\n\n#####");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Добро пожаловать в Программу Психологических Тестирований");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("#####\n\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Для начала работы:\n1)Войдите в ранее созданную учётную запись\n2)Создайте учётную запись, если её нет");
            int Choise = ChoseOneOrTwo(); ;
            Console.ResetColor();
            switch (Choise)
            {
                case 1:
                    {
                        MainAccess(ref Login, ref Password);
                        break;
                    }
                case 2:
                    {
                        CreateAccount(ref Login, ref Password);
                        break;
                    }
            }
        }
        //Методы для осуществления входа пользователей
        static void CreateAccount(ref string Login, ref string Password)
        {
            try
            {
                Console.Clear();
                PSYTESTgraffiti();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nСоздание нового аккаунта: (Следуйте указаниям)\n\n");
                Console.ResetColor();
                Int64 MyPassword = 0;//Запись паролья произоёдт бинарным способом
                bool check = false;
                bool toocheck = false;
                //ЛОГИН
                do
                {
                    if (check == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Такой логин уже существует, либо он не соответстует требованиям...\n\n");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("Придумайте логин (минимум-3 символа, максимум-20):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Login = Console.ReadLine();
                    check = true;
                }
                while (Login.Length < 3 || Login.Length > 20 || CheckRepeatLogin(Login) || string.IsNullOrWhiteSpace(Login));// 1) Проверка существует ли такой логин уже в базе 2) Минимум 3 символа 3) Нельзя вводить логин из пробелов
                //ПАРОЛЬ
                do
                {
                    if (toocheck == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неправильный ввод! Вводите только цифры (Минимум 2, Максимум 19)\n");
                    }
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("\nПридумайте пароль (Пароль может состоять только из чисел!):");
                    Console.ForegroundColor = ConsoleColor.White;
                    toocheck = true;
                    try
                    {
                        MyPassword = Convert.ToInt64(Console.ReadLine());
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Неправильный ввод! Вводите только цифры(Минимум 2, Максимум 19)\n");
                        toocheck = false;
                    }

                }
                while (MyPassword < 2 || MyPassword < 0);
                //путь к папке Debug
                string LoginTXT = AppDomain.CurrentDomain.BaseDirectory + "\\Password\\" + Login + ".dat"; //Сборка полного пути 
                //Записываем пароль бинарным способом
                FileStream fsW = new FileStream(LoginTXT.ToLower(), FileMode.Create, FileAccess.Write);
                BinaryWriter bw = new BinaryWriter(fsW);
                bw.Write(MyPassword);
                bw.Close();
                fsW.Close();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n#################################");
                Console.WriteLine("#### Аккаунт успешно создан! #### ");
                Console.WriteLine("#################################");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.ResetColor();
                Password = Convert.ToString(MyPassword);
                ProgramStart(ref Login, ref Password);
            }
            catch
            {
                Console.Clear();
                PSYTESTgraffiti();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nПроизошла непредвиденная ошибка. Будет произведён возврат в меню");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                ProgramStart(ref Login, ref Password);
            }
        }
        static bool CheckRepeatLogin(string Login) //Проверка логина в файлах!!!
        {
            bool Repeat = false;
            DirectoryInfo dir = new DirectoryInfo("Password");
            FileInfo[] UsersPassword = dir.GetFiles("*.dat");
            for (int i = 0; i < UsersPassword.Length; i++)
            {
                if (Path.GetFileNameWithoutExtension(UsersPassword[i].Name) == Login)
                {
                    Repeat = true;
                }
            }
            return Repeat;
        }

        static void MainAccess(ref string Login, ref string Password)
        {
            Console.Clear();
            PSYTESTgraffiti();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nВведите логин:");
            Console.ForegroundColor = ConsoleColor.White;
            Login = (Console.ReadLine()).ToLower();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nВведите пароль:");
            Console.ForegroundColor = ConsoleColor.White;
            Password = Console.ReadLine();
            string LoginTXT = AppDomain.CurrentDomain.BaseDirectory + "\\Password\\" + Login + ".dat";
            bool check = false;
            if (File.Exists(LoginTXT))
            {
                //считывание
                FileStream fsR = new FileStream(LoginTXT, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fsR);

                if (Convert.ToString(br.ReadInt64()) == Password)//Проверка
                {
                    check = true;
                }
                br.Close();
                fsR.Close();
            }
            if (check == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nНеправильный логин/пароль");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n1)Попробовать ещё раз\n2)Перейти к регистрации аккаунта");
                int Choise = ChoseOneOrTwo(); ;
                switch (Choise)
                {
                    case 1:
                        {
                            MainAccess(ref Login, ref Password);
                            break;
                        }
                    case 2:
                        {
                            CreateAccount(ref Login, ref Password);
                            break;
                        }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\n#################################");
                Console.WriteLine("#### Вход успешно выполнен!  ####");
                Console.WriteLine("#################################");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.ResetColor();
            }
        }
        static string Menu(string Login)
        {
            PSYTESTgraffiti();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("\nЗдравствуйте - ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(Login);
            Console.WriteLine("\n_______________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите пункт меню: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1. Просмотр результатов ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("2. Тест «Карта интересов» ");
            Console.WriteLine("3. Тест на темперамент ");
            Console.WriteLine("4. Опросник «PEN»");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("5. Узнать логины/пароли всех пользователей");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("   *(ТРЕБУЕТСЯ ОСОБЫЙ ПАРОЛЬ)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("6. Внести текстовые изменения в вопросы теста");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("*(ТРЕБУЕТСЯ ОСОБЫЙ ПАРОЛЬ)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("7. Получить результаты всех пользователей");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("    *(ТРЕБУЕТСЯ ОСОБЫЙ ПАРОЛЬ)");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("8. Изменить пароль");
            Console.WriteLine("9. Сменить пользователя");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("0. Завершить сеанс");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("_______________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.White;
            //Считывание нажатой клавиши
            ConsoleKeyInfo K = Console.ReadKey();
            if (K.Key == ConsoleKey.NumPad1 || K.Key == ConsoleKey.D1)
            {
                return "1";
            }
            if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
            {
                return "2";
            }
            if (K.Key == ConsoleKey.NumPad3 || K.Key == ConsoleKey.D3)
            {
                return "3";
            }
            if (K.Key == ConsoleKey.NumPad4 || K.Key == ConsoleKey.D4)
            {
                return "4";
            }
            if (K.Key == ConsoleKey.NumPad5 || K.Key == ConsoleKey.D5)
            {
                return "5";
            }
            if (K.Key == ConsoleKey.NumPad6 || K.Key == ConsoleKey.D6)
            {
                return "6";
            }
            if (K.Key == ConsoleKey.NumPad7 || K.Key == ConsoleKey.D7)
            {
                return "7";
            }
            if (K.Key == ConsoleKey.NumPad8 || K.Key == ConsoleKey.D8)
            {
                return "8";
            }
            if (K.Key == ConsoleKey.NumPad9 || K.Key == ConsoleKey.D9)
            {
                return "9";
            }
            if (K.Key == ConsoleKey.NumPad0 || K.Key == ConsoleKey.D0)
            {
                return "0";
            }
            else
            {
                return null;
            }
        }
        //Всмпомогательные методы
        static void CreatePasswordFolder()
        {
            DirectoryInfo dir = new DirectoryInfo("Password");
            if (Directory.Exists("Password") == false)
            {
                dir.Create();
            }
        }
        static void CreateAnswerFolder()
        {
            DirectoryInfo dir = new DirectoryInfo("Answer");
            if (Directory.Exists("Answer") == false)
            {
                dir.Create();
            }
        }
        static void PSYTESTgraffiti()
        {
            Console.ForegroundColor = ConsoleColor.Green; // устанавливаем цвет
            Console.WriteLine("########   ######  ##    ##     ######## ########  ######  ######## ");
            Console.WriteLine("##     ## ##    ##  ##  ##         ##    ##       ##    ##    ##    ");
            Console.WriteLine("##     ## ##         ####          ##    ##       ##          ##    ");
            Console.WriteLine("########   ######     ##           ##    ######    ######     ##    ");
            Console.WriteLine("##              ##    ##           ##    ##             ##    ##    ");
            Console.WriteLine("##        ##    ##    ##    ###    ##    ##       ##    ##    ##    ");
            Console.WriteLine("##         ######     ##    ###    ##    ########  ######     ##    \n");
            Console.ResetColor(); // сбрасываем в стандартный
        }
        static string GMenu()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите пункт меню: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Начать/Продолжить проходить тест");
            Console.WriteLine("2. Изменить вариант ответа");
            Console.WriteLine("3. Вернуться в главное меню");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo K = Console.ReadKey();
            if (K.Key == ConsoleKey.NumPad1 || K.Key == ConsoleKey.D1)
            {
                return "1";
            }
            if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
            {
                return "2";
            }
            if (K.Key == ConsoleKey.NumPad3 || K.Key == ConsoleKey.D3)
            {
                return "3";
            }
            else
            {
                return null;
            }
        }
        //Тест КАРТА ИНТЕРЕСОВ
        static void Golomstockgraffiti()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("            «Карта интересов»");
            Console.WriteLine("             А.Е. Голомшток\n\n");
            Console.ResetColor();

        }
        static void GolomstockInstruction(string Login)
        {
            Console.Clear();
            Golomstockgraffiti();
            //инструкция
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Инструкция:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" для определения ведущих интересов Вам предлагается перечень вопросов.");
            Console.WriteLine("-Если Вам очень нравится то, о чем спрашивается в вопросе, введите два плюса (++) и нажмите Enter");
            Console.WriteLine("-если просто нравится - один плюс      ( + )");
            Console.WriteLine("-если не знаете, сомневаетесь - ноль   ( 0 )");
            Console.WriteLine("-если не нравится - один минус         ( - )");
            Console.WriteLine("-а если очень не нравиться - два минуса (--).");
            Console.WriteLine("\nЕсли вы ошиблись при выборе ответа, то не переживайте, вы можете отдельно внести изменения в ответ, главное запомните номер вопроса!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nЕсли вы готовы, нажмите любую клавишу для начала...");
            Console.ReadKey();
            GolomstockTEST(Login);
        }
        static void GolomstockTEST(string Login)
        {
            string GolomstockMenu;
            while (true)
            {
                Console.Clear();
                Golomstockgraffiti();
                GolomstockMenu = GMenu();
                switch (GolomstockMenu)
                {
                    case "1"://Прохождение теста
                        {
                            AnswerGolomstock(Login);
                            break;
                        }
                    case "2"://изменение варианта ответа
                        {
                            ChangeAnswerInGolomstock(Login);
                            break;
                        }
                    case "3"://возврат назад
                        {
                            return;
                        }
                }
            }
        }
        static void AnswerGolomstock(string Login)
        {
            string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\Golomstock.txt", Encoding.Default);//Считывание всех вопросов в массив строк       
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Golomstock-" + Login + ".txt";
            if (!File.Exists(TxtAnswer))
            {
                File.Create(TxtAnswer).Close();
            }
            int CountAns = 0;
            using (StreamReader sr = new StreamReader(TxtAnswer, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)   //Позволяет продолжать с того момента, на котором остановился
                {
                    CountAns++;
                }
            }
            if (CountAns == Questions.Length)
            {
                Console.Clear();
                Golomstockgraffiti();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Тест пройден на 100%\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1) Вернуться назад");
                Console.WriteLine("2) Удалить все ответы, что бы пройти тест заново");
                Console.ForegroundColor = ConsoleColor.White;
                int Choise = ChoseOneOrTwo();
                Console.ResetColor();
                switch (Choise)
                {
                    case 1:
                        {
                            return;
                        }
                    case 2://Удаляем полносью заполненые ответы
                        {
                            if (File.Exists(TxtAnswer))
                            {
                                File.Delete(TxtAnswer);
                            }
                            Console.Clear();
                            Golomstockgraffiti();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Ваши ответы успешно удалены!\n");
                            Console.WriteLine("Для продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            return;
                        }
                }
            }
            else
            {
                string MyAnswer;
                for (int i = CountAns; i < Questions.Length; i++)
                {
                    Console.Clear();
                    Golomstockgraffiti();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Любите ли Вы? Нравится ли Вам? Хотели бы Вы:");
                    Console.WriteLine(Questions[i] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Варианты ответа: (+) (++) (0) (-) (--)");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы устали, введите двойку - (2) для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Введите один из вариантов ответа: (+) (++) (0) (-) (--)\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        MyAnswer = Console.ReadLine();
                        check = true;
                    }
                    while (!("+" == MyAnswer) && !("++" == MyAnswer) &&
                    !("0" == MyAnswer) && !("-" == MyAnswer) && !("--" == MyAnswer) && !("2" == MyAnswer));//  Проверка на правильный ввод

                    if (MyAnswer == "2")
                    {
                        break;
                    }
                    WriteMyAnswer(MyAnswer, TxtAnswer);
                }
                Console.Clear();
                Golomstockgraffiti();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Работа с тестом завершена!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        static void ChangeAnswerInGolomstock(string Login)
        {
            Console.Clear();
            Golomstockgraffiti();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Golomstock-" + Login + ".txt";
            if (File.Exists(path))
            {
                string[] Answer = File.ReadAllLines(path, Encoding.Default);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Введите номер вопроса, который следует изменить");
                Console.ForegroundColor = ConsoleColor.White;
                int ChoiseForChange = ChangerChoise(174);
                if (ChoiseForChange < Answer.Length)
                {
                    Console.Clear();
                    Golomstockgraffiti();
                    string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\Golomstock.txt", Encoding.Default);
                    string MyAnswer;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Любите ли Вы? Нравится ли Вам? Хотели бы Вы:");
                    Console.WriteLine(Questions[ChoiseForChange] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Варианты ответа: (+) (++) (0) (-) (--)");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы по ошибке зашли в данный раздел, введите двойку - (2) для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Введите один из вариантов ответа: (+) (++) (0) (-) (--)\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        MyAnswer = Console.ReadLine();
                        check = true;
                    }
                    while (!("+" == MyAnswer) && !("++" == MyAnswer) &&
                    !("0" == MyAnswer) && !("-" == MyAnswer) && !("--" == MyAnswer) && !("2" == MyAnswer));//  Проверка на правильный ввод
                    if (MyAnswer == "2")
                    {
                        return;
                    }
                    Console.Clear();
                    Golomstockgraffiti();
                    string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Golomstock-" + Login + ".txt";
                    WriteChangeAnswer(ChoiseForChange, MyAnswer, Answer, TxtAnswer);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ещё не ответили на вопрос {0}, что бы его изменять!\n", ChoiseForChange + 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вы ещё не начали проходить тест!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        //Тест на темперамент
        static void Belovgraffiti()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("            «Тест на темперамент»");
            Console.WriteLine("                  А.Белов\n\n");
            Console.ResetColor();
        }
        static void BelovInstruction(string Login)
        {
            Console.Clear();
            Belovgraffiti();
            //инструкция
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Инструкция:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" Внимательно вчитывайтесь в свойста, присущие тому или иному темпераменту ");
            Console.WriteLine("-Нажмите на плюс (клавиша [+] на клавиатуре) , если свойство вам присуще ");
            Console.WriteLine("-или нажмите на минус (клавиша [-] на клавиатуре), если это свойство у вас не выражено.");
            Console.WriteLine("\nЕсли вы ошиблись при выборе ответа, то не переживайте, вы можете отдельно внести изменения в ответ, главное запомните номер вопроса!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nЕсли вы готовы, нажмите любую клавишу для начала...");
            Console.ReadKey();
            BelovTEST(Login);
        }
        static void BelovTEST(string Login)
        {
            string BelovMenu;
            while (true)
            {
                Console.Clear();
                Belovgraffiti();
                BelovMenu = GMenu();
                switch (BelovMenu)
                {
                    case "1"://Прохождение теста
                        {
                            AnswerBelov(Login);
                            break;
                        }
                    case "2"://изменение варианта ответа
                        {
                            ChangeAnswerInBelov(Login);
                            break;
                        }
                    case "3"://возврат назад
                        {
                            return;
                        }
                }
            }
        }
        static void AnswerBelov(string Login)
        {
            string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\Belov.txt", Encoding.Default);//Считывание всех вопросов в массив строк       
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Belov-" + Login + ".txt";
            if (!File.Exists(TxtAnswer))
            {
                File.Create(TxtAnswer).Close();
            }
            int CountAns = 0;
            using (StreamReader sr = new StreamReader(TxtAnswer, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)   //Позволяет продолжать с того момента, на котором остановился
                {
                    CountAns++;
                }
            }
            if (CountAns == Questions.Length)
            {
                Console.Clear();
                Belovgraffiti();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Тест пройден на 100%\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1) Вернуться назад");
                Console.WriteLine("2) Удалить все ответы, что бы пройти тест заново");
                Console.ForegroundColor = ConsoleColor.White;
                int Choise = ChoseOneOrTwo();
                Console.ResetColor();
                switch (Choise)
                {
                    case 1:
                        {
                            return;
                        }
                    case 2://Удаляем полносью заполненые ответы
                        {
                            if (File.Exists(TxtAnswer))
                            {
                                File.Delete(TxtAnswer);
                            }
                            Console.Clear();
                            Belovgraffiti();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Ваши ответы успешно удалены!\n");
                            Console.WriteLine("Для продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            return;
                        }
                }
            }
            else
            {
                string MyAnswer;
                ConsoleKeyInfo K;
                for (int i = CountAns; i < Questions.Length; i++)
                {
                    Console.Clear();
                    Belovgraffiti();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Вы:");
                    Console.WriteLine(Questions[i] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Для ответа достаточно просто нажать на нужную клавишу, на клавиатуре!");
                    Console.WriteLine("Варианты ответа: [+]  [-]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы устали, нажмите - клавишу [2] для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nВыберите один из вариантов ответа: [+]  [-]\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        K = Console.ReadKey();   //ВАРИАНТ С НАЖАТИЕМ КЛАВИШИ ДЛЯ ОТВЕТА(т.е. Enter нажимать не требуется)
                        check = true;
                    }
                    while (K.Key != ConsoleKey.OemPlus && K.Key != ConsoleKey.OemMinus && K.Key != ConsoleKey.NumPad2 && K.Key != ConsoleKey.D2
                    && K.Key != ConsoleKey.Subtract && K.Key != ConsoleKey.Add);//  Проверка на правильный ввод
                    if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
                    {
                        break;
                    }
                    if (K.Key == ConsoleKey.OemPlus || K.Key == ConsoleKey.Add)
                    {
                        MyAnswer = "+";
                    }
                    else
                    {
                        MyAnswer = "-";
                    }
                    WriteMyAnswer(MyAnswer, TxtAnswer);
                    Thread.Sleep(100);//Требуется, что бы пользователь не мог спровацировать вылет программы - зажатием плюса /минуса 
                }
                Console.Clear();
                Belovgraffiti();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Работа с тестом завершена!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        static void ChangeAnswerInBelov(string Login)
        {
            Console.Clear();
            Belovgraffiti();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Belov-" + Login + ".txt";
            if (File.Exists(path))
            {
                string[] Answer = File.ReadAllLines(path, Encoding.Default);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Введите номер вопроса, который следует изменить");
                Console.ForegroundColor = ConsoleColor.White;
                int ChoiseForChange = ChangerChoise(80);
                ConsoleKeyInfo K;
                if (ChoiseForChange < Answer.Length)
                {
                    Console.Clear();
                    Belovgraffiti();
                    string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\Belov.txt", Encoding.Default);
                    string MyAnswer;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Вы:");
                    Console.WriteLine(Questions[ChoiseForChange] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Для ответа достаточно просто нажать на нужную клавишу, на клавиатуре!");
                    Console.WriteLine("Варианты ответа: [+]  [-]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы зашли в данный раздел случайно, нажмите - клавишу [2] для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nВыберите один из вариантов ответа: [+]  [-]\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        K = Console.ReadKey();   //ВАРИАНТ С НАЖАТИЕМ КЛАВИШИ ДЛЯ ОТВЕТА(т.е. Enter нажимать не требуется)
                        check = true;
                    }
                    while (K.Key != ConsoleKey.OemPlus && K.Key != ConsoleKey.OemMinus && K.Key != ConsoleKey.NumPad2 && K.Key != ConsoleKey.D2
                    && K.Key != ConsoleKey.Subtract && K.Key != ConsoleKey.Add);//  Проверка на правильный ввод
                    if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
                    {
                        return;
                    }
                    if (K.Key == ConsoleKey.OemPlus || K.Key == ConsoleKey.Add)
                    {
                        MyAnswer = "+";
                    }
                    else
                    {
                        MyAnswer = "-";
                    }
                    Console.Clear();
                    Belovgraffiti();
                    string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Belov-" + Login + ".txt";
                    WriteChangeAnswer(ChoiseForChange, MyAnswer, Answer, TxtAnswer);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ещё не ответили на вопрос {0}, что бы его изменять!\n", ChoiseForChange + 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вы ещё не начали проходить тест!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        //ТЕСТ Айзенка
        static void PENgraffiti()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("              Опросник «PEN»");
            Console.WriteLine("             Г. и С. Айзенк\n\n");
            Console.ResetColor();
        }
        static void PENInstruction(string Login)
        {
            Console.Clear();
            PENgraffiti();
            //инструкция
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Инструкция:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(" Вам будут предложены утверждения, касающиеся вашего характера и здоровья. ");
            Console.WriteLine("-Если вы согласны с утверждением, нажмите «+» на клавиатуре,  ");
            Console.WriteLine("-если нет ,то  «–» , долго не задумывайтесь, здесь правильных или неправильных ответов нет.");
            Console.WriteLine("\nЕсли вы ошиблись при выборе ответа, то не переживайте, вы можете отдельно внести изменения в ответ, главное запомните номер вопроса!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nЕсли вы готовы, нажмите любую клавишу для начала...");
            Console.ReadKey();
            PENTEST(Login);
        }
        static void PENTEST(string Login)
        {
            string PENMenu;
            while (true)
            {
                Console.Clear();
                PENgraffiti();
                PENMenu = GMenu();
                switch (PENMenu)
                {
                    case "1"://Прохождение теста
                        {
                            AnswerPEN(Login);
                            break;
                        }
                    case "2"://изменение варианта ответа
                        {
                            ChangeAnswerInPEN(Login);
                            break;
                        }
                    case "3"://возврат назад
                        {
                            return;
                        }
                }
            }
        }
        static void AnswerPEN(string Login)
        {
            string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\PEN.txt", Encoding.Default);//Считывание всех вопросов в массив строк       
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\PEN-" + Login + ".txt";
            if (!File.Exists(TxtAnswer))
            {
                File.Create(TxtAnswer).Close();
            }
            int CountAns = 0;
            using (StreamReader sr = new StreamReader(TxtAnswer, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)   //Позволяет продолжать с того момента, на котором остановился
                {
                    CountAns++;
                }
            }
            if (CountAns == Questions.Length)
            {
                Console.Clear();
                PENgraffiti();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Тест пройден на 100%\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("1) Вернуться назад");
                Console.WriteLine("2) Удалить все ответы, что бы пройти тест заново");
                Console.ForegroundColor = ConsoleColor.White;
                int Choise = ChoseOneOrTwo();
                Console.ResetColor();
                switch (Choise)
                {
                    case 1:
                        {
                            return;
                        }
                    case 2://Удаляем полносью заполненые ответы
                        {
                            if (File.Exists(TxtAnswer))
                            {
                                File.Delete(TxtAnswer);
                            }
                            Console.Clear();
                            PENgraffiti();
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Ваши ответы успешно удалены!\n");
                            Console.WriteLine("Для продолжения нажмите любую клавишу...");
                            Console.ReadKey();
                            return;
                        }
                }
            }
            else
            {
                string MyAnswer;
                ConsoleKeyInfo K;
                for (int i = CountAns; i < Questions.Length; i++)
                {
                    Console.Clear();
                    PENgraffiti();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Questions[i] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Для ответа достаточно просто нажать на нужную клавишу, на клавиатуре!");
                    Console.WriteLine("Варианты ответа: [+]  [-]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы устали, нажмите - клавишу [2] для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nВыберите один из вариантов ответа: [+]  [-]\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        K = Console.ReadKey();   //ВАРИАНТ С НАЖАТИЕМ КЛАВИШИ ДЛЯ ОТВЕТА(т.е. Enter нажимать не требуется)
                        check = true;
                    }
                    while (K.Key != ConsoleKey.OemPlus && K.Key != ConsoleKey.OemMinus && K.Key != ConsoleKey.NumPad2 && K.Key != ConsoleKey.D2
                    && K.Key != ConsoleKey.Subtract && K.Key != ConsoleKey.Add);//  Проверка на правильный ввод
                    if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
                    {
                        break;
                    }
                    if (K.Key == ConsoleKey.OemPlus || K.Key == ConsoleKey.Add)
                    {
                        MyAnswer = "+";
                    }
                    else
                    {
                        MyAnswer = "-";
                    }
                    WriteMyAnswer(MyAnswer, TxtAnswer);
                    Thread.Sleep(100);//Требуется, что бы пользователь не мог спровацировать вылет программы - зажатием плюса /минуса 
                }
                Console.Clear();
                PENgraffiti();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Работа с тестом завершена!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        static void ChangeAnswerInPEN(string Login)
        {
            Console.Clear();
            PENgraffiti();
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\PEN-" + Login + ".txt";
            if (File.Exists(path))
            {
                string[] Answer = File.ReadAllLines(path, Encoding.Default);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Введите номер вопроса, который следует изменить");
                Console.ForegroundColor = ConsoleColor.White;
                int ChoiseForChange = ChangerChoise(101);
                ConsoleKeyInfo K;
                if (ChoiseForChange < Answer.Length)
                {
                    Console.Clear();
                    PENgraffiti();
                    string[] Questions = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\PEN.txt", Encoding.Default);
                    string MyAnswer;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(Questions[ChoiseForChange] + "\n\n");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Для ответа достаточно просто нажать на нужную клавишу, на клавиатуре!");
                    Console.WriteLine("Варианты ответа: [+]  [-]");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("________________________________________________");
                    Console.WriteLine("Если вы зашли в данный раздел случайно, нажмите - клавишу [2] для выхода\n");
                    bool check = false;
                    do
                    {
                        if (check == true)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\nВыберите один из вариантов ответа: [+]  [-]\n\n");
                        }
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write("Ваш вариант ответа:");
                        K = Console.ReadKey();   //ВАРИАНТ С НАЖАТИЕМ КЛАВИШИ ДЛЯ ОТВЕТА(т.е. Enter нажимать не требуется)
                        check = true;
                    }
                    while (K.Key != ConsoleKey.OemPlus && K.Key != ConsoleKey.OemMinus && K.Key != ConsoleKey.NumPad2 && K.Key != ConsoleKey.D2
                    && K.Key != ConsoleKey.Subtract && K.Key != ConsoleKey.Add);//  Проверка на правильный ввод
                    if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
                    {
                        return;
                    }
                    if (K.Key == ConsoleKey.OemPlus || K.Key == ConsoleKey.Add)
                    {
                        MyAnswer = "+";
                    }
                    else
                    {
                        MyAnswer = "-";
                    }
                    Console.Clear();
                    PENgraffiti();
                    string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\PEN-" + Login + ".txt";
                    WriteChangeAnswer(ChoiseForChange, MyAnswer, Answer, TxtAnswer);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Вы ещё не ответили на вопрос {0}, что бы его изменять!\n", ChoiseForChange + 1);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Для продолжения нажмите любую клавишу...");
                    Console.ReadKey();
                    return;
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Вы ещё не начали проходить тест!\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для продолжения нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        //Некоторые УНИВЕРСАЛЬНЫЕ МЕТОДЫ
        static void WriteChangeAnswer(int ChoiseForChange, string MyNewAnswer, string[] AnswerArray, string TxtAnswer)//Логин| Вопрос который нужно изменить| Изменёный ответ| Массив имеющихся ответов| Файл куда нужно записать изменённыйответ
        {
            //Вносим изменения в файл
            using (StreamWriter sw = new StreamWriter(TxtAnswer, false, Encoding.Default)) //false - перезаписываем файл!
            {
                for (int i = 0; i < AnswerArray.Length; i++)
                {
                    if (ChoiseForChange == i)
                    {
                        sw.WriteLine(MyNewAnswer);
                    }
                    else
                    {
                        sw.WriteLine(AnswerArray[i]);
                    }
                }
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ваш ответ успешно изменён!\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Для продолжения нажмите любую клавишу...");
            Console.ReadKey();
        }
        static void WriteMyAnswer(string MyAnswer, string TxtAnswer) //Запись ответа
        {
            using (StreamWriter sw = new StreamWriter(TxtAnswer, true, Encoding.Default))
            {
                sw.WriteLine(MyAnswer);
            }
        }
        static int ChoseOneOrTwo()
        {
            ConsoleKeyInfo K;
            bool Mistake = false;
            do
            {
                if (Mistake == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nНажмите на 1 или 2");
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.White;

                Mistake = true;
                K = Console.ReadKey();
            }
            while (K.Key != ConsoleKey.NumPad1 && K.Key != ConsoleKey.D1
            && K.Key != ConsoleKey.NumPad2 && K.Key != ConsoleKey.D2);
            if (K.Key == ConsoleKey.NumPad1 || K.Key == ConsoleKey.D1)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }
        static void ReplaceInFile(string filePath, string searchText, string replaceText)
        {
            StreamReader reader = new StreamReader(filePath, Encoding.Default);
            string content = reader.ReadToEnd();
            reader.Close();
            content = Regex.Replace(content, searchText, replaceText);
            //false -перезапись
            StreamWriter writer = new StreamWriter(filePath, false, Encoding.Default);
            writer.Write(content);
            writer.Close();
        }
        //РЕЗУЛЬТАТЫ
        static void ResultGraffity(string Login)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@" ____                            ___    __             ");
            Console.WriteLine(@"/\  _`\                         /\_ \  /\ \__          ");
            Console.WriteLine(@"\ \ \L\ \     __    ____  __  __\//\ \ \ \ ,_\   ____  ");
            Console.WriteLine(@" \ \ ,  /   /'__`\ /',__\/\ \/\ \ \ \ \ \ \ \/  /',__\ ");
            Console.WriteLine(@"  \ \ \\ \ /\  __//\__, `\ \ \_\ \ \_\ \_\ \ \_/\__, `\");
            Console.WriteLine(@"   \ \_\ \_\ \____\/\____/\ \____/ /\____\\ \__\/\____/");
            Console.WriteLine(@"    \/_/\/ /\/____/\/___/  \/___/  \/____/ \/__/\/___/ ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nРезультаты пользователя - ");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(Login);
            Console.ResetColor();
        }
        static void GetResult(string Login)
        {
            ResultGraffity(Login);
            string ResultMenu;
            while (true)
            {
                Console.Clear();
                ResultMenu = ResMenu(Login);
                switch (ResultMenu)
                {
                    case "1":
                        {
                            GetResultGolomstock(Login);
                            break;
                        }
                    case "2":
                        {
                            GetResultBelov(Login);
                            break;
                        }
                    case "3":
                        {
                            GetResultPEN(Login);
                            break;
                        }
                    case "4"://возврат назад
                        {
                            return;
                        }
                }
            }
        }
        static string ResMenu(string Login)
        {
            ResultGraffity(Login);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Выберите пункт меню: ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. Результаты  - <Карта интересов>");
            Console.WriteLine("2. Результаты  - <Тест на темперамент>");
            Console.WriteLine("3. Результаты  - <Опросник PEN>");
            Console.WriteLine("4. Назад");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo K = Console.ReadKey();
            if (K.Key == ConsoleKey.NumPad1 || K.Key == ConsoleKey.D1)
            {
                return "1";
            }
            if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
            {
                return "2";
            }
            if (K.Key == ConsoleKey.NumPad3 || K.Key == ConsoleKey.D3)
            {
                return "3";
            }
            if (K.Key == ConsoleKey.NumPad4 || K.Key == ConsoleKey.D4)
            {
                return "4";
            }
            else
            {
                return null;
            }
        }
        static void GetResultGolomstock(string Login)//Мат.расчёт результатов для Карты Интересов
        {
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Golomstock-" + Login + ".txt";
            try
            {
                string[] Answers = File.ReadAllLines(TxtAnswer, Encoding.Default);
                if (File.Exists(TxtAnswer) && Answers.Length == 174)
                {
                    //Конвертация ответов в числа
                    int[] NumericAnswers = new int[174];
                    for (int n = 0; n < 174; n++)
                    {
                        switch (Answers[n])
                        {
                            case "++":
                                {
                                    NumericAnswers[n] = 2;
                                    break;
                                }
                            case "+":
                                {
                                    NumericAnswers[n] = 1;
                                    break;
                                }
                            case "0":
                                {
                                    NumericAnswers[n] = 0;
                                    break;
                                }
                            case "--":
                                {
                                    NumericAnswers[n] = -2;
                                    break;
                                }
                            case "-":
                                {
                                    NumericAnswers[n] = -1;
                                    break;
                                }
                        }
                    }
                    int[] SortedNumericAnswers = new int[29]; //всего 174 ответа
                    for (int i = 0; i < 29; i++)//29 обработаных вариантов - обобщённых
                    {
                        SortedNumericAnswers[i] += (NumericAnswers[i] +
                     NumericAnswers[i + 29] + NumericAnswers[i + 58] +
                     NumericAnswers[i + 87] + NumericAnswers[i + 116] +
                     NumericAnswers[i + 145]);
                    }

                    string[] NameOfAnswer = new string[29]
                    {  "Биология","География","Геология",
                "Медицина", "Легкая и пищевая промышленность",  "Физика",
                "Химия","Техника","Электро и радиотехника",
                "Металлообработка",
                "Деревообработка",
                "Строительство",
                "Транспорт",
                "Авиация морское_дело",
                "Военные специальности",
                "История",
                "Литература",
                "Журналистика",
                "Общественная деятельность",
                "Педагогика",
                "Юриспруденция",
                "Сфера обслуживания",
                "Математика",
                "Экономика",
                "Иностранные языки",
                "Изобразительное искусство",
                "Сценическое искусство",
                "Музыка",
                "Физкультура и спорт"
                };
                    Array.Sort(SortedNumericAnswers, NameOfAnswer);
                    Array.Reverse(SortedNumericAnswers);//Разворот массивов для вывода от наибольшего к меньшему
                    Array.Reverse(NameOfAnswer);
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nВ данном списке приведены все виды профессиональной деятельности");
                    Console.WriteLine("Числа обозначают степень выраженности:");
                    Console.WriteLine("от -12 до -6 - высшая степень отрицания данного интереса \nот -5 до -1 - интерес отрицается \nот +1 до +4 - интерес выражен слабо \nот +5 до +7 - выраженный интерес \nот +8 до +12 - ярко выраженный интерес.\n\n");
                    for (int k = 0; k < 29; k++)
                    {
                        if (SortedNumericAnswers[k] > 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(NameOfAnswer[k] + ": " + SortedNumericAnswers[k]);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine(NameOfAnswer[k] + ": " + SortedNumericAnswers[k]);
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы ещё не до конца прошли тест <Карта Интересов>!");
                }
            }
            catch
            {
                Console.Clear();
                ResultGraffity(Login);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nДля получения данных результатов, пройдите тест <Карта Интересов>!");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n\nНажмите любую клавишу, что бы вернуться назад...");
            Console.ReadKey();
        }
        static void GetResultBelov(string Login)//Мат.расчёт результатов для Теста на темперамент
        {
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\Belov-" + Login + ".txt";
            int AnswersCount = 0;
            try
            {
                //try нужен для того, что бы исключить вероятность отсутствия файла
                string[] Answers = File.ReadAllLines(TxtAnswer, Encoding.Default);
                AnswersCount = Answers.Length;
                if (File.Exists(TxtAnswer) && Answers.Length == 80)
                {

                    double AllCountPlus = 0;
                    double Plus1 = 0;
                    double Plus2 = 0;
                    double Plus3 = 0;
                    double Plus4 = 0;

                    for (int n = 0; n < Answers.Length; n++)
                    {
                        if (Answers[n] == "+")
                        {
                            AllCountPlus++; //подсчёт всех плюсов
                        }

                        if (Answers[n] == "+" && n < 20)
                        {
                            Plus1++; //плюсы в 1 двадцатке
                        }
                        if (Answers[n] == "+" && n >= 20 && n < 40)
                        {
                            Plus2++; //во 2
                        }
                        if (Answers[n] == "+" && n >= 40 && n < 60)
                        {
                            Plus3++; //в 3
                        }
                        if (Answers[n] == "+" && n >= 60)
                        {
                            Plus4++;//в 4
                        }
                    }
                    int Flegmatik = Convert.ToInt32((Plus1 / AllCountPlus) * 100);
                    int Melanholic = Convert.ToInt32((Plus2 / AllCountPlus) * 100);
                    int Holeric = Convert.ToInt32((Plus3 / AllCountPlus) * 100);
                    int Sangvinic = Convert.ToInt32((Plus4 / AllCountPlus) * 100);
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nВаш темперамент:");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("-на {0}% флегматический,", Flegmatik);
                    Console.WriteLine("-на {0}% меланхолический,", Melanholic);
                    Console.WriteLine("-на {0}% холерический,", Holeric);
                    Console.WriteLine("-на {0}% сангвинический.", Sangvinic);
                }
                else
                {
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы ещё не до конца прошли тест на темперамент !");
                }
            }
            catch
            {
                Console.Clear();
                ResultGraffity(Login);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nДля получения данных результатов, пройдите тест на темперамент!");
                Console.WriteLine("(Если тест пройден, то возможно ни в одном ответе нет плюса [+])");

            }
            if (AnswersCount == 80)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nВведите:");
                Console.WriteLine("1 - Вернуться назад");
                Console.WriteLine("2 - Получить интерпретацию к полученным значениям");
                int Choise = ChoseOneOrTwo(); ;
                switch (Choise)
                {
                    case 1:
                        {
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            ResultGraffity(Login);
                            string[] Inter = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\BelovInt.txt", Encoding.Default);
                            Console.ForegroundColor = ConsoleColor.White;
                            for (int i = 0; i < Inter.Length; i++)
                            {
                                Console.WriteLine(Inter[i]);
                            }
                            Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }

        }
        static void GetResultPEN(string Login)//Мат.подсчёт результатов для опросника PEN
        {
            string TxtAnswer = AppDomain.CurrentDomain.BaseDirectory + @"\Answer\PEN-" + Login + ".txt";
            int AnswersCount = 0;
            try
            {
                //try нужен для того, что бы исключить вероятность отсутствия файла
                string[] Answers = File.ReadAllLines(TxtAnswer, Encoding.Default);
                AnswersCount = Answers.Length;
                if (File.Exists(TxtAnswer) && Answers.Length == 101)
                {
                    int Psihotizm = 0;
                    int EkstroversIntrovers = 0;
                    int Neitrotizm = 0;
                    int Iskrennost = 0;
                    for (int i = 0; i < Answers.Length; i++)
                    {
                        //все пункты номера ответы уменьшенны на -1, в виду особенности начала индекса в массиве
                        if (Answers[i] == "-" && (i == 1 || i == 5 || i == 8 || i == 10 || i == 18 || i == 38 || i == 42 || i == 58 || i == 62 || i == 66 || i == 77 || i == 99))
                        {
                            Psihotizm++;
                        }
                        if (Answers[i] == "+" && (i == 13 || i == 22 || i == 26 || i == 30 || i == 34 || i == 46 || i == 50 || i == 54 || i == 70 || i == 84 || i == 87 || i == 92 || i == 96))
                        {
                            Psihotizm++;
                        }
                        if (Answers[i] == "-" && (i == 21 || i == 29 || i == 45 || i == 83))
                        {
                            EkstroversIntrovers++;
                        }
                        if (Answers[i] == "+" && (i == 0 || i == 4 || i == 9 || i == 14 || i == 17 || i == 25 || i == 33 || i == 37 || i == 41 || i == 49 || i == 53 || i == 57 || i == 61 || i == 65 || i == 69 || i == 73 || i == 76 || i == 80 || i == 89 || i == 91 || i == 95))
                        {
                            EkstroversIntrovers++;
                        }
                        if (Answers[i] == "+" && (i == 2 || i == 6 || i == 11 || i == 15 || i == 1 || i == 23 || i == 27 || i == 31 || i == 35 || i == 3 || i == 43 || i == 47 || i == 51 || i == 55 || i == 71 || i == 74 || i == 78 || i == 82 || i == 88 || i == 94 || i == 97))
                        {
                            Neitrotizm++;
                        }
                        if (Answers[i] == "-" && (i == 3 || i == 7 || i == 16 || i == 24 || i == 28 || i == 40 || i == 44 || i == 48 || i == 52 || i == 56 || i == 64 || i == 68 || i == 75 || i == 79 || i == 81 || i == 90 || i == 94))
                        {
                            Iskrennost++;
                        }
                        if (Answers[i] == "+" && (i == 12 || i == 20 || i == 32 || i == 36 || i == 60 || i == 72 || i == 86 || i == 98))
                        {
                            Iskrennost++;
                        }
                    }
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\nВаши результаты следующие:\n");
                    if (Iskrennost > 10)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("***Результаты обследования недостоверные***");
                        Console.WriteLine("Вам следует отвечать на вопросы более откровенно");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("***Результаты обследования достоверные***");
                    }
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (EkstroversIntrovers > 12)
                    { Console.WriteLine("-Вы экстраверт"); }
                    else
                    { Console.WriteLine("-Вы интроверт"); }
                    if (Neitrotizm > 16)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-У вас высокая психическая неустойчивость");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-Ваша психическая устойчивость в норме");
                    }
                    if (Psihotizm > 12)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("-Присутствует высокая конфликтность");
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("-В большинстве случаев вы не конфликтный человек");
                    }
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n\nЧисловые значения:");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n-Искренность: [{0}] из [25] - чем выше показатель, тем больше лжи в ваших ответах.", Iskrennost);
                    Console.WriteLine("-Экстраверсия – Интроверсия: [{0}] из [25] - Высокие оценки соответствуют экстравертированному типу,", EkstroversIntrovers);
                    Console.WriteLine("низкие – интровертированному.");
                    Console.WriteLine("-Нейтротизм: [{0}] из [21] - Высокие показатели говорят о высокой психической неустойчивости.", Neitrotizm);
                    Console.WriteLine("-Психотизм: [{0}] из [25] -Высокие оценки указывают на высокую конфликтность.", Psihotizm);
                }
                else
                {
                    Console.Clear();
                    ResultGraffity(Login);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nВы ещё не до конца прошли опросник PEN !");
                }
            }
            catch
            {
                Console.Clear();
                ResultGraffity(Login);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nДля получения данных результатов, пройдите опросник PEN!");
            }

            if (AnswersCount == 101)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nВведите:");
                Console.WriteLine("1 - Вернуться назад");
                Console.WriteLine("2 - Получить интерпретацию к полученным значениям");
                int Choise = ChoseOneOrTwo(); ;
                switch (Choise)
                {
                    case 1:
                        {
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            ResultGraffity(Login);
                            string[] Inter = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\PENInt.txt", Encoding.Default);
                            Console.ForegroundColor = ConsoleColor.White;
                            for (int i = 0; i < Inter.Length; i++)
                            {
                                Console.WriteLine(Inter[i]);
                            }
                            Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                            Console.ReadKey();
                            break;
                        }
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }

        //СМЕНА ПАРОЛЯ
        static void ChangePasswordGraffity()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                   ******************");
            Console.WriteLine("                   ***СМЕНА ПАРОЛЯ***");
            Console.WriteLine("                   ******************\n");
            Console.ResetColor();
        }
        static void ChangePassword(string Login, string Password)
        {
            Console.Clear();
            PSYTESTgraffiti();
            ChangePasswordGraffity();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Для начала введите свой старый пароль:");
            Console.ForegroundColor = ConsoleColor.White;
            bool Mistake = false;
            string CheckPass = null;
            do
            {
                if (Mistake == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Пароли не совпадают, попробуйте ещё раз:");
                    Console.WriteLine("Для выхода - введите [2]");
                }
                Console.ForegroundColor = ConsoleColor.White;
                CheckPass = Console.ReadLine();
                if (CheckPass == "2")
                { return; }
                Mistake = true;
            }
            while (CheckPass != Password);
            Console.Clear();
            PSYTESTgraffiti();
            ChangePasswordGraffity();
            //ПАРОЛЬ
            Int64 MyPassword = 0;
            bool toocheck = false;
            do
            {
                if (toocheck == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильный ввод! Вводите только цифры (Минимум 2, Максимум 19)\n");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("ВАЖНО! Новый пароль не должен повторять старый");
                Console.Write("\nПридумайте новый пароль (Пароль может состоять только из чисел!):");
                Console.ForegroundColor = ConsoleColor.White;
                toocheck = true;
                try
                {
                    MyPassword = Convert.ToInt64(Console.ReadLine());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Неправильный ввод! Вводите только цифры(Минимум 2, Максимум 19)\n");
                    toocheck = false;
                }

            }
            while (MyPassword < 2 || MyPassword < 0 || Convert.ToInt32(CheckPass) == MyPassword);
            string LoginTXT = AppDomain.CurrentDomain.BaseDirectory + "\\Password\\" + Login + ".dat"; //Сборка полного пути 
            File.Delete(LoginTXT);
            //Записываем пароль бинарным способом
            FileStream fsW = new FileStream(LoginTXT, FileMode.Create, FileAccess.Write);
            BinaryWriter bw = new BinaryWriter(fsW);
            bw.Write(MyPassword);
            bw.Close();
            fsW.Close();
            Console.Clear();
            PSYTESTgraffiti();
            ChangePasswordGraffity();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Пароль успешно изменён!");
            Console.ResetColor();
        }
        //***Пункты с особым паролем
        static bool SpecialPassword()
        {
            bool Mistake = false;
            string CheckPass = null;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Введите особый пароль:");
            Console.ForegroundColor = ConsoleColor.White;
            do
            {
                if (Mistake == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.WriteLine("Пароли не совпадают, попробуйте ещё раз:");
                    Console.WriteLine("Для выхода - введите [2]");
                }
                Console.ForegroundColor = ConsoleColor.White;
                CheckPass = Console.ReadLine();
                if (CheckPass == "2")
                { return false; }
                Mistake = true;
            }
            while (CheckPass != SPECIALPassword);
            return true;
        }

        //Информация о всех пользователях
        static void AllInfoAboutLoginAndPassword()
        {
            Console.Clear();
            PSYTESTgraffiti();
            if (SpecialPassword() == true)
            {
                DirectoryInfo dir = new DirectoryInfo("Password");
                FileInfo[] UsersPassword = dir.GetFiles("*.dat");
                Access[] AllInfo = new Access[UsersPassword.Length];
                Console.Clear();
                PSYTESTgraffiti();
                for (int i = 0; i < AllInfo.Length; i++)
                {
                    AllInfo[i].Login = Path.GetFileNameWithoutExtension(UsersPassword[i].FullName);
                    // создаем объект BinaryReader
                    using (BinaryReader reader = new BinaryReader(File.Open(UsersPassword[i].FullName, FileMode.Open)))
                    {
                        AllInfo[i].Password = Convert.ToString(reader.ReadInt64());
                    }
                }
                TableResult(AllInfo);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Для выхода нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        static void TableResult(Access[] AllInfo)
        {

            Console.ForegroundColor = ConsoleColor.Yellow;
            char lu = '\x2553'; char ld = '\x2556';
            string ru = "\x2559"; char rd = '\x255C';
            char h = '\x2500'; string v = "\x2551";
            string s1 = string.Concat(lu, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, ld);
            Console.WriteLine(s1);
            string s2 = string.Concat(v, "          Логин             ", v, "         Пароль         ", v);
            string s5 = string.Concat(v, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, v, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, h, v);
            Console.WriteLine("{0}\n{1}", s2, s5);
            for (int i = 0; i < AllInfo.Length; i++)
            {
                Console.WriteLine(v + "{0,25}   \x2551{1,25}", AllInfo[i].Login, AllInfo[i].Password + "\x2551");
                if (i == AllInfo.Length - 1)
                { break; }
                Console.WriteLine(v + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + v + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + v);
            }
            Console.WriteLine(ru + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + h + rd);
            Console.ResetColor();
        }

        //Изменение текста в вопросе
        static void ChangeTextQuestion()
        {
            Console.Clear();
            PSYTESTgraffiti();
            if (SpecialPassword() == true)
            {
                string ResultMenu;
                while (true)
                {
                    Console.Clear();
                    PSYTESTgraffiti();
                    ResultMenu = ChangeTextMenu();
                    switch (ResultMenu)
                    {
                        case "1":
                            {
                                MainChanger("Golomstock", 174);
                                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                break;
                            }
                        case "2":
                            {
                                MainChanger("Belov", 80);
                                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                break;
                            }
                        case "3":
                            {
                                MainChanger("PEN", 101);
                                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                                Console.ReadKey();
                                break;
                            }
                        case "4"://возврат назад
                            {
                                return;
                            }
                    }
                }
            }
        }
        static string ChangeTextMenu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nВыберите один из тестов, в котором следует изменить вопрос: ");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("1. <Карта интересов>");
            Console.WriteLine("2. <Тест на темперамент>");
            Console.WriteLine("3. <Опросник PEN>");
            Console.WriteLine("4. Назад");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("________________________________________________________________________________");
            Console.ForegroundColor = ConsoleColor.White;
            ConsoleKeyInfo K = Console.ReadKey();
            if (K.Key == ConsoleKey.NumPad1 || K.Key == ConsoleKey.D1)
            {
                return "1";
            }
            if (K.Key == ConsoleKey.NumPad2 || K.Key == ConsoleKey.D2)
            {
                return "2";
            }
            if (K.Key == ConsoleKey.NumPad3 || K.Key == ConsoleKey.D3)
            {
                return "3";
            }
            if (K.Key == ConsoleKey.NumPad4 || K.Key == ConsoleKey.D4)
            {
                return "4";
            }
            else
            {
                return null;
            }
        }
        static int ChangerChoise(int max)
        {
            int ChoiseForChange = 0;
            bool Mistake = false;
            do
            {
                if (Mistake == true)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введите номер вопроса от 1 до {0}", max);
                    Console.ResetColor();
                }
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nВаш выбор:");
                Mistake = true;
                try
                {
                    ChoiseForChange = Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Введите номер вопроса от 1 до {0}", max);
                    Mistake = false;
                    Console.ResetColor();
                }
            } while (ChoiseForChange < 1 || ChoiseForChange > max);
            Console.ResetColor();
            return ChoiseForChange - 1;//-1 ввиду особенности начала индексов в массиве
        }
        static void MainChanger(string Name, int max)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\TestQuestions\" + Name + ".txt";
            Console.Clear();
            PSYTESTgraffiti();
            string[] Questions = File.ReadAllLines(path, Encoding.Default);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nВведите номер вопроса, который следует изменить");
            //передаём методу максимальное количество вопросов
            int ChoiseForChange = ChangerChoise(max);
            Console.Clear();
            PSYTESTgraffiti();
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nИсходный текст:");
            Console.WriteLine(Questions[ChoiseForChange]);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("\nВведите изменённый текст и нажмите Enter(Номер вопроса в начале писать не нужно!)");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("(Для выхода введите 2):");
            Console.ForegroundColor = ConsoleColor.White;
            string YourText = Console.ReadLine();
            if (YourText == "2")
            { return; }
            string WriteText = (ChoiseForChange + 1) + ". " + Console.ReadLine();
            ReplaceInFile(path, Questions[ChoiseForChange], WriteText);
            //Завершение
            Console.Clear();
            PSYTESTgraffiti();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n\nВопрос успешно изменён!");
            Console.ForegroundColor = ConsoleColor.White;
        }
        //Узнать результаты других пользователей
        static void LearnSomeoneAnswers()
        {
            DirectoryInfo dir = new DirectoryInfo("Password");
            FileInfo[] UsersPassword = dir.GetFiles("*.dat");
            List<string> InfoLogin = new List<string>();
            Console.Clear();
            PSYTESTgraffiti();
            if (SpecialPassword() == true)
            {
                Console.Clear();
                PSYTESTgraffiti();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nСписок пользователей:");
                Console.WriteLine("____________________________________________________");
                Console.ForegroundColor = ConsoleColor.Yellow;
                for (int i = 0; i < UsersPassword.Length; i++)
                {
                    InfoLogin.Add(Path.GetFileNameWithoutExtension(UsersPassword[i].FullName));
                    Console.WriteLine((i + 1) + ") " + InfoLogin[i]);
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("____________________________________________________");
                Console.WriteLine("\nВведите номер нужного вам пользователя, что бы узнать его результаты:");
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("(Вы войдёте в меню результатов под его Логином)");
                int ChoiseForChange = 0;
                bool Mistake = false;
                do
                {
                    if (Mistake == true)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Введите номер пользователя от 1 до {0}", InfoLogin.Count);
                        Console.ResetColor();
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("\nВаш выбор:");
                    Mistake = true;
                    try
                    {
                        ChoiseForChange = Convert.ToInt32(Console.ReadLine());
                    }
                    catch
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Введите номер пользователя от 1 до {0}", InfoLogin.Count);
                        Mistake = false;
                        Console.ResetColor();
                    }
                } while (ChoiseForChange < 1 || ChoiseForChange > InfoLogin.Count);
                Console.ResetColor();
                string NeedUser = InfoLogin[ChoiseForChange - 1];
                GetResult(NeedUser);
            }
        }
    }
}