using Microsoft.EntityFrameworkCore;
using TestHomeWork.Models;


var optionsBuilder = new DbContextOptionsBuilder<EFDataContext>();
optionsBuilder.UseNpgsql("Host = postgres78.1gb.ru; Username = xgb_ocpio; Password = ZPyDk7j-cfHA; Database = xgb_ocpio; Port = 5432");
string yearStr = Environment.GetCommandLineArgs()[1];
try
{
    int year = Convert.ToInt16(yearStr);
    await GetData(year);
}
catch (Exception ex) {
    Console.WriteLine(ex.Message);
}



async Task GetData(int year) {
    var http = new HttpClient();
    var result = await http.GetStringAsync($"https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt?year={year}");
    if (result != null) {
        var rows = result.Split('\n');
        var currency = rows.Select(_ => _.Split('|').ToList()).ToList();
        await DeleteDataAsync(year);
        var columns = currency.Select(_ => _.Count).First();
        for (int i = 1; i< columns; i++)
        {
            PrepareCurrency(i, currency);
        }
    }
}

async Task DeleteDataAsync(int year)
{
    using (var _context = new EFDataContext(optionsBuilder.Options))
    {

        var data = await _context?.Currencies
            .Where(_ => _.Date >= new DateTime(year,1,1,0,0,0, DateTimeKind.Utc))
            .Where(_ => _.Date < new DateTime(year + 1, 1, 1, 0, 0, 0, DateTimeKind.Utc))
            .ToListAsync();

        _context?.RemoveRange(data);
        await _context?.SaveChangesAsync();

    }

}

void PrepareCurrency(int index, List<List<string>> data)
{
    var tempData = data[0][index];
    var names = tempData.Split(' ');
    var temp = data.GetRange(1, data.Count - 2);
    var result = temp.Select(_ =>
        new Currency
        {
            Date = RepareDate(_[0]),
            CurrencyCount = Convert.ToInt16(names[0]),
            CurrencyName = names[1],
            CurrencyRate = Convert.ToDouble(_[index].Replace('.', ','))
        }).ToList();

    using (var _context = new EFDataContext(optionsBuilder.Options))
    {

        _context?.Currencies.AddRange(result);
        _context?.SaveChanges();
    }
}
   
    DateTime RepareDate(string strDate)
    {
        var date = strDate.Split('.');
        return new DateTime(Convert.ToInt16(date[2]), Convert.ToInt16(date[1]),
                Convert.ToInt16(date[0]), 0, 0, 0, DateTimeKind.Utc);
    }





