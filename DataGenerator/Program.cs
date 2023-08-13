using DataGenerator;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

int dataCount = 100;
var yeniListe = new List<YeniListe>();

var isimler = JsonSerializer.Deserialize<List<Isim>>(File.ReadAllText("isimler.json"));
string[] soyisimler = File.ReadAllLines("soyisimler.txt");


var baslangicTarih = DateTime.Today.AddYears(-60);
var bitisTarih = DateTime.Today.AddYears(-7);
var tarihGun = Convert.ToInt16((bitisTarih - baslangicTarih).TotalDays);
int okulNoBaslangic = 100;


for (int i = 0; i < dataCount; i++)
{

    var yeni = new YeniListe();

    var rndIsim = isimler[Random.Shared.Next(0, isimler.Count)];
    yeni.Isim = rndIsim.isim;
    yeni.Cinsiyet = rndIsim.cinsiyet;
    yeni.Soyisim = soyisimler[Random.Shared.Next(0, soyisimler.Length)];
    //yeni.DogumTarih ="{\"$date\":\""+baslangicTarih.AddDays(Random.Shared.Next(0, tarihGun)).ToString("yyyy-MM-ddThh:mm:ss.000Z")+"\"}";
    yeni.DogumTarih = new IsoDate() { date = baslangicTarih.AddDays(Random.Shared.Next(0, tarihGun)).ToString("yyyy-MM-ddThh:mm:ss.000Z") };

    yeni.OkulNo = okulNoBaslangic + i;
    //yeni.Kilo = (Random.Shared.Next(500, 1500) / 10m);
    yeni.Kilo = new IsoDouble() { doouble = (Random.Shared.Next(500, 1500) / 10m).ToString().Replace(",",".") };
    yeni.Nakil = Random.Shared.Next(0, 2) == 0 ? false : true;

    do
    {
        yeni.TcKimlikNo = TcKimlikNoUret();
    } while (yeniListe.Any(x => x.TcKimlikNo == yeni.TcKimlikNo));

    yeniListe.Add(yeni);
}


var options1 = new JsonSerializerOptions
{
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true
};

//https://www.mongodb.com/docs/manual/reference/mongodb-extended-json/#mongodb-bsontype-Double
File.WriteAllText("moqList.json", JsonSerializer.Serialize(yeniListe, options1).Replace("\"date\"", "\"$date\"").Replace("\"doouble\"", "\"$numberDouble\""), Encoding.UTF8);

long TcKimlikNoUret()
{
    string[] karakter0 = new string[] { "1", "2", "3", "4", "5", "6", "7", "8" };
    string[] karakterx = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    string[] karakter10 = new string[] { "2", "4", "6", "8", "0" };

    var tc = new StringBuilder();
    for (int i = 0; i < 11; i++)
    {
        if (i == 0)
        {
            tc.Append(karakter0[Random.Shared.Next(0, karakter0.Length)]);
        }
        else if (i > 0 && i < 10)
        {
            tc.Append(karakterx[Random.Shared.Next(0, karakterx.Length)]);
        }
        else if (i == 10)
        {
            tc.Append(karakter10[Random.Shared.Next(0, karakter10.Length)]);
        }
    }

    return Convert.ToInt64(tc.ToString());
}
