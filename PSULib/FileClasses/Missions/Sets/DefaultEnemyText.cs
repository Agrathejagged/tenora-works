using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSULib.FileClasses.Missions.Sets
{
    public class DefaultEnemyText
    {
        public static readonly string[] enemyNames = { "Rappy", "Rappy Amure (PSU/AotI only)", "Rappy Noel (PSU/AotI only)", "Rappy Paska (PSU/AotI only)", "Rappy Latan (PSU/AotI only)", 
            "Jaggo", "Jaggo Amure (PSU/AotI only)", "Jaggo Sonichi (PSU/AotI only)", "Jaggo Acte (PSU/AotI only)", 
            "Polty", "Lapucha", "Pannon series", "Badira", "Ageeta", "Naval", "Vahra series", "Golmoro", "Gohmon series", "Vanda series", "Delsaban series", "Gaozoran", 
            "Koltova", "Distova", "Volfu", "Ollaka", "Jishagara", "Sendillan", "Shagreece", "Zoona", "Mizura", "YG-01Z BUG", "YG-01K BUGGE", "YG01U BUGGES", 
            "Gohma Dilla", "Gohma Methna", "GSM-05 Seeker", "GSM-05B Bomalta", "GSM-05M Tirentos", "Bysha type-Koh21", "Bysha type-Otsu32", "Goshin", "Bul Buna", 
            "Jarba", "Bil De Vear", "Darbelan", "Dilnazen", "Svaltus", "Orgdus", "Lutus Jigga", "Kagajibari", "SEED-Magashi", "Gol Dolva", "Polavohra", "Drua Gohra", 
            "Kog Nadd", "Galvapas series", "Tengohg", "Jusnagun", "SEED-Vance series", "Dilla Griena", "Grinna Bete S", "Grinna Bete C", "Kamatoze", "Carriguine", 
            "SEED-Venas (invincible in PSU/AotI)", "Hiru Vol", "Do Vol", "No Vol", "Rogue (Ogg)", "Rogue (Jasse)", "Armed Servant (Taguba)", "Armed Servant (Ozuna)", 
            "Special Ops (Kanohne)", "Special Ops (Solda)", "Special Ops (Assault)", "Ethan Waber (invincible) (PSU/AotI only)", "Lucaim Nav (invincible) (PSU/AotI only)", 
            "Liina Sukaya (invincible) (PSU/AotI only)", "Fulyen Curtz (PSU/AotI only)", "AMF Heavy Infantry", "SEED-Lab Staff", "SEED-Guardian (Sh)", "SEED-Guardian (Kn)", 
            "SEED-Guardian (Tw)", "SEED-Guardian (Sa)", "Pahra (AotI) / Zauzza (PSP1+)", "Nava Ludda (AotI+)", "Grass Assassin (AotI+)", "Kudetob (AotI+)", "Bafal Bragga (AotI+)", 
            "Go Bajilla (AotI+)", "Sageeta (AotI+)", "Galdeen (AotI+)", "Booma (AotI+)", "Bal Soza (AotI+)", "Kakwane (AotI+)", "SEED-Ardite (AotI+)", "Delnadian (AotI+)", 
            "Orcdillan (AotI+)", "Shinowa Hidoki (AotI+)", "SEED-Argine (AotI+)", "Bil De Melan (AotI+)", "Komazli (AotI+)", "Ubakrada (AotI+)", "Zasharogan (AotI+)", "Beade Groode (AotI+)", 
            "Delp Slami (AotI+)", "Gainozeros (AotI+)", "Rygutass (AotI+)", "Rappy Gugg (AotI+)", "Rappy Igg (AotI+)", "Rappy Polec (AotI+)", "Gol Dolva (fire corrupted) (AotI only)", 
            "SEED-Venas (vulnerable) (AotI only)", "Ethan Waber (vulnerable) (AotI only)", "Liina Sukaya (vulnerable) (AotI only)", "Lucaim Nav (vulnerable) (AotI only)", "Vanda Orga (AotI+)", 
            "Go Booma (AotI+)", "Jigo Booma (AotI+)", "Special Ops (Sparda) (AotI+)", "Special Ops (Vobis) (AotI+)", "Armed Servant (Basta) (AotI+)", "Armed Servant (Obme) (AotI+)", 
            "Rogue Mazz (AotI+)", "Rogue Wikko (AotI+)", "Karl F. Howzer (AotI) / Vivienne (PSP1+)", "Renvolt Magashi (AotI+)", "Alfort Tylor (AotI) / SEED-Helga (PSP1+)", 
            "Rappy Paral (PSP2+)", "Sand Rappy (PSP2+)", "Savage Wolf (PSP2+)", "Barbarous Wolf (PSP2+)", "Delbiter (PSP2+)", "Nano Dragon (PSP2+)", "Chaos Sorcerer (PSP2+)", 
            "Ill Gill (PSP2+)", "Evil Shark (PSP2+)", "Pal Shark (PSP2+)", "Guil Shark (PSP2+)", "Poison Lily (PSP2+)", "Nar Lily (PSP2+)", "Sinow Beat (PSP2+)", "Sinow Gold (PSP2+)", 
            "Astark (PSP2+)", "Dark Belra (PSP2+)", "Jeris (PSP2+)", "Dago Gujeri (PSP2+)", "Zoldillan (PSP2+)", "Lu Duggo (PSP2+)", "Walmus (PSP2+)", "Mulnuha (PSP2+)", "Vulgatus (PSP2+)", 
            "Jedein (PSP2+)", "Mog Boggo (PSP2+)", "Dakamazli (PSP2+)", "Darvaguine (PSP2+)", "Bag Degga (PSP2+)", "Danoamaz (PSP2+)", "Vorgadillan (PSP2+)", "Stingee (PSP2+)", 
            "Flavit B1 (PSP2+)", "Gravit S7 (PSP2+)", "Bunari type-Koh3 (PSP2+)", "Tavalus (PSP2+)", "Vegga (PSP2+)", "Svaltia (PSP2+)", "Guardian Rosta (PSP2+)", "Guardian Rosk (PSP2+)", 
            "Little Wing Rosta (PSP2+)", "Little Wing Rosk (PSP2+)", "Kasch Tribesman (PSP2+)", "Shizuru (PSP2+)", "Shizuru (PSP2+)", "Shizuru (PSP2+)", "Black-Haired Swordswoman (PSP2i)", 
            "Nagisa (PSP2i)", "Nagisa (PSP2i)", "Gorangaran (PSP2i)", "Gonan (PSP2i)", "Chaos Bringer (PSP2i)", "Garanz (PSP2i)", "Baranz (PSP2i)", "Finjer R (PSP2i)", "Finjer G (PSP2i)", 
            "Finjer B (PSP2i)", "Coco Melodda (PSP2i)", "Ivarlus (PSP2i)", "Variaran (PSP2i)", "Nargevahl (PSP2i)", "Rappy Machina (PSP2i)", "Bola Vrima (PSP2i)", "Flan Blume (PSP2i)", 
            "Blade Mother (PSP2i)", "Shot Mother (PSP2i)", "Force Mother (PSP2i)", "Heaven's Mother (PSP2i)", "Rappy Rizona (PSP2i)", "Jaggo Rizona (PSP2i)", "Zelumon (PSP2i)", 
            "Brigantia (PSP2i)", "Indi Belra (PSP2i)", "Mil Lily (PSP2i)", "Ob Lily (PSP2i)", "Gol Lily (PSP2i)", "Rappy G'lucky (PSP2i)", "Dark Bringer (INVALID) (PSP2i)" };

        public static string getEnemyName(int layoutMonsterId)
        {
            return layoutMonsterId < enemyNames.Length ? enemyNames[layoutMonsterId] : "INVALID";
        }
        
        public static readonly Dictionary<int, string> elementNames = new Dictionary<int, string>
        {
            { 1, "Default" },
            { 2, "Fire Infected" },
            { 4, "Ice Infected" },
            { 8, "Lightning Infected" },
            { 16, "Ground Infected" },
            { 32, "Light Infected" },
            { 64, "Dark Infected" }
        };

        public static string getElementName(int elementId)
        {
            return elementNames.ContainsKey(elementId) ? elementNames[elementId] : "INVALID";
        }
    }

}
