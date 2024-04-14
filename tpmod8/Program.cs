using System;
using System.IO;
using System.Text.Json;
using System.Xml.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        CovidConfig Konfig = new CovidConfig();

        Console.WriteLine("Berapa suhu badan anda saat ini? Dalam nilai " + Konfig.configuration.satuan_suhu);
        double inputSuhu;
        int inputHari;
        if (!double.TryParse(Console.ReadLine(), out inputSuhu))
        {
            Console.WriteLine("Masukan suhu tidak valid.");
            return;
        }

        Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala deman?");
        if (!int.TryParse(Console.ReadLine(), out inputHari))
        {
            Console.WriteLine("Masukan hari tidak valid.");
            return;
        }

        if (Konfig.configuration.satuan_suhu == "celcius")
        {
            if ((inputSuhu >= 36.5 && inputSuhu <= 37.5) && inputHari < Konfig.configuration.batas_hari_demam)
            {
                Console.WriteLine(Konfig.configuration.pesan_diterima);
            }
            else
            {
                Console.WriteLine(Konfig.configuration.pesan_ditolak);
            }
        }
        else if (Konfig.configuration.satuan_suhu == "fahrenheit")
        {
            inputSuhu = FahrenheitToCelsius(inputSuhu);

            if ((inputSuhu >= 36.5 && inputSuhu <= 37.5) && inputHari < Konfig.configuration.batas_hari_demam)
            {
                Console.WriteLine(Konfig.configuration.pesan_diterima);
            }
            else
            {
                Console.WriteLine(Konfig.configuration.pesan_ditolak);
            }
        }

        Konfig.UbahSatuan();
        Console.WriteLine();

        Console.WriteLine("Berapa suhu badan anda saat ini? Dalam nilai " + Konfig.configuration.satuan_suhu);
        if (!double.TryParse(Console.ReadLine(), out inputSuhu))
        {
            Console.WriteLine("Masukan suhu tidak valid.");
            return;
        }

        Console.WriteLine("Berapa hari yang lalu (perkiraan) anda terakhir memiliki gejala deman?");
        if (!int.TryParse(Console.ReadLine(), out inputHari))
        {
            Console.WriteLine("Masukan hari tidak valid.");
            return;
        }

        if (Konfig.configuration.satuan_suhu == "celcius")
        {
            if ((inputSuhu >= 36.5 && inputSuhu <= 37.5) && inputHari < Konfig.configuration.batas_hari_demam)
            {
                Console.WriteLine(Konfig.configuration.pesan_diterima);
            }
            else
            {
                Console.WriteLine(Konfig.configuration.pesan_ditolak);
            }
        }
        else if (Konfig.configuration.satuan_suhu == "fahrenheit")
        {
            inputSuhu = FahrenheitToCelsius(inputSuhu);

            if ((inputSuhu >= 36.5 && inputSuhu <= 37.5) && inputHari < Konfig.configuration.batas_hari_demam)
            {
                Console.WriteLine(Konfig.configuration.pesan_diterima);
            }
            else
            {
                Console.WriteLine(Konfig.configuration.pesan_ditolak);
            }
        }
    }

    public static double FahrenheitToCelsius(double fahrenheit)
    {
        return (fahrenheit - 32) * 5 / 9;
    }
}

public class Config
{
    public string satuan_suhu { get; set; }
    public int batas_hari_demam { get; set; }
    public string pesan_ditolak { get; set; }
    public string pesan_diterima { get; set; }

    public Config() { }

    public Config(string suhu, int batasDemam, string pesanDitolak, string pesanDiterima)
    {
        satuan_suhu = suhu;
        batas_hari_demam = batasDemam;
        pesan_ditolak = pesanDitolak;
        pesan_diterima = pesanDiterima;
    }
}

public class CovidConfig
{
    public Config configuration;

    public const string filePath = "config.json";

    public CovidConfig()
    {
        try
        {
            ReadConfigFile();
        }
        catch (FileNotFoundException)
        {
            SetDefault();
            WriteNewConfigFile();
        }
    }

    public void SetDefault()
    {
        configuration = new Config("celcius", 14, "Anda tidak diperbolehkan masuk ke dalam gedung ini",
            "Anda diperbolehkan masuk ke dalam gedung ini");
    }

    private Config ReadConfigFile()
    {
        String configJsonData = File.ReadAllText(filePath);
        configuration = JsonSerializer.Deserialize<Config>(configJsonData);
        return configuration;
    }

    public void WriteNewConfigFile()
    {
        JsonSerializerOptions options = new JsonSerializerOptions()
        {
            WriteIndented = true
        };

        String jsonString = JsonSerializer.Serialize(configuration, options);
        File.WriteAllText(filePath, jsonString);
    }

    public void UbahSatuan()
    {
        if (configuration.satuan_suhu == null)
        {
            configuration.satuan_suhu = "celcius";
        }
        else if (configuration.satuan_suhu == "celcius")
        {
            configuration.satuan_suhu = "fahrenheit";
        }
        else if (configuration.satuan_suhu == "fahrenheit")
        {
            configuration.satuan_suhu = "celcius";
        }
    }
}