using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LINQ_Lambda_DatabaseExpressions
{
    public class Oefening
    {
        public int MaxLineLength { get; set; } = Console.WindowWidth - 20;
        public Action Methode { get; set; }
        public string Titel { get; set; }
        public string Opdracht { get; set; }
        public string Query { get; set; }
        public string[] Hoofdstuk { get; set; }
        public Oefening(Action methode, string opdracht, string query) : this(methode)
        {
            Titel = methode.Method.Name;
            Methode = methode;
            Opdracht = "Opdracht:\t" +opdracht;
            Query = "Query:\t\t" +query;
            createHoofdstuk();
        }
        public Oefening(Action methode, string opdracht) : this(methode)
        {
            Titel = methode.Method.Name;
            Methode = methode;
            Opdracht = "Opdracht:\t" + opdracht;
            createHoofdstuk();
        }
        public Oefening(Action methode)
        {
            Titel = methode.Method.Name;
            Methode = methode;
            createHoofdstuk();
        }
        public void StartOefening()
        {
            Console.Clear();
            Print(Titel);
            Print(Opdracht,1,MaxLineLength);
            Print(Query,1, MaxLineLength);
            Methode();
            Console.ReadKey();
        }
        public string[] Print(string ToPrint, int iLowerSpace = 0, int iLineLength = 100, bool bPrint = true)
        {
            int aantalLijnen = (ToPrint.Length / iLineLength) +1;
            string[] splitted;
            if (aantalLijnen > 1)
            {
                splitted = new string[aantalLijnen];
                string ToSplit = ToPrint;
                int iToSplit;
                for (int i = 0; i < aantalLijnen; i++)
                {
                    if (ToSplit.Length > iLineLength)
                    {
                        iToSplit = ToSplit.IndexOf(' ', iLineLength - 5);
                        if (i>0)
                            splitted[i] = "\t\t" + ToSplit.Substring(0, iToSplit + 1);
                        else
                            splitted[i] = ToSplit.Substring(0, iToSplit+1);
                        ToSplit = ToSplit.Substring(iToSplit+1);
                    }
                    else
                        splitted[i] = ToSplit;
                }
            }
            else
            {
                splitted = new string[1] { ToPrint };
            }
            if (bPrint)
            {
                foreach (var item in splitted)
                {
                    Console.WriteLine(item);
                }
                if (iLowerSpace > 0)
                    for (int i = 0; i < iLowerSpace; i++)
                    {
                        Console.WriteLine();
                    }
            }
            return splitted;
        }
        public void PrintOpdracht()
        {
            Print(Opdracht,0,50);
        }
        public void PrintQuery()
        {

        }
        private void createHoofdstuk(string voorvoegsel = "oefening", char tussenVoegsel = '_')
        {
            if (Titel != null)
            {
                string[]hoofdstuk = Titel.Substring(voorvoegsel.Length).Trim().Split(tussenVoegsel);
                if (hoofdstuk[0] != "")
                    Hoofdstuk = hoofdstuk;
            }
        }
    }
    
    class Program
    {
        #region Lists
        public static List<People> people = new List<People>();
        public static List<Hobbies> hobbies = new List<Hobbies>();
        public static List<PeopleHobbies> peopleHobbies = new List<PeopleHobbies>();
        public static List<Orders> orders = new List<Orders>();
        public static List<Statuses> statuses = new List<Statuses>();
        public static List<Roles> roles = new List<Roles>();
        public static List<Oefening> oefeningen = new List<Oefening>();
        #endregion
        static void Main(string[] args)
        {
            CreateDb();
            CreateOpdrachten();
            StartOefeningen();

        }

        static void StartOefeningen()
        {
            string huidigeHoofdstuk = "1";
            string[] mijnMenu = menuHoofdstuk(huidigeHoofdstuk);
            int keuze = 1;
            int iOefeningen = 0;
            while (keuze != mijnMenu.GetUpperBound(0) + 1)
            {
                bool menuChanged = false;
                Console.Clear();
                keuze = SelectMenu(multiLineLeftSpace: "Opdracht: ".Length, titel: "SQL " + huidigeHoofdstuk, clearScreen: false, select:keuze, menu: mijnMenu);
                if ((keuze == mijnMenu.GetLowerBound(0) + 1) && (huidigeHoofdstuk != zoekHoofdstuk(bLaatste: false)))
                {
                    huidigeHoofdstuk = zoekHoofdstuk(huidigeHoofdstuk, false);
                    mijnMenu = menuHoofdstuk(huidigeHoofdstuk);
                    switch (huidigeHoofdstuk)
                    {
                        case "2": iOefeningen -= 9; break;
                        case "3": iOefeningen -= 13; break;
                    }
                    menuChanged = true;
                }
                if ((keuze == mijnMenu.GetUpperBound(0)) && (huidigeHoofdstuk != zoekHoofdstuk()))
                {
                    huidigeHoofdstuk = zoekHoofdstuk(huidigeHoofdstuk);
                    mijnMenu = menuHoofdstuk(huidigeHoofdstuk);
                    switch (huidigeHoofdstuk)
                    {
                        case "2": iOefeningen += 9; break;
                        case "3": iOefeningen += 13; break;
                    }
                    menuChanged = true;
                }
                if ((!menuChanged) && (keuze != mijnMenu.GetUpperBound(0) + 1))
                {
                    if (huidigeHoofdstuk == zoekHoofdstuk()) 
                        keuze--;
                    oefeningen[iOefeningen + keuze - 1].StartOefening(); 
                }
            }
            string[] menuHoofdstuk(string hoofdstuk, int maxLength = 0)
            {
                List<string> menu = new List<string>();
                if (hoofdstuk != zoekHoofdstuk(bLaatste: false)) menu.Add("<vorige hoofdstuk>");
                foreach (Oefening oefening in oefeningen)
                {
                    if (oefening.Hoofdstuk[0] == hoofdstuk)
                    {
                        if (maxLength != 0)
                            menu.Add((oefening.Opdracht.Length > maxLength) ? oefening.Opdracht.Substring(0, maxLength) : oefening.Opdracht);
                        else
                            menu.Add(oefening.Opdracht);
                    }
                }
                if (hoofdstuk != zoekHoofdstuk()) menu.Add("<volgende hoofdstuk>");
                menu.Add("<Exit>");
                return menu.ToArray();
            }
            string zoekHoofdstuk(string hfdstuk = "0", bool bVolgendeHfdstuk = true, bool bLaatste = true)
            {
                if (hfdstuk != "0")
                {
                    bool hfdstukGevonden = false;
                    string vorigeHfdstuk = "";
                    foreach (Oefening oefening in oefeningen)
                    {
                        if (oefening.Hoofdstuk[0] != hfdstuk) vorigeHfdstuk = oefening.Hoofdstuk[0];
                        if (oefening.Hoofdstuk[0] == hfdstuk)
                        {
                            hfdstukGevonden = true;
                            if (!bVolgendeHfdstuk)
                                return vorigeHfdstuk;
                        }
                        else
                            if ((hfdstukGevonden) && (oefening.Hoofdstuk[0] != hfdstuk))
                            return oefening.Hoofdstuk[0];
                    }
                }
                else
                    if (bLaatste)
                    return oefeningen[oefeningen.Count - 1].Hoofdstuk[0];
                else
                    return oefeningen[0].Hoofdstuk[0];
                return "-1";
            }
            int SelectMenu(int multiLineLeftSpace = -1, string titel = "", bool clearScreen = true, int select = 1, params string[] menu)
            {
                int selection = select;
                int cursTop = Console.CursorTop;
                int cursLeft = Console.CursorLeft;
                bool selected = false;
                ConsoleColor selectionForeground = Console.BackgroundColor;
                ConsoleColor selectionBackground = Console.ForegroundColor;
                int extraLine = 0;

                if (clearScreen)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Clear();
                }
                else
                {
                    Console.SetCursorPosition(cursLeft, cursTop);
                }
                if (titel != "")
                {
                    Console.WriteLine(titel);
                    cursTop++;
                }
                Console.CursorVisible = false;
                while (!selected)
                {
                    extraLine = 0;
                    for (int i = 0; i < menu.Length; i++)
                    {
                        if (selection == i + 1)
                        {
                            Console.ForegroundColor = selectionForeground;
                            Console.BackgroundColor = selectionBackground;
                        }
                        if (multiLineLeftSpace != -1)
                        {
                            string leftSpace = "";
                            for (int k = 0; k < multiLineLeftSpace; k++)
                            {
                                leftSpace += ' ';
                            }
                            if ((cursLeft + menu[i].Length + 5) > Console.WindowWidth)
                            {
                                string[] splittedMenu = SplitString(menu[i], Console.WindowWidth - cursLeft - 8 - multiLineLeftSpace);
                                var temp = splittedMenu.GetUpperBound(0);
                                for (int k = 0; k < splittedMenu.GetUpperBound(0) + 1; k++)
                                {
                                    if (k == 0)
                                    {
                                        Console.SetCursorPosition(cursLeft, cursTop + i + k + extraLine);
                                        Console.Write(string.Format("{0,5}:{1,-40}", i + 1, splittedMenu[k]));
                                    }
                                    else
                                    {
                                        Console.SetCursorPosition(cursLeft, cursTop + i + k + extraLine);
                                        Console.Write(string.Format("{0,5} {1,-40}", "", leftSpace + splittedMenu[k]));
                                    }
                                }
                                extraLine += splittedMenu.GetUpperBound(0);
                            }
                            else
                            {
                                Console.SetCursorPosition(cursLeft, cursTop + i + extraLine);
                                Console.Write(string.Format("{0,5}:{1,-40}", i + 1, menu[i]));
                            }
                        }
                        else
                        {
                            Console.SetCursorPosition(cursLeft, cursTop + i);
                            Console.Write(string.Format("{0,5}:{1,-40}", i + 1, menu[i]));
                        }
                        Console.ResetColor();

                    }
                    switch (Console.ReadKey(true).Key)
                    {
                        case ConsoleKey.UpArrow:
                            selection--;
                            break;
                        case ConsoleKey.DownArrow:
                            selection++;
                            break;
                        case ConsoleKey.Enter:
                            selected = true;
                            break;
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1: selection = 1; break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2: selection = 2; break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3: selection = 3 <= menu.Length ? 3 : menu.Length; break;
                        case ConsoleKey.D4:
                        case ConsoleKey.NumPad4: selection = 4 <= menu.Length ? 4 : menu.Length; break;
                    }
                    selection = Math.Min(Math.Max(selection, 1), menu.Length);
                    if (clearScreen)
                        Console.SetCursorPosition(0, 0);
                    else Console.SetCursorPosition(cursLeft, cursTop);
                }
                Console.CursorVisible = true;
                return selection;
            }
            
        }
        static string[] SplitString(string stringToSplit, int maxLength)
        {
            int aantalLijnen = (stringToSplit.Length / maxLength) + 1;
            string[] splitted;
            if (aantalLijnen > 1)
            {
                splitted = new string[aantalLijnen];
                string ToSplit = stringToSplit;
                int iToSplit;
                for (int i = 0; i < aantalLijnen; i++)
                {
                    if (ToSplit.Length > maxLength)
                    {
                        iToSplit = ToSplit.IndexOf(' ', maxLength - 5);
                        splitted[i] = ToSplit.Substring(0, iToSplit + 1);
                        ToSplit = ToSplit.Substring(iToSplit + 1);
                    }
                    else
                    {
                        splitted[i] = ToSplit;
                        break;
                    }
                }
            }
            else
            {
                splitted = new string[1] { stringToSplit };
            }
            return splitted;
        }
        static void CreateOpdrachten()
        {
            oefeningen.Add(new Oefening (
                                        Oefening1_1,
                                        "Selecteer alles van de tabel “orders”.",
                                        "Select* from orders"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_2,
                                        "In diezelfde tabel, sorteer op datum.",
                                        "Select * from orders order by order_date"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_3,
                                        "Selecteer van “people” de voornaam, achternaam, en vdab nummer en sorteer daarna op vdab nummer.",
                                        "Select first_name, last_name, VDAB_number from people order by vdab_number;"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_4,
                                        "Laat iedere unieke postcode zien.",
                                        "Select distinct postcode from people"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_5,
                                        "Selecteer iedereen met een telefoonnummer en een address.",
                                        "Select * from people where phone is not null and address is not null"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_6,
                                        "Selecteer iedereen waarvan ze GEEN telefoonnummer of address hebben.",
                                        "Select * from people where phone is null or address is null"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_7,
                                        "Selecteer iedereen hun voornaam en lengte, sorteer op lengte",
                                        "Select first_name, length from people order by length asc"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_8,
                                        "Iedereen groeit 10cm! Tel 0.10 bij in de kolom lengte, en geef hem een goede naam. Sorteer op die kolom.",
                                        "Select first_name, length, length+0.10 as “nieuwe length” from people order by length asc;"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_9,
                                        "Selecteer van “orders” het product_id, en de unit_price maal de quantity. Sorteer daarop.",
                                        "select product_id, unit_price * quantity from orders order by 2"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening1_10,
                                        "Vraag in “people” de first_name, last_name, vdab_nummer, address, postcode, length (maar 10cm kleiner), waarvan dat de postcode niet 2000 is, maar sorteer wel op postcode, na het sorteren op postcode sorteer je op de first_name, van Z naar A.",
                                        "select first_name, last_name, vdab_number, address, postcode, (length - 0.1) as length from people where postcode <> 2000 order by postcode, first_name desc"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_1,
                                        "Selecteer de mensen waarvan de voornaam een “A” heeft.",
                                        "select * from people where first_name like '%a%'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_2,
                                        "Selecteer de mensen waarvan hun telefoonnummer eindigt op “8”.",
                                        "select * from people where phone like '%8'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_3,
                                        "Selecteer de mensen waarvan de achternaam start met “V”, en een telefoon hebben.",
                                        "select * from people where last_name like 'v%' and phone is not null"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_4,
                                        "Selecteer de mensen waarvan dat de tweede letter van hun achternaam, de letter “i” is.",
                                        "select * from people where last_name like '_i%'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_5,
                                        "Selecteer de mensen waarvan de voornaam eindigt met een klinker.",
                                        "select * from people where first_name like '%[aeoiu]'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_6,
                                        "Selecteer de mensen waarvan de voornaam “Nick” is, of start met een T.",
                                        "select * from people where first_name = 'Nick' or first_name like 't%'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_7,
                                        "Selecteer de mensen geboren tussen 1990 en 1999",
                                        "select * from people where birthday between '1990-01-01' and '1999-12-31'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_8,
                                        "Selecteer de mensen die een letter “B” in hun naam hebben, gevolgd door een letter “R” of “I”",
                                        "select * from people where last_name like '%b[ri]%'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_9,
                                        "Selecteer mensen met een achternaam langer of gelijk aan 5 letters.",
                                        "select * from people where len(last_name) >= 5; select * from people where last_name like '____%'"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_10,
                                        "Selecteer de eerste 5 records van “people”.",
                                        "select top (5) * from people"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_11,
                                        "Voor de mensen dat in de provincie Oost-Vlaanderen wonen (postcode 9XXX), selecteer de mensen groter dan 1.60m, met een geboortedatum na 1995, sorteer op achternaam.",
                                        "select * from people where postcode like '9___' and length > '1.6' and birthday > ‘1995-01-01’ order by last_name"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening2_12,
                                        "Selecteer de 3 langste mensen.",
                                        "select top 3 * from people order by length desc"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_1,
                                        "Laat de voornaam en achternaam zien van mensen dat een product hebben gekocht, en ook het id van dat product.",
                                        "select first_name, last_name, o.product_id from people p join orders o on o.buyer_id = p.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_2,
                                        "Laat de voornaam en achternaam zien van mensen dat een product hebben gekocht, laat de totaalprijs van dat product zien, het id, en de huidige status (als woord), gesorteert van hoog naar laag op de totaalprijs.",
                                        "select first_name, last_name, o.product_id, (o.unit_price * o.quantity) as totaal, s.name from people p join orders o on o.buyer_id = p.id Join order_statuses s on o.status_id = s.id order by totaal"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_3,
                                        "Laat de voornaam en achternaam zien van mensen, en hun hobbies er bij.",
                                        "select first_name, last_name, h.hobby from people p join people_hobbies ph ON p.id = ph.person_id join hobbies h ON ph.hobby_id = h.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_4,
                                        "Sorteer de mensen op “Short” (kleiner dan 1.65), “Medium” en “Tall” (groter dan 1.80).",
                                        "select last_name, first_name, 'short' as LenCategory from people where length < '1.65' UNION select last_name, first_name, 'medium' as LenCategory from people where length between '1.65' and '1.79' Union select last_name, first_name, 'Tall' as LenCategory from people where length > '1.79' order by LenCategory"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_5,
                                        "Laat alle mensen hun voornaam zien dat iets gekocht hebben.",
                                        "select distinct first_name from people p join orders o on o.buyer_id = p.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_6,
                                        "Print mensen hun voornaam en achternaam, en op wie ze dependent zijn.",
                                        "select child.first_name, child.last_name, parent.first_name from people child join people parent on child.dependent = parent.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_7,
                                        "Laat alle mensen hun voornaam en achternaam zien als ze iets gekocht hebben, met het gekochte order_id er bij er bij.",
                                        "select first_name, last_name, o.id from people p join orders o on o.buyer_id = p.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_8,
                                        "Laat iedereen zien, als ze een product hebben gekocht laat je de order_id’s er ook bij zien.",
                                        "select p.*, o.id from people p left join orders o on o.buyer_id = p.id"
                                        ));
            oefeningen.Add(new Oefening(
                                        Oefening3_9,
                                        "Zet producten gekocht voor 2021 op “Archived”, producten gekocht na 2021 op “Active”. Zorg er ook voor dat je hier bij de mensen hun voornaam en achternaam ziet dat het product gekocht hebben, met de totaalprijs van het gekocht product.",
                                        "select p.first_name, p.last_name, 'archived' as OrdeStatus, unit_price*quantity as 'totaalprijs' from orders o join people p on o.buyer_id = p.id where o.order_date < '2020-12-31' union select p.first_name, p.last_name, 'active' as OrdeStatus, unit_price * quantity as 'totaalprijs' from orders o join people p on o.buyer_id = p.id where o.order_date > '2020-12-31'"
                                        ));
        }
        static void PrintCollection(dynamic collection)
        {
            foreach (var item in collection)
            {
                Console.WriteLine(item);
            }
        }
        static void Oefening1_1()
        {
            //Opdracht:     Selecteer alles van de tabel “orders”.
            //Query:        Select* from orders;

            var collection = orders.Select(o => o);

            PrintCollection(collection);
        }
        static void Oefening1_2()
        {
            //Opdracht:     In diezelfde tabel, sorteer op datum.
            //Query:        Select* from orders order by order_date

            var collection = orders.OrderBy(x => x.OrderDate);

            PrintCollection(collection);
        }
        static void Oefening1_3()
        {
            //Opdracht:     Selecteer van “people” de voornaam, achternaam, en vdab nummer en sorteer daarna op vdab nummer.
            //Query:        Select first_name, last_name, VDAB_number from people order by vdab_number;

            var collection = people
                .OrderBy(p => p.VDABNumber)
                .Select(p => new
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    VDABNumber = p.VDABNumber
                });

            PrintCollection(collection);
        }
        static void Oefening1_4()
        {
            //Opdracht:     Laat iedere unieke postcode zien.
            //Query:        Select distinct postcode from people

            var collection = people.Select(p => new
            {
                Postcode = p.PostCode
            }).Distinct();

            PrintCollection(collection);
        }
        static void Oefening1_5()
        {
            //Opdracht:     Selecteer iedereen met een telefoonnummer en een address.
            //Query:        Select* from people where phone is not null and address is not null

            var collection = people
                .Where(p => p.Phone != null && p.Address != null);

            PrintCollection(collection);
        }
        static void Oefening1_6()
        {
            //Opdracht:     Selecteer iedereen waarvan ze GEEN telefoonnummer of address hebben.
            //Query:        Select* from people where phone is null or address is null

            var collection = people
                .Where(p => p.Phone == null || p.Address == null);

            PrintCollection(collection);
        }
        static void Oefening1_7()
        {
            //Opdracht:     Selecteer iedereen hun voornaam en lengte, sorteer op lengte
            //Query:        Select first_name, length from people order by length asc;

            var collection = people
                .Select(p => new
                {
                    Firstname = p.FirstName,
                    Lengte = p.Length
                })
                .OrderBy(p => p.Lengte);


            PrintCollection(collection);
        }
        static void Oefening1_8()
        {
            //Opdracht:     Iedereen groeit 10cm! Tel 0.10 bij in de kolom lengte, en geef hem een goede naam. Sorteer op die kolom.
            //Query:        Select first_name, length, length+0.10 as “nieuwe length” from people order by length asc;

            var collection = people
                .Select(p => new
                {
                    Firstname = p.FirstName,
                    NieuwLengte = p.Length + 0.1
                })
                .OrderBy(p => p.NieuwLengte);

            PrintCollection(collection);
        }
        static void Oefening1_9()
        {
            //Opdracht:     Selecteer van “orders” het product_id, en de unit_price maal de quantity. Sorteer daarop.
            //Query:        select product_id, unit_price *quantity from orders order by 2

            var collection = orders
               .Select(o => new
               {
                   ProductID = o.ProductId,
                   Totaal = o.UnitPrice * o.Quantity
               })
               .OrderBy(o => o.Totaal);

            PrintCollection(collection);
        }
        static void Oefening1_10()
        {
            //Opdracht:     Vraag in “people” de first_name, last_name, vdab_nummer, address, postcode, length (maar 10cm kleiner), waarvan dat de postcode niet 2000 is, maar sorteer wel op postcode, na het sorteren op postcode sorteer je op de first_name, van Z naar A.
            //Query:        select first_name, last_name, vdab_number, address, postcode, (length - 0.1) as length
            //              from people
            //              where postcode <> 2000 order by postcode, first_name desc;

            var collection = people
                .Where(p => p.PostCode != "2000")
                .Select(p => new
                {
                    Firstname = p.FirstName,
                    Lastname = p.LastName,
                    VDAB_nummer = p.VDABNumber,
                    Address = p.Address,
                    Postcode = p.PostCode,
                    NieuwLengte = p.Length - 0.1,
                })
                .OrderBy(p => p.Postcode)
                .ThenByDescending(p => p.Firstname);

            PrintCollection(collection);
        }
        static void Oefening2_1()
        {
            //Opdracht:     Selecteer de mensen waarvan de voornaam een “A” heeft.
            //Query:        select* from people where first_name like '%a%';

            var collection = people
                .Where(p => p.FirstName.ToLower().Contains('a'));

            PrintCollection(collection);
        }
        static void Oefening2_2()
        {
            //Opdracht:     Selecteer de mensen waarvan hun telefoonnummer eindigt op “8”.
            //Query:        select* from people where phone like '%8';

            Regex PhoneCheck = new Regex(@"8\z");
            var collection = people
                //.Where(p => p.Phone != null && p.Phone[p.Phone.Length - 1] == '8'),
                .Where(p => p.Phone != null && PhoneCheck.IsMatch(p.Phone));

            PrintCollection(collection);
        }
        static void Oefening2_3()
        {
            //Opdracht:     Selecteer de mensen waarvan de achternaam start met “V”, en een telefoon hebben.
            //Query:        select* from people where last_name like 'v%' and phone is not null;

            var collection = people
                .Where(p => p.Phone != null && p.LastName != null && p.LastName.ToLower()[0] == 'v');

            PrintCollection(collection);
        }
        static void Oefening2_4()
        {
            //Opdracht:     Selecteer de mensen waarvan dat de tweede letter van hun achternaam, de letter “i” is.
            //Query:        select* from people where last_name like '_i%';

            var collection = people
                .Where(p => p.LastName != null && p.LastName.ToLower()[1] == 'i');

            PrintCollection(collection);
        }
        static void Oefening2_5()
        {
            //Opdracht:     Selecteer de mensen waarvan de voornaam eindigt met een klinker.
            //Query:        select* from people where first_name like '%[aeoiu]';

            Regex FirstNameCheck = new Regex(@"[aeoiu]\z");
            var collection = people
                .Where(p => FirstNameCheck.IsMatch(p.FirstName.ToLower()));

            PrintCollection(collection);
        }
        static void Oefening2_6()
        {
            //Opdracht:     Selecteer de mensen waarvan de voornaam “Nick” is, of start met een T.
            //Query:        select* from people where first_name = 'Nick' or first_name like 't%';

            Regex FirstNameCheck_Nick = new Regex(@"\Anick|t\A");
            Regex FirstNameCheck = new Regex(@"\At");
            var collection = people
                .Where(p => FirstNameCheck_Nick.IsMatch(p.FirstName.ToLower()) || FirstNameCheck.IsMatch(p.FirstName.ToLower()));

            PrintCollection(collection);
        }
        static void Oefening2_7()
        {
            //Opdracht:     Selecteer de mensen geboren tussen 1990 en 1999
            //Query:        select* from people where birthday between '1990-01-01' and '1999-12-31';

            DateTime start = new DateTime(1990, 01, 01);
            DateTime end = new DateTime(1999, 12, 31);
            var collection = people
                .Where(p => p.Birthday >= start && p.Birthday <= end);


            PrintCollection(collection);
        }
        static void Oefening2_8()
        {
            //Opdracht:     Selecteer de mensen die een letter “B” in hun naam hebben, gevolgd door een letter “R” of “I”
            //Query:        select* from people where last_name like '%b[ri]%';

            Regex LastNameCheck = new Regex(@"\Ab(i|r)");
            var collection = people
                .Where(p => LastNameCheck.IsMatch(p.LastName.ToLower()));

            PrintCollection(collection);
        }
        static void Oefening2_9()
        {
            //Opdracht:     Selecteer mensen met een achternaam langer of gelijk aan 5 letters.
            //Query:        select* from people where last_name like '____%';
            //              select* from people where len(last_name) >= 5;

            var collection = people
                .Where(p => p.LastName.Length >= 5);

            PrintCollection(collection);
        }
        static void Oefening2_10()
        {
            //Opdracht:     Selecteer de eerste 5 records van “people”.
            //Query:        select top(5) *from people;

            var collection = people.Take(5);

            PrintCollection(collection);
        }
        static void Oefening2_11()
        {
            //Opdracht:     Voor de mensen dat in de provincie Oost - Vlaanderen wonen(postcode 9XXX), selecteer de mensen groter dan 1.60m, met een geboortedatum na 1995, sorteer op achternaam.
            //Query:        select* from people where postcode like '9___' and length > '1.6' and birthday > ‘1995 - 01 - 01’ order by last_name;

            DateTime start = new DateTime(1996, 1, 1);
            var collection = people
                .Where(p => p.Birthday >= start &&
                            p.PostCode[0] == '9' &&
                            p.Length > 1.6)
                .OrderBy(p => p.LastName);

            PrintCollection(collection);
        }
        static void Oefening2_12()
        {
            //Opdracht:     Selecteer de 3 langste mensen.
            //Query:        select top 3 * from people order by length desc;

            var collection = people
                .OrderByDescending(p => p.Length).Take(3);

            //foreach (var item in collection)
            //    {
            //        Console.WriteLine(item + " " + item.Length);
            //    }

            PrintCollection(collection);
        }
        static void Oefening3_1()
        {
            //Opdracht:     Laat de voornaam en achternaam zien van mensen dat een product hebben gekocht, en ook het id van dat product.
            //Query:        select first_name, last_name, o.product_id
            //              from people p
            //              join orders o
            //              on o.buyer_id = p.id;

            var collection = people.Join(orders,
                p => p.PersonId,
                o => o.PersonId,
                (p,o) => new { p = p, o = o});

            var groupedCollection = collection
                .GroupBy(p => (p.p.FirstName + " " +p.p.LastName));

            var supercollection = people.Join(orders,
                p => p.PersonId,
                o => o.PersonId,
                (p,o) => new { p = p, o = o})
                .GroupBy(p => (p.p.FirstName + " " +p.p.LastName));


            foreach (var group in supercollection)
            {
                Console.WriteLine(group.Key + ": " );
                foreach (var product in group)
	            {
                    Console.WriteLine("< "+product.o.ProductId );
	            }
            }
            Console.ReadLine(); 

        }
        static void Oefening3_2()
        {
            //Opdracht:     Laat de voornaam en achternaam zien van mensen dat een product hebben gekocht, laat de totaalprijs van dat product zien, het id, en de huidige status (als woord), gesorteert van hoog naar laag op de totaalprijs.
            //Query:        select first_name, last_name, o.product_id, (o.unit_price * o.quantity) as totaal,  s.name
            //              from people p
            //              join orders o
            //              on o.buyer_id = p.id
            //              Join order_statuses s
            //              on o.status_id = s.id
            //              order by totaal;

            var collection = people.Join(orders,
                p => p.PersonId,
                o => o.PersonId,
                (p,o) => new { p = p, o = o})
                .Join(statuses,
                sc => sc.o.StatusId,
                s => s.StatusId,
                (sc,s) => new 
                { 
                    sc = sc,
                    Totaal = sc.o.Quantity * sc.o.UnitPrice,
                    s = s 
                })
                .OrderByDescending(p => p.Totaal)
                .GroupBy(p => new 
                {
                    Person = p.sc.p.FirstName + " " +p.sc.p.LastName,
                    Status = p.s.Name
                });

            foreach (var group in collection)
            {
                Console.WriteLine(group.Key.Person + ": " );
                Console.WriteLine("> " +group.Key.Status);
                foreach (var product in group)
	            {
                    Console.WriteLine("-> ProductId: "+product.sc.o.OrderId +" totaal: " + product.Totaal );
	            }
            }
            Console.ReadLine();

        }
        static void Oefening3_3()
        {
            //Opdracht:     Laat de voornaam en achternaam zien van mensen, en hun hobbies er bij.
            //Query:        select first_name, last_name, h.hobby
            //              from people p
            //              join people_hobbies ph
            //              ON p.id = ph.person_id
            //              join hobbies h
            //              ON ph.hobby_id = h.id

            var collection = people.Join(peopleHobbies,
                p => p.PersonId,
                ph => ph.PersonId,
                (p,ph) => new { p, ph })
                .Join(hobbies,
                sc => sc.ph.HobbyId,
                h => h.HobbyId,
                (sc, h) => new { sc = sc, h = h})
                .GroupBy( p=> new
                {
                    p.h.Name
                });

            foreach (var group in collection)
            {
                Console.WriteLine(group.Key.Name + ": " );
                foreach (var person in group)
	            {
                    Console.WriteLine("-> "+person.sc.p.FirstName +" " + person.sc.p.LastName );
	            }
            }
            Console.ReadLine();
        }
        static void Oefening3_4()
        {
            //Opdracht:     Sorteer de mensen op “Short” (kleiner dan 1.65), “Medium” en “Tall” (groter dan 1.80).
            //Query:        select last_name, first_name, 'short' as LenCategory
            //              from people
            //              where length < '1.65'
            //              UNION
            //              select last_name, first_name, 'medium' as LenCategory
            //              from people
            //              where length between '1.65' and '1.79'
            //              Union
            //              select last_name, first_name, 'Tall' as LenCategory
            //              from people
            //              where length > '1.79'
            //              order by LenCategory;

            var collection = people
                .OrderByDescending(p=>p.Length)
                .Select(p=> new
                {
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    Length = p.Length,
                    LengteCategory = (p.Length < 1.65)?"Short":(p.Length < 1.8)?"Medium":"Tall"
                })                
                .GroupBy(p=> p.LengteCategory);

            foreach (var group in collection)
            {
                Console.WriteLine(group.Key + ": " );
                foreach (var person in group)
	            {
                    var currentPerson = person.FirstName +" " + person.LastName;
                    Console.WriteLine("-> "+currentPerson + ((currentPerson.Length <13)? "\t\t":"\t") + "Lengte van: " + person.Length);
	            }
            }
            Console.ReadLine();
        }
        static void Oefening3_5()
        {
            //Opdracht:     Laat alle mensen hun voornaam zien dat iets gekocht hebben.
            //Query:        select distinct first_name
            //              from people p
            //              join orders o
            //              on o.buyer_id = p.id

            var collection = people.Join(orders,
                p => p.PersonId,
                o => o.PersonId,
                (p,o) => new { p = p, o = o })
                .GroupBy(p=>p.p.FirstName);
            foreach (var person in collection)
	        {
                Console.WriteLine(person.Key);
	        }
            Console.ReadLine();
        }
        static void Oefening3_6()
        {
            //Opdracht:     Print mensen hun voornaam en achternaam, en op wie ze dependent zijn.
            //Query:        select child.first_name, child.last_name, parent.first_name
            //              from people child
            //              join people parent
            //              on child.dependent = parent.id

            var collection = people.Join(people,
                child => child.Dependent,
                parent => parent.PersonId,
                (child, parent)=> new 
                {
                    FullName = child.FirstName + " " + child.LastName,
                    Parent = parent.FirstName + " " + parent.LastName,
                })
                .GroupBy(parent => new 
                {
                    Parent = parent.Parent
                });

            foreach (var group in collection)
	        {
                Console.WriteLine("Parent: "+group.Key.Parent);
                foreach (var person in group)
	            {
                    Console.WriteLine("--> " + person.FullName);
	            }
	        }
            Console.ReadLine();
        }
        static void Oefening3_7()
        {
            //Opdracht:     Laat alle mensen hun voornaam en achternaam zien als ze iets gekocht hebben, met het gekochte order_id er bij er bij.
            //Query:        select first_name, last_name, o.id
            //              from people p
            //              join orders o
            //              on o.buyer_id = p.id

            var collection = people.Join(orders,
                p => p.PersonId,
                o => o.PersonId,
                (p, o) => new
                {
                    FullName = p.FirstName + " " + p.LastName,
                    OrderId = o.OrderId,
                })
                .GroupBy(p => new
                {
                    FullName = p.FullName
                });

            foreach (var group in collection)
            {
                Console.WriteLine(group.Key.FullName);
                foreach (var product in group)
                {
                    Console.WriteLine("--> Orderid: " + product.OrderId);
                }
            }
            Console.ReadLine(); 
        }
        static void Oefening3_8()
        {
            //Opdracht:     Laat iedereen zien, als ze een product hebben gekocht laat je de order_id’s er ook bij zien.
            //Query:        select p.*, o.id
            //              from people p
            //              left join orders o
            //              on o.buyer_id = p.id

            var collection = people.GroupJoin(orders,
                 p => p.PersonId,
                 o => o.PersonId,
                 (p, o) => new
                 {
                     p = p,
                     o = o
                 })
                 .SelectMany(
                     oMetLegen => oMetLegen.o.DefaultIfEmpty(),
                     (p, oMetLegen) => new
                     {
                         FullName = p.p.FirstName + " " + p.p.LastName,
                         o = oMetLegen
                     })
                 .GroupBy(p => new
                 {
                     FullName = p.FullName
                 });
            foreach (var group in collection)
            {
                Console.WriteLine(group.Key.FullName);
                foreach (var person in group)
                {
                    Console.WriteLine("-> ProductId: " + ((person.o != null) ? person.o.ProductId.ToString() : "geen"));
                }
            }
            Console.ReadLine(); 
        }
        static void Oefening3_9()
        {
            //Opdracht:     Zet producten gekocht voor 2021 op “Archived”, producten gekocht na 2021 op “Active”. Zorg er ook voor dat je hier bij de mensen hun voornaam en achternaam ziet dat het product gekocht hebben, met de totaalprijs van het gekocht product.
            //Query:        select p.first_name, p.last_name, 'archived' as OrdeStatus, unit_price* quantity as 'totaalprijs'
            //              from orders o
            //              join people p
            //              on o.buyer_id = p.id
            //              where o.order_date < '2020-12-31'
            //              union
            //              select p.first_name, p.last_name, 'active' as OrdeStatus, unit_price* quantity as 'totaalprijs' from orders o
            //              join people p
            //              on o.buyer_id = p.id
            //              where o.order_date > '2020-12-31'


            DateTime voor2021 = new DateTime(2021,01,01);
            var collection = orders.Select(o => new
                {
                    Totaal = o.Quantity * o.UnitPrice,
                    Status = (o.OrderDate < voor2021)?"Archived":"Active",
                    o = o
                })
                .Join(people,
                o => o.o.PersonId,
                p => p.PersonId,
                (o,p) => new
                {
                    o = o,
                    FullName = p.FirstName + " " + p.LastName,
                    p = p
                })
                .OrderByDescending(o => o.o.Totaal)
                .GroupBy( p => new
                {
                    Status = p.o.Status,
                    FullName = p.FullName
                })
                .GroupBy( p => new
                {
                    FullName = p.Key.FullName
                });

            foreach (var group in collection)
	        {
                Console.WriteLine("klant: "+ group.Key.FullName);
                foreach (var order in group)
	            {
                    Console.WriteLine("->Status: " +order.Key.Status);
                    foreach (var item in order)
	                {
                        Console.WriteLine( "\tproductid: " + item.o.o.ProductId + " Totaal: " + item.o.Totaal);
	                }
	            }
	        }
            Console.ReadLine();
        }
        static void CreateDb()
        {
            people.Add(new People()
            {
                PersonId = 1,
                FirstName = "Claire",
                LastName = "Whitten",
                Birthday = new DateTime(1959, 03, 06),
                VDABNumber = 29436315,
                Email = "cw@mm.be",
                Phone = "496456348",
                Address = "clairestraat 21",
                RoleId = 1,
                Length = 1.56,
                PostCode = "2000",
                Dependent = 11,
            });

            people.Add(new People()
            {
                PersonId = 2,
                FirstName = "Gawein",
                LastName = "Verlot",
                Birthday = new DateTime(1960, 09, 08),
                VDABNumber = 78475719,
                Email = "gw@mm.be",
                Phone = "478618965",
                Address = "gaweinstraat 25",
                RoleId = 1,
                Length = 1.64,
                PostCode = "2000",
                Dependent = 11,
            });

            people.Add(new People()
            {
                PersonId = 3,
                FirstName = "Katia",
                LastName = "Verachtert",
                Birthday = new DateTime(1966, 04, 04),
                VDABNumber = 48915365,
                Email = "kv@mm.be",
                Phone = null,
                Address = "katiastraat 12",
                RoleId = 1,
                Length = 1.75,
                PostCode = "2000",
                Dependent = 11,
            });

            people.Add(new People()
            {
                PersonId = 4,
                FirstName = "Tim",
                LastName = "Audenaert",
                Birthday = new DateTime(1970, 06, 05),
                VDABNumber = 15416496,
                Email = "ta@mm.be",
                Phone = "486899966",
                Address = null,
                RoleId = 1,
                Length = 1.95,
                PostCode = "2100",
                Dependent = 11,
            });

            people.Add(new People()
            {
                PersonId = 5,
                FirstName = "Mohamed",
                LastName = "Kaouchi",
                Birthday = new DateTime(1980, 05, 12),
                VDABNumber = 48794894,
                Email = "mk@mm.be",
                Phone = "478652134",
                Address = "mohamedstraat 96",
                RoleId = 1,
                Length = 1.64,
                PostCode = "9200",
                Dependent = 11,
            });

            people.Add(new People()
            {
                PersonId = 6,
                FirstName = "Kenny",
                LastName = "Bruwier",
                Birthday = new DateTime(1999, 12, 05),
                VDABNumber = 45648999,
                Email = "kb@mm.be",
                Phone = "478962453",
                Address = "kennystraat 45",
                RoleId = 1,
                Length = 1.76,
                PostCode = "9000",
                Dependent = 12,
            });

            people.Add(new People()
            {
                PersonId = 7,
                FirstName = "Nick",
                LastName = "Zubrzycki",
                Birthday = new DateTime(2000, 12, 01),
                VDABNumber = 46796746,
                Email = "nz@mm.be",
                Phone = "478645563",
                Address = "nickstraat 1",
                RoleId = 1,
                Length = 1.73,
                PostCode = "9000",
                Dependent = 12,
            });

            people.Add(new People()
            {
                PersonId = 8,
                FirstName = "Tom",
                LastName = "Dilen",
                Birthday = new DateTime(1815, 09, 09),
                VDABNumber = 12348963,
                Email = "td@mm.be",
                Phone = "52569453",
                Address = "tomstraat 12",
                RoleId = 1,
                Length = 1.72,
                PostCode = "9200",
                Dependent = 12,
            });

            people.Add(new People()
            {
                PersonId = 9,
                FirstName = "Vincent",
                LastName = "Callaerts",
                Birthday = new DateTime(2000, 09, 01),
                VDABNumber = 45649996,
                Email = "vc@mm.be",
                Phone = "478648653",
                Address = "vincentstraat 133",
                RoleId = 1,
                Length = 1.82,
                PostCode = "8500",
                Dependent = 12,
            });

            people.Add(new People()
            {
                PersonId = 10,
                FirstName = "Arno",
                LastName = "Slabbinck",
                Birthday = new DateTime(1946, 04, 23),
                VDABNumber = 44893581,
                Email = "as@mm.be",
                Phone = "478965512",
                Address = "arnostraat 9",
                RoleId = 1,
                Length = 1.80,
                PostCode = "9000",
                Dependent = 12,
            });

            people.Add(new People()
            {
                PersonId = 11,
                FirstName = "Ken",
                LastName = "Field",
                Birthday = new DateTime(1996, 05, 19),
                VDABNumber = 1337900169,
                Email = "kf@mm.be",
                Phone = "478123456",
                Address = "kenstraat 5",
                RoleId = 2,
                Length = 1.78,
                PostCode = "8500",
                Dependent = 13,
            });

            people.Add(new People()
            {
                PersonId = 12,
                FirstName = "Hans",
                LastName = "Van Soom",
                Birthday = new DateTime(1990, 12, 31),
                VDABNumber = 11272373,
                Email = "hvs@mm.be",
                Phone = "479663321",
                Address = "hansstraat 6",
                RoleId = 2,
                Length = 1.82,
                PostCode = "8900",
                Dependent = 13,
            });

            people.Add(new People()
            {
                PersonId = 13,
                FirstName = "Nick",
                LastName = "Bauters",
                Birthday = new DateTime(2012, 05, 05),
                VDABNumber = 15566633,
                Email = "nb@mm.be",
                Phone = "478945646",
                Address = "nickstraat 86",
                RoleId = 3,
                Length = 1.76,
                PostCode = "9500"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 1,
                Name = "Programming"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 2,
                Name = "Gaming"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 3,
                Name = "Sports"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 4,
                Name = "Music"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 5,
                Name = "Art"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 6,
                Name = "Bingewatching"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 7,
                Name = "Traveling"
            });

            hobbies.Add(new Hobbies()
            {
                HobbyId = 8,
                Name = "Crafting"
            });

            statuses.Add(new Statuses()
            {
                StatusId = 1,
                Name = "Processing"
            });

            statuses.Add(new Statuses()
            {
                StatusId = 2,
                Name = "Shipping"
            });

            statuses.Add(new Statuses()
            {
                StatusId = 3,
                Name = "Delivered"
            });

            orders.Add(new Orders()
            {
                OrderId = 1,
                ProductId = 1,
                Quantity = 3,
                UnitPrice = 2.75,
                BasketId = 1,
                PersonId = 3,
                StatusId = 3,
                OrderDate = new DateTime(2020, 01, 12)
            });

            orders.Add(new Orders()
            {
                OrderId = 2,
                ProductId = 8,
                Quantity = 2,
                UnitPrice = 2.45,
                BasketId = 1,
                PersonId = 3,
                StatusId = 3,
                OrderDate = new DateTime(2020, 01, 12)
            });

            orders.Add(new Orders()
            {
                OrderId = 3,
                ProductId = 6,
                Quantity = 4,
                UnitPrice = 3.50,
                BasketId = 1,
                PersonId = 3,
                StatusId = 3,
                OrderDate = new DateTime(2020, 01, 12)
            });

            orders.Add(new Orders()
            {
                OrderId = 4,
                ProductId = 14,
                Quantity = 5,
                UnitPrice = 2.45,
                BasketId = 2,
                PersonId = 6,
                StatusId = 2,
                OrderDate = new DateTime(2020, 06, 24)
            });

            orders.Add(new Orders()
            {
                OrderId = 5,
                ProductId = 11,
                Quantity = 1,
                UnitPrice = 1.15,
                BasketId = 2,
                PersonId = 6,
                StatusId = 2,
                OrderDate = new DateTime(2020, 06, 24)
            });

            orders.Add(new Orders()
            {
                OrderId = 6,
                ProductId = 28,
                Quantity = 12,
                UnitPrice = 2.50,
                BasketId = 3,
                PersonId = 3,
                StatusId = 3,
                OrderDate = new DateTime(2020, 07, 02)
            });

            orders.Add(new Orders()
            {
                OrderId = 7,
                ProductId = 1,
                Quantity = 5,
                UnitPrice = 2.75,
                BasketId = 4,
                PersonId = 8,
                StatusId = 2,
                OrderDate = new DateTime(2021, 01, 13)
            });

            orders.Add(new Orders()
            {
                OrderId = 8,
                ProductId = 6,
                Quantity = 5,
                UnitPrice = 3.5,
                BasketId = 4,
                PersonId = 8,
                StatusId = 2,
                OrderDate = new DateTime(2021, 01, 13)
            });

            orders.Add(new Orders()
            {
                OrderId = 9,
                ProductId = 22,
                Quantity = 1,
                UnitPrice = 12.55,
                BasketId = 5,
                PersonId = 10,
                StatusId = 1,
                OrderDate = new DateTime(2021, 03, 05)
            });

            orders.Add(new Orders()
            {
                OrderId = 10,
                ProductId = 96,
                Quantity = 4,
                UnitPrice = 3.58,
                BasketId = 5,
                PersonId = 10,
                StatusId = 1,
                OrderDate = new DateTime(2021, 03, 05)
            });

            peopleHobbies.Add(new PeopleHobbies(1, 3));
            peopleHobbies.Add(new PeopleHobbies(1, 5));
            peopleHobbies.Add(new PeopleHobbies(1, 7));
            peopleHobbies.Add(new PeopleHobbies(1, 8));
            peopleHobbies.Add(new PeopleHobbies(2, 1));
            peopleHobbies.Add(new PeopleHobbies(2, 3));
            peopleHobbies.Add(new PeopleHobbies(2, 6));
            peopleHobbies.Add(new PeopleHobbies(3, 2));
            peopleHobbies.Add(new PeopleHobbies(3, 4));
            peopleHobbies.Add(new PeopleHobbies(3, 8));
            peopleHobbies.Add(new PeopleHobbies(4, 1));
            peopleHobbies.Add(new PeopleHobbies(4, 2));
            peopleHobbies.Add(new PeopleHobbies(4, 3));
            peopleHobbies.Add(new PeopleHobbies(5, 7));
            peopleHobbies.Add(new PeopleHobbies(5, 8));
            peopleHobbies.Add(new PeopleHobbies(6, 6));
            peopleHobbies.Add(new PeopleHobbies(7, 2));
            peopleHobbies.Add(new PeopleHobbies(7, 4));
            peopleHobbies.Add(new PeopleHobbies(7, 7));
            peopleHobbies.Add(new PeopleHobbies(7, 8));
            peopleHobbies.Add(new PeopleHobbies(8, 1));
            peopleHobbies.Add(new PeopleHobbies(8, 2));
            peopleHobbies.Add(new PeopleHobbies(9, 3));
            peopleHobbies.Add(new PeopleHobbies(9, 4));
            peopleHobbies.Add(new PeopleHobbies(9, 7));
            peopleHobbies.Add(new PeopleHobbies(10, 2));
            peopleHobbies.Add(new PeopleHobbies(10, 4));
            peopleHobbies.Add(new PeopleHobbies(11, 3));
            peopleHobbies.Add(new PeopleHobbies(11, 5));
            peopleHobbies.Add(new PeopleHobbies(12, 2));
            peopleHobbies.Add(new PeopleHobbies(13, 1));
            peopleHobbies.Add(new PeopleHobbies(13, 2));

            roles.Add(new Roles()
            {
                RoleId = 1,
                Name = "Student"
            });

            roles.Add(new Roles()
            {
                RoleId = 2,
                Name = "Teacher"
            });

            roles.Add(new Roles()
            {
                RoleId = 3,
                Name = "Principal"
            });
        }
    }
    #region tables
    class Hobbies
    {
        public int HobbyId { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    class PeopleHobbies
    {
        public PeopleHobbies(int person, int hobby)
        {
            PersonId = person;
            HobbyId = hobby;
        }
        public int PersonId { get; set; }
        public int HobbyId { get; set; }
        public override string ToString()
        {
            return $"{PersonId} <=> {HobbyId}";
        }
    }
    class People
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public int VDABNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int RoleId { get; set; }
        public string PostCode { get; set; }
        public double Length { get; set; }
        public int Dependent { get; set; }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
    class Roles
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
    class Orders
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public int BasketId { get; set; } //winkelmandje
        public int PersonId { get; set; } //buyer id
        public int StatusId { get; set; }
        public DateTime OrderDate { get; set; }

        public override string ToString()
        {
            return $"{OrderId} - {ProductId} - {Quantity}";
        }
    }
    class Statuses
    {
        public int StatusId { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
    #endregion
}
