using System.IO;
using System.Text;

namespace titkosito
{
    internal class Program
    {
        //Nem túl szép! Kerülni kell a globális hatókörű változók használatát!!!
        static int kodEltolasErteke; //eltolás mértéke
        static void Main(string[] args)
        {

            string szovegSor = "asd", fajlNev;
            List<string> titkositandoLista = new();

            while (true)
            {
                ConsoleKeyInfo mitValasztott = ValasztasMenubol();
                switch (mitValasztott.Key)
                {
                    case ConsoleKey.F1:
                        Console.Clear();
                        Console.WriteLine("TITKOSÍTÁS\n\n");


                        do
                        {
                            Console.Write("\nKérem a titkosítandó sort! :");
                            szovegSor = Console.ReadLine();
                            if (szovegSor != "")
                            {
                               titkositandoLista.Add(szovegSor);
                            }
                            else
                            {
                                break;
                            }
                        }
                        while (szovegSor != "");



                        Console.Write("Kérem a titkosításhoz az eltolás mértékét! :");
                        kodEltolasErteke = int.Parse(Console.ReadLine());

                        //try
                        //{
                        //    int.Parse(Console.ReadLine());
                        //}
                        //catch (FormatException)
                        //{
                        //    Console.WriteLine("Kérem számot adjon meg!");
                        //}


                        Console.Write("Kérem a fájl nevét! (Kiterjesztés nem kell, mivel .TXT lesz!) :");
                        string fajlNeve = Console.ReadLine();


                        FajlbaIr(fajlNeve + ".txt", Titkositott(titkositandoLista));
                        Console.WriteLine("\nSikeres írás!\nNyomjon le egy billentyűt a menübe történő visszatéréshez!");
                        titkositandoLista.Clear();
                        while (!Console.KeyAvailable) ;  //Várakozik billentyűre
                        //kiolvassuk a lenyomott billentyű kódját, de nem csinálunk vele semmit! Vajon miért tesszük ezt?
                        Console.ReadKey();
                        break;

                    case ConsoleKey.F2:
                        int sorSum = 0;
                        string leghoszzabbSor = "";
                        string legtobbSpaceSor = "";
                        Console.Write("Kérem a kódolt fájl nevét! (Kiterjesztés nem kell, mivel .TXT lesz!) :");
                        fajlNev = Console.ReadLine();

                        try
                        {
                            FajlbolOlvas(fajlNev + ".txt");
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("\nIlyen nevü fájl nem létezik vagy nem található!\nNyomjon le egy billentyűt a menübe történő visszatéréshez!");
                            titkositandoLista.Clear();
                            while (!Console.KeyAvailable);            
                            Console.ReadKey();
                            break;
                        }

                        List<string> dekodoltList = Visszafejt(FajlbolOlvas(fajlNev + ".txt"));

                        foreach (string sor in dekodoltList)
                        {
                            Console.WriteLine(sor);
                            sorSum++;

                            if (sor.Length > leghoszzabbSor.Length)
                            {
                                leghoszzabbSor = sor;
                            }

                            if (SpaceCounter(sor) > SpaceCounter(legtobbSpaceSor))
                            {
                                legtobbSpaceSor = sor;
                            }
                        }


                        dekodoltList.Clear();

                        Console.WriteLine($"\nA fájlban {sorSum} sor van.");
                        Console.WriteLine($"\nA fájl leghosszab sora a <{leghoszzabbSor}> sor");
                        if (legtobbSpaceSor != "")
                        {
                            Console.WriteLine($"\nA fájl legtöbb szóközt tartalmazó sora a <{legtobbSpaceSor}> sor");
                        }
                        else
                        {
                            Console.WriteLine($"\nA fájl nem tartalmaz szóközt.");
                        }

                        Console.WriteLine("\nNyomjon le egy billentyűt a menübe történő visszatéréshez!");
                        while (!Console.KeyAvailable) ;  //Várakozik billentyűre
                        Console.ReadKey();
                        break;

                    case ConsoleKey.Escape:
                        Console.Clear();
                        return;
                }
            }
        }

        private static List<string> FajlbolOlvas(string fajlNev)
        {
            List<string> sorLista = new List<string>();
            StreamReader sr = new StreamReader(fajlNev);
            try
            {
                kodEltolasErteke = int.Parse(sr.ReadLine());
            }
            catch (FormatException)
            {
                Console.WriteLine("\nA fájl első sora nem szám!");
                return sorLista;
            }

            while (!sr.EndOfStream)
            {
                string sor = sr.ReadLine();
                sorLista.Add(sor);
            }
            sr.Close();
            return sorLista;
        }


        /// <summary>
        /// A megadott állományba írja az átalakított szöveget 
        /// </summary>
        /// <param name="fileName">Elérési útvonal és a fájl neve</param>
        /// <param name="textLine">A fájlba írandó szöveg</param>
        private static void FajlbaIr(string fileName, List<string> irandoList)  //módosítani a paramétert pld string listára
        {
            StreamWriter sw = new StreamWriter(fileName);
            sw.WriteLine(kodEltolasErteke);
            foreach (string sor in irandoList)
            {
                sw.WriteLine(sor);
            }
            sw.Close();
        }

        private static ConsoleKeyInfo ValasztasMenubol()
        {
            Console.Clear();
            Console.WriteLine($"[F1] Titkosít");
            Console.WriteLine($"[F2] Visszafejt");
            Console.WriteLine($"[ESC] Kilépés a programból");
            ConsoleKeyInfo karakter;
            do
            {
                Console.WriteLine("\nVálasszon !");
                karakter = Console.ReadKey();

            } while (karakter.Key != ConsoleKey.F1 && karakter.Key != ConsoleKey.F2 && karakter.Key != ConsoleKey.Escape);
            return karakter;
        }

        private static List<string> Titkositott(List<string> sorList)
        {
            List<string> kodoltLista = new List<string>();
            StringBuilder kodolt = new StringBuilder();

            foreach (string sor in sorList)
            {
                foreach (char jel in sor)
                {
                    int jelUjKodja = (byte)jel + kodEltolasErteke;
                    kodolt.Append((char)jelUjKodja);
                }

                kodoltLista.Add(kodolt.ToString());
                kodolt.Clear();
            }

            return kodoltLista;
        }

        private static List<string> Visszafejt(List<string> kodoltList)
        {
            List<string> dekodoltList = new List<string>();
            StringBuilder visszafejtett = new StringBuilder();
            foreach (string szoveg in kodoltList)
            {
                foreach (char jel in szoveg)
                {
                    int jelUjKodja = (byte)jel - kodEltolasErteke;
                    visszafejtett.Append((char)jelUjKodja);
                }

                dekodoltList.Add(visszafejtett.ToString());
                visszafejtett.Clear();
            }
            return dekodoltList;
        }

        private static int SpaceCounter(string sor)
        {
            int spaceCount = 0;
            foreach (char c in sor)
            {
                if (c == ' ')
                {
                    spaceCount++;
                }
            }
            return spaceCount;
        }
    }
}