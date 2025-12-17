using Versenyzok;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Adatbazis
{
    internal class Program
    {
        /// <summary>
        /// Az adatbázishoz való kapcsolódáshoz szükséges adatok (szerver, adatbázis név, felhasználó).
        /// </summary>
        static string connectionString = "SERVER=localhost;DATABASE=mitulatbatihotta;UID=root;";

        /// <summary>
        /// A generált HTML fájl stíluslapja (CSS).
        /// Ez határozza meg a színeket, betűtípusokat és a táblázat kinézetét.
        /// </summary>
        static string cssStilus = @"
:root {
    --primary-brown: #5D4037;
    --secondary-brown: #8D6E63;
    --wheat-gold: #FFB300;
    --bg-color: #F5F5DC;
    --text-dark: #3E2723;
    --white: #ffffff;
}
body {
    font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    color: var(--text-dark);
    margin: 0;
    padding: 0;
    line-height: 1.6;
    background-color: var(--bg-color);
    background-image: url('háttér.png');
    background-size: cover;
    background-position: center;
    background-attachment: fixed;
    background-repeat: no-repeat;
}
.main-header {
    background-color: var(--primary-brown);
    color: var(--wheat-gold);
    padding: 20px 0;
    border-bottom: 5px solid var(--secondary-brown);
    box-shadow: 0 4px 8px rgba(0,0,0,0.3);
}
.cimer {
    display: flex;
    justify-content: center;
    align-items: center;
    max-width: 1200px;
    margin: 0 auto;
    flex-wrap: wrap;
    gap: 20px;
}
.logo {
    height: 80px;
    width: auto;
    border: 2px solid var(--wheat-gold);
    border-radius: 10px;
    background-color: var(--white);
    padding: 5px;
}
.header-content {
    text-align: center;
}
.header-content h1 {
    margin: 0;
    font-size: 2rem;
    text-transform: uppercase;
    letter-spacing: 1px;
    text-shadow: 2px 2px 4px rgba(0,0,0,0.5);
}
.header-content h2 {
    margin: 5px 0;
    color: var(--white);
    font-size: 1.2rem;
}
.header-content p {
    margin: 0;
    font-style: italic;
    opacity: 0.8;
    font-size: 0.9rem;
}
.results-container, .main-page-section {
    max-width: 1000px;
    margin: 30px auto;
    padding: 20px;
    background-color: rgba(255, 255, 255, 0.92);
    backdrop-filter: blur(5px);
    border-radius: 8px;
    box-shadow: 0 0 20px rgba(0, 0, 0, 0.3);
    border: 1px solid rgba(255, 255, 255, 0.5);
}
h3 {
    color: var(--primary-brown);
    border-bottom: 2px solid var(--wheat-gold);
    padding-bottom: 10px;
    margin-top: 0;
}
.intro-text-2 {
    font-size: 0.95rem;
    color: #666;
    margin-bottom: 25px;
    background-color: rgba(255, 248, 225, 0.8);
    padding: 10px;
    border-left: 4px solid var(--wheat-gold);
    border-radius: 4px;
}
.text-muted {
    color: #555;
    font-size: 0.8rem;
    background-color: rgba(255,255,255,0.7);
    padding: 5px;
    border-radius: 4px;
    display: inline-block;
}
#score-table-wrapper {
    /* A JavaScript kezeli a görgetést, itt csak a keretet adjuk meg */
    overflow: hidden;
    height: 400px; 
    border-bottom: 2px solid var(--wheat-gold);
    position: relative;
}
table {
    width: 100%;
    border-collapse: collapse;
    margin-bottom: 20px;
    font-size: 1.1rem;
}
thead {
    background-color: var(--primary-brown);
    color: var(--white);
    position: sticky;
    top: 0;
    z-index: 10;
}
th, td {
    padding: 12px 15px;
    text-align: center;
    border-bottom: 1px solid #ccc;
}
th {
    text-transform: uppercase;
    font-size: 0.9rem;
    letter-spacing: 0.5px;
}
td:nth-child(2), th:nth-child(2) {
    text-align: left;
    font-weight: bold;
    color: var(--primary-brown);
}
tbody tr:nth-child(even) {
    background-color: rgba(0, 0, 0, 0.05);
}
tbody tr:hover {
    background-color: rgba(255, 179, 0, 0.2);
    cursor: default;
}
tbody tr td.elso-hely .helyezes-badge { background-color: #FFD700; color: #000; box-shadow: 0 0 5px #FFD700; }
tbody tr td.masodik-hely .helyezes-badge { background-color: #C0C0C0; color: #000; }
tbody tr td.harmadik-hely .helyezes-badge { background-color: #CD7F32; color: #000; }

.helyezes-badge { 
    border-radius: 50%; 
    width: 35px; 
    height: 35px;
    display: inline-block; 
    line-height: 35px; 
    font-weight: bold;
    text-align: center;
}

.main-footer {
    background-color: var(--primary-brown);
    color: #d7ccc8;
    text-align: center;
    padding: 15px;
    font-size: 0.85rem;
    margin-top: 40px;
    border-top: 4px solid var(--wheat-gold);
}
@media (max-width: 600px) {
    .cimer { flex-direction: column; }
    .header-content h1 { font-size: 1.5rem; }
    th, td { padding: 8px 5px; font-size: 0.9rem; }
    .logo { height: 60px; }
}
";

        /// <summary>
        /// A program belépési pontja.
        /// Itt jelenik meg a főmenü, és innen hívjuk meg a többi funkciót a felhasználó választása alapján.
        /// A program addig fut, amíg a felhasználó az 5-ös gombot nem választja.
        /// </summary>
        static void Main(string[] args)
        {
            bool fut = true;

            while (fut)
            {
                Console.Clear();
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("    Kalaplengető Verseny - Kezelőfelület");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("1. Adatok kiírása (mentés HTML-be)");
                Console.WriteLine("2. Új versenyző felvétele");
                Console.WriteLine("3. Adatok módosítása");
                Console.WriteLine("4. Adat törlése");
                Console.WriteLine("5. Kilépés");
                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("Válassz menüpontot: ");

                string valasztas = Console.ReadLine();

                switch (valasztas)
                {
                    case "1":
                        AdatokKiirasaHTMLbe();
                        break;
                    case "2":
                        UjAdatFelvetele();
                        break;
                    case "3":
                        AdatModositasa();
                        break;
                    case "4":
                        AdatTorlese();
                        break;
                    case "5":
                        fut = false;
                        Console.WriteLine("Viszlát!");
                        break;
                    default:
                        Console.WriteLine("Érvénytelen választás!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        /// <summary>
        /// Lekérdezi az összes versenyzőt az adatbázisból, sorba rendezi őket eredmény szerint,
        /// majd generál egy 'statisztika.html' fájlt.
        /// A HTML fájl tartalmaz egy JavaScript kódot is, ami a táblázat folyamatos, akadásmentes görgetését végzi.
        /// </summary>
        static void AdatokKiirasaHTMLbe()
        {
            Console.Clear();
            Console.WriteLine("--- Adatok kiírása ---");

            List<Versenyzo> versenyzok = new List<Versenyzo>();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    Console.WriteLine("Adatok letöltése...");

                    string sql = "SELECT * FROM versenyzok";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Versenyzo v = new Versenyzo(
                            id: reader.GetInt32("id"),
                            nev: reader.GetString("nev"),
                            ido1: reader.GetDouble("ido1"),
                            ido2: reader.GetDouble("ido2"),
                            ido3: reader.GetDouble("ido3")
                        );
                        versenyzok.Add(v);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adatbázis hiba: " + ex.Message);
                Console.ReadKey();
                return;
            }
            List<Versenyzo> rendezettLista = versenyzok
                .OrderByDescending(x => x.LegjobbPont())
                .ThenBy(x => {
                    double bestScore = x.LegjobbPont();
                    double bestTime = 11;
                    if (x.Pontszam1 == bestScore) bestTime = Math.Min(bestTime, x.Ido1);
                    if (x.Pontszam2 == bestScore) bestTime = Math.Min(bestTime, x.Ido2);
                    if (x.Pontszam3 == bestScore) bestTime = Math.Min(bestTime, x.Ido3);
                    return bestTime;
                })
                .ToList();


            string projectRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..");
            string fullPath = Path.Combine(projectRoot, "statisztika.html");

            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath))
                {
                    sw.WriteLine("<!DOCTYPE html>");
                    sw.WriteLine("<html lang=\"hu\">");
                    sw.WriteLine("<head>");
                    sw.WriteLine("    <meta charset=\"UTF-8\">");
                    sw.WriteLine("    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
                    sw.WriteLine("    <title>Élő Állás - Kalaplengető Verseny</title>");
                    sw.WriteLine("    <meta http-equiv=\"refresh\" content=\"30\">");
                    sw.WriteLine("    <style>");
                    sw.WriteLine(cssStilus);
                    sw.WriteLine("    </style>");
                    sw.WriteLine("</head>");
                    sw.WriteLine("<body>");

                    sw.WriteLine("    <header class=\"main-header\">");
                    sw.WriteLine("        <div class=\"cimer\">");
                    sw.WriteLine("            <img src=\"traki.png\" alt=\"Traktoros Címer\" class=\"logo\">");
                    sw.WriteLine("            <div class=\"header-content\">");
                    sw.WriteLine("                <h1>Szélesbálási Kalaplengető</h1>");
                    sw.WriteLine("                <h2>🏆 AKTUÁLIS EREDMÉNYTÁBLA 🏆</h2>");
                    sw.WriteLine("                <p>TV Kijelző - Élő Frissítés</p>");
                    sw.WriteLine("            </div>");
                    sw.WriteLine("            <img src=\"soroskorso.png\" alt=\"Sör\" class=\"logo\">");
                    sw.WriteLine("            <img src=\"Búza.png\" alt=\"Hurka\" class=\"logo\">");
                    sw.WriteLine("        </div>");
                    sw.WriteLine("    </header>");

                    sw.WriteLine("    <main class=\"results-container\">");
                    sw.WriteLine("        <section id=\"statisztika\">");
                    sw.WriteLine("            <h3>Pillanatnyi Állás és Eredmények</h3>");
                    sw.WriteLine("            <p class=\"intro-text-2\">");
                    sw.WriteLine("                <strong>Szabályzat:</strong> A táblázat a versenyzők aktuális, legjobb eredményét mutatja.");
                    sw.WriteLine("                Rendezés: <em>Legjobb pontszám</em> alapján. Azonos pont esetén a <strong>gyorsabb idő</strong> dönt.");
                    sw.WriteLine("            </p>");

                    sw.WriteLine("            <div id=\"score-table-wrapper\">");
                    sw.WriteLine("                <table id=\"current-scores\">");
                    sw.WriteLine("                    <thead>");
                    sw.WriteLine("                        <tr>");
                    sw.WriteLine("                            <th style=\"width: 60px;\">Hely</th>");
                    sw.WriteLine("                            <th>Név</th>");
                    sw.WriteLine("                            <th>1. kör</th>");
                    sw.WriteLine("                            <th>2. kör</th>");
                    sw.WriteLine("                            <th>3. kör</th>");
                    sw.WriteLine("                            <th style=\"background-color: var(--secondary-brown);\">Legjobb pont</th>");
                    sw.WriteLine("                        </tr>");
                    sw.WriteLine("                    </thead>");
                    sw.WriteLine("                    <tbody>");

                    int sorszam = 1;
                    int kijelzettHely = 1;
                    double elozoPont = -1;
                    double elozoDontoIdo = -1;

                    foreach (Versenyzo v in rendezettLista)
                    {
                        double mostaniPont = v.LegjobbPont();
                        double mostaniDontoIdo = 11;
                        if (v.Pontszam1 == mostaniPont) mostaniDontoIdo = Math.Min(mostaniDontoIdo, v.Ido1);
                        if (v.Pontszam2 == mostaniPont) mostaniDontoIdo = Math.Min(mostaniDontoIdo, v.Ido2);
                        if (v.Pontszam3 == mostaniPont) mostaniDontoIdo = Math.Min(mostaniDontoIdo, v.Ido3);

                        bool holtversenyVan = (mostaniPont == elozoPont && mostaniDontoIdo == elozoDontoIdo);

                        if (!holtversenyVan) kijelzettHely = sorszam;

                        string tdClass = "";
                        if (kijelzettHely == 1) tdClass = "elso-hely";
                        else if (kijelzettHely == 2) tdClass = "masodik-hely";
                        else if (kijelzettHely == 3) tdClass = "harmadik-hely";

                        sw.WriteLine("                        <tr>");
                        sw.WriteLine($"                            <td class=\"{tdClass}\"><span class=\"helyezes-badge\">{kijelzettHely}.</span></td>");
                        sw.WriteLine($"                            <td>{v.Nev}</td>");
                        sw.WriteLine($"                            <td>{v.Pontszam1}</td>");
                        sw.WriteLine($"                            <td>{v.Pontszam2}</td>");
                        sw.WriteLine($"                            <td>{v.Pontszam3}</td>");
                        sw.WriteLine($"                            <td style=\"font-weight:bold;\" title=\"Döntő idő: {mostaniDontoIdo} mp\">{mostaniPont}</td>");
                        sw.WriteLine("                        </tr>");

                        elozoPont = mostaniPont;
                        elozoDontoIdo = mostaniDontoIdo;
                        sorszam++;
                    }

                    sw.WriteLine("                    </tbody>");
                    sw.WriteLine("                </table>");
                    sw.WriteLine("            </div>");

                    string datum = DateTime.Now.ToString("yyyy. MMMM dd. HH:mm:ss");
                    sw.WriteLine("            <p class=\"text-muted\" style=\"text-align: right; margin-top: 15px;\">");
                    sw.WriteLine($"                Utolsó frissítés: <strong>{datum}</strong> (Automatikus)");
                    sw.WriteLine("            </p>");
                    sw.WriteLine("        </section>");
                    sw.WriteLine("    </main>");
                    sw.WriteLine("    <footer class=\"main-footer\">");
                    sw.WriteLine("        <p>A Szélesbálási Fedettpályás Kalaplengető Verseny hivatalos eredménykijelzője<br>");
                    sw.WriteLine("        <span style=\"opacity: 0.6; font-size: 0.8em;\">Powered by <strong>Dell Latitude E5570</strong></span></p>");
                    sw.WriteLine("    </footer>");
                    //folyamatosan gördülős kódrész
                    sw.WriteLine("    <script>");
                    sw.WriteLine("        window.onload = function() {");
                    sw.WriteLine("            const container = document.getElementById('score-table-wrapper');");
                    sw.WriteLine("            const table = document.getElementById('current-scores');");
                    sw.WriteLine("            const tbody = table.querySelector('tbody');");
                    sw.WriteLine("");
                    sw.WriteLine("            // Csak akkor aktiváljuk a görgetést, ha a lista hosszabb, mint a képernyő");
                    sw.WriteLine("            if (table.clientHeight > container.clientHeight) {");
                    sw.WriteLine("                // Tartalom duplázása a végtelen hatáshoz");
                    sw.WriteLine("                const originalContent = tbody.innerHTML;");
                    sw.WriteLine("                tbody.innerHTML += originalContent;");
                    sw.WriteLine("");
                    sw.WriteLine("                let currentScroll = 0;");
                    sw.WriteLine("                const speed = 0.4; // Görgetési sebesség (kisebb szám = lassabb)");
                    sw.WriteLine("                const refreshRate = 20;");
                    sw.WriteLine("");
                    sw.WriteLine("                function autoScroll() {");
                    sw.WriteLine("                    currentScroll += speed;");
                    sw.WriteLine("                    container.scrollTop = currentScroll;");
                    sw.WriteLine("");
                    sw.WriteLine("                    // Ha elértük a lista felét (az eredetit), visszaugrunk az elejére");
                    sw.WriteLine("                    if (currentScroll >= (table.scrollHeight / 2)) {");
                    sw.WriteLine("                        currentScroll = 0;");
                    sw.WriteLine("                    }");
                    sw.WriteLine("                }");
                    sw.WriteLine("");
                    sw.WriteLine("                setInterval(autoScroll, refreshRate);");
                    sw.WriteLine("            }");
                    sw.WriteLine("        };");
                    sw.WriteLine("    </script>");

                    sw.WriteLine("</body>");
                    sw.WriteLine("</html>");
                }
                Console.WriteLine("\nSIKER! A HTML fájl generálása kész.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba történt a fájl írásakor: " + ex.Message);
            }
            Console.WriteLine("Nyomj egy gombot a menühöz...");
            Console.ReadKey();
        }

        /// <summary>
        /// Új versenyző felvétele az adatbázisba.
        /// Bekéri az ID-t, a nevet és a három kör idejét, majd végrehajt egy SQL INSERT parancsot.
        /// </summary>
        static void UjAdatFelvetele()
        {
            Console.Clear();
            Console.WriteLine("--- Új versenyző felvétele ---");
            Console.Write("Add meg a versenyző ID-jét: ");
            string id = Console.ReadLine();
            Console.Write("Add meg a versenyző nevét: ");
            string nev = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(nev))
            {
                Console.WriteLine("A név nem lehet üres!");
                Console.ReadKey();
                return;
            }

            double ido1 = IdoBekerese("1. kör ideje (mp): ");
            double ido2 = IdoBekerese("2. kör ideje (mp): ");
            double ido3 = IdoBekerese("3. kör ideje (mp): ");

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "INSERT INTO versenyzok (id, nev, ido1, ido2, ido3) VALUES (@id, @nev, @ido1, @ido2, @ido3)";

                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nev", nev);
                    cmd.Parameters.AddWithValue("@ido1", ido1);
                    cmd.Parameters.AddWithValue("@ido2", ido2);
                    cmd.Parameters.AddWithValue("@ido3", ido3);

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nSikeres felvétel! Az új versenyző bekerült az adatbázisba.");
                    }
                    else
                    {
                        Console.WriteLine("\nHiba: Nem sikerült rögzíteni az adatot.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Adatbázis hiba: " + ex.Message);
            }

            Console.WriteLine("Nyomj egy gombot a menühöz...");
            Console.ReadKey();
        }

        /// <summary>
        /// Segédfüggvény, amely bekér egy időt a felhasználótól.
        /// Ha a felhasználó nem számot ír be, a program újra megkérdezi.
        /// </summary>
        static double IdoBekerese(string szoveg)
        {
            double ido = 0;
            bool siker = false;
            do
            {
                Console.Write(szoveg);
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input)) return 0;

                siker = double.TryParse(input, out ido);
                if (!siker)
                {
                    Console.WriteLine("Hibás számformátum! Próbáld újra.");
                }
            } while (!siker);

            return ido;
        }

        /// <summary>
        /// Meglévő versenyző adatainak módosítása az ID alapján.
        /// Csak a megadott köridőket frissíti, a nevet nem.
        /// Ha üresen hagyunk egy mezőt, az eredeti adat megmarad.
        /// </summary>
        static void AdatModositasa()
        {
            Console.Clear();
            Console.WriteLine("--- Adatok módosítása ---");
            Console.Write("Add meg a versenyző ID-jét, akit módosítani szeretnél: ");
            string idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Hibás ID formátum!");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("\nAdd meg az új időket! (Ha üresen hagyod és Entert nyomsz, a régi érték marad)");

            Console.Write("1. kör ideje (mp): ");
            string ido1Str = Console.ReadLine();
            Console.Write("2. kör ideje (mp): ");
            string ido2Str = Console.ReadLine();
            Console.Write("3. kör ideje (mp): ");
            string ido3Str = Console.ReadLine();

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    if (string.IsNullOrWhiteSpace(ido1Str) && string.IsNullOrWhiteSpace(ido2Str) && string.IsNullOrWhiteSpace(ido3Str))
                    {
                        Console.WriteLine("Nem adtál meg új adatot.");
                        Console.ReadKey();
                        return;
                    }

                    string updateSql = "UPDATE versenyzok SET ";
                    List<string> setParts = new List<string>();
                    if (!string.IsNullOrWhiteSpace(ido1Str)) setParts.Add($"ido1 = {ido1Str.Replace(',', '.')}");
                    if (!string.IsNullOrWhiteSpace(ido2Str)) setParts.Add($"ido2 = {ido2Str.Replace(',', '.')}");
                    if (!string.IsNullOrWhiteSpace(ido3Str)) setParts.Add($"ido3 = {ido3Str.Replace(',', '.')}");

                    updateSql += string.Join(", ", setParts);
                    updateSql += $" WHERE id = {id}";

                    MySqlCommand cmd = new MySqlCommand(updateSql, connection);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("\nSikeres módosítás!");
                    }
                    else
                    {
                        Console.WriteLine("\nNem található ilyen ID-jű versenyző.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba a módosítás során: " + ex.Message);
            }
            Console.WriteLine("Nyomj egy gombot a menühöz...");
            Console.ReadKey();
        }

        /// <summary>
        /// Egy versenyző törlése az adatbázisból az ID-je alapján.
        /// A törlés előtt megerősítést kér a felhasználótól.
        /// </summary>
        static void AdatTorlese()
        {
            Console.Clear();
            Console.WriteLine("--- Adat törlése ---");
            Console.Write("Add meg a törlendő versenyző ID-jét: ");
            string idInput = Console.ReadLine();

            if (!int.TryParse(idInput, out int id))
            {
                Console.WriteLine("Hibás ID!");
                Console.ReadKey();
                return;
            }

            Console.Write($"Biztosan törölni akarod az {id}. azonosítójú versenyzőt? (i/n): ");
            string valasz = Console.ReadLine();

            if (valasz.ToLower() != "i")
            {
                Console.WriteLine("Törlés megszakítva.");
                Console.ReadKey();
                return;
            }
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "DELETE FROM versenyzok WHERE id = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0) Console.WriteLine("Sikeres törlés.");
                    else Console.WriteLine("Nincs ilyen ID az adatbázisban.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hiba törlés közben: " + ex.Message);
            }
            Console.WriteLine("Nyomj egy gombot a menühöz...");
            Console.ReadKey();
        }
    }
}